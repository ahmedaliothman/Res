import React from 'react';
import ProtectedRouteAdminWithoutMenu from './ProtectedRouteAdminWithoutMenu';
import * as Pages from './../../../Pages/index'
import { Route } from 'react-router-dom';

const Index = () => {
  return <Route>
  <ProtectedRouteAdminWithoutMenu exact path="/Res/CommonPages/LandingPage" component={Pages.ResidencySecPages.CommonPages.LandingPage.default} />
  <ProtectedRouteAdminWithoutMenu exact path="/Res/CommonPages/EditRow" component={Pages.ResidencySecPages.CommonPages.EditRow.default} />
  
  </Route>;
};

export default Index;
