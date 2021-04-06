import { IconButton, ListItem } from "@material-ui/core";
import React, { Component } from "react";
import SearchInput from "./Search/SearchInput";
import AddIcon from "@material-ui/icons/Add";
import VirtualizedList from "../Additional/VirtualizedList";
import ConController from '../../Controllers/Connection/ConController';
import "./Contacts.css";
import { CONTACT_STREAM } from "../../Constants";
import User from "../Tile/User";

class UserListItem extends Component {
    render() {
        const { style } = this.props;

        return <ListItem button className="user-list-item" style={style}>
            <User />
        </ListItem>
    }
}

class Contacts extends Component {

    constructor(props) {
        super(props);

        this.state = {
            source: [1, 2, 3, 4, 5, 6, 7, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 7, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 7, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 7, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 7, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 7, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 7, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 7, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 7, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 7, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 7, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6],
            items: null
        }
    }

    componentDidMount() {
        console.log('mountntntntn');
        this.loadContent();
    }

    loadContent() {
        if (!ConController.connected) {
            setTimeout(() => this.loadContent(), 1000);
            return;
        }
        ConController.getStream(CONTACT_STREAM).subscribe({
            next: (item) => {
                console.log(item);
            },
            complete: () => {
                console.log('finished');
            }
        });
    }

    renderItem = ({ index, style }, source) => {
        return <UserListItem key={index} style={style} />
    };

  render() {
    return (
      <>
        <div className="contacts-content">
          <VirtualizedList
            className="contacts-list"
            rowHeight={72}
            overScanCount={20}
            source={this.state.source}
            renderItem={(x) => this.renderItem(x, this.state.source)}
          />
        </div>
      </>
    );
  }
}

export default Contacts;
