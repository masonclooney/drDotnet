import React, { Component } from "react";
import { getUserName } from "../../Utils/User";
import UserStore from '../../Stores/UserStore';
import "./UserTitle.css";

class UserTitle extends Component {

  state = {};

  static getDerivedStateFromProps(props, state) {
    if(props.userId !== state.prevUserId) {
      const { userId } = props;

      return {
        prevUserId: userId,
        fullName: getUserName(userId)
      }
    }

    return null;
  }

  componentDidMount() {
    UserStore.on('updateUser', this.onUpdateUser);
  }

  componentWillUnmount() {
    UserStore.off('updateUser', this.onUpdateUser);
  }

  onUpdateUser = update => {
    const { userId } = this.props;

    if (userId !== update.data.Id) return;

    this.setState({ fullName: getUserName(userId) });
  }

  render() {

    const { fullName } = this.state;

    return <div className="user-title">{fullName}</div>;
  }
}

export default UserTitle;
