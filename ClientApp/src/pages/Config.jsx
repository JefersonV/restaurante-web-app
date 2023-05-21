import React, { useEffect } from "react";
import { useStore } from "../providers/GlobalProvider";
function Config(props) {
  /* isOpen (globalstate) -> para que el contenido se ajuste según el ancho de la sidebar (navegación) */
  const isOpen = useStore((state) => state.sidebar);
  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Configuración");
  }, []);

  return (
    <div className={isOpen ? "wrapper" : "side"}>
      <h1>Config</h1>
    </div>
  );
}

export default Config;
