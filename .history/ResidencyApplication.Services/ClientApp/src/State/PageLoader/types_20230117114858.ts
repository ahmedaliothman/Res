


export const PAGELOADER_REQUEST = 'PAGELOADER/REQUEST';
export interface PageLoader_request_action_type {
    type: typeof PAGELOADER_REQUEST
    payload: boolean
}

export const PAGELOADER_SUCCESS = 'PAGELOADER/SUCCESS';
export interface PageLoader_success_action_type {
    type: typeof PAGELOADER_SUCCESS
    payload: boolean
}


export type PageLoaderActionTypes = 
PageLoader_request_action_type|PageLoader_success_action_type
;

    