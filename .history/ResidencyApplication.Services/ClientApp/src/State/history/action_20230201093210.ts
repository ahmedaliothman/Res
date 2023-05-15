import { useHistory } from "react-router-dom"
import * as HistoryPushActionActions from './types'


export const getHistoryPushRequest =
 (input:HistoryPushActionActions.INavigation): HistoryPushActionActions.historyPush_request_action_type => {
    return {
        type: HistoryPushActionActions.HISTORYPUSH_REQUEST,
        payload: input
    }
}





