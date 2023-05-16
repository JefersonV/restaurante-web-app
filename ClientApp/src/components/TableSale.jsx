import React, { useEffect, useState } from 'react'
import { Table, Input } from 'reactstrap'
import dayjs from 'dayjs'
import Skeleton from 'react-loading-skeleton'
import { AiFillPlusSquare, AiFillMinusSquare } from 'react-icons/ai'
import { BsFillTrashFill } from 'react-icons/bs';

function TableSale({noComanda, saleDetail, deleteSaleDetail }) {
  // 1 por defecto
  const [cantidades, setCantidades] = useState([1]);
  const [totalesPlatillos, setTotalPlatillos] = useState([])

  /* ------ Función para manejar el cambio de cantidad de un elemento -------*/
  const handleCantidadChange = (indice, e) => {
    // Identifica que input está siendo modificado
    const nuevaCantidad = parseInt(e.target.value);
    // Actualiza el array de cantidades
    setCantidades((prevCantidades) => {
      // Nuevas cantidades es una copia de prevCantidades [2,3,43 ... etc] cada número simboliza el value de cada input
      const nuevasCantidades = [...prevCantidades];
      // Indice representa la posición del elemento en el array de cantidades que queremos modificar.
      nuevasCantidades[indice] = nuevaCantidad; // nuevaCantidad es el nuevo valor de la cantidad que se ha ingresado en el input
      console.warn('nuevas')
      console.log(nuevasCantidades)
      
      return nuevasCantidades;
    });
  };

  const handleCantidadChangePlus = (indice, incremento) => {
    setCantidades((prevCantidades) => {
      const nuevasCantidades = [...prevCantidades];
      nuevasCantidades[indice] += incremento;
      return nuevasCantidades;
    });
  };

  const handleCantidadChangeMinus = (indice, incremento) => {
    setCantidades((prevCantidades) => {
      const nuevasCantidades = [...prevCantidades];
      nuevasCantidades[indice] -= incremento;
      return nuevasCantidades;
    });
  };


  const handleDelete = (id) => {
    console.log(id)
    deleteSaleDetail(id); // Llama a la función deleteSaleDetail del componente padre
    console.log(saleDetail)
  };

  console.log(saleDetail)
  const [totalPlatillo, setTotalPlatillo] = useState(50)
  return (
    <Table 
      bordered
      hover
      responsive
    >
      <thead>
        <tr>
          <th>
            # Comanda
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
            onChange={(event) => handleCantidadChange(index, event)}
          />
            <button 
              className="btn-plus"
              onClick={() => handleCantidadChangePlus(index, 1)}
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
                onClick={() => handleCantidadChangeMinus(index, 1)}
              />
            </button>
          </div>
        </td>
        <td>
          {platillo.platillo}
        </td>
        <td>
          Q.{platillo.precio.toFixed(2)}
        </td>
        <td>
          Q.{totalPlatillo.toFixed(2)}
          <button
            onClick={() => handleDelete(platillo.idPlatillo)}
          >
            <BsFillTrashFill 
              color="#FF0000"
            />
          </button>
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