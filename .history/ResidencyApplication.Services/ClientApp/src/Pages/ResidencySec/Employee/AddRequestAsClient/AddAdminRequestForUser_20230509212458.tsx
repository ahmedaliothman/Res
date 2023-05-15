import { Button } from 'primereact/button';
import React, { ReactElement } from 'react'
import { Item } from 'react-bootstrap/lib/Breadcrumb';
import { getLdabUsersList, getAdminUsers, getUserRoleLookup, addUserToDbWithUserRoleIdCall, putUserRoleIdCall } from '../../../../Services/userPermissions'
import { IResponse } from '../../../../types';
import { Ilookup } from '../../../../types/lookUpTypes';
import Select from 'react-select';
import { useForm, useController, Controller } from "react-hook-form"
import { toast } from 'react-toastify';
import { confirmAlert } from 'react-confirm-alert'; // Import
import FullPageLoader from "../../../../Controls/FullPageLoader"
import { selectUser, clearSelectedUser } from "../../../../State/login/action"
import { useDispatch } from 'react-redux';
import { useHistory } from 'react-router';
import {RequestClearRequest} from "../../../../State/newApp/action"
import {ClearRequest as RequestClearnewApp} from "../../../../State/newApp"
import {RequestClear as RequestClearPersonalInfo} from "../../../../State/personalInfo"
import {RequestClear as RequestClearPassportInfo} from "../../../../State/passportInfo"
import {RequestClear as RequestClearattachmentDocuments} from "../../../../State/attachmentDocuments"
interface Props {

}

interface IUser {
    userName?: string;
    employeeName?: string;
    email?: string;
    civilId?: string;
    userRoleId?: number;
    employeeNumber?: string;
    userId?: number;
    userRoleName?: string;
    comeFromLdab?: boolean;
    SapUser?: boolean;
    exsists?: boolean;
    valid?:string;
    statusMessage?:string;
}



export default function AddAdminRequestForUser({ }: Props): ReactElement {

    const [showAddnewUser, setAddNewUser] = React.useState(false);
    const [selectedUser, setSelectedUser] = React.useState<IUser>({});
    const [searchTextLdab, setSearchTextLdab] = React.useState<string>("");
    const [ldabUserResult, setLdabUserResult] = React.useState<IUser[]>([]);
    const [userRoleLookup, setUserRoleLookup] = React.useState<Ilookup[]>([]);
    const [buzy, setBuzy] = React.useState<boolean>(false);
    const { control, errors, reset, handleSubmit, register, setValue } = useForm();
    let dispatch = useDispatch();
    const onClickNewUserButton = () => {
        setLdabUserResult([]);
        setSearchTextLdab("");
    }
    const onClickEditUserButton = (input: IUser) => {
        // setSelectedUser(input);
        submitForm();
    }
    let history = useHistory();

    React.useEffect(() => {

        if (ldabUserResult!=undefined) {
          setSelectedUser({});
          setSelectedUser(ldabUserResult[0]);
           
        }
    }, [ldabUserResult])
    const onClickLdabSearch = async () => {
        if (searchTextLdab != "") {
            let res: IResponse<IUser[]> = await getLdabUsersList(searchTextLdab);
            setLdabUserResult(res.response as IUser[]);
        }
        else {
            toast.error("برجاء ادخال الرقم المدنى للمستخدم");
        }
    }
    //clear the newapp state 
  React.useEffect(() => {
    dispatch(RequestClearnewApp());
    dispatch(RequestClearPersonalInfo());
    dispatch(RequestClearPassportInfo());
    dispatch(RequestClearattachmentDocuments());    
  }, [])

    const submitForm = async () => {
        confirmAlert({
            customUI: ({ onClose }) => {
                return (
                    <div style={{ textAlign: 'center', width: 500, borderRadius: 10, padding: 40, background: '#005da3', boxShadow: '0 20px 75px rgb(0 0 0 / 23%)', color: '#fff' }} >
                        <h4>هل أنت متأكد من حفظ   ?</h4>
                        <button style={{ margin: 20, width: 50, borderRadius: 10, fontSize: 20, background: 'white' }} onClick={() => { updateUserRole(selectedUser); onClose() }}>نعم </button>
                        <button style={{ margin: 20, width: 50, borderRadius: 10, fontSize: 20, background: 'white' }} onClick={() => { onClose() }}>لا</button>
                    </div>
                );
            }
        });

    }

    async function updateUserRole(selectedUser: IUser) {
        setBuzy(true);
        dispatch(clearSelectedUser());
        if (selectedUser.exsists == false) {
            // save from ldab
            let user: IUser = { userName: selectedUser.userName, civilId: selectedUser.civilId, employeeName: selectedUser.employeeName, userRoleId: 2 };
            let res: IResponse = await addUserToDbWithUserRoleIdCall(user);
            //dispatch returned value 
            dispatch(selectUser(res.response));
            if (res.hasError) {
                toast.error(res.message);
            }
            else {
                if (res.message != null) {
                    toast.success(res.message);
                    setBuzy(false);
                    history.push("/Res/Employee/AddRequestAsClient/NewAppAdmin");

                }
            }
            //dispatch the selected user 
        }
        else {
            dispatch(selectUser(selectedUser));
            setBuzy(false);

            history.push("/Res/Employee/AddRequestAsClient/NewAppAdmin");
        }

        setBuzy(false);

    }
    const keyUpIgnore = (e: any) => {
        if (e.keyCode == 13)
            return false;
    }

    return (
        <>

            {buzy && <FullPageLoader />}
            <div className="container" style={{ marginBottom: "10%"  }}>
                ;<div
                    className="bg-white p-3 mt-5 o-hidden shadow my-5 animate__animated animate__backInDown "
                    style={{ border: "1px solid rgb(189, 189, 189)" }}
                >
                    {/* Outer Row */}
                    <div className="row justify-content-center" >
                        <div className="col-xl-8 col-lg-8 col-md-8">
                            {/*  First tab   */}


                            {/*  second tab   */}
                            <div

                                className="card border-0 pt-5"
                                style={{ border: "1px solid rgb(189, 189, 189)", display: "block" }}
                            >
                                <div className="text-left border-bottom">
                                    <h1
                                        className=" text-gray-900 shorooq gold pl-3 pb-3"
                                        style={{ fontSize: 28 }}
                                    >
                                        اضافة طلب للمستخدم
                                    </h1>
                                </div>
                                <div className="card-body p-0 ">
                                    {/* Nested Row within Card Body */}
                                    <div className="row">
                                        <div className="col-lg-12">
                                            <div className="p-3">
                                                <form className="user">
                                                    {/* ################### form- row-002 #################*/}
                                                    <div className="input-group mb-3">
                                                        <input
                                                            type="text"
                                                            className="form-control"
                                                            placeholder=" .... البحــــث بالرقم المدنى "
                                                            aria-label="Example text with button addon"
                                                            aria-describedby="button-addon1"
                                                            value={searchTextLdab}
                                                            onChange={e => setSearchTextLdab(e.target.value)}
                                                            onKeyUp={() => onClickLdabSearch()}
                                                        />
                                                        <Button
                                                            className="btn btn-warning btn-user shorooq"
                                                            style={{ borderRadius: "0%" }}
                                                            type="button"
                                                            id="button-addon1"
                                                            onClick={() => onClickLdabSearch()}
                                                        >
                                                            البحث
                                                        </Button>
                                                    </div>
                                                    <table className="table table-bordered text-center datatable">
                                                        <tbody>
                                                            <tr>
                                                                <td>اسم المستخدم</td>
                                                                <td>الرقم المدنى</td>
                                                                <td>اسم المستخدم</td>
                                                                <td>اضافة طلب</td>
                                                            </tr>
                                                            {ldabUserResult ? ldabUserResult.map((item: IUser) =>
                                                            
                                                            item.valid=="valid"? <tr> <td>{item.employeeName}</td> <td>{item.civilId}</td> <td>{item.userName}</td> <td>   <a onClick={() => onClickEditUserButton(item)} className="btn btn-primary btn-user shorooq  " style={{ fontSize: 22, cursor: "pointer" }} > اضافة </a> </td></tr>:<tr><td style={{width:"100%"}}>{item.statusMessage}</td></tr>) 
                                                            : <td style={{width:"100%"}}> برجاء التقدم لاصدار مستخدم     </td>
                                                             }


                                                        </tbody>
                                                    </table>
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
            </div>
<div></div>
        </>

    )
}
