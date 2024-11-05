import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import logo from "../../assets/images/logo.ico";
import { InputText } from "primereact/inputtext";
import { Password } from "primereact/password";
import { Button } from "primereact/button";
import { API_BASE_URL, LOGIN_FAIL_WAIT_PERIOD } from "../../data";
import axios from "axios";
import "./Login.css";

const Login = () => {
  const navigate = useNavigate();
  const [credentials, setCredentials] = useState({
    userName: "",
    hashedPassword: "",
  });
  const [validation, setValidation] = useState({
    form: "",
    userName: "",
    hashedPassword: "",
    failures: "",
  });
  const [retryCount, setRetryCount] = useState(5);
  const [disableRetry, setDisableRetry] = useState(false);

  useEffect(() => {
    if (retryCount === 0) {
      setDisableRetry(true);
      setValidation({ ...validation, failures: "عدد المحاولات المتبقية 0" });
      setTimeout(() => {
        setDisableRetry(false);
        setRetryCount(5);
      }, LOGIN_FAIL_WAIT_PERIOD);
    }
  }, [retryCount]);

  function handleInputChange(e) {
    setCredentials({ ...credentials, [e.target.name]: e.target.value });
  }


  function validateUsername(input) {
    if (!input.length) {
      return { value: false, message: "برجاء ادخال اسم المستخدم" };
    }
    if (input.length < 3) {
      return { value: false, message: "اسم المستخدم يجب ألا يقل عن 3 حروف" };
    }
    return { value: true, message: "" };
  }

  function validatePassword(input) {
    if (!input.length) {
      return { value: false, message: "برجاء ادخال كلمة المرور" };
    }
    if (input.length < 5) {
      return { value: false, message: "كلمة المرور يجب ألا تقل عن 5 حروف" };
    }
    return { value: true, message: "" };
  }

  function handleLogin(e) {
    e.preventDefault();
    const resultUsername = validateUsername(credentials.userName);
    const resultPassword = validatePassword(credentials.hashedPassword);
    setValidation({
      ...validation,
      userName: resultUsername.message,
      hashedPassword: resultPassword.message,
    });
    if (resultUsername.value && resultPassword.value) {
      axios
        .post(`${API_BASE_URL}/User/UserLogin`, credentials)
        .then((response) => {
          localStorage.setItem("token", response.data.token);
          localStorage.setItem("hashid", response.data.id);
          localStorage.setItem("id", response.data.user.userId);
          localStorage.setItem("degree", response.data.user.degree);
          localStorage.setItem("officeId", response.data.user.officeId);
          window.location.reload();
          navigate("/");
        })
        .catch((error) => {
          if (error.response.status === 400) {
            setValidation({
              ...validation,
              form: "حدث خطأ برجاء التواصل مع فرع النظم",
            });
          } else {
            setValidation({
              ...validation,
              form: "المستخدم غير موجود",
              failures: `عدد المحاولات المتبقية ${retryCount}`,
            });
            setRetryCount(retryCount - 1);
          }
        });
    }
  }

  return (
    <main className="login__wrapper">
      <div className="login__right-section">
        <img src={logo} alt="" className="" />
      </div>
      <div className="login__left-section">
        <div className="login__greeting mb-3">
          <span className="text-2xl font-bold">
            اهلا بكم في منظومة المقابلات
          </span>
          <span className="validation__text">{validation.form}</span>
        </div>
        <form className="flex flex-column align-items-center justify-content-center w-full p-2">
          <div className="field w-full pb-5">
            <span className="p-float-label">
              
              <InputText
                id="userName"
                name="userName"
                className="p-inputtext-lg w-full"
                onChange={handleInputChange}
              />
              <label htmlFor="userName">اسم المستخدم</label>
            </span>
            <span className="validation__text">{validation.userName}</span>
          </div>
          <div className="field w-full pb-5">
            <span className="p-float-label">
              <Password
                id="hashedPassword"
                name="hashedPassword"
                className="p-inputtext-lg w-full"
                onChange={handleInputChange}
                feedback={false}
                autoComplete="off"
                toggleMask
              />
              <label htmlFor="hashedPassword">كلمة المرور</label>
            </span>
            <span className="validation__text">
              {validation.hashedPassword}
            </span>
          </div>
          <Button
            type="submit"
            label="تسجيل الدخول"
            onClick={handleLogin}
            disabled={disableRetry}
          />
        </form>
        <span className="validation__text">{validation.failures}</span>
      </div>
    </main>
  );
};

export default Login;
