import React from 'react'
import { Table, Input } from 'reactstrap'
import dayjs from 'dayjs'
import Skeleton from 'react-loading-skeleton'
import { AiFillPlusSquare, AiFillMinusSquare } from 'react-icons/ai'

function TableSale() {
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
    <tr>
      <th scope="row">
        1
      </th>
      <td>
        <div className="table-buttons">
        <Input
          id="exampleEmail"
          name="email"
          type="number"
        />
          <button className="btn-plus">
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
        Otto
      </td>
      <td>
        @mdo
      </td>
      <td>
        @mdo
      </td>
    </tr>
    <tr>
      <td colspan="4" className="footer-table">Total Comanda: </td>
      <td>Sumatoria</td>
    </tr>
   
  </tbody>
    </Table>
  )
}

export default TableSale