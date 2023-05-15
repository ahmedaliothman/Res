import { put, call, takeEvery, all, fork } from "redux-saga/effects";
import * as services from "../../Services/GeneralSettings"
import * as Types from "./types"
import * as actionTypes from "./action"
export interface ResponseObject {
    Status?:number;
    Message?:String;
    HasError?:boolean;
    response?:any;
}
function* onGenralSettingsRequest() {
    try {
        let res:ResponseObject  =yield call(services.getGeneralSettings);
        yield put(actionTypes.getGeneralSettingsSuccess(res?.response))
    }
    catch
    {

    }
}

function* watchOnGenralSettings() {
    yield   takeEvery(Types.GENERALSETTING_REQUEST, onGenralSettingsRequest);
}




export default function* generalSettingsSaga() {
    yield all([fork(watchOnGenralSettings)]);
}


