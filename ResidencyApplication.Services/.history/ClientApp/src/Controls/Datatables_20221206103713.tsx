import React, { useEffect, useState, useRef, forwardRef, useImperativeHandle } from 'react';
import MockUpData from "../MockUp/useraplications.json";
import DataTable from 'react-data-table-component';
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import BootstrapTable from 'react-bootstrap-table-next';
import { SelectRowProps } from 'react-bootstrap-table-next';
import paginationFactory from 'react-bootstrap-table2-paginator';
import { useDispatch } from "react-redux";
import PrintProvider, { Print, NoPrint } from 'react-easy-print';
import { useReactToPrint } from 'react-to-print';
import { getUserApplicationsByApplicationNumber, loadingRequest } from "../State/newApp"
import { ApplicationStatusUpdateRequst, CreateRequestAdmin } from "../State/ManageRequests/actions"
import { IApplicationStatusUpdateRequst } from "../State/ManageRequests/types"
import { useHistory } from "react-router-dom";
import { ClearRequest } from "../State/newApp"
import { RequestClear as RequestClearPersonalInfo } from "../State/personalInfo"
import { RequestClear as RequestClearPassportInfo } from "../State/passportInfo"
import { RequestClear as RequestClearattachmentDocuments } from "../State/attachmentDocuments"
import { confirmAlert } from 'react-confirm-alert'; // Import
import 'react-confirm-alert/src/react-confirm-alert.css';
import ResReport from "./ResReport";
import TablePrint from "./TablePrint";
import { IRequests } from "../State/ManageRequests/types";
import { INonSapUsers, IResponse } from "../types";
import { updateNonSapUsersRegistered } from "../Services/nonSapUsers";
import { UpdateApplicationAssignToFun } from "../Services/userApplication";
import { toast } from 'react-toastify';
import { useSelector } from "react-redux";
import { RootState } from "../State/rootReducer";
import { Row } from 'react-bootstrap';
import { userInfo } from 'os';

export interface TableBootStrapTablecolumn {

  text?: string,
  dataField?: string,
  formatter?: any
}
export interface columnsBootStrap {
  columns: TableBootStrapTablecolumn[],
  data: any,
  tableName: string,
  hasSelection: Boolean

}
export type handleRowFunctions = {
  handlePopUp: (row: any) => void,
  EditRow: (row: any) => void,
  EditRowAdmin: (row: any) => void,
  showUserApplication: (row: any) => void,
  SubmitRow: (row: any) => void,
  RejectRow: (row: any) => void,
  ReturnRow: (row: any) => void,
  handlePopUpAll: () => void,
  acceptUsers: (row: any) => void,
  AssignTo: (row: any) => void,
  rejectUsers: (row: any) => void,
  EditUserTypesRow: (row: any) => void,
  handlepopUpComments:(row:any)=>void,
  handelpopUpAllTable:()=>void
}
export const TableBootStrap = forwardRef<handleRowFunctions, columnsBootStrap>(({ columns, data, tableName, hasSelection }, ref) => {
  const [show, setShow] = useState(true);
  const [showAll, setShowAll] = useState(false);
  const [showAllTable, setShowAllTable] = useState(false);
  const [showPopUpComment, setPopUpComment] = useState(false);
  const [SelectedList, setSelectedList] = useState<any>([]);
  const [SelectedObject, setSelectedObject] = useState<any>(null);
  let PrintArray: any[] = [];
  const dispatch = useDispatch();
  const history = useHistory();
  const componentRef = useRef<any>();
  const componentRef2 = useRef<any>();
  const userData = useSelector<RootState,RootState["login"]>(state => state.login);

  const pageStyle = `
  @page {
    size: 210mm 297mm;
  }

  @media all {
    .pagebreak {
      display: none;
    }
  }

  @media print {
    .pagebreak {
      page-break-before: always;
    }
  }
`;
const pageStyleLandScape = `



`;
  const handlePrint = useReactToPrint({
    content: () => componentRef?.current,
    pageStyle:pageStyle,
    copyStyles:true
  }); //0020
  const handlePrint2 = useReactToPrint({
    content: () => componentRef2?.current,
    pageStyle:pageStyle,
    copyStyles:true,
    
  });
  const handlePrintLandScape = useReactToPrint({
    content: () => componentRef2?.current,
    pageStyle:pageStyleLandScape,
    copyStyles:true
    
  });
  const submitAlert = (message: string, type: number, applicationNumber: number, userId: number, functionType: number) => {
    if (functionType == 1) {

      let input: IApplicationStatusUpdateRequst = {
        applicationNumber: applicationNumber,
        statusId: type,
        userId: userId,
        submittedBy:userData?.userInfo?.userId,
        remark:"تم "+message
      }
      confirmAlert({
        customUI: ({ onClose }) => {
          return (
            <div style={{ textAlign: 'center', width: 500, borderRadius: 10, padding: 40, background: '#005da3', boxShadow: '0 20px 75px rgb(0 0 0 / 23%)', color: '#fff' }} >
              <h4>هل أنت متأكد من {message}?</h4>
              <button style={{ margin: 20, width: 50, borderRadius: 10, fontSize: 20, background: 'white' }} onClick={() => { submit(input); onClose() }}>نعم </button>
              <button style={{ margin: 20, width: 50, borderRadius: 10, fontSize: 20, background: 'white' }} onClick={() => { onClose() }}>لا</button>
            </div>
          );
        }
      });

    }
    if (functionType == 2) {

      let input: INonSapUsers = {
        RegistrationStatusId: type,
        userId: userId
      }
      confirmAlert({
        customUI: ({ onClose }) => {
          return (
            <div style={{ textAlign: 'center', width: 500, borderRadius: 10, padding: 40, background: '#005da3', boxShadow: '0 20px 75px rgb(0 0 0 / 23%)', color: '#fff' }} >
              <h4>هل أنت متأكد من {message}?</h4>
              <button style={{ margin: 20, width: 50, borderRadius: 10, fontSize: 20, background: 'white' }} onClick={() => { submitUsers(input); onClose() }}>نعم </button>
              <button style={{ margin: 20, width: 50, borderRadius: 10, fontSize: 20, background: 'white' }} onClick={() => { onClose() }}>لا</button>
            </div>
          );
        }
      });

    }

  }
async  function  submit(input: IApplicationStatusUpdateRequst) {
   await   dispatch(ApplicationStatusUpdateRequst(input));
   const sleep = (delay:any) => new Promise((resolve) => setTimeout(resolve, delay));
   await sleep(2000);
   await dispatch(CreateRequestAdmin(input.userId));
   
  // window.location.reload();
  }
  function submitUsers(input: INonSapUsers) {
    updateRegisteredUsersStatus(input);
  }
  const updateRegisteredUsersStatus = async (input: INonSapUsers) => {
    let retobj: IResponse = await updateNonSapUsersRegistered(input);
    if (!retobj.hasError) {
      if (retobj.message != "")
        toast.success(retobj.message);
        history.push("/settingsRegistrationActivationSettings");
        const sleep = (delay:any) => new Promise((resolve) => setTimeout(resolve, delay));
        await sleep(2000);
        window.location.reload();
    }
    else {
      toast.error(retobj.message);
    }
  }
  useImperativeHandle(ref, () => ({
    handlePopUpAll() {
      setShowAll(true);
    },
    handlePopUp(row: any) {
      setShow(true);
      setPopUpComment(false);
      setSelectedObject(row);
    },
    async AssignTo(row: any) {
      try {
        SelectedList.map(async (r: any) => {
          await UpdateApplicationAssignToFun(r.applicationNumber, row.adminUser.value);
        });
         } 
         catch (error) 
         {
            return false;
         }
         return true;

    },
    handlepopUpComments(row:any){
      setPopUpComment(true);
      setSelectedObject(row);
    },
    handelpopUpAllTable(){
      setShowAllTable(true);
    },
    EditRow(row: any) {
      dispatch(ClearRequest());
      dispatch(RequestClearPersonalInfo());
      dispatch(RequestClearPassportInfo());
      dispatch(RequestClearattachmentDocuments());
      dispatch(loadingRequest(true));
      dispatch(getUserApplicationsByApplicationNumber(row.applicationNumber));
      history.push("/newapp");
    },
    EditRowAdmin(row: any) {

      dispatch(loadingRequest(true));
      dispatch(getUserApplicationsByApplicationNumber(row.applicationNumber));
      history.push("/Res/Employee/OnlyView/NewApp");
    },
    showUserApplication(row: any) {

      dispatch(loadingRequest(true));
      dispatch(getUserApplicationsByApplicationNumber(row.applicationNumber));
      history.push("/Client/OnlyView/NewApp");
    },
    SubmitRow(row: any) {

      submitAlert("اعتماد المعاملة", 1, row.applicationNumber, row.userId, 1);
    },
    RejectRow(row: any) {
      submitAlert("رفض المعاملة", 2, row.applicationNumber, row.userId, 1);

    },
    ReturnRow(row: any) {
      //submitAlert("إرجاع المعاملة",3,row.applicationNumber,row.userId);
      history.push({ pathname: '/Res/CommonPages/EditRow', state: row });
    },
    EditUserTypesRow(row: any) {
      //submitAlert("إرجاع المعاملة",3,row.applicationNumber,row.userId);
      history.push({ pathname: '/Res/Manager/UserTypesSettings', state: row });
    },
    acceptUsers(row: any) {
      submitAlert("الموافقة على المستخدم ", 2, row.applicationNumber, row.userId, 2);
    },
    rejectUsers(row: any) {
      submitAlert("رفض  المستخدم ", 3, row.applicationNumber, row.userId, 2);

    }
  }));
  const PopUpComments=(row:any)=>{
    const handelClose = () => {
      setPopUpComment(false);
      setSelectedObject(null);
    }
   
   
    return (
      <>
 
        <Modal
          centered
          size="xl"
          show={showPopUpComment}
          onHide={() => handelClose()}
          dialogClassName="modal-90w"
        >
          <Modal.Header closeButton className="headertablecells">
            <Modal.Title  >
              عرض التعليقات 
          </Modal.Title>
          </Modal.Header>

          <Modal.Body style={{ alignContent: "center" }}>
            <Print name="ss">
               {row.row.remark}
            </Print>

          </Modal.Body>

        </Modal>



      </>
  
    )}
  const PopUp = (row: any) => {

    setPopUpComment(false);
    const handelClose = () => {
      setShow(false);
      setSelectedObject(null);
    }

    const printContent = () => {
      window.print();
    };
    let list: Array<IRequests> = [];
    list.push(row.row);


    return (
      <>
        <Modal
          centered
          size="xl"
          show={show}
          onHide={() => handelClose()}
          dialogClassName="modal-90w"
        >
          <Modal.Header closeButton className="headertablecells">
            <Modal.Title  >
             عرض الطلب
          </Modal.Title>
            <button className="btn btn-primary btn-sm  " style={{ margin: "2px 1cm 0cm " }} onClick={handlePrint} id="print">طباعة</button>
          </Modal.Header>

          <Modal.Body style={{ alignContent: "center" }}>
            <Print name="ss">
              <ResReport ref={componentRef} reportInput={list} />

              <NoPrint>
                <button className="btn btn-primary btn-sm  " onClick={handlePrint} id="print">طباعة</button>
              </NoPrint>
            </Print>

          </Modal.Body>

        </Modal>



      </>
    );
  }

  const PopUpAllTable = () => {
    setPopUpComment(false);
    const handelClose = () => {
      setShowAllTable(false);
      setSelectedObject(null);
    }

    const printContent = () => {
      window.print();
    }


    return (
      <>
        <Modal
          centered
          size="xl"
          show={showAllTable}
          onHide={() => handelClose()}
          dialogClassName="modal-90w"
        style={{outerWidth:"790px !important"}}
        >
          <Modal.Header closeButton className="headertablecells">
            <Modal.Title  >
              <NoPrint>  </NoPrint>


            </Modal.Title>
            <button className="btn btn-primary btn-sm  " style={{ margin: "2px 1cm 0cm " }} onClick={handlePrintLandScape} id="print">طباعة</button>

          </Modal.Header>

          <Modal.Body style={{ alignContent: "center" }} className="">
            <Print name="ss">
              <TablePrint ref={componentRef2} reportInput={data} />
              <NoPrint>
                <button className="btn btn-primary btn-sm  " onClick={handlePrintLandScape} id="print">طباعة</button>
              </NoPrint>
            </Print>

          </Modal.Body>

        </Modal>



      </>
    );
  }
  const PopUpAll = () => {
    setPopUpComment(false);
    const handelClose = () => {
      setShowAll(false);
      setSelectedObject(null);
    }

    const printContent = () => {
      window.print();
    }


    return (
      <>
        <Modal
          centered
          size="xl"
          show={showAll}
          onHide={() => handelClose()}
          dialogClassName="modal-90w"
        style={{outerWidth:"790px !important"}}
        >
          <Modal.Header closeButton className="headertablecells">
            <Modal.Title  >
              <NoPrint>  عرض كل الطلبات </NoPrint>


            </Modal.Title>
            <button className="btn btn-primary btn-sm  " style={{ margin: "2px 1cm 0cm " }} onClick={handlePrint2} id="print">طباعة</button>

          </Modal.Header>

          <Modal.Body style={{ alignContent: "center" }} className="">
            <Print name="ss">
              <ResReport ref={componentRef2} reportInput={SelectedList} />
               
              <NoPrint>
                <button className="btn btn-primary btn-sm  " onClick={handlePrint2} id="print">طباعة</button>
              </NoPrint>
            </Print>

          </Modal.Body>

        </Modal>



      </>
    );
  }
  const sizePerPageOptionRenderer = ({
    text,
    page,
    onSizePerPageChange
  }: any) => (
    <li
      key={text}
      role="presentation"
      className="dropdown-item"
    >
      <a
        href="#"
        tabIndex={-1}
        role="menuitem"
        data-page={page}
        onMouseDown={(e) => {
          e.preventDefault();
          onSizePerPageChange(page);
        }}
        style={{ color: 'red' }}
      >
        {text}
      </a>
    </li>
  );
  const options = {
    sizePerPageOptionRenderer
  };

  const selectRowProp: SelectRowProps<any> = {
    mode: 'checkbox',
    onSelect: (row, isSelected, e) => {
      PrintArray= SelectedList;
      setShowAll(false);
      setShowAllTable(false);
      if (isSelected) 
      {
        PrintArray.push(row);
      }
      else 
      {
        const index = PrintArray.findIndex((element, index) => {
          if (element.applicationNumber === row.applicationNumber) {
            return true
          }
        })
        PrintArray.splice(index, 1);
      }
      setSelectedList(PrintArray);
    },
    onSelectAll: (isSelected, row, e) => {
    
      setSelectedList([]);
      if (isSelected) {
        PrintArray = [];
        data.map((r: any) => {
          PrintArray.push(r);
        })
        setSelectedList(PrintArray);
      }
      else {
        PrintArray = [];
        setSelectedList(PrintArray);
      }
   
    }

  };


  const selectRowPropEmpty: SelectRowProps<any> = { mode: "checkbox" };


  // Render the UI for your table
  return (<>
    <main className="login-bg">
      <div className="container-fluid" style={{ marginBottom: 10 }}>
        <div className="row justify-content-center">
          <div className="col-xl-12 col-lg-12 col-md-12">
            <div className="card o-hidden border-1 shadow my-5 animate__animated animate__fadeIn " style={{ border: '1px solid rgb(189, 189, 189)' }}>
              <div className="card-header bg-dark">
                <div className="text-center">
                  <h1 className=" text-gray-900 mb-1 shorooq gold" style={{ fontSize: 28 }}> {tableName}</h1>
                </div>
              </div>
              <div className="card-body p-5 ">
                <div className="table-responsive">
                  <BootstrapTable
                    classes="table table-striped  table-bordered naskh"
                    headerClasses="headertablecells"
                    rowClasses="rowtablecells"
                    keyField="applicationNumber"
                    data={data}
                    columns={columns as any}
                    pagination={paginationFactory(options)}
                    selectRow={hasSelection ? selectRowProp : selectRowPropEmpty}


                  />
                </div>
                {SelectedObject != null && !showPopUpComment ? <PopUp row={SelectedObject}></PopUp> : null}
                {SelectedObject != null&& showPopUpComment ? <PopUpComments  row={SelectedObject}></PopUpComments> : null}
                {(SelectedList.length != 0&&showAll==true) &&<PopUpAll ></PopUpAll>}
                {(showAllTable==true) &&<PopUpAllTable ></PopUpAllTable>}
                
              </div>
            </div>
          </div>
        </div>
      </div>
    </main>

  </>
  )
}
);
