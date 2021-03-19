import React from 'react';
import { Component } from "react";
import * as signalR from '@microsoft/signalr';
import authService from './components/api-authorization/AuthorizeService';

class DrDotnetApp extends Component {
    constructor(props) {
        super(props);

        this.state = {
            id: "",
            msg: ""
        }

        this.connection = null;
    }

    changeId(id) {
        this.setState({
            ...this.state,
            id
        });
    }

    changeMsg(msg) {
        this.setState({
            ...this.state,
            msg
        });
    }

    componentDidMount() {
        var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:8001/chatHub", {
            accessTokenFactory: async () => {
                return await authService.getAccessToken();
            }
        }).build();

        connection.on("update", function(msg) {
            console.log(msg);
        });

        connection.start().then(() => {
            console.log('started');
            this.connection = connection;
        }).catch(function (err) {
            return console.error(err.toString());
        });
    }

    render() {
        return (
            <div>
                <input type="text" onChange={e => this.changeId(e.target.value)} />
                <input type="text" onChange={e => this.changeMsg(e.target.value)} />
                <button onClick={() => {
                    this.connection.invoke("update", this.state.id, this.state.msg);
                }}>send</button>
            </div>
        );
    }
}

export default DrDotnetApp;