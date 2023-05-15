import { put, call, takeEvery, all, fork } from "redux-saga/effects";
import * as actionTypes from "./types";


function* onHistoryPushRequest({ type, payload }: actionTypes.historyPush_request_action_type) {
  try {
    console.log("before push "+ payload );
    const { history ,module,pageName}  = payload ;
    history.push(pageName);
    console.log("after push "+ payload );

  } 
  catch (error) 
  {
  
  }
}

function* watchOnHistory() {
  yield takeEvery(actionTypes.HISTORYPUSH_REQUEST, onHistoryPushRequest);
}

export default function* historySaga() {
  yield all([fork(watchOnHistory)]);
}