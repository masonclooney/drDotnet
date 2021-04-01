import React, { Component } from 'react';
import "./Dialogs.css";
import DialogsList from './DialogsList';

class Dialogs extends Component {
    render () {
        return <div className="dialogs">
            <div className="sidebar-page">
                <div className="dialogs-content">
                    <div className="dialogs-content-internal">
                        <DialogsList />
                    </div>
                </div>
            </div>
        </div>
    }
}

export default Dialogs;