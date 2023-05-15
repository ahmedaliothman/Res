import React, { SyntheticEvent, useContext, useEffect, useState } from "react";
import { authenticateResponse } from "../../types/userInfo";
import { useHistory } from "react-router-dom";
import FullPageLoader from "../../Controls/FullPageLoader";
import { Redirect, useParams, useLocation } from "react-router-dom";
import { getLocalStorage } from "../../Services/utils/localStorageHelper";
import Layout from "../../Layout/Common/LayoutPublicSettings";
import { useSelector, useDispatch } from 'react-redux';
import { RootState } from '../../State/rootReducer';
import { getloginRequest } from "../../State/login";
import { RequestClearRequest } from "../../State/newApp";
import generalSettingReducer, * as GeneralSettings from '../../State/GeneralSettings'
import Redirection from "../Common/Redirection";
import { routerReducer, routerMiddleware ,push} from 'react-router-redux';
import { getHistoryPushRequest } from "../../State/History/action";
import { delay } from "redux-saga/effects";
const Login = () => {
	let userAuth = getLocalStorage("user", authenticateResponse);
	const history = useHistory();
	let dispatch = useDispatch();
	const loginstate = useSelector<RootState, RootState["login"]>(state => state.login);

	const [username, setUsername] = useState<string>("");
	const [password, setPassword] = useState<string>("");

	const [isFieldEmpty, setIsFieldEmpty] = useState(false);
	const [wrongCredintials, setWrongCredintials] = useState(false);
	let params: any = useParams();
	let location = useLocation();


	useEffect(() => {

		document.body.classList.add("sb-sidenav-toggled");
		return () => {
			document.body.classList.remove("sb-sidenav-toggled");
		}
	}, []);

	const handleSubmit = async (e: React.SyntheticEvent) => {
		e.preventDefault();
		dispatch(GeneralSettings.getGeneralSettingsRequest());
		await dispatch(getloginRequest({ username, password }));
		await dispatch(RequestClearRequest());
		
		
		

	}
	let redirectionUrl = location.search.toLowerCase().replace("?", "");
	const handleNewUser = (e: SyntheticEvent) => {
		e.preventDefault();
		history.push("/Public/register");
		 dispatch(getHistoryPushRequest({pageName:"/Public/register" }));

	}


	return (
		<Layout>
			
				<main className="login-bg">
					<div className="container" style={{ marginBottom: '80px' }}>
						{/* Outer Row */}
						<div className="row justify-content-center">
							<div className="col-xl-10 col-lg-12 col-md-9">
								<div className="card o-hidden border-1 shadow my-5 animate__animated animate__backInDown " style={{ border: '1px solid rgb(189, 189, 189)' }}>
									<div className="card-header bg-dark">
										<div className="text-center">

											<h2 className=" text-gray-900 mb-1 shorooq gold" style={{ fontSize: '28px' }}>تسجيل دخول لنظام تجديد الإقامات</h2>
											<h1 className=" text-gray-900 mb-1 shorooq gold" style={{ fontSize: '28px' }}>قسم الجوازات - إدارة الشئون الإدارية</h1>
										</div>
									</div>
									<div className="card-body p-0 ">
										{/* Nested Row within Card Body */}
										<div className="row">
											<div className="col-lg-8 p-5 text-center">
												<h1 className=" text-gray-900 mb-4 gold" style={{ fontSize: '16px', lineHeight: '30px' }}>ادخل اسم المستخدم وكلمة السر
													للدخول الى نظام تجديد الإقامات </h1>
												{wrongCredintials &&
													<div className="alert alert-danger" role="alert">
														اسم المتسخدم / كلمة المرور غير صحيحة!
													</div>
												}
												{isFieldEmpty &&
													<div className="alert alert-danger" role="alert">
														مطلوب اسم المستخدم / كلمة المرور!
													</div>
												}
												<form className="user" onSubmit={handleSubmit}>
													<div className="form-group">
														<input value={username} type="text" className="form-control form-control-user"
															onChange={e => setUsername(e.target.value)}
															id="exampleInputEmail" placeholder="ادخل اسم المستخدم" />
													</div>
													<div className="form-group">
														<input value={password} type="password" className="form-control form-control-user"
															onChange={e => setPassword(e.target.value)}
															id="exampleInputPassword" placeholder="ادخل كلمة السر " />
													</div>
													<div className="row justify-content-between">
														<button className="btn btn-primary btn-user  shorooq " style={{ fontSize: '22px' }}> تسجيل الدخول الى النظام  </button>
														<button className="btn btn-info btn-user shorooq " style={{ fontSize: '22px' }} onClick={handleNewUser}> تسجيل مستخدم جديد </button>
														{/* <button  className="btn btn-info btn-user shorooq " style={{ fontSize: '22px' }} onClick={handleAdminLogin}> تسجيل دخول مسئول نظام</button> */}

													</div>
													{loginstate.hasError &&
														<div className="alert alert-danger" role="alert">
															{loginstate.message}
														</div>
													}
												</form>
											</div>
											<div className="col-lg-4 d-none d-lg-block text-center my-auto" style={{ backgroundColor: 'transparent' }}>
												<img className="img-fluid" src={process.env.PUBLIC_URL + 'img/logo.jpg'} alt="" width="80%" />
											</div>
										</div>
									</div>
								</div>
							</div>
						</div>
					</div>
				</main>
			

	
		</Layout>
	);
};

export default Login;