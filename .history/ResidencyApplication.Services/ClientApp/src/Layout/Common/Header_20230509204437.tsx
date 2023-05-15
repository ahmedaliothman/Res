import React, { useContext, useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { authenticateResponse } from '../../types/userInfo';
import { getLocalStorage } from '../../Services/utils/localStorageHelper';
import { getllogout } from "../../State/login";
import { ClearRequest } from "../../State/newApp";
import { RequestClear as RequestClearPersonalInfo } from "../../State/personalInfo";
import { RequestClear as RequestClearPassportInfo } from "../../State/passportInfo";
import { RequestClear as RequestClearattachmentDocuments } from "../../State/attachmentDocuments";
import { useHistory } from "react-router-dom";
import { RootState } from "../../State/rootReducer";
import { useSelector, useDispatch } from "react-redux";
import {AppTypes_Clear} from '../../State/lookUps/types';
import FullPageLoader from '../../Controls/FullPageLoader';
import { toast, ToastContainer } from 'react-toastify';
import {ToastMessageStatus}  from '../../types/Enums'
const Header = () => {

  const authenticationState= useSelector<RootState,RootState["login"]>(state=>state.login);
  let dispatch = useDispatch();
  const history = useHistory();
  const pageLoaderState = useSelector<RootState, RootState["pageLoader"]>(state => state.pageLoader);
  const {status}=pageLoaderState;
  const toastMessageState = useSelector<RootState, RootState["toastMessage"]>(state => state.toastMessage);
  const {message} = toastMessageState;
  const logout = () => {
    localStorage.removeItem("user");
    dispatch(getllogout());
    dispatch(ClearRequest());
    dispatch(RequestClearPersonalInfo());
    dispatch(RequestClearPassportInfo());
    dispatch(RequestClearattachmentDocuments());
    dispatch({type:AppTypes_Clear});
  }
  useEffect(()=>{
    if(message!=undefined&&message!=''&&message!=null&&toastMessageState.status==ToastMessageStatus.success) 
    toast.success(message);
    if(message!=undefined&&message!=''&&message!=null&&toastMessageState.status==ToastMessageStatus.error) 
    toast.error(message);
    if(message!=undefined&&message!=''&&message!=null&&toastMessageState.status==ToastMessageStatus.info) 
    toast.info(message);
    
  },[toastMessageState.message,toastMessageState.status]);

  return (
    <>
      <nav className="sb-topnav navbar navbar-expand navbar-dark bg-dark">
        <img className="navbar-brand d-none d-md-block" src={process.env.PUBLIC_URL + 'img/brand.png'} alt="" />

        <p className="navbar-brand d-block d-sm-none">وزارة الاوقاف والشئون اﻹسلامية</p>
        <button className="btn btn-link btn-sm order-1 order-lg-0" id="sidebarToggle"
        > <i className="fas fa-bars" />
        </button>
        {/* profle*/}
        {/*#######################################*/}
        <div className="d-none d-md-inline-block">
          <ul className="navbar-nav ml-auto ml-md-0">
            {authenticationState.isLoggedIn ?
              <li className="nav-item dropdown"> <a className="top-nav-link dropdown-toggle" id="userDropdown" href="#" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i className="fas fa-user fa-fw" />&nbsp;&nbsp; ملف المستخدم
              &nbsp;&nbsp;</a>
                <div className="dropdown-menu dropdown-menu-right" aria-labelledby="userDropdown">
                  {/* <button className="dropdown-item sb-nav-link-icon">
                    <i className="fas fa-angle-double-left" />&nbsp;&nbsp; الملف الشخصي
                  </button> */}
                    {(authenticationState.userInfo?.userTypeId!=26)&&(authenticationState.userInfo?.nationalityId?.toLowerCase()!="kw") &&
                  <Link to="/Client/newapp" className="dropdown-item sb-nav-link-icon" onClick={() => history.push("/newapp")} >
                    <i className="fas fa-angle-double-left" />&nbsp;&nbsp;  الحساب الشخصي
                  </Link>}
                  {(authenticationState.userInfo?.userRoleId==1||authenticationState.userInfo?.userRoleId==3)  &&
                    <Link to="/Res/CommonPages/LandingPage" className="dropdown-item sb-nav-link-icon" onClick={() => history.push("/Landingpage") }>
                      <i className="fas fa-angle-double-left" />&nbsp;&nbsp; مسئول النظام
                  </Link>}
                  <Link to="/Public/login" className="dropdown-item sb-nav-link-icon" onClick={logout} >
                    <i className="fas fa-angle-double-left" />&nbsp;&nbsp; تسجيل الخروج
                  </Link>
                  <div className="dropdown-divider " />
                </div>
              </li> : <li className="nav-item"><Link to="/Public/login" className="top-nav-link">تسجيل الدخول</Link></li>}
          </ul>
        </div>
        <div className="d-none d-md-inline-block" />
        {/*#######################################*/}
      </nav>
      {status && <FullPageLoader/>}
      { <ToastContainer  position="top-left" rtl={true}/>}
    </>
  );
}

export default Header;
