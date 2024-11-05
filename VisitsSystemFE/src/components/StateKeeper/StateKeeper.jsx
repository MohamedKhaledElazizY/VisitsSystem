import React from "react";
import { API_BASE_URL } from "../../data";
import axios from "axios";

export const axiosInstance = axios.create({});

const StateKeeper = ({ children }) => {
  axiosInstance.defaults.baseURL = API_BASE_URL;
  axiosInstance.defaults.headers.common.Authorization = `Bearer ${localStorage.getItem(
    "token"
  )}`;
  axiosInstance.defaults.headers.common.ID = JSON.parse(
    localStorage.getItem("id")
  );
  axiosInstance.defaults.headers.common.hashid = localStorage.getItem("hashid");
  axiosInstance.interceptors.response.use(undefined, function (error) {
    if (error.response.status === 401) {
      localStorage.clear();
      window.location.reload();
    }
    if (error.response.status === 400) {
      if (
        error.response.data === "invalid authed id" ||
        error.response.data === "not logged in" ||
        error.response.data === "invalid hased id"
      ) {
        localStorage.clear();
        window.location.reload();
      }
    }
    return Promise.reject(error);
  });

  return <>{children}</>;
};

export default StateKeeper;
