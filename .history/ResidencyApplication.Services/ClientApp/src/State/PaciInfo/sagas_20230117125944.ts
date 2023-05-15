import { put, call, takeEvery, all, fork } from "redux-saga/effects";
import * as services from "../../Services/paciService"
import * as Types from "./types"
import * as actionTypes from "./action"
import { setLoading } from "../lookUps";
import { pageLoaderActions } from "../PageLoader";
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
    catch
    {
        yield put(pageLoaderActions.getPageLoaderRequest(false));
    }
}

function* watchOnPaci() {
    yield   takeEvery(Types.PACI_REQUEST, onPaciRequest);
}




export default function* paciSaga() {
    yield all([fork(watchOnPaci)]);
}


