import React, { useEffect } from "react";
import { useStore } from "../providers/GlobalProvider";

function GroupSale(props) {
  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Venta Grupal");
  }, []);
  /* isOpen (globalstate) -> para que el contenido se ajuste según el ancho de la sidebar (navegación) */
  const isOpen = useStore((state) => state.sidebar);
  return <div className={isOpen ? "wrapper" : "side"}>GrupalSale</div>;
}

export default GroupSale;
