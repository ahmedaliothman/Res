import * as React from 'react';
import { useHistory,useLocation } from 'react-router-dom'
import { useDispatch, useSelector } from "react-redux"
import { RootState } from '../../../State/rootReducer';
import { getLocalStorage } from "../../../Services/utils/localStorageHelper";
import { authenticateResponse } from "../../../types/userInfo";
import { Link, NavLink } from 'react-router-dom';
import {getHistoryRequest} from '../../../State/history/action'

interface IAppProps {
}

const LandingPage = () => {
  
  let publicUrl=process.env.PUBLIC_URL;
  let imagePathInfo=publicUrl + '../../../img/information.png';
  const history = useHistory();
  const dispatch = useDispatch();
  const location=useLocation();
  let userAuth = getLocalStorage("user", authenticateResponse);
  const historyBrowser= useSelector<RootState, RootState["history"]>(state => state.history);
  const gotoSettings=()=>
  {
    
  }

  const gotoInquery=async()=>
  {
   await dispatch(getHistoryRequest("/Res/Employee/InwardApplication"));
  }
  return (<>
  <div className="container py-4" >
   <div className="row justify-content-xl-center">
    <div className="col-md-8">
     <div className="row justify-content-xl-center">
         <div className="col-md-4 text-center mb-3 mb-lg-0"> 
         {/* <input type="button" className="btn btn-name" onClick={()=>{gotoInquery()}}  /> */}
         {(userAuth.isLoggedIn && (userAuth.response.userInfo.userRoleId == 1)||(userAuth.response.userInfo.userRoleId == 3)) &&
         
         <NavLink  to="/Res/Employee/InwardApplication">
         <img  src={imagePathInfo}  width="100%" />
                <span  className="btn btn-primary" >
                استعلام المعاملات
                </span>
              </NavLink>
}
               </div>
         <div className="col-md-4 text-center">
         {(userAuth.isLoggedIn && (userAuth.response.userInfo.userRoleId == 1) ) &&
            <NavLink  to="/Res/Manager/InwardApplicationassign">
         <img src={publicUrl + '../../../img/setting.png'}   width="100%" />
                <span  className="btn btn-warning" >
                اعدادات النظام
                </span>
              </NavLink> }
              </div>
      </div>
    </div>
  </div>
</div>
  </>);
};

export default LandingPage;
