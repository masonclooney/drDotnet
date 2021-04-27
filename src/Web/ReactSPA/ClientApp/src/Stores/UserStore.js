import EventEmitter from "./EventEmitter";
import ConController from '../Controllers/Connection/ConController';

class UserStore extends EventEmitter {
    constructor() {
        super();

        console.log('user store init');

        this.reset();

        this.addConListener();
    }

    reset = () => {
        this.items = new Map();
    }

    onUpdate = update => {

        switch(update["type"]) {
            case 'updateUser':
                this.set(update.data);

                this.emmit(update['type'], update);
                break;
            case 'updateContact':
                this.emit(update['type'], update);
                break;
            default:
                break;
        }
    }

    onClientUpdate = update => {

        switch (update["type"]) {
            case 'clientUpdateAddContactOpen':
                this.emit(update['type'], update);
                break;
        
            default:
                break;
        }
    }

    get(userId) {
        return this.items.get(userId);
    }

    set(user) {
        this.items.set(user.Id, user);
    }

    addConListener = () => {
        ConController.on("update", this.onUpdate);
        ConController.on("clientUpdate", this.onClientUpdate);
    }

    removeConListener = () => {
        ConController.off("update", this.onUpdate);
        ConController.off("clientUpdate", this.onClientUpdate);
    }
}

const store = new UserStore();
window.user = store;
export default store;