import React, { useEffect, useState } from 'react'
import Layout from "../../../Layout/ClientSec/Layout";
import { useSelector, useDispatch } from 'react-redux';
import { useForm, Controller } from "react-hook-form";
import { IState, IFileAttachment } from "../../../State/attachmentDocuments";
import { RootState } from '../../../State/rootReducer';
import { useHistory } from "react-router-dom";
import { Steps } from "../../../types/Enums";
import { getFetchIncompleteRequest, getUpdateRequest } from "../../../State/newApp";
import { getCreateRequest, getFetchRequest as fetchAttachment, getUpdateRequest as updateAttachment } from "../../../State/attachmentDocuments";
import { ClearRequest } from "../../../State/newApp"
import { RequestClear as RequestClearPersonalInfo } from "../../../State/personalInfo"
import { RequestClear as RequestClearPassportInfo } from "../../../State/passportInfo"
import { RequestClear as RequestClearattachmentDocuments } from "../../../State/attachmentDocuments"

type TempFormData = {
    ApprovedLetterForResidencyRenewal: FileList;
    SalaryCertification: FileList;
    CivilIdCopy: FileList;
    PassportCopy: FileList;
    OtherRelatedDocuments: FileList;
}
interface IFormData extends IState {
    employeeNumber?: number
}

function FileAttachmentsAdmin() {
    const newAppState = useSelector<RootState, RootState["newApp"]>(state => state.newApp);
    const loginData = useSelector<RootState, RootState["login"]>(state => state.login);
    const stateData = useSelector<RootState, RootState["attachmentInfo"]>(state => state.attachmentInfo);
    const { userInfo } = loginData;
    const { register, handleSubmit, watch, errors, setValue, getValues, control } = useForm<IFormData>(
        {
            defaultValues: {
                employeeNumber: userInfo?.employeeNumber ? Number(userInfo?.employeeNumber) : undefined
            }
        }
    );
    const history = useHistory();
    const [removeletter, setRemoveletter] = useState(true);
    const [removeSalary, setremoveSalary] = useState(true);
    const [removeCivilId, setremoveCivilId] = useState(true);
    const [removePassport, setremovePassport] = useState(true);
    const [removeOtherDocs, setRemoveOtherDocs] = useState(true);
    const [Direction, setDirection] = useState<string>("");
    let dispatch = useDispatch();
    useEffect(() => {
        if (stateData.approvedLetterForResidencyRenewal == "") { setRemoveletter(false); }
        if (stateData.salaryCertification == "") setremoveSalary(false);
        if (stateData.civilIdCopy == "") setremoveCivilId(false);
        if (stateData.passportCopy == "") setremovePassport(false);
        if (stateData.otherRelatedDocuments == "") setRemoveOtherDocs(false);

    }, [stateData]);
    const onSubmit = async (dir: string) => {

        if (dir == "fwd") {
            //clear all states   dispatch(getFetchRequestFileAttachmentInfo(newAppState.IState?.applicationNumber as number));
            dispatch(ClearRequest());
            dispatch(RequestClearPersonalInfo());
            dispatch(RequestClearPassportInfo());
            dispatch(RequestClearattachmentDocuments());
            history.push("/Client/ManageRequests");

        }
        else if (dir == "bwd") {
            history.push("/commonpassportInfo");
        }
    }

    return (

        <main className="login-bg">
            <div className="container" style={{ marginBottom: '80px' }}>
                {/* Outer Row */}
                <div className="row justify-content-center">
                    <div className="col-xl-12 col-lg-12 col-md-12">
                        <div className="card o-hidden border-1 shadow my-5 animate__animated animate__backInDown " style={{ border: '1px solid rgb(189, 189, 189)' }}>
                            <div className="card-header bg-dark">
                                <div className="text-center">
                                    <h1 className=" text-gray-900 mb-1 shorooq gold" style={{ fontSize: '28px' }}>معاملة تجديد اقامة - المرفقات
                                    </h1>
                                </div>
                            </div>
                            <div className="card-body p-0 ">
                                {/* Nested Row within Card Body */}
                                <div className="row">
                                    <div className="col-lg-12">
                                        <div className="p-5">
                                            <form className="user">
                                                {/* ################### form- row-001 #################*/}
                                                <div className="form-group row">
                                                    <label className="col-sm-4 col-form-label">رقم الموظف</label>
                                                    <div className="col-sm-3">
                                                        <input type="text" name="employeeNumber" ref={register()} disabled className="form-control form-control-user" />
                                                    </div>
                                                </div>
                                                {/* ################### form- row-002 #################*/}
                                                <div className="form-group row">
                                                    <label className="col-sm-4 col-form-label">كتاب الادارة بالموافقة عل المعاملة</label>

                                                    <div className="col-sm-8" >
                                                        {(removeletter) &&
                                                            <>
                                                                <a href={stateData.approvedLetterForResidencyRenewal} target="blank" >
                                                                صورة كتاب الادارة بالموافقة عل المعاملة</a>

                                                            </>
                                                        }



                                                    </div>


                                                </div>
                                                {/* ################### form- row-003 #################*/}
                                                <div className="form-group row">
                                                    <label className="col-sm-4 col-form-label">شهادة الراتب</label>
                                                    <div className="col-sm-8">

                                                        {(removeSalary) &&
                                                            <>
                                                                <a href={stateData.salaryCertification} target="blank">
                                                                شهادة راتب</a>

                                                            </>}

                                                    </div>
                                                </div>
                                                {/* ################### form- row-004 #################*/}
                                                <div className="form-group row">
                                                    <label className="col-sm-4 col-form-label">صورة البطاقة المدنية</label>
                                                    <div className="col-sm-8">

                                                        {(removeCivilId) && <>
                                                            <a href={stateData.civilIdCopy} target="blank">
                                                            صورة البطاقة المدنية</a>

                                                        </>}



                                                    </div>
                                                </div>
                                                {/* ################### form- row-005 #################*/}
                                                <div className="form-group row">
                                                    <label className="col-sm-4 col-form-label">صورة الجواز</label>
                                                    <div className="col-sm-8">


                                                        {(removePassport) && <>
                                                            <a href={stateData.passportCopy} target="blank">
                                                            صورة الجواز  </a>

                                                        </>}


                                                    </div>
                                                </div>
                                                {/* ################### form- row-006 #################*/}
                                                <div className="form-group row">
                                                    <label className="col-sm-4 col-form-label">مستندات اخرى</label>
                                                    <div className="col-sm-8">
                                                    {stateData.otherRelatedDocuments?.split(",").map((r,i)=>{
                                                            if(r)
                                                          return  <> <a href={r} target="blank"> ملف رقم{i+1}     </a></>
                                                        })}

                                                        {/* {(removeOtherDocs) && <>
                                                           
                                                            <a href={stateData.otherRelatedDocuments} target="blank">
                                                                Uploaded otherRelatedDocuments   </a>

                                                        </>} */}


                                                    </div>
                                                </div>
                                                {/* ################# submit btn ##################### */}
                                                <div className="row justify-content-between">
                                                    <button type="submit" className="btn btn-primary btn-user shorooq  " onClick={() => { onSubmit("bwd"); }} style={{ fontSize: '22px' }}>

                                                        <a className="btn btn-primary btn-user shorooq  " style={{ fontSize: '22px' }}>
                                                            السابق
                                                        </a>
                                                    </button>

                                                    <button type="submit" className="btn btn-primary btn-user shorooq  " onClick={() => { onSubmit("fwd"); }} style={{ fontSize: '22px' }}>
                                                        <a className="btn btn-primary btn-user shorooq  " style={{ fontSize: '22px' }}>   قائمة المعاملات
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
            </div></main>

    )
}

export default FileAttachmentsAdmin
