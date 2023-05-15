import * as React from 'react';
import { useHistory } from "react-router-dom";
interface IAppProps {
}




const Result: React.FunctionComponent<IAppProps> = (props) => {

const history= useHistory();
  const onSubmit=(e:React.SyntheticEvent)=>{
    e.preventDefault();
    history.push("/Res/Employee/EditView/newapp");
  }

return <>
    	<main className="login-bg">
  <div className="container" style={{marginBottom: 80}}>
    {/* Outer Row */}
    <div className="row justify-content-center">
      <div className="col-xl-12 col-lg-12 col-md-12">
        <div className="card o-hidden border-1 shadow my-5 animate__animated animate__backInDown " style={{border: '1px solid rgb(189, 189, 189)'}}>
          <div className="card-header bg-dark">
            <div className="text-center">
              <h1 className=" text-gray-900 mb-1 shorooq gold" style={{fontSize: 28,textAlign:'center'}}></h1>
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
                       
                      </div>
                      <label className="col-sm-10 col-form-label text-justfiy" style={{textAlign: 'center', marginRight: '-75px'}}>تم حفظ المعاملة بنجاح</label>

                    </div>
                    {/* ################# submit btn ##################### */}
                    <div className="row justify-content-center">
                         <button  onClick={(e)=>onSubmit(e)}  className="btn btn-primary btn-user shorooq  " style={{fontSize: 22}}>
                        العودة للصفحة الرئيسية
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
    
    </>;
};

export default Result;
