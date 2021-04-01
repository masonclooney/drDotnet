import { StylesProvider } from "@material-ui/styles";
import React, { Component } from "react";
import { getDisplayName } from "./Utils/HOC";

function withTheme(WrappedComponent) {
  class WithTheme extends Component {
    constructor(props) {
      super(props);
    }

    render() {
      return (
        <StylesProvider injectFirst={true}>
          <WrappedComponent {...this.props} />
        </StylesProvider>
      );
    }
  }

  WithTheme.displayName = `WithTheme(${getDisplayName(WrappedComponent)})`;
  return WithTheme;
}

export default withTheme;