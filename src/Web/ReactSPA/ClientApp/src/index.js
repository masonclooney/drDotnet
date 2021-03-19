import 'bootstrap/dist/css/bootstrap.css';
import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter, Route } from 'react-router-dom';
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import AuthorizeRoute from './components/api-authorization/AuthorizeRoute';
import DrDotnetApp from './DrDotnetApp';
import Home from './Home';
import registerServiceWorker from './registerServiceWorker';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');

ReactDOM.render(
  <BrowserRouter basename={baseUrl}>
    <Route exact path='/' component={Home} />
    <AuthorizeRoute path='/app' component={DrDotnetApp} />
    <Route path='/authentication' component={ApiAuthorizationRoutes} />
  </BrowserRouter>,
  rootElement);

registerServiceWorker();

