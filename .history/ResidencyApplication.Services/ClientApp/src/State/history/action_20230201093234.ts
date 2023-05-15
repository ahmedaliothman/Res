

export const getHistoryPushRequest =
 (input:HistoryPushActionActions.INavigation): HistoryPushActionActions.historyPush_request_action_type => {
    return {
        type: HistoryPushActionActions.HISTORYPUSH_REQUEST,
        payload: input
    }
}





