import React, { Component } from 'react';
import Dialogs from './ColumnLeft/Dialogs';
import DialogDetails from './ColumnMiddle/DialogDetails';
import "./MainPage.css";

class MainPage extends Component {
    render() {
        return <div className="page">
            <Dialogs />
            <DialogDetails />
        </div>;
    }
}

export default MainPage;