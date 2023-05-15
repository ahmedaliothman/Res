import { all, fork } from "redux-saga/effects";
import {loginSaga} from "./login";
import {lookUpSaga} from "./lookUps";
import {newAppSaga} from "./newApp"
import {personalInfoSaga} from "./personalInfo"
import {passportInfoSaga} from "./passportInfo";
import { ManageRequests } from "./ManageRequests";
import { generalSettingsSaga } from "./GeneralSettings";
import {attachmentInfoSaga} from "./attachmentDocuments"
import {paciSaga} from "./PaciInfo"
import {historySaga} from "./history"
import {pageLoaderSaga} from './PageLoader'

export default function* rootSaga() {
  yield all([fork(loginSaga),
             fork(lookUpSaga),
             fork(pageLoaderSaga),
             fork(generalSettingsSaga),
             fork(newAppSaga),
             fork(personalInfoSaga),
             fork(passportInfoSaga),
             fork(ManageRequests),
             fork(attachmentInfoSaga),
             fork(paciSaga),
             fork(historySaga)
             
            ]);
}
