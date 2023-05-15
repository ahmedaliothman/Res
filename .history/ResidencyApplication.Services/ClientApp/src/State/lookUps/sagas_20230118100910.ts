import { put, call, takeEvery, all, fork } from "redux-saga/effects";
import {SelectOptions} from "../../types/UIRelated"
import * as actionTypes from "./types";
import {getAppTypesSuccess,getNationalitiesSuccess,getOrganizationTypesSuccess,getOrganizationTypesRequest,setLoading,getUserTypesSuccess,getJobTypesSuccess,getJobTypesRequest} from "./action";
import {getAppTypes,getNations,getUserTypes,getJobTypes,getOrganizationTypes} from "../../Services/lookup"
import { pageLoaderActions } from "../PageLoader";
import  * as toastMessagesActions from '../ToastMessages/action'
import { IMessage } from "../../types/IMessage";
import { ToastMessageStatus } from "../../types/Enums";


function* onAppTypesRequest() {
    try {
      yield put(pageLoaderActions.getPageLoaderRequest(true));
        const appTypes:SelectOptions[]  = yield call(getAppTypes);
        yield put(getAppTypesSuccess(appTypes));
        yield put(pageLoaderActions.getPageLoaderRequest(false));
    } catch (error) {
      yield put(pageLoaderActions.getPageLoaderRequest(false));
    let message: IMessage = {};
    message.message = error as unknown as string;
    message.status = ToastMessageStatus.error;
    yield put(toastMessagesActions.getToastMesageRequest(message));
    }
  }

  
function* onJobTypesRequest() {
  try {
    yield put(pageLoaderActions.getPageLoaderRequest(true));
      const appTypes:SelectOptions[]  = yield call(getJobTypes);
      yield put(getJobTypesSuccess(appTypes));
      yield put(pageLoaderActions.getPageLoaderRequest(false));
  } catch (error) {
    yield put(pageLoaderActions.getPageLoaderRequest(false));
    let message: IMessage = {};
    message.message = error as unknown as string;
    message.status = ToastMessageStatus.error;
    yield put(toastMessagesActions.getToastMesageRequest(message));
  }
}


function* onOrganizationTypesRequest() {
  try {
    yield put(pageLoaderActions.getPageLoaderRequest(true));
      const appTypes:SelectOptions[]  = yield call(getOrganizationTypes);
      yield put(getOrganizationTypesSuccess(appTypes));
      yield put(pageLoaderActions.getPageLoaderRequest(false));
  } catch (error) {
    yield put(pageLoaderActions.getPageLoaderRequest(false));
  }
}
  
function* onNationalitiesRequest() {
  try {
    yield put(pageLoaderActions.getPageLoaderRequest(true));
      const nationalities:SelectOptions[]  = yield call(getNations);
      yield put(getNationalitiesSuccess(nationalities));
      yield put(pageLoaderActions.getPageLoaderRequest(false));
  } catch (error) {
    yield put(pageLoaderActions.getPageLoaderRequest(false));
  }
}

function* onUserTypesRequest() {
  try {
    yield put(pageLoaderActions.getPageLoaderRequest(true));
      const userTypes:SelectOptions[]  = yield call(getUserTypes);
      yield put(getUserTypesSuccess(userTypes));
      yield put(pageLoaderActions.getPageLoaderRequest(false));
  } catch (error) {
    yield put(pageLoaderActions.getPageLoaderRequest(false));
  }
}

function* watchOnLookUps() {
    yield takeEvery(actionTypes.AppTypes_request, onAppTypesRequest);
    yield takeEvery(actionTypes.Nationalities_request, onNationalitiesRequest);
    yield takeEvery(actionTypes.UserTypes_request, onUserTypesRequest);
    yield takeEvery(actionTypes.jobTypes_request, onJobTypesRequest);
    yield takeEvery(actionTypes.organizationTypes_request, onOrganizationTypesRequest);
  }

  export default function* lookUpSaga() {
    yield all([fork(watchOnLookUps)]);
  }