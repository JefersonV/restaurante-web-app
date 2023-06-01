import React, { useState, useEffect }from 'react'
import { Table, Button } from "reactstrap"
import "../styles/Cash.scss"
import Select from '../components/Select';
import { useStore } from '../providers/GlobalProvider'
import DatePicker from '../components/DatePicker';
import { AiOutlineClose } from "react-icons/ai"
import ModalBoxClose from '../components/caja/ModalBoxClose';
import dayjs from 'dayjs';

function CashBoxOpen(props) {
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
      // console.log(data)
      setCashData(data)
    } catch(error) {
      console.log(error)
    }
  }

  useEffect(() => {
    getDataCashBox()
  }, [])
  return (
    <div className={isOpen ? "wrapper" : "side" }>
      <div className="container mt-3 mb-3">
        <div className="row mb-3">
          <div className="col-6">
            <DatePicker />
          </div>
        </div>
        <div className="row">
          <div className="col-6">
            <Select />
          </div>
          <div className="col-6">
            <ModalBoxClose />
            
          </div>
      </div>
      </div>
      
       <Table
        bordered
        >
        <thead>
          <tr>
            <th>#</th>
            <th>Fecha</th>
            <th>Inicio</th>
            <th>Ingreso</th>
            <th>Egreso</th>
            <th>Saldo en caja</th>
            <th>Saldo Bruto</th>
            <th>Ganancia (saldo neto)</th>
          </tr>
        </thead>
        <tbody>
          {cashData.length === 0 ? (
            <tr>
              <td colSpan={8}>No hay ninguna caja registrada</td>
            </tr>
          ) : (
          cashData.map((item, index) => 
            /* Identifica cuál es la caja activa, y en caso de haber una le pone la clase que pone el fondo verde */
            <tr className={item.estado === true ? "cash-active" : "inactive-cash-s"} key={index}>
              <td>1</td>
              <td>{dayjs(item.fecha).format('DD/MM/YYYY')}<br />
                <span className="table-content-bold">Estado:
                {/* Si el item está activo el texto dirá activo */} 
                {item.estado === true ? "Activo" : "Inactivo"}</span>
              </td>
              <td>Q.{item.saldoInicial}</td>
              <td>
                <span className="table-content-bold">Efectivo</span><br />
                Q.{item.ingreso}
              </td>
              <td>
                <span className="table-content-bold">Efectivo</span><br />
                Q.{item.egreso}
              </td>
              <td>
                <span className="table-content-bold">Caja</span> 
                  Q.{item.caja}<br /><span className={item.estado === true ? "d-none" : "table-content-bold"}>Entregó:Q.{item.entrega} 
                  </span> 
              </td>
              <td>Q.{item.saldoBruto}</td>
              <td className="table-content-bold">Q.{item.ganancia}</td>
            </tr>
          ))}
        </tbody>
    </Table>
    </div>
    
  )
}

export default CashBoxOpen