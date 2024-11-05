import React, { useState, useEffect } from "react";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { axiosInstance } from "../../components/StateKeeper/StateKeeper";

const VisitsHistory = () => {
  const [history, setHistory] = useState([]);

  useEffect(() => {
    axiosInstance
      .get(`/Visits/finished/${JSON.parse(localStorage.getItem("officeId"))}`, {
        headers: {
          Accept: "text/plain",
        },
      })
      .then((response) => {
        setHistory([...response.data]);
      })
  }, []);

  return (
    <div className="history__container">
      <div className="flex justify-content-center pt-5">
        <DataTable
          value={history}
          size="small"
          rows={15}
          showGridlines
          resizableColumns
          tableStyle={{ minWidth: "95rem" }}
          paginator
        >
          <Column
            field="visitor.rank"
            header="الرتبة"
            style={{ width: "10%" }}
          />
          <Column
            field="visitor.visitorName"
            header="الاسم الشخصى"
            style={{ width: "15%" }}
          />
          <Column
            field="visitor.jobTitle"
            header="الوظيفة"
            style={{ width: "20%" }}
          />

          <Column
            // field="arrivalDate"
            header="توقيت الوصول"
            style={{ width: "5%" }}
            body={(data) => {
              let datetime = data.arrivalDate.replace("T", " | ");
              const final = datetime.split(":");
              return (
                <span>
                  {final[0]}:{final[1]}
                </span>
              );
            }}
          />
          <Column
            // field="entryDate"
            header="توقيت الدخول"
            style={{ width: "5%" }}
            body={(data) => {
              if (data.entryDate) {
                const time = new Date(data.entryDate).toTimeString().split(":");
                return (
                  <span>
                    {time[0]}:{time[1]}
                  </span>
                );
              }
            }}
          />
          <Column
            // field="leavingDate"
            header="توقيت المغادرة"
            style={{ width: "5%" }}
            body={(data) => {
              if (data.leavingDate) {
                const time = new Date(data.leavingDate)
                  .toTimeString()
                  .split(":");
                return (
                  <span>
                    {time[0]}:{time[1]}
                  </span>
                   
                );
              }
            }}
          />

          <Column field="notes" header="ملاحظات" style={{ width: "20%" }} />
          <Column
            field="state"
            header="حالة المقابلة"
            style={{ width: "10%" }}
          />
        </DataTable>
      </div>
    </div>
  );
};

export default VisitsHistory;
