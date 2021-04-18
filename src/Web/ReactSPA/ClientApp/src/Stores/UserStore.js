import EventEmitter from "./EventEmitter";
import ConController from '../Controllers/Connection/ConController';

class UserStore extends EventEmitter {
    constructor() {
        super();

        this.addConListener();
    }

    onUpdate = update => {
        switch(update["type"]) {
            case 'updateUser':
                console.log(update);
                break;
            default:
                break;
        }
    }

    addConListener = () => {
        ConController.on("update", this.onUpdate);
    }

    removeConListener = () => {
        ConController.off("update", this.onUpdate);
    }
}

const store = new UserStore();
window.user = store;
export default store;