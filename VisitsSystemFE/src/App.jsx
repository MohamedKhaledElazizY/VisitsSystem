import { BrowserRouter, Routes, Route } from "react-router-dom";
import Navbar from "./components/Navbar/Navbar";
import PrivateRoute from "./components/PrivateRoute/PrivateRoute";
import AuthRoute from "./components/AuthRoute/AuthRoute";
import Home from "./pages/Home/Home";
import Login from "./pages/Login/Login";
import Visitors from "./pages/Visitors/Visitors";
import VisitsEntry from "./pages/VisitsEntry/VisitsEntry";
import VisitsView from "./pages/VisitsView/VisitsView";
import Register from "./pages/Register/Register";
import VisitsHistory from "./pages/VisitsHistory/VisitsHistory";
import StateKeeper from "./components/StateKeeper/StateKeeper";

const App = () => {
  return (
    <StateKeeper>
      <BrowserRouter>
        <Routes>
          <Route exact element={<PrivateRoute />}>
            <Route
              exact
              path="/"
              element={
                <>
                  <Navbar />
                  <Home />
                </>
              }
            />
          </Route>
          <Route exact element={<PrivateRoute />}>
            <Route
              exact
              path="/visitors"
              element={
                <>
                  <Navbar />
                  <Visitors />
                </>
              }
            />
          </Route>
          <Route exact element={<PrivateRoute />}>
            <Route
              exact
              path="/visits-entry"
              element={
                <>
                  <Navbar />
                  <VisitsEntry />
                </>
              }
            />
          </Route>
          <Route exact element={<PrivateRoute />}>
            <Route
              exact
              path="/visits-view"
              element={
                <>
                  <Navbar />
                  <VisitsView />
                </>
              }
            />
          </Route>

          <Route exact element={<PrivateRoute />}>
            <Route
              exact
              path="/register"
              element={
                <>
                  <Navbar />
                  <Register />
                </>
              }
            />
          </Route>

          <Route exact element={<PrivateRoute />}>
            <Route
              exact
              path="/visits-history"
              element={
                <>
                  <Navbar />
                  <VisitsHistory />
                </>
              }
            />
          </Route>

          <Route exact element={<AuthRoute />}>
            <Route exact path="/login" element={<Login />} />
          </Route>
        </Routes>
      </BrowserRouter>
    </StateKeeper>
  );
};

export default App;
