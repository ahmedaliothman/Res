import { combineReducers } from "redux"; 

import loginReducer from "./login" ;
import lookUpReducer from "./lookUps";
import newAppReducer from "./newApp";
import personalInfoReducer  from "./personalInfo";
import passportInfoReducer from "./passportInfo";
import generalSettingReducer from "./GeneralSettings";
import ManageRequestsReducer from "./ManageRequests"
import  attachmentInfoReducer from "./attachmentDocuments"
import ToastMessagesReducer from "./ToastMessages/index";

import  paciReducer from "./PaciInfo"
import  historyReducer from "./history"
import { routerReducer, routerMiddleware } from 'react-router-redux';
import { connectRouter } from "connected-react-router";
import { RouterState } from 'connected-react-router';
import pageLoaderReducer from "./PageLoader/index";
  const rootReducer3 =
     combineReducers({
      generalSettings:generalSettingReducer,
     login: loginReducer,
    pageLoader:pageLoaderReducer,
    lookUp:lookUpReducer,
    newApp:newAppReducer,
    personalInfo:personalInfoReducer,
     passportInfo:passportInfoReducer,
     manageRequest:ManageRequestsReducer,
    attachmentInfo:attachmentInfoReducer,
    paci:paciReducer,
    router:routerReducer ,
    history:historyReducer
  })

export const rootReducer = (history:any) => {

  return combineReducers({
    generalSettings:generalSettingReducer,
    login: loginReducer,
    pageLoader:pageLoaderReducer,
    lookUp:lookUpReducer,
    newApp:newAppReducer,
    personalInfo:personalInfoReducer,
    passportInfo:passportInfoReducer,
    manageRequest:ManageRequestsReducer,
    attachmentInfo:attachmentInfoReducer,
    paci:paciReducer,
    router:routerReducer ,
    history:historyReducer
  })
};

type rootReducer = ReturnType<typeof rootReducer3>;
interface RootState2 extends rootReducer   {

}

  export type RootState = RootState2

