import { IMessage } from "../../types/IMessage"
import * as toastMessagesActions from "./types"


export const getToastMesageRequest = (input:IMessage): toastMessagesActions.toastMessages_request_action_type => {
    return {
        type: toastMessagesActions.TOASTMESSAGES_REQUEST,
        payload: input
    }
}


export const getToastMesageSuccess = (input:IMessage): toastMessagesActions.toastMessages_success_action_type => {
    return {
        type: toastMessagesActions.TOASTMESSAGES_SUCCESS,
        payload: input
    }
}


export const getToastMesageClear = (): toastMessagesActions.toastMessages_clear_action_type => {
    return {
        type: toastMessagesActions.TOASTMESSAGES_CLEAR
    }
}


