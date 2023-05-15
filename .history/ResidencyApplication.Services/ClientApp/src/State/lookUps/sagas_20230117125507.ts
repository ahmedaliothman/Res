import { put, call, takeEvery, all, fork } from "redux-saga/effects";
import {SelectOptions} from "../../types/UIRelated"
import * as actionTypes from "./types";
import {getAppTypesSuccess,getNationalitiesSuccess,getOrganizationTypesSuccess,getOrganizationTypesRequest,setLoading,getUserTypesSuccess,getJobTypesSuccess,getJobTypesRequest} from "./action";
import {getAppTypes,getNations,getUserTypes,getJobTypes,getOrganizationTypes} from "../../Services/lookup"
import { pageLoaderActions } from "../PageLoader";



function* onAppTypesRequest() {
    try {
      yield put(pageLoaderActions.getPageLoaderRequest(true));
        const appTypes:SelectOptions[]  = yield call(getAppTypes);
        yield put(getAppTypesSuccess(appTypes));
        yield put(pageLoaderActions.getPageLoaderRequest(false));
    } catch (error) {
      yield put(pageLoaderActions.getPageLoaderRequest(false));
    }
  }

  
function* onJobTypesRequest() {
  try {
      yield put(setLoading(true));
      const appTypes:SelectOptions[]  = yield call(getJobTypes);
      yield put(getJobTypesSuccess(appTypes));
      yield put(setLoading(false));
  } catch (error) {

  }
}


function* onOrganizationTypesRequest() {
  try {
      yield put(setLoading(true));
      const appTypes:SelectOptions[]  = yield call(getOrganizationTypes);
      yield put(getOrganizationTypesSuccess(appTypes));
      yield put(setLoading(false));
  } catch (error) {

  }
}
  
function* onNationalitiesRequest() {
  try {
      yield put(setLoading(true));
      const nationalities:SelectOptions[]  = yield call(getNations);
      yield put(getNationalitiesSuccess(nationalities));
      yield put(setLoading(false));
  } catch (error) {

  }
}

function* onUserTypesRequest() {
  try {
      yield put(setLoading(true));
      const userTypes:SelectOptions[]  = yield call(getUserTypes);
      yield put(getUserTypesSuccess(userTypes));
      yield put(setLoading(false));
  } catch (error) {

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