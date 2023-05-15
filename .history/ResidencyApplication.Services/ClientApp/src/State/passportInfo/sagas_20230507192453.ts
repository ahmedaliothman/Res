import { put, call, takeEvery, all, fork } from "redux-saga/effects";
import * as actionTypes from "./types";
import * as actions from "./action";
import { createPassportInfo,fetchPassportInfo,updatePassportInfo } from "../../Services/passportInfo";
import { getHistoryPushRequest } from "../history/action";
import { pageLoaderActions } from "../PageLoader";
import  * as toastMessagesActions from '../ToastMessages/action'
import { IMessage } from "../../types/IMessage";
import { ToastMessageStatus } from "../../types/Enums";

function* onPassportInfoRequest({ type, payload }: actionTypes.CreateRequestActionType) {
  try {
    yield put(pageLoaderActions.getPageLoaderRequest(true));
    let res: actionTypes.IState = yield call(createPassportInfo, payload);
    yield put(actions.getCreateSuccess(res));
    
    if(payload?.navigationUrl!=undefined && payload?.navigationUrl!=null &&res.applicationNumber!=null && res.applicationNumber!=undefined)
    yield put(getHistoryPushRequest({history:payload.history,pageName:payload?.navigationUrl }));

     


     yield put(pageLoaderActions.getPageLoaderRequest(false));
     
  } catch (error) {
    yield put(pageLoaderActions.getPageLoaderRequest(false));
    let message: IMessage = {};
    message.message = error as unknown as string;
    message.status = ToastMessageStatus.error;
    yield put(toastMessagesActions.getToastMesageRequest(message));
  }
}

function* onFetchRequest({ type,payload }: actionTypes.FetchActionType) {
  try {
    yield put(pageLoaderActions.getPageLoaderRequest(true));
    let res: actionTypes.IState = yield call(fetchPassportInfo, payload);
    yield put(actions.getFetchIncompleteSuccess(res));
    yield put(pageLoaderActions.getPageLoaderRequest(false));
  } catch (error) {
    yield put(pageLoaderActions.getPageLoaderRequest(false));
    let message: IMessage = {};
    message.message = error as unknown as string;
    message.status = ToastMessageStatus.error;
    yield put(toastMessagesActions.getToastMesageRequest(message));
  }
}

function* onUpdateRequest({ type,payload }: actionTypes.UpdateActionType) {
  try {
    yield put(pageLoaderActions.getPageLoaderRequest(true));
     let res: actionTypes.IState = yield call(updatePassportInfo, payload);
     yield put(actions.getUpdateSuccess(res));

     if(payload?.navigationUrl!=undefined && payload?.navigationUrl!=null &&res.applicationNumber!=null && res.applicationNumber!=undefined)
      yield put(getHistoryPushRequest({pageName:payload?.navigationUrl }));



     yield put(pageLoaderActions.getPageLoaderRequest(false));
     
  } catch (error) {
    yield put(pageLoaderActions.getPageLoaderRequest(false));
    let message: IMessage = {};
    message.message = error as unknown as string;
    message.status = ToastMessageStatus.error;
    yield put(toastMessagesActions.getToastMesageRequest(message));
  }
}




function* watchOnPassportInfo() {
  yield takeEvery(actionTypes.CreateRequest, onPassportInfoRequest);
  yield takeEvery(actionTypes.FetchRequest, onFetchRequest);
  yield takeEvery(actionTypes.UpdateRequest, onUpdateRequest);

}

export default function* passportInfoSaga() {
  yield all([fork(watchOnPassportInfo)]);
}