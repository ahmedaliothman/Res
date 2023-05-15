import { put, call, takeEvery, all, fork } from "redux-saga/effects";
import * as services from "../../Services/paciService"
import * as Types from "./types"
import * as actionTypes from "./action"
import { setLoading } from "../lookUps";
export interface ResponseObject<T> {
    Status?:number;
    Message?:String;
    HasError?:boolean;
    response?:T;
}
function* onPaciRequest({type,payload}:Types.paciRequestActionType) {
    try {
        yield put(setLoading(true));
        let res:services.paciServiceObj =yield call(services.getPaciServiceInfo,payload);
        yield put(actionTypes.getPaciSuccess(res));
        yield put(setLoading(false));
    }
    catch
    {
        yield put(setLoading(false));
    }
}

function* watchOnPaci() {
    yield   takeEvery(Types.PACI_REQUEST, onPaciRequest);
}




export default function* paciSaga() {
    yield all([fork(watchOnPaci)]);
}


