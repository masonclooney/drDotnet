import React from 'react';
import { Component } from "react";
import { Redirect, Route } from 'react-router-dom';
import authService from './AuthorizeService';

export default class AuthorizeRoute extends Component {
    constructor(props) {
        super(props);

        this.state = {
            ready: false,
            authenticated: false
        }
    }

    componentDidMount() {
        // this._subscription = authService.subscribe(() => );
        this.populateAuthenticationState();
    }

    async populateAuthenticationState() {
        const authenticated = await authService.isAuthenticated();
        this.setState({ ready: true, authenticated });
    }

    render() {
        const { ready, authenticated } = this.state;
        var link = document.createElement("a");
        link.href = this.props.path;
        const returnUrl = `${link.protocol}//${link.host}${link.pathname}${link.search}${link.hash}`;
        const redirectUrl = `/authentication/login?returnUrl=${encodeURI(returnUrl)}`;
        console.log(redirectUrl);

        if (!ready) {
            return <div>wait...</div>
        } else {
            const { component: Component, ...rest } = this.props;

            return <Route { ...rest } render={(props) => {
                if (authenticated) {
                    return <Component {...props} />
                } else {
                    return <Redirect to={redirectUrl} />
                }
            }} />
        }
    }
}