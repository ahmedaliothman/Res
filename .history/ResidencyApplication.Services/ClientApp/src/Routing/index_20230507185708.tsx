import React from 'react';
import {  Redirect, Route, Switch, withRouter } from "react-router-dom";
//import { Router, Route, Switch } from 'react-router-dom';
import { Router } from 'react-router';
import RoutesClientSec from './ClientSec/index';
import RoutesResSec from './ResidencySec/index';
import RoutePublic from './Public/index';
import Logged from './Logged/index';



const Index = (history?:any) => {
	return <Router history={history} >
		<Switch>
			<Route>
				<RoutePublic />
				<RoutesClientSec />
				<RoutesResSec />
				<Logged />

			</Route>
		</Switch>
	</Router>

};

export default Index;
