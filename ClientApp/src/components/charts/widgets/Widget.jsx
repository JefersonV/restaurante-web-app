import React, { useEffect, useState } from "react";
import * as FaIcons from "react-icons/fa";
import { Link } from "react-router-dom";
import "./widget.scss";

const Widget = ({ type }) => {
/*   //Urls para las peticiones
  const urlSuma = "http://localhost:3000/dashboard/suma";
  const urlBolsasVend = "http://localhost:3000/dashboard/bolsas-vendidas";
  const urlBolsasDisp = "http://localhost:3000/dashboard/bolsas-disponibles";
  const urlClientesFrec = "http://localhost:3000/dashboard/clientes-frec";

  //estado para la suma de todas las ventas del mes
  const [venta, setVenta] = useState([]);
  //estado para las bolsas vendidas durante el mes
  const [bolsasVendidas, setBolsasVendidas] = useState([]);
  //estado para las bolsas disponibles para la venta
  const [bolsasDispon, setBolsasDispon] = useState([]);
  //estado para clientes frecuentas
  const [clientesFrec, setClientesFrec] = useState([]);

  //Funcion para obtener la lista de datos
  const getSumaVenta = async (url) => {
    const response = await fetch(url, {
      headers: {
        token: localStorage.token,
      },
    });
    const data = await response.json();
    setVenta(data[0]);
  };
  //Funcion para obtener la lista de datos
  const getBolsasVend = async (url) => {
    const response = await fetch(url, {
      headers: {
        token: localStorage.token,
      },
    });
    const data = await response.json();
    setBolsasVendidas(data[0]);
  };
  //Funcion para obtener la lista de datos
  const getBolsasDisp = async (url) => {
    const response = await fetch(url, {
      headers: {
        token: localStorage.token,
      },
    });
    const data = await response.json();
    setBolsasDispon(data[0]);
  };
  //Funcion para obtener la lista de datos
  const getClientesFrec = async (url) => {
    const response = await fetch(url, {
      headers: {
        token: localStorage.token,
      },
    });
    const data = await response.json();
    setClientesFrec(data);
  };
  //funcion useffect para llamar y cargar los datos
  useEffect(() => {
    getSumaVenta(urlSuma);
    getBolsasVend(urlBolsasVend);
    getBolsasDisp(urlBolsasDisp);
    getClientesFrec(urlClientesFrec);
  }, []);

  let data = {}; */

  let data = {}
  switch (type) {
    case "ventasMes":
      data = {
        title: "VENTAS DEL MES",
        isMoney: true,
        link: "Ver todas las ventas",
        path: "/sales",
        monto: 5000,
        // monto: venta.suma_total,
        icon: (
          <FaIcons.FaShoppingCart
            className="icon"
            style={{
              color: "crimson",
              backgroundColor: "rgba(255, 0, 0, 0.2)",
            }}
          />
        ),
      };
      break;
    case "bolsasVend":
      data = {
        title: "BOLSAS VENDIDAS",
        isMoney: false,
        link: "Ver todas...",
        path: "/sales",
        monto: 5000,
        // monto: bolsasVendidas.bolsas_vendidas,
        icon: (
          <FaIcons.FaShoppingCart
            className="icon"
            style={{
              color: "crimson",
              backgroundColor: "rgba(255, 0, 0, 0.2)",
            }}
          />
        ),
      };
      break;
    case "cliente":
      data = {
        title: "CLIENTES FRECUENTES",
        isMoney: false,
        link: "Ver todos...",
        path: "/customers",
        monto: 'someone',
        // monto: clientesFrec.clientes_frecuentes,
        icon: (
          <FaIcons.FaUserAlt
            className="icon"
            style={{
              backgroundColor: "rgba(128, 0, 128, 0.2)",
              color: "purple",
            }}
          />
        ),
      };
      break;
    case "bolsasDisp":
      data = {
        title: "BOLSAS DISPONIBLES",
        isMoney: false,
        link: "Ver todos...",
        path: "/products",
        // monto: bolsasDispon.bolsas_disponibles,
        monto: 500,
        icon: (
          <FaIcons.FaShoppingBasket
            className="icon"
            style={{
              backgroundColor: "rgba(218, 165, 32, 0.2)",
              color: "goldenrod",
            }}
          />
        ),
      };
      break;
    default:
      break;
  }

  return (
    <div className="widget">
      <div className="left">
        <span className="title">{data.title}</span>
        <span className="counter">
          {data.isMoney && "Q"} {data.monto}
        </span>
        {/* <span className="link">{data.link}</span> */}
        <Link className="link" to={data.path}>
          {data.link}
        </Link>
      </div>
      <div className="right">
        <div className="percentage positive">
          <FaIcons.FaAngleUp />
        </div>
        {data.icon}
      </div>
    </div>
  );
};

export default Widget;
