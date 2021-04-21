import React, { Component } from 'react';
import UserTile from './UserTile';
import UserTitle from './UserTitle';
import './User.css';
import UserStatus from './UserStatus';
import UserStore from '../../Stores/UserStore';

class User extends Component {

    constructor(props) {
        super(props);

        this.state = {
            user: UserStore.get(props.userId)
        };
    }

    render() {
        const { userId } = this.props;

        const user = UserStore.get(userId);
        if (!user) {
            console.error('[user] can\'t find', userId);
            return null;
        }

        const { Name } = user;

        return <div className="user">
            <div className="user-wrapper">
                <UserTile userId={userId} />
                <div className="user-inner-wrapper">
                    <div className="tile-first-row">
                        <UserTitle userId={userId} />
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