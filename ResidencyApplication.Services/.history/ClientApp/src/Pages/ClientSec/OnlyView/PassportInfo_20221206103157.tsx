import React,{useState,useEffect} from 'react'
import Layout from "../../../Layout/ClientSec/Layout";
import ReactDatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import Select from 'react-select';
import { useForm, Controller } from "react-hook-form";
import {IState} from "../../../State/passportInfo";
import {SelectOptions} from "../../../types/UIRelated"
import { useSelector,useDispatch } from "react-redux";
import { RootState } from "../../../State/rootReducer";
import { assignToType} from "../../../Services/utils/assignType";
import {getCreateRequest,getFetchRequest,getUpdateRequest as updatePassportInfo} from "../../../State/passportInfo";
import {getNationalitiesRequest} from "../../../State/lookUps";
import { getFetchIncompleteRequest,getUpdateRequest } from "../../../State/newApp";
import {Steps} from "../../../types/Enums"

import { useHistory } from "react-router-dom";
import {getFetchRequest as getFetchRequestPersonalInfo} from "../../../State/personalInfo";
import {getFetchRequest as getFetchRequestFileAttachmentInfo} from "../../../State/attachmentDocuments";


export interface IFormData extends IState {
  selectedNationality?:SelectOptions,
  selectedIssueCountry?:SelectOptions,
  employeeNumber?:number
  }


class TempClass implements IState{
  id?:number ;
  civilID?:number;  
  nationalityId?:string;
  passportNumber?:number;
  issueCountry?:string;
  issueDate?:Date;
  expiryDate?:Date;
  address?:string; 
  residencyExpiryDate?:Date; 
  applicationNumber?:number;    
  userId?:number;    
  createdDate?:Date; 
  updatedDate?:Date;

}
const PassportInfo = () => {


  const history = useHistory();
  const LookUpState = useSelector<RootState,RootState["lookUp"]>(state => state.lookUp);
  const newAppState = useSelector<RootState, RootState["newApp"]>(state => state.newApp);
  const loginData = useSelector<RootState, RootState["login"]>(state => state.login);
  const stateData = useSelector<RootState,RootState["passportInfo"]>(state => state.passportInfo);
  const {Nationalities} = LookUpState;
  const { userInfo } = loginData;
  const [IsDisabled, setIsDisabled] = useState<boolean>(true);
  const [Direction, setDirection] = useState<string>("");

  let dispatch = useDispatch();
  const { register, handleSubmit, watch, errors,setValue, getValues,control } = useForm<IFormData>({
    defaultValues:{
      userId : userInfo?.userId,
      civilID: userInfo?.civilId?Number(userInfo?.civilId):undefined,
      employeeNumber: userInfo?.employeeNumber ? Number(userInfo?.employeeNumber) : undefined
      
    }
  });
  useEffect(() => {
    const GetDropdownValues = async () => {
      if (Nationalities === undefined) {
        dispatch(getNationalitiesRequest());
      }
    };
    GetDropdownValues();

  }, []);

  useEffect(() => {
    if (newAppState.IState.applicationNumber === undefined) {
       dispatch(getFetchIncompleteRequest(userInfo?.userId as number));
    }
  }, []);

  useEffect(() => {
    if (newAppState.IState.stepNo as number  >= Steps.PassportInfo) {
       dispatch(getFetchRequest(newAppState.IState.applicationNumber as number));
   }
  }, [newAppState.IState.stepNo]);


  useEffect(() => {

    setValue("passportNumber", stateData.passportNumber);
    stateData?.issueDate!==undefined && setValue("issueDate",new Date(stateData.issueDate as Date) );
    stateData?.expiryDate!== undefined&& setValue("expiryDate", new Date(stateData.expiryDate as Date));
    stateData.residencyExpiryDate!== undefined&& setValue("residencyExpiryDate", new Date(stateData.residencyExpiryDate as Date));
     setValue("address", stateData.address);
    
    if(Nationalities!==undefined ) 
    {
      setValue("selectedNationality", (Nationalities as SelectOptions[]).find(j => j.value === stateData.nationalityId)?.label);
      setValue("selectedIssueCountry", (Nationalities as SelectOptions[]).find(j => j.value === stateData.issueCountry)?.label);
    
    }
    
    
  }, [stateData,Nationalities]);

  const onSubmit = async (data:IFormData) => {
  
  if(Direction=="fwd")
  {
     dispatch(getFetchRequestFileAttachmentInfo(newAppState.IState?.applicationNumber as number));
     history.push("/Client/OnlyView/FileAttachments");
  }
  else if(Direction=="bwd")
  {
    dispatch(getFetchRequestPersonalInfo(newAppState.IState?.applicationNumber as number));
    history.push("/Client/OnlyView/PassportInfo");
  }
  }
    return (
        <main className="login-bg">
        <div className="container" style={{marginBottom: '80px'}}>
          {/* Outer Row */}
          <div className="row justify-content-center">
            <div className="col-xl-12 col-lg-12 col-md-12">
              <div className="card o-hidden border-1 shadow my-5 animate__animated animate__backInDown " style={{border: '1px solid rgb(189, 189, 189)'}}>
                <div className="card-header bg-dark">
                  <div className="text-center">
                    <h1 className=" text-gray-900 mb-1 shorooq gold" style={{fontSize: '28px'}}> بيانات
                      الجواز والاقامة</h1>
                  </div>
                </div>
                <div className="card-body p-0 ">
                  {/* Nested Row within Card Body */}
                  <div className="row">
                    <div className="col-lg-12">
                      <div className="p-5">
                        <form className="user" onSubmit={handleSubmit(onSubmit)} >
                          <div className="form-group row">
                             <label  className="col-sm-3 col-form-label">رقم الموظف</label>
                            <div className="col-sm-3">
                              <input type="text" name="employeeNumber"  ref={register} disabled={IsDisabled} className="form-control form-control-user" />
                            </div> 
                            <label  className="col-sm-3 col-form-label">الجنسية</label>
                            <div className="col-sm-3"  >
                               
                              <input type="text" name="selectedNationality" disabled={IsDisabled} ref={register}  className="form-control form-control-user" />

                            </div>
                          </div>
                          {/* ################### form- row-003 #################*/}
                          <div className="form-group row">
                            <label  className="col-sm-3 col-form-label">رقم الجواز</label>
                            <div className="col-sm-3">
                              <input type="text" disabled={IsDisabled} className="form-control form-control-user"  
                              name="passportNumber" ref={register} />
                            </div>
                            <label  className="col-sm-3 col-form-label">مكان الاصدار</label>
                            <div className="col-sm-3" >
                            <input type="text" name="selectedIssueCountry" disabled={IsDisabled} ref={register}  className="form-control form-control-user" />
                            </div>
                          </div>
                          {/* ################### form- row-004 #################*/}
                          <div className="form-group row">
                            <label  className="col-sm-3 col-form-label">تاريخ الاصدار</label>
                            <div className="col-sm-3">
                            <Controller
                                  control={control}
                                  name="issueDate"
                                  render={({ onChange, onBlur, value }) => (
                                    <ReactDatePicker
                                      onChange={onChange}
                                      onBlur={onBlur}
                                      selected={value}
                                      dateFormat="dd/MM/yyyy"
                                      placeholderText="dd/MM/yyyy "   
                                      className="form-control form-control-user"
                                      disabled                                   
                                       />
                                  )}
                                />

                            </div>
                            <label  className="col-sm-3 col-form-label">تاريخ الانتهاء</label>
                            <div className="col-sm-3" >
                            <Controller
                                  control={control}
                                  name="expiryDate"
                                  render={({ onChange, onBlur, value }) => (
                                    <ReactDatePicker
                                      onChange={onChange}
                                      onBlur={onBlur}
                                      selected={value}
                                      dateFormat="dd/MM/yyyy"
                                      placeholderText="dd/MM/yyyy "   
                                      disabled
                                      className="form-control form-control-user"
                                    />
                                  )}
                                />
                            </div>
                           
                          </div>
                          {/* ################### form- row-005 #################*/}
                          <div className="form-group row">
                            <label  className="col-sm-3 col-form-label">عنوان السكن</label>
                            <div className="col-sm-3">
                              <input type="text"  disabled={IsDisabled}  className="form-control form-control-user"  
                              name="address" ref={register} />
                            </div>
                            <label  className="col-sm-3 col-form-label">تاريخ انتهاء الاقامة</label>
                            <div className="col-sm-3">
                            <Controller
                                  control={control}
                                  name="residencyExpiryDate"
                                  render={({ onChange, onBlur, value }) => (
                                    <ReactDatePicker
                                      onChange={onChange}
                                      onBlur={onBlur}
                                      selected={value}
                                      dateFormat="dd/MM/yyyy"
                                      placeholderText="dd/MM/yyyy "   
                                      className="form-control form-control-user"
                                      disabled
                                      />
                                  )}
                                />
                            </div>
                          </div>
                          {/* ################### form- row-006 #################*/}
                          {/* ################# submit btn ##################### */}
                          <div className="row justify-content-between">
                             <button onClick={()=> {setDirection("bwd");  }} type="submit" className="btn btn-primary btn-user shorooq  " style={{fontSize: '22px'}}>
                              السابق
                            </button>
                            <button onClick={()=> {setDirection("fwd");  }} type="submit" className="btn btn-primary btn-user shorooq  " style={{fontSize: '22px'}}>
                              التالي
                            </button >
                          </div>
                          {/* ################# end submit btn ##################### */}
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

export default PassportInfo;
