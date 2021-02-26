import React from 'react';
import { Component, Fragment } from "react";
import { Route } from "react-router-dom";
import { Login } from "./Login";

export default class ApiAuthorizationRoutes extends Component {
    render() {
        return <Fragment>
            <Route path='/authentication/login' render={() => loginAction('login')} />
            <Route path='/authentication/login-callback' render={() => loginAction('login-callback')} />
        </Fragment>
    }
}

function loginAction(name) {
    return (<Login action={name}></Login>);
}