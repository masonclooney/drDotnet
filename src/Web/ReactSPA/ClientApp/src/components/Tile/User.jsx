import React, { Component } from 'react';
import UserTile from './UserTile';
import UserTitle from './UserTitle';
import './User.css';
import UserStatus from './UserStatus';

class User extends Component {
    render() {
        return <div className="user">
            <div className="user-wrapper">
                <UserTile />
                <div className="user-inner-wrapper">
                    <div className="tile-first-row">
                        <UserTitle />
                    </div>
                    <div className="tile-second-row">
                        <UserStatus />
                    </div>
                </div>
            </div>
        </div>
    }
}

export default User;