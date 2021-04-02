import React, { Component } from "react";
import ScrollableTabsButtonPrevent, { TabPanel } from "../temp/TabPanel";
import Contacts from "./Contacts";
import ContactsHeader from "./ContactsHeader";
import "./Dialogs.css";
import DialogsHeader from "./DialogsHeader";
import DialogsList from "./DialogsList";

class Dialogs extends Component {
  constructor(props) {
    super(props);

    this.state = {
      value: 0,
    };
  }

  handleChangeValue = (value) => {
    this.setState({ value });
  };

  render() {
    const { value } = this.state;
    return (
      <div className="dialogs">
        <div className="sidebar-page">
          <ScrollableTabsButtonPrevent
            value={value}
            setValue={this.handleChangeValue}
          >
            <TabPanel value={value} index={0}>
              <DialogsHeader />
              <div className="dialogs-content">
                <div className="dialogs-content-internal">
                  <DialogsList />
                </div>
              </div>
            </TabPanel>
            <TabPanel value={value} index={1}>
                <ContactsHeader />
              <div className="dialogs-content">
                <div className="dialogs-content-internal">
                  <Contacts />
                </div>
              </div>
            </TabPanel>
          </ScrollableTabsButtonPrevent>
        </div>
      </div>
    );
  }
}

export default Dialogs;
