import { useHistory } from "react-router-dom"
import { INavigation } from "../../types/INavigation"
import * as HistoryPushActionActions from "./types"


export const getHistoryPushRequest = (input:INavigation): HistoryPushActionActions.historyPush_request_action_type => {
    return {
        type: HistoryPushActionActions.HISTORYPUSH_REQUEST,
        payload: input
    }
}





