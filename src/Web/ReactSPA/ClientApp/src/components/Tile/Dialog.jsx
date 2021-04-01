import React, { Component } from 'react';
import ListItem from '@material-ui/core/ListItem';
import ChatTile from './ChatTile';
import DialogTitle from './DialogTitle';
import DialogContent from './DialogContent';
import classNames from 'classnames';
import "./Dialog.css";

class Dialog extends Component {

    constructor(props) {
        super(props);
    }

    render() {
        return <ListItem button className={classNames('dialog', { 'item-selected': false }, { 'dialog-hidden': false })} style={null}>
            <div className="dialog-wrapper">
                <ChatTile />
                <div className="dialog-inner-wrapper">
                    <div className="tile-first-row"><DialogTitle /></div>
                    <div className="tile-second-row"><DialogContent /></div>
                </div>
            </div>
        </ListItem>
    }
}

export default Dialog;