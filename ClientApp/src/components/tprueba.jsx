import React, { useEffect, useState } from 'react';
import { Table, Alert } from 'reactstrap'


export default function Tablep ( props ) {

    const { data } = props

  return (
    <div>
    {data.length > 0 ? (
      <Table>
        <thead>
          <tr>
            <th>No</th>
            <th>Cliente</th>
            <th>Ventas</th>
            <th>Total</th>
            <th>Fecha</th>
          </tr>
        </thead>
        <tbody>
          {data.map((item, index) => (
            <tr key={index} >
              <td>{index+1}</td>
              <td>{item.cliente.nombreApellido}</td>
              <td align="left">
                <ul>
                  {item.ventas.map((venta, index1) => (
                    <li key={index1}>
                      <>No.</> {index1 +1} <br />
                      <>Ordenó: </> {venta.platillo} <br /> 
                      <>Cantidad: </> {venta.cantidad}, <br /> 
                      <>Subtotal: </> {venta.subtotal} <br /> 
                    </li>
                  ))}
                </ul>
              </td>
              <td>{item.total}</td>
              <td>
                 {item.ventas.length > 0 && item.ventas[0].fecha}
              </td>
            </tr>
          ))}
        </tbody>
      </Table>
    ) : (
      <Alert color="danger">No se econtraron ventas.</Alert>
    )}
  </div>
  );
  };
  
