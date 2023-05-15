import { put, call, takeEvery, all, fork } from "redux-saga/effects";
import * as ManageRequestsTypes from "./types";
import * as ManageRequestsActions from "./actions";
import * as ManageRequestsActionsTypes from "./types";
import * as api from "./api";
import { createNewApp,updateNewAppStatus } from "../../Services/newApp";
import {setLoading} from "../lookUps/action"
import { pageLoaderActions } from "../PageLoader";
import  * as toastMessagesActions from '../ToastMessages/action'
import { IMessage } from "../../types/IMessage";
import { ToastMessageStatus } from "../../types/Enums";
function* ManageRequest({type,payload}:ManageRequestsTypes.ManageRequestsCREATE) {
    try {
      yield put(pageLoaderActions.getPageLoaderRequest(true));
      let res: ManageRequestsTypes.ResponseObject= yield call(api.GetUserApplicationsByUserId, payload);
     yield put(ManageRequestsActions.RequestSuccess(res.response));
     yield put(pageLoaderActions.getPageLoaderRequest(false));

    }
     catch (error) 
     {
      yield put(pageLoaderActions.getPageLoaderRequest(false));
      let message: IMessage = {};
    message.message = error as unknown as string;
    message.status = ToastMessageStatus.error;
    yield put(toastMessagesActions.getToastMesageRequest(message));

    }
  }
  
  function* watchOnManageRequest() {
    yield takeEvery(ManageRequestsActionsTypes.CreateRequest, ManageRequest);
  }
  
  function* ManageRequestAdmin({type,payload}:ManageRequestsTypes.ManageRequestsCREATEAdmin) {
    try {
      yield put(pageLoaderActions.getPageLoaderRequest(true));
      let res: ManageRequestsTypes.ResponseObject = yield call(api.GetUserApplicationsAdmin,payload);
      yield put(ManageRequestsActions.ClearRequest());
      yield put(ManageRequestsActions.RequestSuccess(res.response));
      yield put(pageLoaderActions.getPageLoaderRequest(false));
    
    }
     catch (error) 
     {
      yield put(pageLoaderActions.getPageLoaderRequest(false));
    }
  }
  
  function* watchOnManageRequestAdmin() {
    yield takeEvery(ManageRequestsActionsTypes.CreateRequestAdmin, ManageRequestAdmin);
  }
  
  
  function* ManageRequestAdminOutwardApplications({type,payload}:ManageRequestsTypes.ManageRequestsAdminOutwardApplications) {
    try {
      yield put(pageLoaderActions.getPageLoaderRequest(true));
      let res: ManageRequestsTypes.ResponseObject= yield call(api.GetUserApplicationsAdminOutward,payload);
     yield put(ManageRequestsActions.RequestSuccess(res.response));
     yield put(pageLoaderActions.getPageLoaderRequest(false));
    }
     catch (error) 
     {
      yield put(pageLoaderActions.getPageLoaderRequest(false));

    }
  }
  
  function* watchOnManageRequestAdminOutwardApplications() {
    yield takeEvery(ManageRequestsActionsTypes.CreateRequestAdminOutwardApplications, ManageRequestAdminOutwardApplications);
  }

  function* ManageRequestAdminInwardSearch({type,payload}:ManageRequestsTypes.ManageRequestsAdminInwardSearch) {
    try {
      yield put(pageLoaderActions.getPageLoaderRequest(true));
      let res: ManageRequestsTypes.ResponseObject = yield call(api.GetUserApplicationsAdminInwardSearch,payload);
     yield put(ManageRequestsActions.RequestSuccess(res.response));
     yield put(pageLoaderActions.getPageLoaderRequest(false));
   
    }
     catch (error) 
     {
      yield put(pageLoaderActions.getPageLoaderRequest(false));

    }
  }
  
  function* watchOnManageRequestAdminInwardSearch() {
    yield takeEvery(ManageRequestsActionsTypes.CreateRequestAdminInwardSearch, ManageRequestAdminInwardSearch);
  }

  function* SelectedRequest({type,payload}:ManageRequestsTypes.ManageSelectedRequest) {
    try {
      yield put(ManageRequestsActions.SelectedRequest(payload));
    } catch (error) {
  

    }
  }
  
  
  function* watchOnSelectedRequest() {
    yield takeEvery(ManageRequestsActionsTypes.SelectedRequest, SelectedRequest);
  }
  

function* onApplicationStatusRequst({ type,PAYLOAD }: ManageRequestsTypes.ApplicationStatusUpdateRequstType) {
  try {
    yield put(pageLoaderActions.getPageLoaderRequest(true));
    let res: ManageRequestsTypes.IApplicationStatusUpdateRequst = yield call(updateNewAppStatus, PAYLOAD);
    yield put(ManageRequestsActions.ApplicationStatusUpdate(res));
    yield put(pageLoaderActions.getPageLoaderRequest(false));

  } 
  catch (error) {
    yield put(pageLoaderActions.getPageLoaderRequest(false));
  }
}


function* watchonApplicationStatusRequst() {
  yield takeEvery(ManageRequestsTypes.ApplicationStatusRequst, onApplicationStatusRequst);

}



function* onClearAppRequest({ type }: ManageRequestsTypes.ClearRequstActionType) {
  try {
    yield put(ManageRequestsActions.ClearRequest());
  } catch (error) {
  }
}



function* watchonClearAppRequest() {
  yield takeEvery(ManageRequestsTypes.ClearRequst, onClearAppRequest);

}
  export default function* ManageRequests() {
    yield all([fork(watchOnManageRequest),fork(watchOnManageRequest),fork(watchonClearAppRequest),fork(watchOnManageRequestAdmin),fork(watchonApplicationStatusRequst),fork(watchOnManageRequestAdminOutwardApplications),fork(watchOnManageRequestAdminInwardSearch)]);
  }