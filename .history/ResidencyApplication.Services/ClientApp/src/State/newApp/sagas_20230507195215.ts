import { put, call, takeEvery, all, fork } from "redux-saga/effects";
import * as actionTypes from "./types";
import * as actions from "./action";
import { setLoading } from "../lookUps/action";
import { createNewApp, fetchIncompleteNewApp, updateNewApp, getAppRequest } from "../../Services/newApp";
import { IResponseObject } from "../../types/ResponseObject";
import { push, getLocation, RouterState } from 'connected-react-router';
import { store } from "../../index"
import { pageLoaderActions } from "../PageLoader";
import  * as toastMessagesActions from '../ToastMessages/action'
import { IMessage } from "../../types/IMessage";
import { ToastMessageStatus } from "../../types/Enums";
import { getHistoryPushRequest } from "../history/action";
import history from "../../types/exportedHistory";
function* onNewAppRequest({ type, payload }: actionTypes.CreateRequestActionType) {
  try {
  
    yield put(pageLoaderActions.getPageLoaderRequest(true));
    let res: actionTypes.IStateApp = yield call(createNewApp, payload);
    yield put(actions.getCreateSuccess(res));
    if (payload?.navigationUrl != undefined && payload?.navigationUrl != null && res.hasError == false)
      yield put(getHistoryPushRequest({history:history,pageName:payload?.navigationUrl }));

    yield put(pageLoaderActions.getPageLoaderRequest(false));
  }
  catch (error) {
    yield put(pageLoaderActions.getPageLoaderRequest(false));
    let message: IMessage = {};
    message.message = error as unknown as string;
    message.status = ToastMessageStatus.error;
    yield put(toastMessagesActions.getToastMesageRequest(message));
  }
}

function* onIncompleteAppRequest({ type, payload }: actionTypes.FetchIncompleteActionType) {
  try {
    yield put(pageLoaderActions.getPageLoaderRequest(true));

    let res: actionTypes.IStateApp = yield call(fetchIncompleteNewApp, payload);
    yield put(actions.getFetchIncompleteSuccess(res));
    yield put(pageLoaderActions.getPageLoaderRequest(false));

  }
  catch (error) {
    yield put(pageLoaderActions.getPageLoaderRequest(false));
    let message: IMessage = {};
    message.message = error as unknown as string;
    message.status = ToastMessageStatus.error;
    yield put(toastMessagesActions.getToastMesageRequest(message));
  }
}

function* onUpdateAppRequest({ type, payload }: actionTypes.UpdateActionType) {
  try {

    yield put(pageLoaderActions.getPageLoaderRequest(true));
    let res: actionTypes.IStateApp = yield call(updateNewApp, payload);
    yield put(actions.getUpdateSuccess(res));
    if ((store.getState().router.location?.pathname == "/" || store.getState().router.location?.pathname.toLowerCase() == "/newapp") && res.hasError == false) {

    }
    alert();
console.log(payload)
    if (payload?.navigationUrl != undefined && payload?.navigationUrl != null && res.hasError == false)
    {
      alert("inside");
      yield put(getHistoryPushRequest({history:payload.history,pageName:payload?.navigationUrl }));


    }


    if (store.getState().router.location?.pathname.toLowerCase() == "/client/result") {
      yield put(actions.ClearRequest());
    }

      yield put(pageLoaderActions.getPageLoaderRequest(false));

  }
  catch (error) {
    yield put(pageLoaderActions.getPageLoaderRequest(false));
    let message: IMessage = {};
    message.message = error as unknown as string;
    message.status = ToastMessageStatus.error;
    yield put(toastMessagesActions.getToastMesageRequest(message));
  }
}


function* onGetAppRequest({ type, payload }: actionTypes.GetRequestActionType) {
  try {
    yield put(pageLoaderActions.getPageLoaderRequest(true));

    let res: actionTypes.IStateApp = yield call(getAppRequest, payload);
    yield put(actions.getUpdateSuccess(res));
    yield put(pageLoaderActions.getPageLoaderRequest(false));

  }
  catch (error) {
    yield put(pageLoaderActions.getPageLoaderRequest(false));
    let message: IMessage = {};
    message.message = error as unknown as string;
    message.status = ToastMessageStatus.error;
    yield put(toastMessagesActions.getToastMesageRequest(message));
  }
}


function* onLoadingAppRequest({ type, payload }: actionTypes.GetRequestActionType) {
  try {
    yield put(actions.loadingRequest(true));
  }
  catch (error) {
    yield put(actions.loadingRequest(false));
  }
}


function* onClearAppRequest({ type }: actionTypes.ClearRequstActionType) {
  try {
    yield put(actions.ClearRequest());
  }
  catch (error) {
    yield put(actions.loadingRequest(false));
  }
}

function* watchOnNewApp() {
  yield takeEvery(actionTypes.CreateRequest, onNewAppRequest);
  yield takeEvery(actionTypes.IncompleteFetchRequest, onIncompleteAppRequest);
  yield takeEvery(actionTypes.UpdateRequest, onUpdateAppRequest);
  yield takeEvery(actionTypes.GetRequest, onGetAppRequest);
  yield takeEvery(actionTypes.RequestLoading, onLoadingAppRequest);
  yield takeEvery(actionTypes.ClearRequst, onClearAppRequest);
}

export default function* newAppSaga() {
  yield all([fork(watchOnNewApp)]);
}