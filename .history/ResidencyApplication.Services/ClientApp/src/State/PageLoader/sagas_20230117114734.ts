import { put, call, takeEvery, all, fork } from "redux-saga/effects";
import { IResponse } from "../../ITypes/Loaded/IResponse";
import * as actionTypes from "./types";
import * as actions  from "./action"
import * as services from "../../Services/Index";
import IState from '../../ITypes/Loaded/IState'


function* onPageLoaderRequest({ type, payload }: actionTypes.PageLoader_request_action_type) {
  try {
  yield put(actions.getPageLoaderSuccess(payload));
  } 
  catch (error) 
  {
    yield put(actions.getPageLoaderSuccess(false));
  
  }
}

function* watchOnPageLoader() {
  yield takeEvery(actionTypes.PAGELOADER_REQUEST, onPageLoaderRequest);
}

export default function* authenticationSaga() {
  yield all([fork(watchOnPageLoader)]);
}