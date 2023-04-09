import React, { useEffect } from 'react'
import { useStore } from '../providers/GlobalProvider'
import DatePicker from '../components/DatePicker';
function Sales(props) {
  /* isOpen (globalstate) -> para que el contenido se ajuste según el ancho de la sidebar (navegación) */
  const isOpen = useStore((state) => state.sidebar)
  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Ventas");
  });
  return (
    <div className={ isOpen ? "wrapper" : "side" }>
      <DatePicker />
      
    </div>
  )
}

export default Sales