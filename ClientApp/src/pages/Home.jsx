import React, { useEffect, useState } from "react";
import { useStore } from "../providers/GlobalProvider";
import Widget from "../components/charts/widgets/Widget";
// import Featured from "../components/featured/Featured";
import Chart from "../components/charts/chart/Chart";

function Home(props) {
  const [ganancias, setGanancias] = useState(false);
  const [datosAnuales, setDatosAnuales] = useState([]);

  const requestOptions = {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${localStorage.token}`,
    },
  };

  const datosDashboard = async () => {
    const response = await fetch(
      "http://localhost:5173/api/Dashboard/total",
      requestOptions
    );
    const data = await response.json();
    setGanancias(data);
  };

  const datosGrafica = async () => {
    const response = await fetch(
      "http://localhost:5173/api/Dashboard/ganancias",
      requestOptions
    );
    const data = await response.json();
    setDatosAnuales(data);
  };

  useEffect(() => {
    datosDashboard();
    datosGrafica();
  }, []);

  const isOpen = useStore((state) => state.sidebar);

  return (
    <div className={isOpen === true ? "wrapper" : "side"}>
      <div className="home">
        <div
          className="homeContainer"
          style={{ marginTop: "1rem", marginRight: "1rem" }}
        >
          <div className="widgets" style={{ gap: "1rem" }}>
            <Widget
              type={"ingresosMes"}
              monto={ganancias ? ganancias.totalVentasMesActual.toFixed(2) : 0}
              porcentaje={ganancias.porcentajeCambioVentas}
            />
            <Widget
              type={"gastosMes"}
              monto={ganancias ? ganancias.totalGastosMesActual.toFixed(2) : 0}
              porcentaje={ganancias.porcentajeCambioGastos}
            />
            <Widget
              type={"gananciaMes"}
              monto={ganancias ? ganancias.ganancia.toFixed(2) : 0}
              porcentaje={ganancias.porcentajeCambioGanancia}
            />
            <Widget
              type={"saldoCaja"}
              monto={ganancias.cajaActual ? ganancias.cajaActual.toFixed(2) : 0}
              porcentaje={
                ganancias.porcentajeCambioCaja
                  ? ganancias.porcentajeCambioCaja
                  : 0
              }
            />
          </div>
          <div className="charts">
            {/* <Featured /> */}
            <Chart datos={datosAnuales} />
          </div>
        </div>
      </div>
    </div>
  );
}

export default Home;
