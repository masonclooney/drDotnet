import React, { Component } from "react";
import FormDialog from "../temp/FormDialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import Button from "@material-ui/core/Button";
import TextField from "@material-ui/core/TextField";
import ConController from "../../Controllers/Connection/ConController";

class CreateContact extends Component {
  constructor(props) {
    super(props);

    this.state = {
        name: "",
        email: ""
    }


  }

  handleClose() {
    ConController.clientUpdate({
      type: "clientUpdateAddContactOpen",
      open: false,
    });
  }

  handleInputChange = event => {
    const target = event.target;
    this.setState({
        [target.name]: target.value
    });
  }

  handleCreate = () => {
    
  }

  render() {
    return (
      <FormDialog open={this.props.open} title="Add Contact">
        <DialogContent>
          <TextField
            autoFocus
            required
            margin="dense"
            id="name"
            label="Name"
            type="text"
            name='name'
            autoComplete="off"
            fullWidth
            onChange={this.handleInputChange}
          />
          <TextField
            autoFocus
            required
            margin="dense"
            id="email"
            name='email'
            label="Email Address"
            type="email"
            autoComplete="off"
            fullWidth
            onChange={this.handleInputChange}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={this.handleClose} color="primary">
            Cancel
          </Button>
          <Button onClick={this.handleCreate} color="primary">
            Create
          </Button>
        </DialogActions>
      </FormDialog>
    );
  }
}

export default CreateContact;
