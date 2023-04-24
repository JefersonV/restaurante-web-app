import React, { useState, useEffect } from 'react'
import { Table } from 'reactstrap'
import { BiEditAlt } from 'react-icons/bi'
import { AiFillDelete, AiOutlinePrinter } from 'react-icons/ai'
import ModalEdit from './ModalEdit'
import SwalDelete from './SwalDelete'
import dayjs from 'dayjs'
import '../styles/Table.scss'
import Skeleton from 'react-loading-skeleton'

export default function TableData(props) {
  const { data, actualizarListaProveedores } = props

  return (
    <Table
      bordered
      striped
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
        Teléfono
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
            {item.nombre}
          </td>
          <td>
            {item.telefono}
          </td>
          {/* <td>
            Q.500.00
          </td> */}
          <td>
              {/* Item que fue clickado  */}
            <ModalEdit 
              idProveedor={item.idProveedor} 
              actualizarListaProveedores={actualizarListaProveedores}/>
            <SwalDelete 
              idProveedor={item.idProveedor} 
              actualizarListaProveedores={actualizarListaProveedores}/>
            {/* <button>
              <AiOutlinePrinter
                className="icon-action icon-action--print"
                title="Imprimir venta individual"
              />
            </button> */}
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
