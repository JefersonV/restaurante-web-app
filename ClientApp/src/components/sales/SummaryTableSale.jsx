import React, { useState } from "react";
import { Table } from 'reactstrap'
import dayjs from "dayjs";
import ModalEditSale from "../sales/ModalEditSale"
import SwalDeleteSale from "./SwalDeleteSale";

function SummaryTableSale(props) {
  const { data, actualizarListaVentas } = props
  return (
    <>
      <Table
        bordered
        hover
        // striped
        responsive
        className="fixed-header"
      >
        <thead>
          <tr className="red">
            <th>#</th>
            <th>No comanda</th>
            <th>Fecha</th>
            <th>Cliente</th>
            <th>Total</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          {data.length === 0 ? (
            <tr>
              {/* Si el filtrado de la búsqueda es = [] */}
              <td colSpan={6}>
                Resultados no encontrados, el registro no existe ..
              </td>
            </tr>
          ) : (
            /* Si filtró con éxito */
            data.map((item, index) => {
              return (
                <tr key={index}>
                  <th scope="row">{index + 1}</th>
                  <td>{item.numeroComanda}</td>
                  <td>{dayjs(item.fecha).format('DD/MM/YYYY')}</td>
                  <td>{item.cliente}</td>
                  <td>Q.{item.total?.toFixed(2) || 0}</td>
                  <td>
                    {/* Item que fue clickado  */}
                    <ModalEditSale 
                      idVenta={item.idVenta}
                      actualizarListaVentas={actualizarListaVentas}
                    /> 
                    <SwalDeleteSale 
                      idVenta={item.idVenta}
                      actualizarListaVentas={actualizarListaVentas}
                    />
                  </td>
                </tr>
              );
            })
          )}
        </tbody>
      </Table>
    </>
  );
}

export default SummaryTableSale;
