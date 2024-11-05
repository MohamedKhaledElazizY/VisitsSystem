import React, { useRef, useEffect, useState } from "react";
import { Button } from "primereact/button";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { ProgressSpinner } from "primereact/progressspinner";
import { Toast } from "primereact/toast";
import { Calendar } from "primereact/calendar";
import { Dialog } from "primereact/dialog";
import { SOCKET_URL, TOAST_LIFETIME } from "../../data";
import { HubConnectionBuilder } from "@microsoft/signalr";
import { axiosInstance } from "../../components/StateKeeper/StateKeeper";
import notificationAudio from "../../assets/sounds/notification.wav";
import "./VisitsView.css";

const VisitsView = () => {
  const toast = useRef(null);
  const [postponeDate, setPostponeDate] = useState("");
  const [postponeTime, setPostponeTime] = useState("");
  const [postponeId, setPostponeId] = useState(null);
  const [showPostponeDialog, setShowPostponeDialog] = useState(false);
  const [showTodaysPostponedDialog, setshowTodaysPostponedDialog] =
    useState(false);
  const [todaysPostponed, setTodaysPostponed] = useState([]);
  const [visits, setVisits] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [socket, setSocket] = useState(null);
  const audioPlayer = useRef(null);

  useEffect(() => {
    fetchCurrentVisits();
    const connect = new HubConnectionBuilder()
      .withUrl(SOCKET_URL)
      .withAutomaticReconnect()
      .build();
    setSocket(connect);
    setIsLoading(false);
  }, []);

  useEffect(() => {
    if (socket) {
      socket
        .start()
        .then(() => {
          socket.on("ReceiveVisits", (notification) => {
            const { message, officeId, degree } = JSON.parse(notification);
            if (
              officeId === localStorage.getItem("officeId") &&
              degree === "false"
            ) {
              if (message !== "visit exit") {
                audioPlayer.current.play();
              }
              fetchCurrentVisits();
            }
          });
        })
    }
  }, [socket]);

  function fetchCurrentVisits() {
    axiosInstance
      .get(`/Visits/current/${JSON.parse(localStorage.getItem("officeId"))}`)
      .then((response) => {
        setVisits([...response.data]);
      })
  }

  const rowClass = (data) => {
    switch (data.state) {
      case 0:
        return "arrived__row";
      case 1:
        return "entered__row";
      case 2:
        return "wait__row";
      case 3:
        return "canceled__row";
      case 4:
        return "postponed__row";
      default:
        break;
    }
  };

  const dialogFooterContent = (
    <>
      <Button
        className="direction__ltr"
        label="تنفيذ"
        icon="pi pi-check"
        onClick={handlePostpone}
      />
      <Button
        className="direction__ltr"
        label="الغاء"
        icon="pi pi-times"
        onClick={() => setShowPostponeDialog(false)}
      />
    </>
  );

  function handleEnter(visitId) {
    axiosInstance
      .put(`/Visits/${visitId}`, { stateId: 1 })
      .then(() => {
        socket.send(
          "SendVisits",
          JSON.stringify({
            message: "visit enter",
            officeId: localStorage.getItem("officeId"),
            degree: localStorage.getItem("degree"),
          })
        );
        fetchCurrentVisits();
      })
  }

  function handleWait(visitId) {
    axiosInstance
      .put(`/Visits/${visitId}`, { stateId: 2 })
      .then(() => {
        socket.send(
          "SendVisits",
          JSON.stringify({
            message: "visit wait",
            officeId: localStorage.getItem("officeId"),
            degree: localStorage.getItem("degree"),
          })
        );
        fetchCurrentVisits();
      })
  }

  function handleCancel(visitId) {
    axiosInstance
      .put(`/Visits/${visitId}`, { stateId: 3 })
      .then(() => {
        socket.send(
          "SendVisits",
          JSON.stringify({
            message: "visit cancel",
            officeId: localStorage.getItem("officeId"),
            degree: localStorage.getItem("degree"),
          })
        );
        fetchCurrentVisits();
      })
  }

  function handlePostpone() {
    if (!postponeDate || !postponeTime) {
      toast.current.show({
        severity: "warn",
        summary: "برجاء اختيار التاريخ والوقت",
        life: TOAST_LIFETIME,
      });
      return;
    }
    let date = new Date(postponeDate).toLocaleDateString();
    let time = new Date(postponeTime).toTimeString().split(":");
    time = `${time[0]}:${time[1]}`;
    axiosInstance
      .put(`/Visits/${postponeId}`, {
        stateId: 4,
        date: `${date} ${time}`,
      })
      .then(() => {
        socket.send(
          "SendVisits",
          JSON.stringify({
            message: "visit postpone",
            officeId: localStorage.getItem("officeId"),
            degree: localStorage.getItem("degree"),
          })
        );
        fetchCurrentVisits();
        setShowPostponeDialog(false);
      })
  }

  function showTodaysPostponed() {
    axiosInstance
      .get(`/Visits/postponed/${JSON.parse(localStorage.getItem("officeId"))}`)
      .then((response) => {
        setshowTodaysPostponedDialog(true);
        setTodaysPostponed([...response.data]);
      })
  }

  function callSecretary() {
    socket.send(
      "SendVisits",
      JSON.stringify({
        message: "call",
        officeId: localStorage.getItem("officeId"),
        degree: localStorage.getItem("degree"),
      })
    );
  }

  return (
    <div className="p-4">
      {isLoading ? (
        <ProgressSpinner className="flex justify-content-center" />
      ) : (
        <div>
          <Toast ref={toast} />
          <audio src={notificationAudio} ref={audioPlayer} hidden></audio>
          <Dialog
            visible={showPostponeDialog}
            onHide={() => setShowPostponeDialog(false)}
            header="معاد المقابلة المؤجلة"
            footer={dialogFooterContent}
            position="top"
            resizable={false}
            draggable={false}
            closeOnEscape={true}
            blockScroll={true}
          >
            <div className="flex flex-column gap-3">
              <div className="flex gap-4">
                <label>التاريخ</label>
                <Calendar
                  value={postponeDate}
                  onChange={(e) => setPostponeDate(e.value)}
                  dateFormat="yy-mm-dd"
                  minDate={new Date()}
                />
              </div>
              <div className="flex gap-4">
                <label>الوقت</label>
                <Calendar
                  value={postponeTime}
                  onChange={(e) => setPostponeTime(e.value)}
                  hourFormat="24"
                  minDate={new Date()}
                  timeOnly
                />
              </div>
            </div>
          </Dialog>
          <Dialog
            visible={showTodaysPostponedDialog}
            onHide={() => setshowTodaysPostponedDialog(false)}
            className="w-full"
            header="مؤجلات اليوم"
            position="top"
            resizable={false}
            draggable={false}
            closeOnEscape={true}
            blockScroll={true}
          >
            <DataTable
              value={todaysPostponed}
              rowClassName="postponed__row"
              emptyMessage="لا يوجد مقابلات مؤجلة اليوم"
              showGridlines
            >
              <Column
                header="الاسم"
                body={(data) => (
                  <span>
                    {data.visit.visitor.rank} / {data.visit.visitor.visitorName}
                  </span>
                )}
                style={{ maxWidth: "100px" }}
              />
              <Column
                field="visit.visitor.jobTitle"
                header="الوظيفة"
                style={{ maxWidth: "100px" }}
              />
              <Column
                header="توقيت الوصول"
                body={(data) => {
                  const time = new Date(data.visit.arrivalDate)
                    .toTimeString()
                    .split(":");
                  return (
                    <span>
                      {time[0]}:{time[1]}
                    </span>
                  );
                }}
                style={{ maxWidth: "60px" }}
              />
              <Column
                field="visit.notes"
                header="ملاحظات"
                className="text-overflow-ellipsis overflow-x-hidden"
                style={{ maxWidth: "120px" }}
              />
              <Column
                header="توقيت الدخول"
                body={(data) => {
                  if (data.visit.entryDate) {
                    const time = new Date(data.visit.entryDate)
                      .toTimeString()
                      .split(":");
                    return (
                      <span>
                        {time[0]}:{time[1]}
                      </span>
                    );
                  }
                }}
                style={{ maxWidth: "60px" }}
              />
            </DataTable>
          </Dialog>
          <div className="flex justify-content-between w-3 mb-4">
            <Button
              className="direction__ltr"
              label="إستدعاء السكرتارية"
              onClick={callSecretary}
            />
            <Button
              className="direction__ltr"
              label="عرض مؤجلات اليوم"
              onClick={showTodaysPostponed}
            />
          </div>
          <div className="w-full flex justify-content-center">
            <span className="text-4xl font-bold">
              {new Date().toJSON().split("T")[0]}
            </span>
          </div>
          <DataTable
            value={visits}
            rowClassName={rowClass}
            emptyMessage="لا يوجد مقابلات اليوم"
            showGridlines
          >
            <Column
              header="الاسم"
              body={(data) => (
                <span>
                  {data.visitor.rank} / {data.visitor.visitorName}
                </span>
              )}
              style={{ maxWidth: "100px" }}
            />
            <Column
              field="visitor.jobTitle"
              header="الوظيفة"
              style={{ maxWidth: "100px" }}
            />
            <Column
              field="arrivalDate"
              header="توقيت الوصول"
              body={(data) => {
                const time = new Date(data.arrivalDate)
                  .toTimeString()
                  .split(":");
                return (
                  <span>
                    {time[0]}:{time[1]}
                  </span>
                );
              }}
              style={{ maxWidth: "60px" }}
            />
            <Column
              field="notes"
              header="ملاحظات"
              className="text-overflow-ellipsis overflow-x-hidden"
              style={{ maxWidth: "120px" }}
            />
            <Column
              header="توقيت الدخول"
              body={(data) => {
                if (data.entryDate) {
                  const time = new Date(data.entryDate)
                    .toTimeString()
                    .split(":");
                  return (
                    <span>
                      {time[0]}:{time[1]}
                    </span>
                  );
                }
              }}
              style={{ maxWidth: "60px" }}
            />
            <Column
              header="خيارات"
              body={(data) => (
                <div className="flex gap-2">
                  <Button
                    label="دخول"
                    onClick={() => handleEnter(data.visitId)}
                  />
                  <Button
                    label="انتظار"
                    onClick={() => handleWait(data.visitId)}
                  />
                  <Button
                    label="الغاء"
                    onClick={() => handleCancel(data.visitId)}
                  />
                  <Button
                    label="تأجيل"
                    onClick={() => {
                      setShowPostponeDialog(true);
                      setPostponeId(data.visitId);
                    }}
                  />
                </div>
              )}
              style={{ maxWidth: "150px" }}
            />
          </DataTable>
        </div>
      )}
    </div>
  );
};

export default VisitsView;
