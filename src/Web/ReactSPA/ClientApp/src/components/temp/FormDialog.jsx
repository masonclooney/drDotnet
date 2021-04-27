import React from 'react';
import Dialog from '@material-ui/core/Dialog';

import DialogTitle from '@material-ui/core/DialogTitle';

export default function FormDialog(props) {

  const { open, title, children } = props;

  const handleClose = () => {
  };

  return (
      <Dialog open={open} onClose={handleClose} aria-labelledby="form-dialog-title">
        <DialogTitle id="form-dialog-title">{title}</DialogTitle>
        {children}  
      </Dialog>
  );
}
