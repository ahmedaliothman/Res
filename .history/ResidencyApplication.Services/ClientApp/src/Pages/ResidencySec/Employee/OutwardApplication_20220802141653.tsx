import * as React  from 'react';
import { useSelector, useDispatch } from 'react-redux';
import {RootState} from "../../../State/rootReducer"
import {TableBootStrap,columnsBootStrap} from "../../../Controls/Datatables"
import Layout from "../../../Layout/ClientSec/Layout";
import NoData from "../../Common/NoData";
import { IRequests, IRequestsState } from '../../../State/ManageRequests/types';
import * as actions from "../../../State/ManageRequests/actions"
import * as Types from "../../../State/ManageRequests/types"
import { Alert } from 'reactstrap';
import { types } from 'util';

export default function InwardApplication () {

     const RequestsState:IRequestsState = useSelector<RootState,RootState["manageRequest"]>(state => state.manageRequest);
     const {userInfo} = useSelector<RootState,RootState["login"]>(state => state.login) ;
     const {IRequests} = RequestsState;
     const {SelectedRequest}=RequestsState;
     const[alertVisible,setAlertVisible]=React.useState(false);
     const[alertType,setAlertType]=React.useState("");
     const[IRequests_,setIRequests_]=React.useState(RequestsState?.IRequests);
    let dispatch = useDispatch();   

    //Table boot strap input {columns,handel pop up function its (ref) function}
type handleRowFunctions = React.ElementRef<typeof TableBootStrap>;
const handleRowFunctionsref = React.useRef<handleRowFunctions>(null);
let columnsBootStrap_: columnsBootStrap={columns:[],data:null,tableName:"" , hasSelection:false};
  columnsBootStrap_.columns=[
    {
      text: "رقم المعاملة",
      dataField: "applicationNumber"
    },
    {
        text: "الرقم المدنى",
        dataField: "civilId"
    },
    {
        text: "اسم الموظف",
        dataField: "userName"
    },
    {
        text: "حالة الطلب",
        dataField: "applicationStatusName",
        formatter:(row:any,cell:any)=> <td className="h-100 w-100 d-inline-block"  style={{background:cell?.applicationStatusId==1?"#28a745":cell?.applicationStatusId==2?"#dc3545":cell?.applicationStatusId==3?"#ffc107":"#0260ae"}}>{cell.applicationStatusName}</td>

    },
    {
        text: "نوع الطلب",
        dataField: "applicationTypeName"
        
    },
    {
        text: "تاريخ الطلب",
        dataField: "applicationDate",
        formatter:(row:any,cell:any)=> <span> {new Date(cell.applicationDate).toLocaleDateString('ar-EG-u-nu-latn',{weekday: 'long', year: 'numeric', month: 'short', day: 'numeric'})}   </span>
    },
    {
        text: "طباعه",
        formatter: (row:any,cell:any) => <button className="btn btn-primary btn-sm  rounded-circle"  onClick={()=>handleRowFunctionsref.current?.handlePopUp(cell)}><i className="fa fa-print"aria-hidden="true"></i></button>
        
    }
    ,
    {
        text: "عرض",
        formatter:(row:any,cell:any) => <button className="btn btn-primary btn-sm  rounded-circle"   onClick={()=>handleRowFunctionsref.current?.EditRowAdmin(cell)} ><i className="fas fa-eye"aria-hidden="true"/></button>
    },
    {
        text: "التعليقات",
        formatter: (row:any,cell:any) => <button  className="btn btn-primary btn-sm  rounded-circle"   onClick={()=>handleRowFunctionsref.current?.handlepopUpComments(cell)}><i className="fa fa-comments"aria-hidden="true"></i></button>
    }
  ];
  React.useEffect(() => {
      dispatch({type:Types.Clear});
     

   }, []);
   React.useEffect(() => {
    dispatch(actions.CreateRequestAdminOutwardApplications(userInfo?.userId));

  }, [userInfo?.userId]);

  
   React.useEffect(()=>{
   setIRequests_(RequestsState?.IRequests)
    
   },[RequestsState]);


  return (
    <>
         <Alert color={alertType}isOpen={alertVisible} >
          {RequestsState.message}
      </Alert>
    {(userInfo?.userId != 0  &&  IRequests_.length!=0)&& <TableBootStrap  hasSelection={false}  ref={handleRowFunctionsref}  data={IRequests_} columns={columnsBootStrap_.columns} tableName="الطلبات الصادرة"  />}
    {(userInfo?.userId == 0 ||  IRequests_.length==0) && <NoData Message="لا يوجد معاملات سابقة  "/>}
    {/* {( IRequests.length !=0) && <TableBootStrap   ref={handleRowFunctionsref}  data={columnsBootStrap_.data} columns={columnsBootStrap_.columns}  />} */}

    </>
  );
}
