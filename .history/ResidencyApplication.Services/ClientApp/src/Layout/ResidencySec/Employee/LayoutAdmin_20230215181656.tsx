import React, { FC } from 'react'
import SideMenuAdmin from "./SideMenuAdmin";
import AuthHeader from "../../Common/AuthHeader";
import Header from '../../Common/Header';
import FullPageLoader from '../../../Controls/FullPageLoader';
import { getLocalStorage } from '../../../Services/utils/localStorageHelper';
import { useSelector,useDispatch } from "react-redux";
import { RootState } from "../../../State/rootReducer";

import { authenticateResponse } from '../../../types/userInfo';
import '../../../assets/js/all';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import '../../../assets/js/all';

const LayoutAdmin: FC = ({ children }) => {
    const LookUpState = useSelector<RootState,RootState["lookUp"]>(state => state.lookUp);
    const userData = useSelector<RootState,RootState["login"]>(state => state.login);
    const {IsLoading} = LookUpState;



    return (
        <>
         <Header />
            <div id="layoutSidenav">
                <SideMenuAdmin />
                <div id="layoutSidenav_content">              
                    {(userData.isLoggedIn&&userData?.userInfo?.userRoleId==3 )&& 
                    <AuthHeader loggedIn={userData.isLoggedIn} 
                    fullName={ userData.userInfo.employeeName}
                    civilId = {userData.userInfo.civilId} />}
                    {children}
                </div>
            </div>
            { IsLoading && <FullPageLoader />}
            <ToastContainer  position="top-left" rtl={true}/>
        </>
    )
}

export default LayoutAdmin;
