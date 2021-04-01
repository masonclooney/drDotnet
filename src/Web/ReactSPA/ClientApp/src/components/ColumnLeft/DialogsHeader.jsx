import React, { Component } from "react";
import SearchInput from "./Search/SearchInput";
import './DialogsHeader.css';

class DialogsHeader extends Component {
  render() {
    return <div className="header-master">
        <SearchInput />
    </div>;
  }
}

export default DialogsHeader;
