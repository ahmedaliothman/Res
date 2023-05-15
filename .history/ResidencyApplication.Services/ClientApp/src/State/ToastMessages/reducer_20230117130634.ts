import * as types from './types';
import * as actions from './action';
import { IMessage } from '../../types/IMessage';
import {ToastMessageStatus} from './../../types/Enums'

const initialState: IMessage = {
       message: "",
       status:ToastMessageStatus.idel
}

export function authenticationReducer(state: IMessage = initialState, action: types.toastMessagesActionTypes): IMessage {

  switch (action.type) {
    case types.TOASTMESSAGES_SUCCESS:
      return {
        message: action.payload.message,
        status: action.payload.status

      }
      case types.TOASTMESSAGES_CLEAR:
        return  initialState;
    default:


      return state;

  }
}

export default authenticationReducer;