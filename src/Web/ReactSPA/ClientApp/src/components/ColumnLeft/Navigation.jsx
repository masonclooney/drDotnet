import React, { Component } from "react";
import CallIcon from "@material-ui/icons/Call";
import WhatsAppIcon from "@material-ui/icons/WhatsApp";
import GroupWorkIcon from "@material-ui/icons/GroupWork";
import SettingsIcon from "@material-ui/icons/Settings";
import SecurityIcon from "@material-ui/icons/Security";
import TimerIcon from "@material-ui/icons/Timer";
import "./Navigation.css";
import { IconButton } from "@material-ui/core";
import SendIcon from "@material-ui/icons/Send";

class Navigation extends Component {
  render() {
    return (
      <div className="navigation">

        <IconButton color="primary" aria-label="add to shopping cart">
          <SendIcon />
        </IconButton>
        <IconButton color="primary" aria-label="add to shopping cart">
          <WhatsAppIcon />
        </IconButton>
        <IconButton color="primary" aria-label="add to shopping cart">
          <GroupWorkIcon />
        </IconButton>
        <IconButton color="primary" aria-label="add to shopping cart">
          <SecurityIcon />
        </IconButton>
        <IconButton color="primary" aria-label="add to shopping cart">
          <TimerIcon />
        </IconButton>
        <IconButton color="primary" aria-label="add to shopping cart">
          <SettingsIcon />
        </IconButton>
      </div>
    );
  }
}

export default Navigation;
