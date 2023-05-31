import React, { useEffect, useState } from 'react';
import { Table, Alert } from 'reactstrap'


export default function Tableboxweek ( props ) {

    const { data } = props




    
  return (
    <div>

                




        {data.cajasSemanales.map((caja) => (
          <div key={caja.cajaDiaria.idCajaDiaria} className="bg-primary rounded-3 bg-opacity-25 mb-3">
            {/* <h4>Caja Diaria ID: {caja.cajaDiaria.idCajaDiaria}</h4>
            <p>Fecha: {caja.cajaDiaria.fecha}</p>
            <p>Saldo Inicial: {caja.cajaDiaria.saldoInicial}</p>
            <p>Saldo Final: {caja.cajaDiaria.saldoFinal}</p>
            <p>Estado: {caja.cajaDiaria.estado}</p> */}

            <Table>
                      <thead>
                        <tr>
                          <th colSpan="6">Caja Diaria -| {caja.cajaDiaria.fecha}|</th>
                        </tr>
                        <tr>
                          <th >No. Caja Diaria</th>
                          <th >Fecha</th>
                          <th >Saldo Inicial</th>
                          <th >Saldo Final</th>
                          <th >Estado</th>

                        </tr>
                      </thead>
                      <tbody>
                        <tr>
                          <td>{caja.cajaDiaria.idCajaDiaria}</td>
                          <td>{caja.cajaDiaria.fecha}</td>
                          <td>{caja.cajaDiaria.saldoInicial}</td>
                          <td>{caja.cajaDiaria.saldoFinal}</td>
                          <td><p >Estado: {caja.cajaDiaria.estado ? 'activo' : 'cerrado'}</p></td>
                        </tr>
                      </tbody>
                    </Table>

<hr />
            <Table>
              <thead>
                <tr>
                  <th colSpan="4">MOVIMIENTOS REALIZADOS</th>
                </tr>
                <tr>
                  <th>Cod. Movimiento</th>
                  {/* <th>ID Tipo Movimiento</th> */}
                  <th>Concepto</th>
                  <th>Total</th>
                </tr>
              </thead>
              <tbody>
                {caja.movimientosCaja.map((movimiento) => (
                  <tr key={movimiento.idMovimiento}>
                    <td>{movimiento.idMovimiento}</td>
                    {/* <td>{movimiento.idTipoMovimiento}</td> */}
                    <td>{movimiento.concepto}</td>
                    <td>{movimiento.total}</td>
                  </tr>
                ))}
              </tbody>
            </Table>

<hr />
            <div className='d-flex' style={{ maxHeight: "100%", overflow: "auto" }}>
            <Table className=" mb-0">
                  <thead>
                    <tr>
                      <th colSpan="3">Ventas</th>
                    </tr>
                    <tr>
                    <th>Fecha</th>
                    <th>comanda</th>
                    <th>total</th>
                    </tr>
                  </thead>
                  <tbody>
                  {caja.movimientosCaja.flatMap((movimiento) => movimiento.ventas).map((venta) => (
              <tr key={venta.idVenta}>
                {/* <td>{venta.idVenta}</td> */}
                <td>{venta.numeroComanda}</td>
                <td>{venta.fecha}</td>
                <td>{venta.total}</td>
                {/* <td>{venta.idMesero}</td> */}
                {/* <td>{venta.idCliente}</td> */}
              </tr>
            ))}
                  </tbody>

                </Table>

                {/* <Table className="table table-bordered "> */}
                <Table className="mb-0">

                  <thead>
                    <tr>
                      <th colSpan="4">Compras</th>
                    </tr>
                    <tr>
                    <th>No.Doc</th>
                    {/* <th>Fecha</th> */}
                    <th>Concepto</th>
                    <th>Total</th>
                    </tr>
                  </thead>
                  <tbody>
                  {caja.movimientosCaja.flatMap((movimiento) => movimiento.gastos).map((gasto) => (
                    <tr key={gasto.idGasto}>
                      {/* <td>{gasto.idGasto}</td> */}
                      <td>{gasto.numeroDocumento}</td>
                      {/* <td>{gasto.fecha}</td> */}
                      <td>{gasto.concepto}</td>
                      <td>{gasto.total}</td>

                  
                      {/* <td>{gasto.idProveedor}</td> */}
                    </tr>
                    
                  ))}

                  </tbody>

                </Table  >
                
              </div>
              <div className='d-flex' >
              <Table>
              <tbody>
                <tr>
                <td align='left' ><b>Total de ventas</b></td>
                
                <td align='right'><b>{caja.totalVentas}</b></td>
                </tr>
                <tr>
                  
                </tr>
              </tbody>
              </Table>

              <Table >
              <tbody>
                <tr>
                <td align='left'><b>Total de gastos</b></td>

                <td align='right'><b>{caja.totalGastos}</b></td>
                </tr>
              </tbody>
              </Table>

              </div>

          </div>
        ))}

  </div>
  );
  };








                  




  
