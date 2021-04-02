import { IconButton } from "@material-ui/core";
import React, { Component } from "react";
import "./ContactsHeader.css";
import SearchInput from "./Search/SearchInput";
import AddIcon from "@material-ui/icons/Add";

class ContactsHeader extends Component {
  render() {
    return (
      <div className="header-master">
        <SearchInput />
        <IconButton color="primary" className="header-right-button">
          <AddIcon />
        </IconButton>
      </div>
    );
  }
}

export default ContactsHeader;
