import { put, call, takeEvery, all, fork } from "redux-saga/effects";
import { AuthenticateResponse } from "../../types/userInfo";
import * as actionTypes from "./types";
import { getloginSuccess, USERUPDATEINFOSUCCESS, selectUserSuccess } from "./action"
import { setLoading } from "../lookUps/action";
import { login } from "../../Services/login";
import { getUpdateUserInfo } from "../../Services/userProfile";
import * as Types from '../../types'
import {getHistoryRequest} from "../../State/history/action"
import { routerReducer, routerMiddleware ,push} from 'react-router-redux';
import { pageLoaderActions } from "../PageLoader";
import  * as toastMessagesActions from '../ToastMessages/action'
import { IMessage } from "../../types/IMessage";
import { ToastMessageStatus } from "../../types/Enums";


function* onLoginRequest({ type, payload }: actionTypes.login_request_action_type) {
  const { username, password } = payload;

  try {
    //yield put(setLoading(true));
    yield put(pageLoaderActions.getPageLoaderRequest(true));
    const loginRes: AuthenticateResponse = yield call(login, username, password);
    if (!loginRes.hasError) {
      loginRes.isLoggedIn = true;
      localStorage.setItem("user", JSON.stringify(loginRes));
      let tempState: actionTypes.AuthState = {
        isLoading: false,
        jwtToken: loginRes.response.jwtToken,
        userInfo: loginRes.response.userInfo,
        isLoggedIn: true,
        hasError: false,
        message: ""
      }
      yield put(getloginSuccess(tempState));
      if(tempState.hasError==false)
      {
        yield put(getHistoryRequest("/common/Redirection"));
      }
      

    }
    else {
      loginRes.isLoggedIn = false;
      // localStorage.setItem("user", JSON.stringify(loginRes));
      let tempState: actionTypes.AuthState = {
        isLoading: false,
        jwtToken: undefined,
        userInfo: undefined,
        isLoggedIn: false,
        hasError: loginRes.hasError,
        message: loginRes.message
      }
      yield put(getloginSuccess(tempState));

    }
    yield put(pageLoaderActions.getPageLoaderRequest(false));


  } catch (error) {
    yield put(pageLoaderActions.getPageLoaderRequest(false));
    let message: IMessage = {};
    message.message = error as unknown as string;
    message.status = ToastMessageStatus.error;
    yield put(toastMessagesActions.getToastMesageRequest(message));
  }
}


function* USERUPDATEINFO({ type, payload }: actionTypes.UserUpdateInfo_action_type) {
  try {

    const userInfoResponse: Types.IResponseObject = yield call(getUpdateUserInfo, payload);
    yield put(USERUPDATEINFOSUCCESS(userInfoResponse.response));
  }
  catch (error) {

  }



}

function* selectUser({ type, payload }: actionTypes.login_select_user_action_type) {

  try 
  {
    yield put(selectUserSuccess(payload))
  }
  catch (error) {

  }

}



function* watchOnLgin() {
  yield takeEvery(actionTypes.LOGIN_REQUEST, onLoginRequest);
  yield takeEvery(actionTypes.USERUPDATEINFO, USERUPDATEINFO);
  yield takeEvery(actionTypes.SELECTUSER, selectUser);
}

export default function* loginSaga() {
  yield all([fork(watchOnLgin)]);
}