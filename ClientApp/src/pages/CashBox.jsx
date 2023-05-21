import React, { useEffect } from 'react'
import { useStore } from '../providers/GlobalProvider'
import { FaCashRegister } from 'react-icons/fa'
import '../styles/Cash.scss'
function CashBox(props) {
  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Caja");
  }, []);
  /* isOpen (globalstate) -> para que el contenido se ajuste según el ancho de la sidebar (navegación) */
  const isOpen = useStore((state) => state.sidebar)
  
  return (
    <div className={ isOpen ? "wrapper" : "side" }>
      <div className="card-front">
        <div className="card-up">
          <p>Caja Cerrada</p>
          <FaCashRegister 
            size={28}
          />
        </div>
        <div className="card-bottom">
          <label htmlFor="Abrir">
            Click para Aperturar 
          </label>
          <span>
            <FaCashRegister />
          </span>
          <button className="d-none" id="Abrir"></button>
        </div>
      </div>
    </div>
  )
}

export default CashBox