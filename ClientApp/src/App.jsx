import { useState, useEffect } from "react";
import "./styles/App.scss";
import Home from "./pages/Home";
import Login from "./pages/Login";
import Inventory from "./pages/Inventory";
import Purchases from "./pages/Purchases";
import Sales from "./pages/Sales";
import CashBox from "./pages/CashBox";
// import Reports from "./pages/Reports";
import Users from "./pages/Users";
import Config from "./pages/Config";
import Sidebar from "./components/sidebar/Sidebar";
import Menu from "./pages/Menu";
import Providers from "./pages/Providers";
import Customers from "./pages/Customers";
import IndividualSale from "./pages/IndividualSale";
import GroupSale from "./pages/GroupSale";
import { BrowserRouter, Routes, Route, Link, Navigate } from "react-router-dom";
import { SkeletonTheme } from "react-loading-skeleton";
import { ProtectedRoute } from "./components/ProtectedRoute";
import useAuthStore from "./providers/User";
import Waiters from "./pages/Waiters";
import Reports from "./pages/report/Reports";
// import Rango from "./pages/report/rango";
import Rango from "./pages/report/Rango.jsx";
import Reportmonth from "./pages/report/Reportmonth";
import Reportsweek from "./pages/report/Reportweek.jsx";
import ShoppDay from "./pages/report/ShoppDay";
import Shoppweek from "./pages/report/ShoppWeek";
import ShoppRange from "./pages/report/ShoppRange";
import ShoppMonth from "./pages/report/ShoppMonth";
import ShoppAll from "./pages/report/ShoppAll";
import ReportAll from "./pages/report/ReportAll";
import BoxDay from "./pages/report/BoxDay";
import BoxWeek from "./pages/report/BoxWeek";

function App() {
  const [pageTitle, setPageTitle] = useState("");
  //const [isAuth, setIsAuth] = useState(false) // Estado para controlar si el usuario está autenticado

  //Traemos los datos que necesitemos del estado global
  const datosUsuario = useAuthStore((state) => ({
    isAuth: state.isAuth,
    rol: state.rol,
  }));

  //Traemos el método para cambiar los valores del estado
  const { setUser } = useAuthStore();

  /* Cambiar el encabezado de la página */
  const setTitle = (title) => {
    setPageTitle(title);
  };

  useEffect(() => {
    const token = localStorage.getItem("token"); // Obtener el token almacenado en el navegador
    if (token) {
      setUser(
        true,
        localStorage.getItem("token"),
        localStorage.getItem("user"),
        localStorage.getItem("rol")
      ); // Si el token existe, el usuario está autenticado
    }
  }, []);

  if (!datosUsuario.isAuth) {
    return (
      <>
        <BrowserRouter>
          <Routes>
            <Route path="/login" element={<Login />} />
            <Route path="*" element={<Navigate replace to="/login" />} />
          </Routes>
        </BrowserRouter>
      </>
    );
  }

  return (
    <>
      <SkeletonTheme baseColor="#313131" highlightColor="#525252">
        <BrowserRouter>
          {/* Sidebar */}
          <Sidebar pageTitle={pageTitle}>
            <Routes>
              {/* Rutas que pueden ser accedidas por usuarios autenticados*/}
              <Route
                exact
                path="/"
                element={
                  <ProtectedRoute isAllowed={datosUsuario.isAuth}>
                    <Home setTitle={setTitle} />
                  </ProtectedRoute>
                }
              />
              {/* Rutas que pueden ser accedidas por usuarios autenticados y que sean 'Administrador' */}
              <Route
                element={
                  <ProtectedRoute
                    redirectTo="/"
                    isAllowed={
                      datosUsuario.isAuth &&
                      datosUsuario.rol === "Administrador"
                    }
                  />
                }
              >
                <Route
                  path="/cash-box"
                  element={<CashBox setTitle={setTitle} />}
                />
                <Route path="/users" element={<Users setTitle={setTitle} />} />
                <Route
                  path="/reports"
                  element={<Reports setTitle={setTitle} />}
                />
                <Route
                  path="/reports/week"
                  element={<Reportsweek setTitle={setTitle} />}
                />
                <Route
                  path="/reports/month"
                  element={<Reportmonth setTitle={setTitle} />}
                />
                <Route
                  path="/reports/rango"
                  element={<Rango setTitle={setTitle} />}
                />
                <Route
                  path="/reports/all"
                  element={<ReportAll setTitle={setTitle} />}
                />
                <Route
                  path="/purchasesday"
                  element={<ShoppDay setTitle={setTitle} />}
                />
                <Route
                  path="/purchasesweek"
                  element={<Shoppweek setTitle={setTitle} />}
                />
                <Route
                  path="/purchasesrange"
                  element={<ShoppRange setTitle={setTitle} />}
                />
                <Route
                  path="/purchasesmonth"
                  element={<ShoppMonth setTitle={setTitle} />}
                />
                <Route
                  path="/purchasesall"
                  element={<ShoppAll setTitle={setTitle} />}
                />
                <Route
                  path="/boxday"
                  element={<BoxDay setTitle={setTitle} />}
                />
                <Route
                  path="/boxweek"
                  element={<BoxWeek setTitle={setTitle} />}
                />
                <Route
                  path="/boxmonth"
                  element={<BoxWeek setTitle={setTitle} />}
                />
                <Route
                  path="/config"
                  element={<Config setTitle={setTitle} />}
                />
                <Route path="/menu" element={<Menu setTitle={setTitle} />} />
                <Route
                  path="/providers"
                  element={<Providers setTitle={setTitle} />}
                />
                <Route
                  path="/customers"
                  element={<Customers setTitle={setTitle} />}
                />
                <Route
                  path="/waiters"
                  element={<Waiters setTitle={setTitle} />}
                />
                {/* Colocar las demas */}
              </Route>
              {/* Rutas que pueden ser accedidas por usuarios autenticados y que sean 'Invitado' o 'Administrador' */}
              <Route
                element={
                  <ProtectedRoute
                    redirectTo="/"
                    isAllowed={
                      datosUsuario.isAuth &&
                      (datosUsuario.rol === "Administrador" ||
                        datosUsuario.rol === "Invitado")
                    }
                  />
                }
              >
                <Route path="/sales" element={<Sales setTitle={setTitle} />} />
                <Route
                  path="/inventory"
                  element={<Inventory setTitle={setTitle} />}
                />
                <Route
                  path="/purchases"
                  element={<Purchases setTitle={setTitle} />}
                />
                <Route
                  path="/new"
                  element={<IndividualSale setTitle={setTitle} />}
                />
                <Route
                  path="/group"
                  element={<GroupSale setTitle={setTitle} />}
                />
                {/* Colocar las demas */}
              </Route>
              {/* */}

              <Route path="*" element={<Navigate replace to="/" />} />
            </Routes>
          </Sidebar>
        </BrowserRouter>
      </SkeletonTheme>
    </>
  );
}

export default App;
