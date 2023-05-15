import { put, call, takeEvery, all, fork } from "redux-saga/effects";
import * as services from "../../Services/GeneralSettings"
import * as Types from "./types"
import * as actionTypes from "./action"
import { pageLoaderActions } from "../PageLoader";
import  * as toastMessagesActions from '../ToastMessages/action'
import { IMessage } from "../../types/IMessage";
import { ToastMessageStatus } from "../../types/Enums";

export interface ResponseObject {
    Status?:number;
    Message?:String;
    HasError?:boolean;
    response?:any;
}
function* onGenralSettingsRequest() {
    try {
        yield put(pageLoaderActions.getPageLoaderRequest(true));
        let res:ResponseObject  =yield call(services.getGeneralSettings);
        yield put(actionTypes.getGeneralSettingsSuccess(res?.response));
        yield put(pageLoaderActions.getPageLoaderRequest(false));
    }
    catch(error)
    {
        yield put(pageLoaderActions.getPageLoaderRequest(false));
        let message: IMessage = {};
        message.message = error as unknown as string;
        message.status = ToastMessageStatus.error;
        yield put(toastMessagesActions.getToastMesageRequest(message));
    }
}

function* watchOnGenralSettings() {
    yield   takeEvery(Types.GENERALSETTING_REQUEST, onGenralSettingsRequest);
}




export default function* generalSettingsSaga() {
    yield all([fork(watchOnGenralSettings)]);
}


