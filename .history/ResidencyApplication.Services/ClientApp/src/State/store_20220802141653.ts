

import { applyMiddleware, createStore,compose } from "redux";
import createSagaMiddleware from "redux-saga";
import {rootReducer} from "./rootReducer";
import rootSaga from "./rootSaga";
// import { routerMiddleware ,routerReducer} from "connected-react-router";
import { composeWithDevTools } from "redux-devtools-extension";
import { LocationState,History } from "history";
import { ConnectedRouter, routerReducer, routerMiddleware, push } from 'react-router-redux'


const sagaMiddleware = createSagaMiddleware();

declare global {
  interface Window {
    __REDUX_DEVTOOLS_EXTENSION_COMPOSE__?: typeof compose;
  }
}

const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;
const configureStore = (history :History<LocationState>) => {
    let middleware = [routerMiddleware(history), sagaMiddleware];
    const store = createStore(rootReducer(history), composeWithDevTools(applyMiddleware(...middleware)));    
    sagaMiddleware.run(rootSaga);
    return store;
  };

  export default configureStore;

