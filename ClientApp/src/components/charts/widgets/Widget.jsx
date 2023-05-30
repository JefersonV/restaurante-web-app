import React, { useEffect, useState } from "react";
import * as FaIcons from "react-icons/fa";
import { BsCash } from "react-icons/bs";
import { AiOutlineStock } from "react-icons/ai";
import { FaCashRegister } from "react-icons/fa";
import { Link } from "react-router-dom";
import "./widget.scss";

const Widget = ({ type, monto, porcentaje = 0 }) => {
  let data = {};
  switch (type) {
    case "ingresosMes":
      data = {
        title: "INGRESOS DEL MES",
        isMoney: true,
        link: "Ver todas...",
        path: "/sales",
        monto: 5000,
        // monto: venta.suma_total,
        icon: <BsCash className="icon" color="black" />,
        colors: { backgroundColor: "#ffc107", color: "black" },
        colorSpam: { color: "black" },
        colorLink: { color: "black"}
      };
      break;
    case "gastosMes":
      data = {
        title: "GASTOS DEL MES",
        isMoney: true,
        link: "Ver todas...",
        path: "/purchases",
        monto: 5000,
        // monto: bolsasVendidas.bolsas_vendidas,
        icon: <FaIcons.FaShoppingCart className="icon" color="white" />,
        colors: { backgroundColor: "#dc3545", color: "white" },
        colorSpam: { color: "white" },
      };
      break;
    case "gananciaMes":
      data = {
        title: "GANANCIA DEL MES",
        isMoney: true,
        link: "Ver todos...",
        path: "/reports",
        monto: 500,
        // monto: clientesFrec.clientes_frecuentes,
        icon: <AiOutlineStock className="icon" color="white" />,
        colors: { backgroundColor: "#198754", color: "white" },
        colorSpam: { color: "white" },
      };
      break;
    case "saldoCaja":
      data = {
        title: "SALDO EN CAJA",
        isMoney: true,
        link: "Ver todos...",
        path: "/cash-box",
        // monto: bolsasDispon.bolsas_disponibles,
        monto: 500,
        icon: <FaCashRegister className="icon" color="black" />,
        colors: { backgroundColor: "#0dcaf0", color: "black" },
        colorSpam: { color: "black" },
        colorLink: { color: "black"}
      };
      break;
    default:
      break;
  }

  return (
    <div className="widget" style={data.colors}>
      <div className="left">
        <span className="title" style={data.colorSpam}>
          {data.title}
        </span>
        <span className="counter" style={data.colorSpam}>
          {data.isMoney && "Q."}{monto}
        </span>
        {/* <span className="link">{data.link}</span> */}
        {/* style={{display: isOpen ? "block" : "none"}} */}
        <Link className="link" to={data.path} style={data.colorLink}>
          {data.link}
        </Link>
      </div>
      <div className="right">
        <div
          className="percentage positive"
          style={{ backgroundColor: "white", borderRadius: "5px" }}
        >
          {porcentaje > 0 ? (
            <>
              <span className="percentage-title" style={{ color: "green" }}>{porcentaje}</span>
              <FaIcons.FaAngleUp color="green" />
            </>
          ) : (
            <>
              <span className="percentage-title" style={{ color: "red" }}>{porcentaje}</span>
              <FaIcons.FaAngleUp color="red" />
            </>
          )}
        </div>
        {data.icon}
      </div>
    </div>
  );
};

export default Widget;
