import React, {useEffect,useState} from 'react';
import Layout from "../../../Layout/ClientSec/Layout";
import Select from 'react-select';
import { useSelector, useDispatch } from 'react-redux';
import {RootState} from '../../../State/rootReducer';
import {getAppTypesRequest} from "../../../State/lookUps";
import {SelectOptions} from '../../../types/UIRelated';
import { useHistory } from "react-router-dom";
import {getFetchIncompleteRequest} from "../../../State/newApp";
import {getFetchRequest} from "../../../State/personalInfo";
import {Steps} from "../../../types/Enums"
import {INewAppState} from '../../../types/newApp'
import {ClearRequest} from "../../../State/newApp"
import {RequestClear as RequestClearPersonalInfo} from "../../../State/personalInfo"
import {RequestClear as RequestClearPassportInfo} from "../../../State/passportInfo"
import {RequestClear as RequestClearattachmentDocuments} from "../../../State/attachmentDocuments"




const NewApp: React.FunctionComponent<INewAppState> =() => {
	const LookUpState = useSelector<RootState,RootState["lookUp"]>(state => state.lookUp);
  const newAppState = useSelector<RootState,RootState["newApp"]>(state => state.newApp);
  const userData = useSelector<RootState,RootState["login"]>(state => state.login);
  const [appType, setappType] = useState<SelectOptions|undefined>({label:"",value:""});
  const [IsDisabled, setIsDisabled] = useState<boolean>(true);
  const history = useHistory();
  const {AppTypes} = LookUpState;
  let dispatch = useDispatch();
  const [Direction, setDirection] = useState<string>("");


  useEffect(() => {
    const GetDropdownValues = async () => {
      if (AppTypes === undefined) {
        dispatch(getAppTypesRequest());
      }
    };
    GetDropdownValues();
  }, []);

  useEffect(() => {
   let selectedObj = AppTypes?.find(a=>a.value===newAppState.IState?.applicationTypeId?.toString());
     setappType(selectedObj);
    return () => {
    }
  }, [newAppState.IState?.applicationTypeId,AppTypes]);

  

	const handleSubmit = async (e: React.SyntheticEvent) => {
		 e.preventDefault();
    
    if(Direction=="fwd")
    {
      dispatch(getFetchRequest(newAppState.IState?.applicationNumber as number));
      history.push("/Client/OnlyView/PersonalInfo");
    }
    else if(Direction=="bwd")
    {
      dispatch(ClearRequest());
      dispatch(RequestClearPersonalInfo());
      dispatch(RequestClearPassportInfo());
      dispatch(RequestClearattachmentDocuments());
      history.push("/Client/ManageRequests");
    }
	}

  
    return (
                  <main className="login-bg">
        <div className="container" style={{marginBottom: '80px'}}>
          {/* Outer Row */}
          <div className="row justify-content-center">
            <div className="col-xl-10 col-lg-12 col-md-9">
              <div className="card o-hidden border-1 shadow my-5 animate__animated animate__backInDown " style={{border: '1px solid rgb(189, 189, 189)'}}>
                <div className="card-header bg-dark">
                  <div className="text-center">
                    <h1 className=" text-gray-900 mb-1 shorooq gold" style={{fontSize: '28px'}}>نوع المعاملة</h1>
                  </div>
                </div>
                <div className="card-body p-0 ">
                  {/* Nested Row within Card Body */}
                  <div className="row">
                    <div className="col-lg-12">
                      <div className="p-5">
                        <form className="user" onSubmit={handleSubmit}>
                          <div className="form-group">
                            <input type="email" disabled={IsDisabled} className="form-control form-control-user" id="exampleInputEmail" aria-describedby="emailHelp" placeholder="ادخل رقم الموظف " />
                          </div>
                          <div className="form-group">
                           
                            <Select 
                             name="AppType" 
                             placeholder=" Choose App Type "
                             options={AppTypes}
                             value= {appType}
                             isDisabled={IsDisabled}
                              />
                          </div>
                       
                          <div className="row justify-content-between">
                             <button onClick={()=> {setDirection("bwd");  }} type="submit" className="btn btn-primary btn-user shorooq  " style={{fontSize: '22px'}}>
                              قائمة المعاملاتى
                            </button>
                            <button onClick={()=> {setDirection("fwd");  }} type="submit" className="btn btn-primary btn-user shorooq  " style={{fontSize: '22px'}}>
                              التالي
                            </button >
                          </div>
                        </form>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div></main>
   
            
        
    )
}

export default NewApp
