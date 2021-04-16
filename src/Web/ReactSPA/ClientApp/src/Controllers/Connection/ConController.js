import EventEmitter from "../../Stores/EventEmitter";
import * as signalR from '@microsoft/signalr';
import authService from "../../components/api-authorization/AuthorizeService";

class ConController extends EventEmitter {
    constructor() {
        super();

        this.disableLog = true;
        this.connected = false;
    }

    handleRreconnected = () => {
        this.connected = true;
    }

    handleReconnecting = () => {
        this.connected = false;
    }

    init = () => {
        this.connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:8001/chatHub", {
            accessTokenFactory: async () => {
                return await authService.getAccessToken();
            }
        }).withAutomaticReconnect().build();

        this.connection.onreconnected(this.handleRreconnected);
        this.connection.onreconnecting(this.handleReconnecting);

        this.connection.on('update', update => {
            if (!this.disableLog) {
                console.log('receive update', update);
            }
            this.emit('update', update);
        });

        this.connection.start().then(() => {
            console.log('connection controller started');
            this.connected = true;
        }).catch(function (err) {
            return console.error(err.toString());
        });
    }

    send = msg => {
        if (!this.connection) {
            console.log('connection controller not ready');
            return;
        }

        this.connection.invoke("Update", msg).catch(err => console.error(err));
    }
}

const controller = new ConController();
export default controller;