

import {INewAppState} from "../../types/newApp"

export interface IState extends INewAppState{
   directionNavigation?:string;
   farwordUrl?:string;
   backwordUrl?:string;
}

export interface IStateApp {
    IState:IState,
    isloading:boolean,
    hasError:Boolean,
    message:string
}

export interface IApplicationStatusUpdateRequst {
    applicationNumber:number,
    userId:number,
    statusId:number
}

export const ApplicationStatusRequst = 'NEWAPP/APPLICATIONSTATUSUPDATEREQUEST';
export interface ApplicationStatusUpdateRequstType {
    type: typeof ApplicationStatusRequst,
    PAYLOAD:IApplicationStatusUpdateRequst

}

export const ApplicationStatus = 'NEWAPP/APPLICATIONSTATUSUPDATE';
export interface ApplicationStatusUpdateType {
    type: typeof ApplicationStatus
}



export const ClearRequst = 'NEWAPP/CLEARREQUEST';
export interface ClearRequstActionType {
    type: typeof ClearRequst
}

export const Clear = 'NEWAPP/CLEAR';
export interface ClearActionType {
    type: typeof Clear
}


export const GetRequest = 'NEWAPP/GET';
export interface GetRequestActionType {
    type: typeof GetRequest,
    payload:number
}


export const RequestLoading = 'NEWAPP/REQUESTLOADING';

export interface RequestLoadingActionType {
    type: typeof RequestLoading,
    payload:boolean
}

export const Loading = 'NEWAPP/LOADING';

export interface LoadingActionType {
    type: typeof Loading,
    payload:boolean
}


export const CreateRequest = 'NEWAPP/CREATE';
export interface CreateRequestActionType {
    type: typeof CreateRequest,
    payload:IState
}

export const CreateSuccess = 'NEWAPP/CREATE_SUCCESS';
export interface CreateSuccessActionType {
    type: typeof CreateSuccess,
    payload:IStateApp
}

export const IncompleteFetchRequest = 'NEWAPP/FETCH';
export interface FetchIncompleteActionType {
    type: typeof IncompleteFetchRequest,
    payload:number
}

export const IncompleteFetchSuccess = 'NEWAPP/FETCH_SUCCESS';
export interface IncompleteFetchSuccessActionType {
    type: typeof IncompleteFetchSuccess,
    payload:IStateApp
}

export const UpdateRequest = 'NEWAPP/UPDATE';
export interface UpdateActionType {
    type: typeof UpdateRequest,
    payload:IState
}

export const UpdateSuccess = 'NEWAPP/UPDATE_SUCCESS';
export interface UpdateSuccessActionType {
    type: typeof UpdateSuccess,
    payload:IStateApp
}




export type NewAppActionsTypes =CreateRequestActionType|CreateSuccessActionType|
                                FetchIncompleteActionType|IncompleteFetchSuccessActionType|
                                UpdateActionType|UpdateSuccessActionType|LoadingActionType|ClearActionType|ClearRequstActionType;



