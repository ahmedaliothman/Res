import { put, call, takeEvery, all, fork } from "redux-saga/effects";
import * as actionTypes from "./types";
import * as actions from "./action";
import { createAttachemnt, fetchAttachemnt, updateAttachment } from "../../Services/fileAttachment";
import { getHistoryPushRequest } from "../history/action";
import { pageLoaderActions } from "../PageLoader";
import { ToastMessageStatus } from "../../types/Enums";
import { IMessage } from "../../types/IMessage";
import  * as toastMessagesActions from '../ToastMessages/action'

function* onCreateRequest({ type, payload }: actionTypes.CreateRequestActionType) {
  try {
    yield put(pageLoaderActions.getPageLoaderRequest(true));
    let res: actionTypes.IState = yield call(createAttachemnt, payload);
    yield put(actions.getCreateSuccess(res));

    if (payload?.navigationUrl != undefined && payload?.navigationUrl != null && res.applicationNumber!=null && res.applicationNumber!=undefined) 
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
function* onUpdateRequest({ type, payload }: actionTypes.UpdateActionType) {
  try {
    yield put(pageLoaderActions.getPageLoaderRequest(true));
    let res: actionTypes.IState = yield call(updateAttachment, payload);
    yield put(actions.getUpdateSuccess(res));

    if (payload?.navigationUrl != undefined && payload?.navigationUrl != null && res.applicationNumber!=null && res.applicationNumber!=undefined) 
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

function* onFetchRequest({ type, payload }: actionTypes.FetchActionType) {
  try {
    yield put(pageLoaderActions.getPageLoaderRequest(true));
    let res: actionTypes.IState = yield call(fetchAttachemnt, payload);
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





function* watchOnAttachmentInfo() {
  yield takeEvery(actionTypes.CreateRequest, onCreateRequest);
  yield takeEvery(actionTypes.FetchRequest, onFetchRequest);
  yield takeEvery(actionTypes.UpdateRequest, onUpdateRequest);

}

export default function* attachmentInfoSaga() {
  yield all([fork(watchOnAttachmentInfo)]);
}