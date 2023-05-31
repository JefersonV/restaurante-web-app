import React, { useEffect, useState } from 'react';
import { Table, Alert } from 'reactstrap'


export default function Tableboxmonth ( props ) {

    const { data } = props




    
  return (
    <div >
    <h2>Resumen del Mes de {data.title}</h2>
      <Table>
        <thead>
          <tr>
            {/* <th>Total Ingresos Mes</th> */}
            <th>Total Ventas Mes</th>
            <th>Total Compras Mes</th>
            <th>Total Saldo Final Mes</th>
          </tr>
        </thead>
        <tbody>
          <tr>
            {/* <td>{data.totalIngresosMes}</td> */}
            <td>{data.totalVentasMes}</td>
            <td>{data.totalComprasMes}</td>
            <td>{data.totalSaldoFinalMes}</td>
          </tr>
        </tbody>
      </Table>

      {/* <h2>Detalles por Semana</h2> */}
      {data.semanas.map((semana) => (
          

        <div key={semana.startDate}  className="p-3 mt-3 bg-secondary bg-gradient bg-opacity-25 rounded-5">

        <div style={{ maxHeight: "100%", overflow: "auto" }}>
        <Table >
            <thead>
              <tr>

            <th colSpan="5"><h4>Información Semanal</h4></th>

              </tr>
              <tr>
                <th>Fecha de Inicio de la Semana</th>
                <th>Fecha de Fin de la Semana</th>
                <th>Total Ventas Semana</th>
                <th>Total Compras Semana</th>
                <th>Total Saldo Final Semana</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td>{semana.startDate}</td>
                <td>{semana.endDate}</td>
                <td>{semana.totalVentasSemana}</td>
                <td>{semana.totalComprasSemana}</td>
                <td>{semana.totalSaldoFinalSemana}</td>
              </tr>
            </tbody>
          </Table>

        </div>
         

          
          {semana.cajasSemanales.map((caja) => (
            <div key={caja.cajaDiaria.idCajaDiaria}>
                <Table className="table table-bordered bg-primary rounded-3 bg-opacity-25 mb-1">
                <thead>
                  <tr>
                    <th colSpan="5">Detalles de Cajas Diarias | {caja.cajaDiaria.fecha}</th>
                  </tr>
                  <tr>
                    {/* <th>Id Caja Diaria</th> */}
                    {/* <th>Fecha</th> */}
                    <th>Saldo Inicial</th>
                    <th>Saldo Final</th>
                    <th>Estado</th>
                  </tr>
                </thead>
                <tbody>
                  <tr>
                    {/* <td>{caja.cajaDiaria.idCajaDiaria}</td> */}
                    {/* <td></td> */}
                    <td>{caja.cajaDiaria.saldoInicial}</td>
                    <td>{caja.cajaDiaria.saldoFinal}</td>
                    <td>{caja.cajaDiaria.estado ? 'activo' : 'cerrado'}</td>
                  </tr>
                </tbody>
              </Table>

              {/* <h5>Movimientos de Caja</h5> */}
              {/* <Table>
                <thead>
                  <tr>
                    <th>Id Movimiento</th>
                    <th>Id Tipo Movimiento</th>
                    <th>Concepto</th>
                    <th>Total</th>
                  </tr>
                </thead>
                <tbody>
                  {caja.movimientosCaja.map((movimiento) => (
                    <tr key={movimiento.idMovimiento}>
                      <td>{movimiento.idMovimiento}</td>
                      <td>{movimiento.idTipoMovimiento}</td>
                      <td>{movimiento.concepto}</td>
                      <td>{movimiento.total}</td>
                    </tr>
                  ))}
                </tbody>
              </Table> */}


              <div className='d-flex' style={{ maxHeight: "100%", overflow: "auto" }}>
                <Table className="table table-bordered bg-primary rounded-3 bg-opacity-25 mb-0">
                  <thead>
                    <tr>
                      <th colSpan="3">Ventas</th>
                    </tr>
                    <tr>
                    <th>Fecha</th>
                    <th>#comanda</th>
                    <th>total</th>
                    </tr>
                  </thead>
                  <tbody>
                  {caja.movimientosCaja.flatMap((movimiento) => movimiento.ventas).map((venta) => (
              <tr key={venta.idVenta}>
                {/* <td>{venta.idVenta}</td> */}
                <td>{venta.fecha}</td>
                <td>{venta.numeroComanda}</td>
                <td>{venta.total}</td>
                {/* <td>{venta.idMesero}</td> */}
                {/* <td>{venta.idCliente}</td> */}
              </tr>
            ))}
                  </tbody>

                </Table>

                {/* <Table className="table table-bordered "> */}
                <Table className="table table-bordered bg-primary rounded-3 bg-opacity-25 mb-0">

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
              <div className='d-flex'>
              <Table className="table  bg-primary rounded-3 bg-opacity-25 ">
              <tbody>
                <tr>
                <td align='left' ><b>Total de ventas</b></td>
                
                <td align='right'><b>{caja.totalVentas}</b></td>
                </tr>
                <tr>
                  
                </tr>
              </tbody>
              </Table>

              <Table className="table table-bordered bg-primary rounded-3 bg-opacity-25 ">
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
        

      ))}
        

      </div>
  );
  };
  
