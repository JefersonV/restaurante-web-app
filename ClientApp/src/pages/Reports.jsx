import React, { useEffect } from "react";
import { useStore } from "../providers/GlobalProvider";
function Reports(props) {
  /* isOpen (globalstate) -> para que el contenido se ajuste según el ancho de la sidebar (navegación) */
  const isOpen = useStore((state) => state.sidebar);
  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Reportes");
  }, []);
  return (
    <div className={isOpen ? "wrapper" : "side"}>
      <h1>Reports</h1>
    </div>
  );
}

export default Reports;
