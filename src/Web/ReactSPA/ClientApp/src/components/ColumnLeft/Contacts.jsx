import { IconButton, ListItem } from "@material-ui/core";
import React, { Component } from "react";
import SearchInput from "./Search/SearchInput";
import AddIcon from "@material-ui/icons/Add";
import VirtualizedList from "../Additional/VirtualizedList";
import ConController from "../../Controllers/Connection/ConController";
import "./Contacts.css";
import User from "../Tile/User";
import UserStore from '../../Stores/UserStore';

class UserListItem extends Component {
  render() {
    const { style, userId } = this.props;

    return (
      <ListItem button className="user-list-item" style={style}>
        <User userId={userId} />
      </ListItem>
    );
  }
}

class Contacts extends Component {
  constructor(props) {
    super(props);

    this.state = {
      items: null,
    };
  }

  componentDidMount() {
    UserStore.on('updateContact', this.onUpdateContact);
    this.loadContent();
  }

  componentWillUnmount() {
    UserStore.off('updateContact', this.onUpdateContact);
  }

  onUpdateContact = update => {
    const { data } = update;
    console.log(data);
    this.setState({
      items: data
    });
  }

  loadContent() {
    if (!ConController.connected) {
      setTimeout(() => this.loadContent(), 1000);
      return;
    }
    ConController.send({
      type: "getContacts",
      data: JSON.stringify({ PageSize: 10, PageIndex: 0 }),
    });
  }

  renderItem = ({ index, style }, items) => {
    const userId = items.Ids[index];

    return <UserListItem key={userId} userId={userId} style={style} />;
  };

  render() {
    const { items } = this.state;

    return (
      <>
        <div className="contacts-content">
          {items && (
            <VirtualizedList
              className="contacts-list"
              rowHeight={72}
              overScanCount={20}
              source={items.Ids}
              renderItem={(x) => this.renderItem(x, items)}
            />
          )}
        </div>
      </>
    );
  }
}

export default Contacts;
