import React,{useState,useEffect} from 'react'
import Layout from "../../../../Layout/ClientSec/Layout";
import ReactDatePicker from "react-datepicker";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import Select from 'react-select';
import { useForm, Controller } from "react-hook-form";
import {IState} from "../../../../State/passportInfo";
import {SelectOptions} from "../../../../types/UIRelated"
import { useSelector,useDispatch } from "react-redux";
import { RootState } from "../../../../State/rootReducer";
import { assignToType} from "../../../../Services/utils/assignType";
import {getCreateRequest,getFetchRequest,getUpdateRequest as updatePassportInfo} from "../../../../State/passportInfo";
import {getNationalitiesRequest} from "../../../../State/lookUps";
import { getFetchIncompleteRequest,getUpdateRequest } from "../../../../State/newApp";
import {Steps,ErrorMessages} from "../../../../types/Enums"
import { useHistory } from "react-router-dom";
import { ReactDatePicker as CustomReactDatePicker } from "../../../../Controls/CustomDatePicker";
import { getPaciServiceInfo, paciServiceInput, paciServiceObj } from '../../../../Services/paciService';
import { IResponse } from '../../../../types';


export interface IFormData extends IState {
  selectedNationality?:SelectOptions,
  selectedIssueCountry?:SelectOptions,
  EmployeeNumber?:number
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
  navigationUrl?:string; 

}
const PassportInfo = () => {


  const history = useHistory();
  const LookUpState = useSelector<RootState,RootState["lookUp"]>(state => state.lookUp);
  const newAppState = useSelector<RootState, RootState["newApp"]>(state => state.newApp);
  const loginData = useSelector<RootState, RootState["login"]>(state => state.login);
  const stateData = useSelector<RootState,RootState["passportInfo"]>(state => state.passportInfo);
  const paci = useSelector<RootState,RootState["paci"]>(state => state.paci);
  const {Nationalities} = LookUpState;
  const { userInfo } = loginData;
  const [startDate, setStartDate] = useState(new Date());
  const [Direction, setDirection] = useState<string>("");
  const [paciService, setPaciService] = useState<paciServiceObj>({});

  let dispatch = useDispatch();
  const { register, handleSubmit, watch, errors,setValue, getValues,control } = useForm<IFormData>({
    defaultValues:{
      userId : userInfo?.userId,
      civilID: userInfo?.civilId?Number(userInfo?.civilId):undefined,
      EmployeeNumber: userInfo?.employeeNumber ? Number(userInfo?.employeeNumber) : undefined,
  

    }
  });

  useEffect(  () => {
    const GetDropdownValues = async () => {
      if (Nationalities === undefined) {
        dispatch(getNationalitiesRequest());
      }
    
    };
    GetDropdownValues();
  

  }, []);

  
  
  useEffect(  () => {
   if(paci!=undefined && paci !== null)
   {
    setValue("passportNumber", paci?.pASSPORT_NOField);
    setValue("address", paci?.dISTRICTNAMEField);
    const dateString = paci?.aDDITIONAL_PARAM_2Field?.toString();
    if (dateString!= undefined)
      {
      const year = +dateString.substring(0, 4);
      const month = +dateString.substring(4, 6);
      const day = +dateString.substring(6, 8);
      const date = new Date(year, month - 1, day);
      setValue("residencyExpiryDate",date);
      }
    
   }
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
    setValue("EmployeeNumber", userInfo?.employeeNumber);
    
  }, [userInfo?.employeeNumber]);
  useEffect(() => {
    if(stateData.passportNumber!== undefined)
     setValue("passportNumber", stateData.passportNumber);
     setValue("issueDate", stateData.issueDate?new Date(stateData.issueDate as Date):undefined);
     setValue("expiryDate", stateData.expiryDate?new Date(stateData.expiryDate as Date):undefined);
     
     if(stateData.residencyExpiryDate!== undefined)
     setValue("residencyExpiryDate", stateData.residencyExpiryDate?new Date(stateData.residencyExpiryDate as Date):undefined);

     if(stateData.address!== undefined)
     setValue("address", stateData.address);

     

    if(Nationalities!==undefined ) 
    {
      setValue("selectedNationality", (Nationalities as SelectOptions[]).find(j => j.value === stateData.nationalityId));
      setValue("selectedIssueCountry", (Nationalities as SelectOptions[]).find(j => j.value === stateData.issueCountry));
    }


    
  }, [stateData,Nationalities]);

  const onSubmit = async (data:IFormData) => {
    let res  = new TempClass();
    res = assignToType(data,res); 
    res.nationalityId=data.selectedNationality?.value;
    res.issueCountry = data.selectedIssueCountry?.value;
    res.applicationNumber=newAppState.IState.applicationNumber;;
    res.userId=newAppState.IState.userId;
    res.civilID = userInfo?.civilId?Number(userInfo?.civilId):undefined;
    
    if (Direction === "fwd") {
      res.navigationUrl="/Res/Employee/EditView/FileAttachments";
    }
    else if (Direction === "bwd") {
      res.navigationUrl="/Res/Employee/EditView/personalinfo";
    }
 
    if(stateData.id ===undefined ||stateData.id===0)
    {
      res.createdDate = new Date();
      dispatch(getCreateRequest(res));
      newAppState.IState.stepNo =Steps.PassportInfo;
      dispatch(getUpdateRequest(newAppState.IState));
    }
   else{
     res.id = stateData.id;
     dispatch(updatePassportInfo(res));
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
                    <h1 className=" text-gray-900 mb-1 shorooq gold" style={{fontSize: '28px'}}>  بيانات
                      الجواز والاقامة</h1>
                  </div>
                </div>
                <div className="card-body p-0 ">
                  {/* Nested Row within Card Body */}
                  <div className="row">
                    <div className="col-lg-12">
                      <div className="p-5">
                        <form className="user" onSubmit={handleSubmit(onSubmit)}>
                          {/* ################### form- row-001 #################*/}
                          {/* <div className="form-group row">
                            <label  className="col-sm-3 col-form-label">رقم الموظف</label>
                            <div className="col-sm-3">
                              <input type="text" className="form-control form-control-user" />
                            </div>
                          </div> */}
                          {/* ################### form- row-002 #################*/}
                          <div className="form-group row">
                            <label  className="col-sm-3 col-form-label">رقم الموظف</label>
                            <div className="col-sm-3">
                              <input type="text" name="EmployeeNumber" disabled={true} ref={register()} className="form-control form-control-user" />
                            </div>
                            <label  className="col-sm-3 col-form-label">الجنسية</label>
                            <div className="col-sm-3 d-flex">
                            <span className="mt-2  " style={{color:"#ff0000"}}>*</span>
                                <Controller
                                  name="selectedNationality"
                                  control={control}
                                  placeholder=" اختر الادار  "
                                  rules={{
                                    required: true  }}
                                  options={Nationalities}
                                  className="form-control form-control-user border-0"

                                  as={Select}
                                />
                                      {errors?.selectedNationality !==undefined && 
                                 <span className="text-danger">{ErrorMessages.required}</span>  }
                            </div>
                          </div>
                          {/* ################### form- row-003 #################*/}
                          <div className="form-group row">
                            <label  className="col-sm-3 col-form-label">رقم الجواز</label>
                            <div className="col-sm-3 d-flex">
                            <span className="mt-2 " style={{color:"#ff0000", marginLeft:"12px"}}>*</span>
                              <input type="text" className="form-control form-control-user"  
                              name="passportNumber"  ref={register({ required: true})} />
                                 {errors?.passportNumber?.type==="required" && 
                                 <span className="text-danger">{ErrorMessages.required}</span>  }
                            </div>
                            <label  className="col-sm-3 col-form-label">مكان الاصدار</label>
                            <div className="col-sm-3 d-flex">
                            <span className="mt-2 " style={{color:"#ff0000"}}>*</span>

                               <Controller
                                  name="selectedIssueCountry"
                                  control={control}
                                  placeholder=" اختر الادار  "
                                  options={Nationalities}
                                  rules={{
                                    required: true  }}
                                    className="form-control form-control-user border-0"

                                  as={Select}
                                />
                                 {errors?.selectedIssueCountry !==undefined && 
                                 <span className="text-danger">{ErrorMessages.required}</span>  }
                            </div>
                          </div>
                          {/* ################### form- row-004 #################*/}
                          <div className="form-group row">
                            <label  className="col-sm-3 col-form-label">تاريخ الاصدار</label>
                            <div className="col-sm-3 d-flex">
                            <span className="mt-2 " style={{color:"#ff0000"  , marginLeft:"12px"}}>*</span>
                            <Controller
                                  control={control}
                                  name="issueDate"
                                  rules={{
                                    required: true  }}
                                  render={({ onChange, onBlur, value }) => (
                                    // <ReactDatePicker
                                    //   onChange={onChange}
                                    //   onBlur={onBlur}
                                    //   selected={value}
                                    //   dateFormat="dd/MM/yyyy"
                                    //   placeholderText="dd/MM/yyyy "   
                                    //   className="form-control form-control-user"
                                    // />
                                    <CustomReactDatePicker required={true} onChange={onChange} onBlur={onBlur} ss={value} />
                     
                                  )}
                                />
                                {errors?.issueDate !==undefined && 
                                 <span className="text-danger">{ErrorMessages.required}</span>  }

                            </div>
                            <label  className="col-sm-3 col-form-label">تاريخ الانتهاء</label>
                            <div className="col-sm-3 d-flex">
                            <span className="mt-2 " style={{color:"#ff0000" , marginLeft:"12px"}}>*</span>
                            <Controller
                                  control={control}
                                  name="expiryDate"
                                  rules={{
                                    required: true  }}
                                  render={({ onChange, onBlur, value }) => (
                                    // <ReactDatePicker
                                    //   onChange={onChange}
                                    //   onBlur={onBlur}
                                    //   selected={value}
                                    //   dateFormat="dd/MM/yyyy"
                                    //   placeholderText="dd/MM/yyyy "   
                                    //   className="form-control form-control-user"
                                    // />
                                    <CustomReactDatePicker required={true} onChange={onChange} onBlur={onBlur} ss={value} />

                                  )}
                                />
                                {errors?.expiryDate !==undefined && 
                                 <span className="text-danger">{ErrorMessages.required}</span>  }
                            </div>
                           
                          </div>
                          {/* ################### form- row-005 #################*/}
                          <div className="form-group row">
                            <label  className="col-sm-3 col-form-label">عنوان السكن</label>
                            <div className="col-sm-3 d-flex">
                            <span className="mt-2 " style={{color:"#ff0000" , marginLeft:"12px"}}>*</span>
                              <input type="text" className="form-control form-control-user"  
                              name="address" ref={register({ required: true})} />
                                   {errors?.address?.type==="required" && 
                                 <span className="text-danger">{ErrorMessages.required}</span>  }
                            </div>
                            <label  className="col-sm-3 col-form-label">تاريخ انتهاء الاقامة</label>
                            <div className="col-sm-3 d-flex">
                            <span className="mt-2 " style={{color:"#ff0000", marginLeft:"12px"}}>*</span>
                            <Controller
                                  control={control}
                                  name="residencyExpiryDate"
                                  rules={{
                                    required: true  }}
                                  render={({ onChange, onBlur, value }) => (
                                    // <ReactDatePicker
                                    //   onChange={onChange}
                                    //   onBlur={onBlur}
                                    //   selected={value}
                                    //   dateFormat="dd/MM/yyyy"
                                    //   placeholderText="dd/MM/yyyy "   
                                    //   className="form-control form-control-user"
                                    // />
                                    <CustomReactDatePicker required={true} onChange={onChange} onBlur={onBlur} ss={value} />

                                  )}
                                />
                                    {errors?.residencyExpiryDate !==undefined && 
                                 <span className="text-danger">{ErrorMessages.required}</span>  }
                            </div>
                          </div>
                          {/* ################### form- row-006 #################*/}
                          {/* ################# submit btn ##################### */}
                          <div  className="row justify-content-between"> 
                          <button type="submit" className="btn btn-primary btn-user shorooq  " onClick={()=> {setDirection("bwd");  }} style={{ fontSize: '22px' }}>
                          
                          <a   className="btn btn-primary btn-user shorooq  " style={{ fontSize: '22px' }}>
                            السابق
                            </a>
                            </button>

                            <button type="submit" className="btn btn-primary btn-user shorooq  " onClick={()=> {setDirection("fwd");  }} style={{ fontSize: '22px' }}>
                            <a  className="btn btn-primary btn-user shorooq  " style={{ fontSize: '22px' }}>التالي
                          </a>
                            </button>
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
        </div>
        </main>
    )
}

export default PassportInfo;
