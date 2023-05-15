import { put, call, takeEvery, all, fork } from "redux-saga/effects";
import * as services from "../../Services/paciService"
import * as Types from "./types"
import * as actionTypes from "./action"
import { setLoading } from "../lookUps";
import { pageLoaderActions } from "../PageLoader";
import  * as toastMessagesActions from '../ToastMessages/action'
import { IMessage } from "../../types/IMessage";
import { ToastMessageStatus } from "../../types/Enums";
export interface ResponseObject<T> {
    Status?:number;
    Message?:String;
    HasError?:boolean;
    response?:T;
}
function* onPaciRequest({type,payload}:Types.paciRequestActionType) {
    try {
        yield put(pageLoaderActions.getPageLoaderRequest(true));
        let res:services.paciServiceObj =yield call(services.getPaciServiceInfo,payload);
        yield put(actionTypes.getPaciSuccess(res));
        yield put(pageLoaderActions.getPageLoaderRequest(false));
    }
    catch(error) {
        yield put(pageLoaderActions.getPageLoaderRequest(false));
        let message: IMessage = {};
        message.message = error as unknown as string;
        message.status = ToastMessageStatus.error;
        yield put(toastMessagesActions.getToastMesageRequest(message));
        
    }
}

function* watchOnPaci() {
    yield   takeEvery(Types.PACI_REQUEST, onPaciRequest);
}




export default function* paciSaga() {
    yield all([fork(watchOnPaci)]);
}


