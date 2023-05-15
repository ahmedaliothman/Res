import React from 'react';
import { BrowserRouter as Router, Redirect, Route, Switch, withRouter } from "react-router-dom";
//import { Router, Route, Switch } from 'react-router-dom';
import RoutesClientSec from './ClientSec/index';
import RoutesResSec from './ResidencySec/index';
import RoutePublic from './Public/index';
import Logged from './Logged/index';



const Index = () => {
	return <Router >
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
