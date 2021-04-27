import { IconButton } from "@material-ui/core";
import React, { Component } from "react";
import "./ContactsHeader.css";
import SearchInput from "./Search/SearchInput";
import AddIcon from "@material-ui/icons/Add";
import ConController from "../../Controllers/Connection/ConController";

class ContactsHeader extends Component {
  handleOpen = () => {
    ConController.clientUpdate({
      "type": "clientUpdateAddContactOpen",
      open: true,
    });
  };

  render() {
    return (
      <div className="header-master">
        <SearchInput />
        <IconButton
          onClick={this.handleOpen}
          color="primary"
          className="header-right-button"
        >
          <AddIcon />
        </IconButton>
      </div>
    );
  }
}

export default ContactsHeader;
