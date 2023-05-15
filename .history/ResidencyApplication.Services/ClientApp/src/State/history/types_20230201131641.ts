

import history from "../../types/exportedHistory";
export const HISTORYPUSH_REQUEST = 'HISTORYPUSH/REQUEST';
export interface historyPush_request_action_type {
    type: typeof HISTORYPUSH_REQUEST
    payload: INavigation
}

export class INavigation {
    module?:string
    pageName?: string;
    history?:any=history;
}


export type HistoryPushActionTypes = 
historyPush_request_action_type
;

