import { IPageLoader } from '../../types/IPageLoader';
import * as types from './types';
import {getPageLoaderRequest,getPageLoaderSuccess} from './action';

const initialState: IPageLoader = {
status:false
}

export function authenticationReducer(state: IPageLoader = initialState, action:types.PageLoaderActionTypes): IPageLoader {

  switch (action.type) {
    case types.PAGELOADER_SUCCESS:
      return {
        status:action.payload
      }

    default:
      

      return state;

  }
}

export default authenticationReducer;