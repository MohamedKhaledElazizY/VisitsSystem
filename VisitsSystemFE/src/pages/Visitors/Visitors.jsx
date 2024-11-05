import React, { useEffect, useState, useRef } from "react";
import { InputText } from "primereact/inputtext";
import { Dropdown } from "primereact/dropdown";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { Ranks, TOAST_LIFETIME } from "../../data";
import { Divider } from "primereact/divider";
import { Toast } from "primereact/toast";
import { Button } from "primereact/button";
import { Dialog } from "primereact/dialog";
import { axiosInstance } from "../../components/StateKeeper/StateKeeper";
import "./visitors.css";

const Visitors = () => {
  const [getVisitors, setGetVisitors] = useState(0);
  const [visitors, setVisitors] = useState(null);
  const [deletemembers, setDeleteMembers] = useState(false);
  const [deletedMember, setDeletedMember] = useState([]);
  const [search, setSearch] = useState({
    visitorName: "",
    jobTitle: "",
    rank: "",
  });

  const toast = useRef(null);

  useEffect(() => {
    axiosInstance
      .get("/Visitors")
      .then((response) => {
        setVisitors([...response.data]);
      })
  }, [getVisitors]);

  const [credentials, setCredentials] = useState({
    rank: "",
    visitorName: "",
    jobTitle: "",
  });

  const [validation, setValidation] = useState({
    form: "",
    rank: "",
    visitorName: "",
    jobTitle: "",
  });

  function handleInputChange(e) {
    setCredentials({
      ...credentials,
      [e.target.name]: e.target.value,
    });
  }

  // start add members===========================
  function validateName(input) {
    if (!input.length) {
      return { value: false, message: "برجاء ادخال الاسم الشخصي" };
    }
    return { value: true, message: "" };
  }

  function validateRank(input) {
    if (!input.length) {
      return { value: false, message: "برجاء ادخال الرتبة" };
    }
    return { value: true, message: "" };
  }

  function handleAdd(e) {
    e.preventDefault();
    const resultName = validateName(credentials.visitorName);
    const resultRank = validateRank(credentials.rank);
    const resultJob = credentials.jobTitle;

    setValidation({
      ...validation,
      visitorName: resultName.message,
      rank: resultRank.message,
      jobTitle: resultJob.message,
    });
    if (resultName.value && resultRank.value) {
      axiosInstance
        .post("/Visitors", credentials)
        .then(() => {
          setGetVisitors(!getVisitors);
          showSuccess();
          setCredentials({
            rank: "",
            visitorName: "",
            jobTitle: "",
          });
        })
        .catch((error) => {
          setValidation({ ...validation, form: "فشلت عملية اضافة فرد جديد" });
        });
    }
  }

  const showSuccess = () => {
    toast.current.show({
      severity: "success",
      summary: "حفظ",
      detail: "تم أضافة فرد جديد بنجاح",
      life: TOAST_LIFETIME,
    });
  };
  // end add members=============================

  // start edit members==========================
  const textEditor = (options) => {
    return (
      <InputText
        type="text"
        value={options.value}
        onChange={(e) => options.editorCallback(e.target.value)}
      />
    );
  };

  const rankEditor = (options) => {
    return (
      <Dropdown
        value={options.value}
        options={Ranks}
        onChange={(e) => options.editorCallback(e.value)}
        placeholder="أختر الرتبة"
      />
    );
  };

  const onRowEditComplete = (e) => {
    let _visitors = [...visitors];
    let { newData, index } = e;
    _visitors[index] = newData;
    axiosInstance
      .put(`/Visitors/${_visitors[index].visitorId}`, _visitors[index])
      .then((response) => {
        setVisitors(_visitors);
      })
    setVisitors(_visitors);
  };
  // end edit members=============================

  // start delete members=========================
  function handleDelete() {
    let _visitors = visitors.filter(
      (val) => val.visitorId !== deletedMember.visitorId
    );
    axiosInstance
      .delete(`/Visitors/?VisitorID=${deletedMember.visitorId}`)
      .then(() => {
        setVisitors(_visitors);
        hideDeleteMembersDialog();
      })
  }

  const actionBodyTemplate = (data) => {
    return (
      <Button
        type="button"
        icon="pi pi-trash"
        rounded
        outlined
        severity="danger"
        onClick={() => prepareDeletedMember(data)}
      />
    );
  };

  function prepareDeletedMember(data) {
    setDeleteMembers(true);
    setDeletedMember(data);
  }

  const hideDeleteMembersDialog = () => {
    setDeleteMembers(false);
  };

  const deleteProductDialogFooter = (
    <React.Fragment>
      <Button
        label="لا"
        icon="pi pi-times"
        outlined
        onClick={hideDeleteMembersDialog}
      />
      <Button
        label="نعم"
        icon="pi pi-check"
        severity="danger"
        onClick={handleDelete}
      />
    </React.Fragment>
  );
  // end delete members===========================

  // start search members=========================
  function handleSearch(e) {
    e.preventDefault();
    axiosInstance
      .get(
        `/Visitors/VisitorsSearch?Name=${search.visitorName}&Rank=${search.rank}&JobTitle=${search.jobTitle}`
      )
      .then((response) => {
        setVisitors([...response.data]);
      })
  }

  function clearSearchCondition(e) {
    e.preventDefault();

    setSearch({
      visitorName: "",
      jobTitle: "",
      rank: "",
    });

    axiosInstance
      .get(`/Visitors/VisitorsSearch?Name=${""}&Rank=${""}&JobTitle=${""}`)
      .then((response) => {
        setVisitors([...response.data]);
      })
  }
  // end search members============================
  return (
    <form action="">
      <div className="container vistors_container">
        {/************************** entry area *************************************/}

        <div className="input_area">
          <div className="field w-full pb-3">
            <span className="p-float-label">
              <InputText
                value={credentials.visitorName}
                name="visitorName"
                className="p-inputtext-lg w-full"
                onChange={handleInputChange}
              />
              <label htmlFor="visitorName">الاسم</label>
            </span>
            <span className="validation__text">{validation.name}</span>
          </div>

          <div className="drop__down">
            <Dropdown
              value={credentials.rank}
              name="rank"
              onChange={handleInputChange}
              options={Ranks}
              editable
              placeholder="الدرجة/الرتبة"
              className="w-full md:w-15rem"
            />
            <span className="validation__text">{validation.rank}</span>
          </div>

          <div className="flex flex-column gap-2 ml-1">
            <span className="p-float-label">
              <InputText
                value={credentials.jobTitle}
                name="jobTitle"
                className="p-inputtext-lg w-full"
                onChange={handleInputChange}
              />
              <label htmlFor="jobTitle">الوظيفة</label>
            </span>
            <span className="validation__text">{validation.jobTitle}</span>
          </div>

          <div className="save__btn">
            <Toast ref={toast} />
            <Button className="btn" onClick={handleAdd}>
              حفظ البيانات
            </Button>
          </div>
        </div>

        <Divider layout="vertical" />

        {/************************** search area ***********************************/}

        <div className="table_area pl-7">
          <div className="card p-fluid flex flex-wrap gap-3">
            <div className="">
              <InputText
                placeholder="الاسم"
                value={search.visitorName}
                onChange={(e) =>
                  setSearch({ ...search, visitorName: e.target.value })
                }
              />
            </div>

            <div className="">
              <Dropdown
                value={search.rank}
                onChange={(e) => setSearch({ ...search, rank: e.target.value })}
                options={Ranks}
                editable
                placeholder="أختر الرتبة"
                className="w-full md:w-16rem"
              />
            </div>

            <div className="">
              <InputText
                placeholder="الوظيفة"
                // keyfilter="num"
                value={search.jobTitle}
                onChange={(e) =>
                  setSearch({ ...search, jobTitle: e.target.value })
                }
              />
            </div>

            <div className="search__btn">
              <Button type="submit" className="btn" onClick={handleSearch}>
                بحث
              </Button>
            </div>
            <div className="undo_search__btn">
              <Button
                type="submit"
                className="btn"
                onClick={clearSearchCondition}
              >
                الغاء شروط البحث
              </Button>
            </div>
          </div>

          <Divider type="solid" />

          {/************************** table area *************************************/}

          <div className="card p-fluid">
            <DataTable
              value={visitors}
              editMode="row"
              dataKey="visitorId"
              size="small"
              rows={7}
              onRowEditComplete={onRowEditComplete}
              resizableColumns
              showGridlines
              tableStyle={{ minWidth: "50rem" }}
              paginator
            >
              <Column
                field="visitorId"
                header="الكود الخاص"
                style={{ width: "10%" }}
              />
              <Column
                field="visitorName"
                header="الاسم"
                editor={(options) => textEditor(options)}
                style={{ width: "30%" }}
              />
              <Column
                field="jobTitle"
                header="الوظيفة"
                editor={(options) => textEditor(options)}
                style={{ width: "20%" }}
              />
              <Column
                field="rank"
                header="الرتبة/الدرجة"
                editor={(options) => rankEditor(options)}
                style={{ width: "20%" }}
              />
              <Column
                rowEditor
                headerStyle={{ width: "10%", minWidth: "8rem" }}
                bodyStyle={{ textAlign: "center" }}
              />
              <Column
                body={actionBodyTemplate}
                exportable={false}
                style={{ minWidth: "6rem" }}
                bodyStyle={{ textAlign: "center" }}
              />
            </DataTable>
          </div>
        </div>

        <Dialog
          visible={deletemembers}
          style={{ width: "32rem" }}
          breakpoints={{ "960px": "75vw", "641px": "90vw" }}
          header="حذف فرد"
          modal
          footer={deleteProductDialogFooter}
          onHide={hideDeleteMembersDialog}
        >
          <div className="confirmation-content">
            <i
              className="pi pi-exclamation-triangle mr-3"
              style={{ fontSize: "2rem" }}
            />
            <span className="mr-2">هل تريد حذف هذا الفرد؟</span>
          </div>
        </Dialog>
      </div>
    </form>
  );
};

export default Visitors;
