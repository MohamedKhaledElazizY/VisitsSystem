import React from "react";
import { useNavigate } from "react-router-dom";
import { Menubar } from "primereact/menubar";
import logo from "../../assets/images/logo.ico";
import { Button } from "primereact/button";
import { BiLogOutCircle } from "react-icons/bi";
import { Link } from "react-router-dom";
import { axiosInstance } from "../../components/StateKeeper/StateKeeper";
import "./Navbar.css";

const Navbar = () => {
  const navigate = useNavigate();
  const navbarLinks = [
    {
      template: (item, options) => {
        return (
          (!JSON.parse(localStorage.getItem("degree")) ||
            localStorage.getItem("id") === "1") && (
            <Link to="/visitors" className={options.className}>
              <span
                className={`${options.iconClassName} pi pi-user-plus`}
              ></span>
              تكويد الزائرين
            </Link>
          )
        );
      },
    },
    {
      template: (item, options) => {
        return (
          (!JSON.parse(localStorage.getItem("degree")) ||
            localStorage.getItem("id") === "1") && (
            <Link to="/visits-entry" className={options.className}>
              <span
                className={`${options.iconClassName} pi pi-plus-circle`}
              ></span>
              إضافة مقابلة
            </Link>
          )
        );
      },
    },
    {
      template: (item, options) => {
        return (
          (JSON.parse(localStorage.getItem("degree")) ||
            localStorage.getItem("id") === "1") && (
            <Link to="/visits-view" className={options.className}>
              <span className={`${options.iconClassName} pi pi-eye`}></span>
              عرض المقابلات
            </Link>
          )
        );
      },
    },
    localStorage.getItem("id") === "1" && {
      label: "الاعدادات",
      icon: "pi pi-cog",
      items: [
        {
          template: (item, options) => {
            return (
              <Link to="/register" className={options.className}>
                <span
                  className={`${options.iconClassName} pi pi-user-plus`}
                ></span>
                اضافة مستخدم
              </Link>
            );
          },
        },
        {
          template: (item, options) => {
            return (
              <Link to="/visits-history" className={options.className}>
                <span
                  className={`${options.iconClassName} pi pi-history`}
                ></span>
                تاريخ المقابلات
              </Link>
            );
          },
        },
      ],
    },
    {
      template: (item, options) => {
        return (
          <>
            <Button
              severity="secondary"
              className="mr-5"
              onClick={handleLogout}
              rounded
            >
              <BiLogOutCircle />
              <span>تسجيل الخروج</span>
            </Button>
          </>
        );
      },
    },
  ];

  function handleLogout(e) {
    axiosInstance.get("/User/Logout").then(() => {
      localStorage.removeItem("token");
      localStorage.removeItem("hashid");
      localStorage.removeItem("id");
      localStorage.removeItem("degree");
      localStorage.removeItem("officeId");
      navigate("/login");
    });
  }

  return (
    <div className="card">
      <Menubar
        model={navbarLinks}
        start={
          <div className="right_header flex">
            <Link to="/">
              <img src={logo} height="60" />
              <span className="prog__title">
                {"مــنظـــومـــة الـــُمقــــابـــــلات "}
              </span>
            </Link>
          </div>
        }
      />
    </div>
  );
};

export default Navbar;
