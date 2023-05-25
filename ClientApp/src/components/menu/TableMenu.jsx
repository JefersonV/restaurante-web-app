import React, { useState, useEffect } from 'react'
import { Table } from 'reactstrap'
import ModalEditMenu from './ModalEditMenu'
import SwalDeleteMenu from './SwalDeleteMenu'
import '../../styles/Table.scss'
import Skeleton from 'react-loading-skeleton'

export default function TableMenu(props) {
  const { data, actualizarListaMenu } = props

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
      {/* <th>
        Fecha
      </th> */}
      <th>
        Nombre
      </th>
      <th>
        Precio
      </th>
      {/* <th>
        Total
      </th> */}
      <th>
        Acciones
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
          {/* <td>
            {dayjs().format('DD/MM/YYYY')}
          </td> */}
          <td>
            {item.platillo}
          </td>
          <td>
            Q.{item.precio.toFixed(2)}
          </td>
          {/* <td>
            Q.500.00
          </td> */}
          <td>
              {/* Item que fue clickado  */}
            <ModalEditMenu
              idPlatillo={item.idPlatillo} 
              actualizarListaMenu={actualizarListaMenu}
            />
            <SwalDeleteMenu 
              idPlatillo={item.idPlatillo} 
              actualizarListaMenu={actualizarListaMenu}
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
