import React, { useState, useEffect } from 'react'
import { useStore } from '../providers/GlobalProvider'
import DatePicker from '../components/DatePicker';
import Searchbar from '../components/Searchbar';
import SelectOption from '../components/SelectOption';

function Inventory(props) {
  /* isOpen (globalstate) -> para que el contenido se ajuste según el ancho de la sidebar (navegación) */
  const isOpen = useStore((state) => state.sidebar);
  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Inventario");
  }, []);

  return (
    <div className={ isOpen ? "wrapper" : "side" }>
      Inventario
    </div>
  );
}

export default Inventory;
