import React, { useState, useEffect } from 'react'
import { Table } from 'reactstrap'
import ModalEditCliente from './ModalEditCliente'
import SwalDeleteCliente from './SwalDeleteCliente'
import '../../styles/Table.scss'
import Skeleton from 'react-loading-skeleton'
import dayjs from 'dayjs'

function TableDataCliente(props) {
  const { data, actualizarListaClientes } = props

  return (
    <Table
      bordered
      hover
      // striped
      responsive
      className="fixed-header"
    >
  <thead>
    <tr className="red">
      <th>
        #
      </th>
      <th>
        Fecha
      </th>
      <th>
        Nombre
      </th>
      <th>
        Empresa o institución
      </th>
      <th>
        Puesto
      </th>
    </tr>
  </thead>
  <tbody>
  { data.length === 0 ? (
    <tr>
      {/* Si el filtrado de la búsqueda es = [] */}
      <td colSpan={6}>Resultados no encontrados, el registro no existe ..</td>
    </tr>
  )
  :
  /* Si filtró con éxito */
  (
    data.map((item, index) => {
      return (
        <tr key={index}>
          <th scope="row">
            {index + 1}
          </th>
          <td>
            {dayjs(item.fecha).format('DD/MM/YYYY')}
          </td>
          <td>
            {item.nombreApellido}
          </td>
          <td>
            {item.institucion}
          </td>
          <td>
            {item.puesto}
          </td>
          <td>
              {/* Item que fue clickado  */}
            <ModalEditCliente
              idCliente={item.idCliente} 
              actualizarListaClientes={actualizarListaClientes}
            />
            <SwalDeleteCliente 
              idCliente={item.idCliente} 
              actualizarListaClientes={actualizarListaClientes}
            />
          </td>
        </tr>
      )
    })
  )
}
  
  </tbody>
</Table>
  )
}

export default TableDataCliente