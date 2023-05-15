import { put, call, takeEvery, all, fork } from "redux-saga/effects";
import * as actionTypes from "./types";
import * as actions  from "./action"


function* onToastMessageRequest({ type, payload }: actionTypes.toastMessages_request_action_type) {
  try {
       yield put(actions.getToastMesageSuccess(payload));
       yield put(actions.getToastMesageClear());
      } 
    catch (error) 
   {
   // yield put(actions.getToastMesageSuccess(error as string));
   }
}

function* watchOnPageLoader() {
  yield takeEvery(actionTypes.TOASTMESSAGES_REQUEST, onToastMessageRequest);
}

export default function* authenticationSaga() {
  yield all([fork(watchOnPageLoader)]);
}