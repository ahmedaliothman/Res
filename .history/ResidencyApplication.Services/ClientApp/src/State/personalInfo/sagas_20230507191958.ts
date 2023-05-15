import { put, call, takeEvery, all, fork } from "redux-saga/effects";
import * as actionTypes from "./types";
import * as actions from "./action";
import {store } from "../../index"
import { createPersonalInfo,fetchPersonalInfo,updatePersonalInfo } from "../../Services/personalInfo";
import {Steps} from "../../types/Enums";
import { getUpdateRequest } from "../../State/newApp";
import {getHistoryPushRequest} from "../history/action"
import { push  ,getLocation ,RouterState} from 'connected-react-router';  
import { pageLoaderActions } from "../PageLoader";
import  * as toastMessagesActions from '../ToastMessages/action'
import { IMessage } from "../../types/IMessage";
import { ToastMessageStatus } from "../../types/Enums";

function* onPersonalInfoRequest({ type, payload }: actionTypes.CreateRequestActionType) {
  try 
  {
    yield put(pageLoaderActions.getPageLoaderRequest(true));
    let res: actionTypes.IState = yield call(createPersonalInfo, payload);
    yield put(actions.getCreateSuccess(res));
  
    let newAppSt = store.getState().newApp;
    newAppSt.IState.stepNo = Steps.PersonalInfo;
    yield store.dispatch(getUpdateRequest(newAppSt.IState)) 
    
    if(payload?.navigationUrl!=undefined && payload?.navigationUrl!=null && newAppSt.IState.applicationNumber!=null && newAppSt.IState.applicationNumber!=undefined)
    yield put(getHistoryPushRequest({history:payload.history,pageName:payload?.navigationUrl }));


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

function* onFetchRequest({ type,payload }: actionTypes.FetchActionType) {
  try 
  {
    yield put(pageLoaderActions.getPageLoaderRequest(true));
    let res: actionTypes.IState = yield call(fetchPersonalInfo, payload);
    yield put(actions.getFetchIncompleteSuccess(res));
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

function* onUpdateRequest({ type,payload }: actionTypes.UpdateActionType) {
  try 
  {
    yield put(pageLoaderActions.getPageLoaderRequest(true));
     let res: actionTypes.IState = yield call(updatePersonalInfo, payload);
     yield put(actions.getUpdateSuccess(res));

     if(payload?.navigationUrl!=undefined && payload?.navigationUrl!=null && res.applicationNumber!=null && res.applicationNumber!=undefined)
    //  yield put(getHistoryPushRequest(payload?.navigationUrl));
     yield put(getHistoryPushRequest({history:payload.history,pageName:payload?.navigationUrl }));

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



function* watchOnPersonalInfo() {
  yield takeEvery(actionTypes.CreateRequest, onPersonalInfoRequest);
  yield takeEvery(actionTypes.FetchRequest, onFetchRequest);
  yield takeEvery(actionTypes.UpdateRequest, onUpdateRequest);
}

export default function* personalInfoSaga() {
  yield all([fork(watchOnPersonalInfo)]);
}