import React, { useState } from "react";
import { useHistory } from "react-router-dom";
import { useSelector,useDispatch} from "react-redux";
import  {RootState} from "../../../../State/rootReducer"
import auth from "../../../../auth/auth";
import { Steps ,StatusType} from "../../../../types/Enums";
import {  getUpdateRequest} from "../../../../State/newApp/action";
import {ClearRequest} from "../../../../State/newApp"
import {RequestClear as RequestClearPersonalInfo} from "../../../../State/personalInfo"
import {RequestClear as RequestClearPassportInfo} from "../../../../State/passportInfo"
import {RequestClear as RequestClearattachmentDocuments} from "../../../../State/attachmentDocuments"
import {  toast } from 'react-toastify';



const Agreament = () => {

	const history = useHistory();
    const [Enabled,setEnabled]=useState(false);
	const {IState} =useSelector<RootState,RootState["newApp"]>(state=>state.newApp);
    const dispatch=useDispatch();
    const [Direction, setDirection] = useState<string>("");
    const loginData = useSelector<RootState, RootState["login"]>(state => state.login);

React.useEffect(() => {
  if(loginData?.selectedUser?.civilId ==undefined)
  {
    toast.error("برجاء اختيار موظف ");
    history.push("/Res/Employee/AddAdminRequestForUser");
  }

  //uiuoiuo
    }, [])
    const onSubmit =(e:React.SyntheticEvent,dir:string)=>{
        e.preventDefault();
        
        if(dir=="fwd")
        {
        IState.stepNo =Steps.RequestSent;
        IState.applicationStatusId=StatusType.AssignedPending;
        dispatch(getUpdateRequest(IState));
        // dispatch(ClearRequest());
        // dispatch(RequestClearPersonalInfo());
        // dispatch(RequestClearPassportInfo());
        // dispatch(RequestClearattachmentDocuments());
        history.push("/Res/Employee/AddRequestAsClient/resultAdmin");
        }
        else if(dir=="bwd")
        {
        history.push("/Res/Employee/AddRequestAsClient/FileAttachmentsAdmin");
        }
    }
	return (
		<>
			<main className="login-bg">
  <div className="container" style={{marginBottom: 80}}>
    {/* Outer Row */}
    <div className="row justify-content-center">
      <div className="col-xl-12 col-lg-12 col-md-12">
        <div className="card o-hidden border-1 shadow my-5 animate__animated animate__backInDown " style={{border: '1px solid rgb(189, 189, 189)'}}>
          <div className="card-header bg-dark">
            <div className="text-center">
              <h1 className=" text-gray-900 mb-1 shorooq gold" style={{fontSize: 28}}>معاملة تجديد اقامة - إقرار
                السداد</h1>
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
                      <div className="col-sm-2">
                        <div className="custom-control custom-checkbox  mt-1">
                          <input type="checkbox" onClick={()=> setEnabled(!Enabled)} defaultChecked data-toggle="toggle" data-on="<i class='fa fa-check'></i>" data-off="<i class='fas fa-times'></i>" data-size="sm" />
                        </div>
                      </div>
                      <label  className="col-sm-10 col-form-label text-justfiy">اتعهد بسداد كامل الرسوم المستحقة لطلبي للجهات المعنية بمواعيدها ، وذلك
                        دون أدنى مسوؤلية على وزارة الأوقاف ، وفي حال عدم التزامي بالسداد بالتاريخ
                        المحدد وفق اعلامي من وزارة الأوقاف، فإني افوض الوزارة باتخاذ كافة الإجراءات
                        اللازمة تجاهي ، واتحمل ما يترتب علي من التزامات، وهذا إقرار مني بذلك.</label>
                    </div>
                    {/* ################# submit btn ##################### */}
                    <div className="row justify-content-center">
                         <button disabled={Enabled} onClick={(e)=> onSubmit(e,"fwd")}  className="btn btn-primary btn-user shorooq  " style={{fontSize: 22}}>
                        الموافقة
                      </button>
                    </div>
                    <div  className="row justify-content-between"> 
                          <button type="submit" className="btn btn-primary btn-user shorooq  "
                           onClick={(e)=>  onSubmit(e,"bwd")} style={{ fontSize: '22px' }}>
                          
                          <a   className="btn btn-primary btn-user shorooq  " style={{ fontSize: '22px' }}>
                            السابق
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

		</>
	);
};

export default  Agreament;
