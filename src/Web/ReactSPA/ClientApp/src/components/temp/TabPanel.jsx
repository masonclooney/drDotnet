import React from "react";
import { makeStyles } from "@material-ui/core/styles";
import Paper from "@material-ui/core/Paper";
import Tabs from "@material-ui/core/Tabs";
import Tab from "@material-ui/core/Tab";
import PhoneIcon from "@material-ui/icons/Phone";
import FavoriteIcon from "@material-ui/icons/Favorite";
import PersonPinIcon from "@material-ui/icons/PersonPin";
import ChatIcon from "@material-ui/icons/Chat";
import ContactsIcon from "@material-ui/icons/Contacts";
import { Box, Typography } from "@material-ui/core";

const useStyles = makeStyles({
  root: {
    maxWidth: 500,
    height: "100%",
    display: 'flex',
     flexDirection: 'column'
  },
});

function a11yProps(index) {
  return {
    id: `scrollable-prevent-tab-${index}`,
    "aria-controls": `scrollable-prevent-tabpanel-${index}`,
  };
}

export function TabPanel(props) {
  const { children, value, index, ...other } = props;

  return (
    <div
    style={{ height: '100%', display: 'flex', flexDirection: 'column' }}
      role="tabpanel"
      hidden={value !== index}
      id={`scrollable-prevent-tabpanel-${index}`}
      aria-labelledby={`scrollable-prevent-tab-${index}`}
      {...other}
    >
      {children}
    </div>
  );
}

export default function IconTabs(props) {
  const classes = useStyles();
  const { value, setValue } = props;
  // const [value, setValue] = React.useState(0);

  const handleChange = (event, newValue) => {
    setValue(newValue);
  };

  

  return (
    <Paper square className={classes.root}>
      <Tabs
        value={value}
        onChange={handleChange}
        variant="fullWidth"
        indicatorColor="primary"
        textColor="primary"
        aria-label="icon tabs example"
      >
        <Tab icon={<ChatIcon />} aria-label="phone" {...a11yProps(0)} />
        <Tab icon={<ContactsIcon />} aria-label="favorite" {...a11yProps(0)} />
      </Tabs>

      {props.children}
    </Paper>
  );
}
