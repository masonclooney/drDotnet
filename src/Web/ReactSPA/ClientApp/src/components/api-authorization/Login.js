import { param } from 'jquery';
import React from 'react';
import { Component } from "react";
import authService, { AuthenticationResultStatus } from './AuthorizeService';

export class Login extends Component {
    constructor(props) {
        super(props);
    }

    componentDidMount() {
        const action = this.props.action;
        console.log('............................................');
        switch(action) {
            case 'login':
                this.login(this.getReturnUrl());
                break;
            case 'login-callback':
                this.processLoginCallback();
                break;
            default:
                throw new Error('invalid action');
        }
    }

    async processLoginCallback() {
        const url = window.location.href;
        console.log(url);
        const result = await authService.completeSignIn(url);
        console.log(result.status);
        switch(result.status) {
            case AuthenticationResultStatus.Redirect:
                throw new Error('Should not redirect.');
            case AuthenticationResultStatus.Success:
                window.location.replace(this.getReturnUrl(result.state));
                break;
            default:
                throw new Error('invalid authentication redult status');
        }
    }

    async login(returnUrl) {
        const state = { returnUrl };
        const result = await authService.signIn(state);
        console.log(result);
        switch(result.status) {
            case AuthenticationResultStatus.Success:
                window.location.replace(returnUrl);
                break;
            case AuthenticationResultStatus.Redirect:
                break;
            default:
                throw new Error('invalid status result');
        }
    }

    getReturnUrl(state) {
        const params = new URLSearchParams(window.location.search);
        const fromQuery = params.get('returnUrl');
        console.log(fromQuery);
        if (fromQuery && !fromQuery.startsWith(`${window.location.origin}/`)) {
            // This is an extra check to prevent open redirects.
            throw new Error("Invalid return url. The return url needs to have the same origin as the current page.")
        }
        return (state && state.returnUrl) || fromQuery || `${window.location.origin}/`;
    }

    render() {
        return <div>wait.. (processing login)</div>
    }
}