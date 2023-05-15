
import { IMessage } from "../../types/IMessage";



export const TOASTMESSAGES_REQUEST = 'TOASTMESSAGES/REQUEST';
export interface toastMessages_request_action_type {
    type: typeof TOASTMESSAGES_REQUEST
    payload: IMessage
}

export const TOASTMESSAGES_SUCCESS = 'TOASTMESSAGES/SUCCESS';
export interface toastMessages_success_action_type {
    type: typeof TOASTMESSAGES_SUCCESS
    payload: IMessage
}
export const TOASTMESSAGES_CLEAR = 'TOASTMESSAGES/CLEAR';
export interface toastMessages_clear_action_type {
    type: typeof TOASTMESSAGES_CLEAR
}


export type toastMessagesActionTypes = 
toastMessages_request_action_type|toastMessages_success_action_type|toastMessages_clear_action_type
;

