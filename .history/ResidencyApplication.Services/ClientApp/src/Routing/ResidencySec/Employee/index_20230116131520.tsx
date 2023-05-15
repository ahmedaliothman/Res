import React from 'react';
import RouteResidencySecEmployee from './ProtectedRouteAdmin';
import * as Pages from './../../../Pages/index'
import { Route } from 'react-router-dom';

const Index = () => {
  return <Route>
  <RouteResidencySecEmployee exact path="/Res/Employee/" component={Pages.ResidencySecPages.EmployeePages.InwardApplicationAssign.default} />
  <RouteResidencySecEmployee exact path="/Res/Employee/InwardApplicationAssign" component={Pages.ResidencySecPages.EmployeePages.InwardApplicationAssign.default} />
  <RouteResidencySecEmployee exact path="/Res/Employee/InwardApplication" component={Pages.ResidencySecPages.EmployeePages.InwardApplication.default} />
  <RouteResidencySecEmployee exact path="/Res/Employee/OutwardApplication" component={Pages.ResidencySecPages.EmployeePages.OutwardApplication.default} />
  <RouteResidencySecEmployee exact path="/Res/Employee/SearchApplications" component={Pages.ResidencySecPages.EmployeePages.SearchApplications.default} />
  <RouteResidencySecEmployee exact path="/Res/Employee/ApplicationsReports" component={Pages.ResidencySecPages.EmployeePages.ApplicationsReports.default} />
  <RouteResidencySecEmployee exact path="/Res/Employee/EditRow" component={Pages.ResidencySecPages.CommonPages.EditRow.default} />
  
  <RouteResidencySecEmployee exact path="/Res/Employee/OnlyView/NewApp" component={Pages.ResidencySecPages.CommonPages.OnlyView.NewApp.default} />
  <RouteResidencySecEmployee exact path="/Res/Employee/OnlyView/PersonalInfo" component={Pages.ResidencySecPages.CommonPages.OnlyView.PersonalInfo.default} />
  <RouteResidencySecEmployee exact path="/Res/Employee/OnlyView/PassportInfo" component={Pages.ResidencySecPages.CommonPages.OnlyView.PassportInfo.default} />
  <RouteResidencySecEmployee exact path="/Res/Employee/OnlyView/FileAttachments" component={Pages.ResidencySecPages.CommonPages.OnlyView.FileAttachments.default} />

  <RouteResidencySecEmployee exact path="/Res/Employee/EditView/NewApp" component={Pages.ResidencySecPages.CommonPages.EditView.NewApp.default} />
  <RouteResidencySecEmployee exact path="/Res/Employee/EditView/PersonalInfo" component={Pages.ResidencySecPages.CommonPages.EditView.PersonalInfo.default} />
  <RouteResidencySecEmployee exact path="/Res/Employee/EditView/PassportInfo" component={Pages.ResidencySecPages.CommonPages.EditView.PassportInfo.default} />
  <RouteResidencySecEmployee exact path="/Res/Employee/EditView/FileAttachments" component={Pages.ResidencySecPages.CommonPages.EditView.FileAttachments.default} />
  <RouteResidencySecEmployee exact path="/Res/Employee/EditView/Result" component={Pages.ResidencySecPages.CommonPages.EditView.Result.default} />
  <RouteResidencySecEmployee exact path="/Res/Employee/EditView/Agreament" component={Pages.ResidencySecPages.CommonPages.EditView.Agreement.default} />





  <RouteResidencySecEmployee exact path="/Res/Employee/AddAdminRequestForUser" component={Pages.ResidencySecPages.EmployeePages.AddRequestAsClient.AddAdminRequestForUser.default} />
  <RouteResidencySecEmployee exact path="/Res/Employee/AddRequestAsClient/NewAppAdmin" component={Pages.ResidencySecPages.EmployeePages.AddRequestAsClient.NewAppAdmin.default} />
  <RouteResidencySecEmployee exact path="/Res/Employee/AddRequestAsClient/PersonalInfoAdmin" component={Pages.ResidencySecPages.EmployeePages.AddRequestAsClient.PersonalInfoAdmin.default} />
  <RouteResidencySecEmployee exact path="/Res/Employee/AddRequestAsClient/PassportInfoAdmin" component={Pages.ResidencySecPages.EmployeePages.AddRequestAsClient.PassportInfoAdmin.default} />
  <RouteResidencySecEmployee exact path="/Res/Employee/AddRequestAsClient/FileAttachmentsAdmin" component={Pages.ResidencySecPages.EmployeePages.AddRequestAsClient.FileAttachmentsAdmin.default} />
  <RouteResidencySecEmployee exact path="/Res/Employee/AddRequestAsClient/AgreamentAdmin" component={Pages.ResidencySecPages.EmployeePages.AddRequestAsClient.AgreamentAdmin.default} />
  <RouteResidencySecEmployee exact path="/Res/Employee/AddRequestAsClient/ResultAdmin" component={Pages.ResidencySecPages.EmployeePages.AddRequestAsClient.ResultAdmin.default} />
</Route>;
};

export default Index;
