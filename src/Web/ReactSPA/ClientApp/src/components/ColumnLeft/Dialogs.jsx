import React, { Component } from 'react';
import ScrollableTabsButtonPrevent from '../temp/TabPanel';
import "./Dialogs.css";
import DialogsHeader from './DialogsHeader';
import DialogsList from './DialogsList';

class Dialogs extends Component {
    render () {
        return <div className="dialogs">
            <div className="sidebar-page">
                <ScrollableTabsButtonPrevent />

                <DialogsHeader />

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