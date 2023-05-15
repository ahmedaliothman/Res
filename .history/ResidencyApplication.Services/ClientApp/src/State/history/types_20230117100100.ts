import { UserInfo } from "../../types/userInfo";
import UserRole from "../../ITypes/Loaded/IUserRole";
import UserPhase from "../../ITypes/Loaded/IUserPhase";
import UserType from "../../ITypes/Loaded/IUserType";
import IAuthenticationObj from "../../ITypes/SentToApi/IAuthenticationObj";
import IState from "../../ITypes/Loaded/IState";
import { useHistory } from "react-router-dom";
import { INavigation } from "../../ITypes/Common/INavigation";



export const HISTORYPUSH_REQUEST = 'HISTORYPUSH/REQUEST';
export interface historyPush_request_action_type {
    type: typeof HISTORYPUSH_REQUEST
    payload: INavigation
}



export  type HistoryPushActionTypes = 
historyPush_request_action_type
;

