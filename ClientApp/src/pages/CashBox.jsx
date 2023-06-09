import React, { useEffect, useState } from 'react'
import { useStore } from '../providers/GlobalProvider'
import { FaCashRegister } from 'react-icons/fa'
import '../styles/Cash.scss'
import { Table } from "reactstrap"
import ModalBox from "../components/caja/ModalBox"
// import CashBoxOpen from './CashBoxOpen'
import CashTest from '../components/caja/CashTest'

function CashBox(props) {
  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Caja");
  }, []);
  /* isOpen (globalstate) -> para que el contenido se ajuste según el ancho de la sidebar (navegación) */
  const isOpen = useStore((state) => state.sidebar);

  const [saldoCajaAnterior, setSaldoCajaAnterior] = useState([])
  /* Data de la última caja registrada */
  const getDataCashBox = async () => {
    try {
      const response = await fetch('http://localhost:5173/api/Caja/caja', {
        headers: {
          'Content-Type': 'application/json',
          "Authorization": `Bearer ${localStorage.token}`
        }
      })
      const data = await response.json()
      console.log(data)
      setSaldoCajaAnterior(data)
    } catch(error) {
      console.log(error)
    }
  }

  useEffect(() => {
    getDataCashBox()
  }, [])

  /* Estado para local que se pasa al componente hijo para validar si la caja está abierta */
  const [cajaAbierta, setCajaAbierta] = useState("false")

  /* Cambia el estado, le establece el valor del localStorage */
  const setCajaSesion = (valor) => {
    setCajaAbierta(valor)
  }
  
  let test = ""
  test = localStorage.getItem("cajaAbierta")
  if(test === "true") {
    return (
      <div className={isOpen ? "wrapper" : "side"}>
        <CashTest /> 
      </div>
    )
  } else {
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
          <ModalBox 
            cajaAbierta={cajaAbierta}
            setCajaSesion={setCajaSesion}
            saldoCajaAnterior={saldoCajaAnterior}
          />
          </div>
        </div>
       
      </div>
    );

  }
}

export default CashBox;
