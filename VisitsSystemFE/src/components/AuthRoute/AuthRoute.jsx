import React from "react";
import { Outlet, Navigate } from "react-router-dom";

export default function AuthRoute() {
  return (
    <>{localStorage.getItem("token") ? <Navigate to="/" /> : <Outlet />}</>
  );
}
