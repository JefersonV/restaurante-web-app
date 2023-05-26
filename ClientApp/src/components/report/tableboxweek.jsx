import React, { useEffect, useState } from 'react';
import { Table, Alert } from 'reactstrap'


export default function Tableboxweek ( props ) {

    const { cajasSemanales } = props




    
  return (
    <div>

                {cajasSemanales.map((caja, index) => (
                  <div key={index}>
                          <hr />
                    <Table>
                      <thead>
                        <tr>
                          <th colSpan="6">Caja Diaria -| {caja.cajaDiaria.fecha}|</th>
                        </tr>
                        <tr>
                          <th >No. Diaria</th>
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
                          <th colSpan="4">Movimientos</th>
                        </tr>
                        <tr>
                          <th >Cod. Mov</th>
                          <th >Descripcion</th>
                          <th >Total</th>
                        </tr>
                      </thead>
                      <tbody>
                        {caja.movimientosCaja.map(movimiento => (
                          <React.Fragment key={movimiento.idMovimiento}>
                            <tr>
                              {/* <td>ID:</td> */}
                              <td>{movimiento.idMovimiento}</td>
                              <td>{movimiento.concepto}</td>
                              <td>{movimiento.total}</td>
                            </tr>
                          </React.Fragment>
                        ))}
                      </tbody>
                    </Table>
                <hr />
                <div className='d-flex'>
                    <Table>
                      <thead>

                        <tr>
                          <th colSpan="3">Ventas</th>
                        </tr>
                        <tr>
                          <th >#Comanda</th>
                          <th >Fecha</th>
                          <th >Total</th>
                        </tr>
                      </thead>
                      <tbody>
                        {caja.movimientosCaja.map(movimiento => (
                          <React.Fragment key={movimiento.idMovimiento}>
                            {movimiento.ventas.map((venta, ind) => (
                              <tr key={ind}>
                                <td>{venta.numeroComanda}</td>
                                <td>{venta.fecha}</td>
                                <td>{venta.total}</td>
                              </tr>
                            ))}
                          </React.Fragment>
                        ))}
                      </tbody>
                    </Table>

                    <Table>
                      <thead>
                        <tr>
                          <th colSpan="7">Gastos</th>
                        </tr>
                        <tr>
                          <th >Doc</th>
                          <th >Fecha</th>
                          <th >Total</th>
                          
                        </tr>
                      </thead>
                      <tbody>
                        {caja.movimientosCaja.map(movimiento => (
                          <React.Fragment key={movimiento.idMovimiento}>
                            {movimiento.gastos.map((gasto, ind1) => (
                              <tr key={ind1}>
                                <td>{gasto.numeroDocumento}</td>
                                <td>{gasto.fecha}</td>
                                <td>{gasto.total}</td>
                              </tr>
                            ))}
                          </React.Fragment>
                        ))}
                      </tbody>
                    </Table>
                    </div>

                    <Table>
                      
                      <tbody>
                        <tr>
                          <td>Total Ventas: {caja.totalVentas}     </td>
                          <td>Total Compras: {caja.totalGastos}</td>
                        </tr>
                        
                        
                      </tbody>
                    </Table>

                    

                    
                  </div>
                  
                ))}

  </div>
  );
  };
  
