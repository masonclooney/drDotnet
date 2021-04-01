import React, { Component } from "react";
import VirtualizedList from "../Additional/VirtualizedList";
import Dialog from "../Tile/Dialog";
import "./DialogsList.css";

class DialogListItem extends Component {
  render() {
    const { style } = this.props;

    return (
      <div className="dialogs-list-item" style={style}>
        <Dialog />
      </div>
    );
  }
}

class DialogsList extends Component {

    constructor() {
        super();
        this.source = [1, 2, 3, 4, 5, 6, 7, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 7, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 7, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 7, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 7, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 7, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 7, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 7, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 7, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 7, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 7, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6];
    }

  renderItem = ({ index, style }, source) => {

    return <DialogListItem key={index} style={style} />
    // return <div style={style}>{index}</div>;
  };

  render() {
    return (
      <VirtualizedList
        className="dialogs-list"
        overScanCount={20}
        rowHeight={76}
        renderItem={(x) => this.renderItem(x, this.source)}
        source={this.source}
      />
    );
  }
}

export default DialogsList;
