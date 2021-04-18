import React from 'react';
import { Component } from "react";
import * as signalR from '@microsoft/signalr';
import authService from './components/api-authorization/AuthorizeService';
import ConController from './Controllers/Connection/ConController';
import './DrDotnetApp.css';
import ChatTile from './components/Tile/ChatTile';
import DialogTitle from './components/Tile/DialogTitle';
import DialogContent from './components/Tile/DialogContent';
import Dialog from './components/Tile/Dialog';
import MainPage from './components/MainPage';
import withDrDotnetTheme from './Theme';
import { compose } from './Utils/HOC';
import SearchInput from './components/ColumnLeft/Search/SearchInput';
import UserStore from './Stores/UserStore';

class DrDotnetApp extends Component {
    constructor(props) {
        super(props);

        this.state = {
            id: "",
            msg: ""
        }

        ConController.init();
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
        ConController.on('update', this.onUpdate);
    }

    componentWillUnmount() {
        ConController.off('update', this.onUpdate);
    }

    onUpdate = update => {
        
    }

    render() {
        return <MainPage />
    }

    // render() {
    //     return (
    //         <div>
    //             <input type="text" onChange={e => this.changeId(e.target.value)} />
    //             <input type="text" onChange={e => this.changeMsg(e.target.value)} />
    //             <button onClick={() => {
    //                 ConController.send("CreateContact");
    //             }}>create contact</button>
    //             <button onClick={() => {
    //                 ConController.send("GetContacts");
    //             }}>get contact list</button>
    //         </div>
    //     );
    // }
}

const enhance = compose(withDrDotnetTheme);
export default enhance(DrDotnetApp);