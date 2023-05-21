import React, { useEffect } from "react";
import { useStore } from "../providers/GlobalProvider";
function Purchases(props) {
  /* isOpen (globalstate) -> para que el contenido se ajuste según el ancho de la sidebar (navegación) */
  const isOpen = useStore((state) => state.sidebar);
  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Compras");
  }, []);

  return (
    <div className={isOpen ? "wrapper" : "side"}>
      <h1>Purchases</h1>
    </div>
  );
}

export default Purchases;
