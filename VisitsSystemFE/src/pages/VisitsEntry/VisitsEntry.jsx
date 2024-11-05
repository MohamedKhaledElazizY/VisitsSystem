import React, { useRef, useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { Dropdown } from "primereact/dropdown";
import { InputText } from "primereact/inputtext";
import { InputTextarea } from "primereact/inputtextarea";
import { InputMask } from "primereact/inputmask";
import { Button } from "primereact/button";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { ProgressSpinner } from "primereact/progressspinner";
import { ConfirmDialog, confirmDialog } from "primereact/confirmdialog";
import { Toast } from "primereact/toast";
import { SOCKET_URL, TOAST_LIFETIME } from "../../data";
import { axiosInstance } from "../../components/StateKeeper/StateKeeper";
import notificationAudio from "../../assets/sounds/notification.wav";
import callNotificationAudio from "../../assets/sounds/call-to-attention-123107.wav";
import { HubConnectionBuilder } from "@microsoft/signalr";
import rank1 from "../../assets/images/1.png";
import rank2 from "../../assets/images/2.png";
import rank3 from "../../assets/images/3.png";
import rank4 from "../../assets/images/4.png";
import rank5 from "../../assets/images/5.png";
import rank6 from "../../assets/images/6.png";
import rank7 from "../../assets/images/7.png";
import rank8 from "../../assets/images/8.png";
import rank12 from "../../assets/images/12.png";
import rank13 from "../../assets/images/13.png";
import rank14 from "../../assets/images/14.png";
import rank15 from "../../assets/images/15.png";
import rank16 from "../../assets/images/16.png";
import "./VisitsEntry.css";

export const mappedRanks = {
  "لواء أ ح": rank8,
  لواء: rank8,
  "عميد أ ح": rank7,
  عميد: rank7,
  "عقيد أح": rank6,
  عقيد: rank6,
  "مقدم أ ح": rank5,
  مقدم: rank5,
  رائد: rank4,
  نقيب: rank3,
  "ملازم أ": rank2,
  ملازم: rank1,
  "مقدم ش": rank5,
  "رائد ش": rank4,
  "نقيب ش": rank3,
  "ملازم أ.ش": rank2,
  "ملازم ش": rank1,
  "عميد مهندس": rank7,
  "عميد طبيب": rank7,
  "عقيد مهندس": rank6,
  "عقيد طبيب": rank6,
  "مقدم طبيب": rank5,
  "رائد ط": rank4,
  "رائد ط ب": rank4,
  "م أ ح": rank2,
  مساعد: rank12,
  "مساعد اول": rank13,
  عريف: rank14,
  رقيب: rank15,
  "رقيب اول": rank16,
};

const VisitsEntry = () => {
  const toast = useRef(null);
  const [visitors, setVisitors] = useState([]);
  const [selectedVisitor, setSelectedVisitor] = useState({
    visitorId: null,
    rank: "",
    visitorName: "",
    jobTitle: "",
  });
  const [visits, setVisits] = useState([]);
  const [visit, setVisit] = useState({
    notes: "",
    visitorId: null,
  });
  const [arrivalDate, setArrivalDate] = useState("");
  const [isLoading, setIsLoading] = useState(true);
  const [socket, setSocket] = useState(null);
  const audioPlayer = useRef(null);
  const callAudioPlayer = useRef(null);

  useEffect(() => {
    axiosInstance
      .get("/Visitors")
      .then((response) => {
        setVisitors([...response.data]);
      })
    const connect = new HubConnectionBuilder()
      .withUrl(SOCKET_URL)
      .withAutomaticReconnect()
      .build();
    setSocket(connect);
    fetchCurrentVisits();
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
              degree === "true"
            ) {
              if (message === "call") {
                callAudioPlayer.current.play();
              } else {
                audioPlayer.current.play();
              }
              fetchCurrentVisits();
            }
          });
        })
    }
  }, [socket]);

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

  function fetchCurrentVisits() {
    axiosInstance
      .get(`/Visits/current/${JSON.parse(localStorage.getItem("officeId"))}`)
      .then((response) => {
        setVisits([...response.data]);
      })
  }

  function handleChangeVisitor(e) {
    setSelectedVisitor(e.value);
    const currentTime = new Date().toTimeString().split(":");
    setArrivalDate(`${currentTime[0]}:${currentTime[1]}`);
    setVisit({ ...visit, visitorId: e.value.visitorId });
  }

  function selectedVisitorTemplate(option, props) {
    if (option) {
      return (
        <div className="flex align-items-center">
          <img
            alt={option.visitorName}
            src={mappedRanks[option.rank]}
            className="dropdown__image dropdown__image--selected"
          />
          <div>{option.visitorName}</div>
        </div>
      );
    }
    return <span>{props.placeholder}</span>;
  }

  function visitorOptionTemplate(option) {
    return (
      <div className="flex align-items-center">
        <img
          alt={option.visitorName}
          src={mappedRanks[option.rank]}
          className="dropdown__image"
        />
        <div>{option.visitorName}</div>
      </div>
    );
  }

  function handleInputChange(e) {
    setVisit({ ...visit, notes: e.target.value });
  }

  function confirmAddVisit() {
    confirmDialog({
      message: "هل تريد حفظ المقابلة",
      header: "تأكيد الحفظ",
      icon: "pi pi-exclamation-triangle",
      accept: handleAddVisit,
      acceptLabel: "نعم",
      rejectLabel: "لا",
      position: "center",
      resizable: false,
      draggable: false,
      closeOnEscape: true,
      blockScroll: true,
    });
  }

  function handleAddVisit() {
    if (visit.visitorId) {
      axiosInstance
        .post(`/Visits/${JSON.parse(localStorage.getItem("officeId"))}`, visit)
        .then(() => {
          toast.current.show({
            severity: "info",
            summary: "تم حفظ المقابلة",
            life: TOAST_LIFETIME,
          });
          socket.send(
            "SendVisits",
            JSON.stringify({
              message: "new visit",
              officeId: localStorage.getItem("officeId"),
              degree: localStorage.getItem("degree"),
            })
          );
          fetchCurrentVisits();
          setVisit({
            notes: "",
            visitorId: null,
          });
          setArrivalDate("");
          setSelectedVisitor({
            visitorId: null,
            rank: "",
            visitorName: "",
            jobTitle: "",
          });
        })
    } else {
      toast.current.show({
        severity: "warn",
        summary: "برجاء ادخال اسم الشخص والملاحظات",
        life: TOAST_LIFETIME,
      });
    }
  }

  function handleExit(visitId) {
    axiosInstance
      .put(`/Visits/end/${visitId}`)
      .then(() => {
        socket.send(
          "SendVisits",
          JSON.stringify({
            message: "visit exit",
            officeId: localStorage.getItem("officeId"),
            degree: localStorage.getItem("degree"),
          })
        );
        fetchCurrentVisits();
      })
  }

  return (
    <div className="p-4">
      <section className="mb-4">
        <Toast ref={toast} />
        <ConfirmDialog />
        <audio src={callNotificationAudio} ref={callAudioPlayer} hidden></audio>
        <audio src={notificationAudio} ref={audioPlayer} hidden></audio>
        <div className="flex gap-2 align-items-end mb-2">
          <div className="flex flex-column w-3">
            <label className="table__cell">اسم الشخص</label>
            <Dropdown
              className="h-4rem"
              value={selectedVisitor}
              onChange={handleChangeVisitor}
              options={visitors}
              filterBy="visitorName"
              emptyFilterMessage="الاسم غير موجود"
              optionLabel="visitorName"
              placeholder="اختر الاسم"
              valueTemplate={selectedVisitorTemplate}
              itemTemplate={visitorOptionTemplate}
              filter
            />
          </div>
          <div className="flex flex-column">
            <label className="table__cell">الوظيفة</label>
            <InputText
              className="w-13rem h-4rem"
              value={selectedVisitor.jobTitle}
              readOnly
            />
          </div>
          <div className="flex flex-column">
            <label className="table__cell">توقيت الوصول</label>
            <InputMask
              className="w-6rem h-4rem"
              mask="99:99"
              placeholder="00:00"
              value={arrivalDate}
              readOnly
            />
          </div>
          <div className="flex flex-column">
            <label className="table__cell">ملاحظات</label>
            <InputTextarea
              value={visit.notes}
              onChange={handleInputChange}
              rows={2}
              cols={30}
              autoResize
            />
          </div>
          <Button
            type="button"
            label="حفظ المقابلة"
            className="h-4rem w-auto"
            onClick={confirmAddVisit}
          />
        </div>
      </section>
      {isLoading ? (
        <ProgressSpinner className="flex justify-content-center" />
      ) : (
        <section className="">
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
              header="توقيت الوصول"
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
                <Button label="خروج" onClick={() => handleExit(data.visitId)} />
              )}
              style={{ maxWidth: "40px" }}
            />
          </DataTable>
        </section>
      )}
    </div>
  );
};

export default VisitsEntry;
