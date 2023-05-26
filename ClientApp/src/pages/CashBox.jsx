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

  const [cashData, setCashData] = useState([])
  const getDataCashBox = async () => {
    try {
      const response = await fetch('http://localhost:5173/api/Caja', {
        headers: {
          'Content-Type': 'application/json',
          "Authorization": `Bearer ${localStorage.token}`
        }
      })
      const data = await response.json()
      console.log(data)
      setCashData(data)
    } catch(error) {
      console.log(error)
    }
  }

  useEffect(() => {
    getDataCashBox()
  }, [])

  /* Estado para local que se pasa al componente hijo para validar si la caja está abierta */
  const [cajaAbierta, setCajaAbierta] = useState("false")

  const setCajaSesion = (valor) => {
    setCajaAbierta(valor)
  }
  
  // const [openBox, setOpenBox] = useState("")
  // setOpenBox(localStorage.getItem("cajaAbierta") || "")
  // console.log('openBox')
  // console.log(openBox)
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
          />
          </div>
        </div>
       
      </div>
    );

  }
}

export default CashBox;
