import React, { useEffect, useState } from 'react';
import { Table, Alert } from 'reactstrap'


export default function Tablebox ( props ) {

    const { movimientosCaja, data } = props

    // const [cajaDiaria, setCajaDiaria] = useState(null);
    // const [movimientosCaja, setMovimientosCaja] = useState([]);
    // const [ventas, setVentas] = useState([]);
    // const [gastos, setGastos] = useState([]);
    // const [totalVentas, setTotalVentas] = useState(0);
    // const [totalGastos, setTotalGastos] = useState(0);


    
  return (
    <div>

{/* <h6>Resumen de ventas: {sumaTotal}</h6> */}
    {movimientosCaja ?  (

movimientosCaja.map((movimiento, index) => (
  <div key={index} className='d-flex'>
    {/* <p>Id Movimiento: {movimiento.idMovimiento}</p>
    <p>Concepto: {movimiento.concepto}</p>
    <p>Total: {movimiento.total}</p> */}

    {/* <h4>Ventas</h4> */}
    <Table >

      
      <tbody>
        {movimiento.ventas.map((venta, ventaIndex) => (
          <tr key={ventaIndex}>
            {/* <td  align="left">{ventaIndex+1}</td> */}
            <td className='w-25' align="left">{venta.numeroComanda}</td>
            <td className='w-25' align="right">{venta.fecha}</td>
            <td className='w-25' align="center">Q. {venta.total}</td>
            {/* <td>{venta.idMesero}</td>
            <td>{venta.idCliente}</td> */}
          </tr>
        ))}
      </tbody>
    </Table>

    {/* <h4>Gastos</h4> */}
    <Table>
      
      <tbody>
        {movimiento.gastos.map((gasto, gastoIndex) => (
          <tr key={gastoIndex}>
            {/* <td>{gasto.idGasto}</td> */}
            <td className='w-25' align='center' >{gasto.numeroDocumento}</td>
            <td className='w-25' align='left'>{gasto.fecha}</td>
            <td className='w-50'>{gasto.concepto}</td>
            <td className='w-25'>{gasto.total}</td>
            {/* <td>{gasto.idProveedor}</td> */}
          </tr>
        ))}
      </tbody>
    </Table>

  
  </div>
))

      
      
    ) : (
      <Alert color="danger">No se econtraron ventas.</Alert>
    )}
  </div>
  );
  };
  
