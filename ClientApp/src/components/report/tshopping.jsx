import React, { useEffect, useState } from 'react';
import { Table, Alert } from 'reactstrap'


export default function Tablep ( props ) {

    const { data } = props

    // const sumaTotal = data.reduce((total, item) => {
    //   return total + item.ventas.reduce((subtotal, venta) => {
    //     return subtotal + venta.subtotal;
    //   }, 0);
    // }, 0);

    let totalSum = 0;

data.forEach((item) => {
  totalSum += item.total;
});

  return (
    <div>
      <h6>Resumen de Gastos: {totalSum}</h6>
    {data.length > 0 ? (
      <Table>
        <thead>
          <tr>
            <th>No</th>
            <th>Fecha</th>
            <th>Descripción</th>
            <th>Proveedor</th>
            <th>Total</th>
          </tr>
        </thead>
        <tbody>
          {data.map((item, index) => (
            <tr key={index} >
              <td>{index+1} </td>
              <td> {item.gastos[0].fecha}</td>
              <td >{item.gastos[0].concepto}</td>
              <td> {item.proveedor.nombre}</td>
              <td> {item.total}</td>

            </tr>
          ))}
        </tbody>
      </Table>
    ) : (
      <Alert color="danger">No se econtraron compras.</Alert>
    )}
  </div>
  );
  };
  
