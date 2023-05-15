import React from 'react';
import RouteResidencySecManger from './ProtectedRouteAdminSettings';
import * as Pages from './../../../Pages/index'
import { Route } from 'react-router-dom';

const Index = () => {
  return <Route>
  <RouteResidencySecManger exact path="/Res/Manager/" component={Pages.ResidencySecPages.ManagerPages.GeneralSettings.default} />
  <RouteResidencySecManger exact path="/Res/Manager/GeneralSettings" component={Pages.ResidencySecPages.ManagerPages.GeneralSettings.default} />
  <RouteResidencySecManger exact path="/Res/Manager/AppTypesSettings" component={Pages.ResidencySecPages.ManagerPages.AppTypesSettings.default} />
  <RouteResidencySecManger exact path="/Res/Manager/EmployeeReports" component={Pages.ResidencySecPages.ManagerPages.EmployeeReports.Viewer} />
  <RouteResidencySecManger exact path="/Res/Manager/NotificationSettings" component={Pages.ResidencySecPages.ManagerPages.NotificationSettings.default} />
  <RouteResidencySecManger exact path="/Res/Manager/RegistrationActivationSettings" component={Pages.ResidencySecPages.ManagerPages.RegistrationActivationSettings.default} />
  <RouteResidencySecManger exact path="/Res/Manager/UpdateUserInfo" component={Pages.ResidencySecPages.ManagerPages.UpdateUserInfo.default} />
  <RouteResidencySecManger exact path="/Res/Manager/UserManagement" component={Pages.ResidencySecPages.ManagerPages.UserManagement.default} />
  <RouteResidencySecManger exact path="/Res/Manager/UserTypesSettings" component={Pages.ResidencySecPages.ManagerPages.UserTypesSettings.default} />
  <RouteResidencySecManger exact path="/Res/Manager/UsersReports" component={Pages.ResidencySecPages.ManagerPages.UsersReports.default} />
  <RouteResidencySecManger exact path="/Res/Manager/InwardApplicationAssign" component={Pages.ResidencySecPages.ManagerPages.InwardApplicationAssign.default} />
  <RouteResidencySecManger exact path="/Res/Manager/EditRow" component={Pages.ResidencySecPages.CommonPages.EditRow.default} />


  <RouteResidencySecManger exact path="/Res/Manager/EditView/NewApp" component={Pages.ResidencySecPages.CommonPages.EditView.NewApp.default} />
  <RouteResidencySecManger exact path="/Res/Manager/EditView/PersonalInfo" component={Pages.ResidencySecPages.CommonPages.EditView.PersonalInfo.default} />
  <RouteResidencySecManger exact path="/Res/Manager/EditView/PassportInfo" component={Pages.ResidencySecPages.CommonPages.EditView.PassportInfo.default} />
  <RouteResidencySecManger exact path="/Res/Manager/EditView/FileAttachments" component={Pages.ResidencySecPages.CommonPages.EditView.FileAttachments.default} />
  <RouteResidencySecManger exact path="/Res/Manager/EditView/Result" component={Pages.ResidencySecPages.CommonPages.EditView.Result.default} />
  <RouteResidencySecManger exact path="/Res/Manager/EditView/Agreement" component={Pages.ResidencySecPages.CommonPages.EditView.Agreement.default} />
  
</Route>;
};

export default Index;
