import React, { useState } from 'react'
import { Table, Input } from 'reactstrap'
import dayjs from 'dayjs'
import Skeleton from 'react-loading-skeleton'
import { AiFillPlusSquare, AiFillMinusSquare } from 'react-icons/ai'

function TableSale({noComanda, saleDetail}) {
  const [cantidades, setCantidades] = useState(Array(saleDetail.length).fill(0));
  console.log(cantidades)
  const handleCantidadChange = (index, value) => {
    // NewCantidades hace una copia de cantidades
    const newCantidades = [...cantidades];
    // Coincide index y value
    newCantidades[index] = value;
    setCantidades(newCantidades);
  }

  const handleIncrement = (index) => {
  // const newCantidades = [...cantidades];
  // newCantidades[index] += 1;
  // setCantidades(newCantidades);
  const newCantidades = [...cantidades];
  newCantidades[index] += 1;
  setCantidades(newCantidades);
  }
  return (
    <Table 
      bordered
      // hover
      responsive
    >
      <thead>
        <tr>
          <th>
            No. Comanda
          </th>
          <th>
            Fecha
          </th>
          <th>
            Cantidad
          </th> 
          <th>
            Platillo
          </th>
          <th>
            Precio Unitario
          </th>
          <th>
            Total
          </th>
        </tr>
      </thead>
      <tbody>
      {saleDetail.map((platillo, index) => 
      saleDetail.length === 0 ? (
        <tr>
          <td colSpan="5" align="center">No hay items seleccionados</td>
        </tr>
      ) : (
      <tr key={index}>
        <td scope="row">
          {/* Si no se a registrado el número de comanda -> renderiza # */}
          {noComanda.trim() === "" ? "#" : noComanda}
        </td>
        <td>
          {dayjs().format('DD/MM/YYYY')}
        </td>
        <td>
          <div className="table-buttons">
          <input
            className="form-control input-cantidad"
            name={`input-${index}`}
            id="cantidad-platillo"
            type="number"
            tabIndex={index + 1}
            value={cantidades[index]}
            onChange={(e) =>
              handleCantidadChange(index, e.target.value)
            }
          />
            <button 
              className="btn-plus"
              onClick={(index) => handleIncrement(index)}
            >
              <AiFillPlusSquare 
                size={24}
                color="#0d6efd"
              />
            </button>
            <button className="btn-minus">
              <AiFillMinusSquare 
                size={24}
                color="#dc3545"
              />
            </button>
          </div>
        </td>
        <td>
          {platillo.name}
        </td>
        <td>
          {platillo.phone}
        </td>
        <td>
          @mdo
        </td>
      </tr>
      )
      )}
    <tr>
      <th colSpan="5" className="footer-table">Total Comanda: </th>
      <td>Q.350.00</td>
    </tr>
   
  </tbody>
    </Table>
  )
}

export default TableSale