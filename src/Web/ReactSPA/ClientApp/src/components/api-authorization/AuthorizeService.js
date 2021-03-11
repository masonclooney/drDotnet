import { UserManager } from 'oidc-client';

export class AuthorizeService {
    _callbacks = [];
    _nextSubscriptionId = 0;

    _user = null;
    _isAuthenticated = false;

    subscribe(callback) {
        this._callbacks.push({ callback, subscription: this._nextSubscriptionId++ });
        return this._nextSubscriptionId - 1;
    }

    async ensureUserManagerInitialized() {
        if(this.userManager !== undefined) {
            return;
        }

        let response = await fetch('https://localhost:5001/_configuration/web');
        if(!response.ok) {
            throw new Error(`Could not load settings`);
        }

        let settings = await response.json();
        settings.automaticSilentRenew = true;
        this.userManager = new UserManager(settings);
    }

    async isAuthenticated() {
        const user = await this.getUser();
        return !!user;
    }

    async getUser() {
        if(this._user && this._user.profile) {
            return this._user.profile
        }

        await this.ensureUserManagerInitialized();
        const user = await this.userManager.getUser();
        return user && user.profile;
    }

    async signIn(state) {
        await this.ensureUserManagerInitialized();
        console.log('............................................');
        try {
            const silentUser = await this.userManager.signinSilent(this.createArguments());
            this.updateState(silentUser);
            return this.success(state);
        } catch (silentError) {
            console.log("Silent authentication error: ", silentError);

            try {
                await this.userManager.signinRedirect(this.createArguments(state));
                return this.redirect();
            } catch (redirectError) {
                console.log("Redirect authentication error: ", redirectError);
                return this.error(redirectError);
            }
        }
    }

    async completeSignIn(url) {
        try {
            console.log(url);
            await this.ensureUserManagerInitialized();
            const user = await this.userManager.signinCallback(url);
            this.updateState(user);
            return this.success(user && user.state);
        } catch (error) {
            console.log('There was an error signing in: ', error);
            return this.error('There was an error signing in.');
        }
    }

    createArguments(state) {
        return { useReplaceToNavigate: true, data: state };
    }

    updateState(user) {
        this._user = user;
        this._isAuthenticated = !!this._user;

    }

    notifySubscribers() {
        for (let i = 0; i < this._callbacks.length; i++) {
            const callback = this._callbacks[i].callback;
            callback();
        }
    }

    success(state) {
        return { status: AuthenticationResultStatus.Success, state };
    }

    redirect() {
        return { status: AuthenticationResultStatus.Redirect };
    }

    error(message) {
        return { status: AuthenticationResultStatus.Fail, message };
    }
}

const authService = new AuthorizeService();

export default authService;

export const AuthenticationResultStatus = {
    Redirect: 'redirect',
    Success: 'success',
    Fail: 'fail'
};