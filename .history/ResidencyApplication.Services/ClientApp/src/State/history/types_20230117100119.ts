
import { INavigation } from "../../types/INavigation";



export const HISTORYPUSH_REQUEST = 'HISTORYPUSH/REQUEST';
export interface historyPush_request_action_type {
    type: typeof HISTORYPUSH_REQUEST
    payload: INavigation
}



export  type HistoryPushActionTypes = 
historyPush_request_action_type
;

