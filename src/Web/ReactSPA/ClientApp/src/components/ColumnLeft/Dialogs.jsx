import React, { Component } from "react";
import ScrollableTabsButtonPrevent, { TabPanel } from "../temp/TabPanel";
import Contacts from "./Contacts";
import ContactsHeader from "./ContactsHeader";
import CreateContact from "./CreateContact";
import "./Dialogs.css";
import DialogsHeader from "./DialogsHeader";
import DialogsList from "./DialogsList";
import UserStore from '../../Stores/UserStore';

class Dialogs extends Component {
  constructor(props) {
    super(props);

    this.state = {
      value: 0,
      openCreateContact: false
    };
  }

  handleChangeValue = (value) => {
    this.setState({ value });
  };

  componentDidMount() {
    UserStore.on('clientUpdateAddContactOpen', this.onClientUpdateAddContactOpen);
  }

  componentWillUnmount() {
    UserStore.off('clientUpdateAddContactOpen', this.onClientUpdateAddContactOpen);
  }

  onClientUpdateAddContactOpen = async update => {
    const { open } = update;

    this.setState({ openCreateContact: open });
  }

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
        <CreateContact open={this.state.openCreateContact} />
      </div>
    );
  }
}

export default Dialogs;
