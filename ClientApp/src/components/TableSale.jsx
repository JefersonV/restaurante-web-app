import React, { useEffect, useState, useMemo } from 'react'
import { Table, Input } from 'reactstrap'
import dayjs from 'dayjs'
import Skeleton from 'react-loading-skeleton'
import { AiFillPlusSquare, AiFillMinusSquare } from 'react-icons/ai'
import { BsFillTrashFill } from 'react-icons/bs';

function TableSale({noComanda, saleDetail, deleteSaleDetail, cantidades, updateCantidades, updateCantidadesDelete }) {
  const [subtotal, setSubtotal] = useState(0)
  const [totalesPlatillos, setTotalPlatillos] = useState([])

  /* ------ Función para manejar el cambio de cantidad de un elemento -------*/
  useEffect(() => {
    console.log(cantidades)
  }, [cantidades])

  useEffect(() => {
    console.log(saleDetail)
  }, [saleDetail])



  /* Nota: el state cantidades está definido en el componente padre, y se recibe como prop */
  // Manejador del <input /> de cantidad de la tabla, identifica que input está siendo modificado
  const handleCantidadChange = (index, e) => {
    // Si no se a ingresado una cantidad, o el usuario la borra antes de eliminar el item, entonces se iguala a ""
    const nuevaCantidad = e.target.value.trim() === "" || e.target.value === undefined || e.target.value === null ? "" 
    : // si ingresa correctamente un número, entonces se parsea y agrega el número al array "cantidades"
    parseInt(e.target.value)
    // Actualizamos el state con el array de cantidades
    updateCantidades(index, nuevaCantidad)
  };
  
  // Problema: al eliminar el item del array saleDetail [{props}] se debe de eliminar también la cantidad asociada =[10,2,3 ....] en la tabla
  // Derivado
  // Problema 2: cuando el <input /> tiene undefined o null, y se elimina toma el valor que tenía el anterior que fue eliminado, porque el array cantidades solo almacena los que ya tienen definido su value
  /* Eliminar la cantidad asociada */
  
  
  const handleCantidadDelete = (indexItem) => {
    // Actualiza la cantidad asociada de la venta, evalúa si está vacía entonces le asigna "", para que aunque esté vacía se elimine junto con su item asociado
    updateCantidadesDelete(indexItem)
  }

  /* const handleCantidadDelete = (indexItem) => {
    setCantidades((prevCantidades) => {
      const cantidadesActualizadas = [...prevCantidades];
      cantidadesActualizadas.splice(indexItem, 1); // Elimina la cantidad correspondiente en el mismo índice
      return cantidadesActualizadas;
    })} */
    // Eliminar la cantidad asociada en la tabla
    /* setCantidades((prevCantidades) => {

      const cantidadesActualizadas = prevCantidades.filter((_,index) => index !== indexItem)
      console.log(cantidades)
      return cantidadesActualizadas
    })
  } */
  /* const handleCantidadChangePlus = (index, incremento) => {
    setCantidades((prevCantidades) => {
      const nuevasCantidades = [...prevCantidades];
      nuevasCantidades[index] += incremento;
      return nuevasCantidades;
    });
  };

  const handleCantidadChangeMinus = (index, incremento) => {
    setCantidades((prevCantidades) => {
      const nuevasCantidades = [...prevCantidades];
      nuevasCantidades[index] -= incremento;
      return nuevasCantidades;
    });
  }; */


  const handleDelete = (id, index) => {
    console.log(id)
    // Array que almacena los items seleccionados del componente de búsqueda

    deleteSaleDetail(id); 
    // Otra función
    handleCantidadDelete(index)
    console.log(saleDetail)
  };

  // console.log(saleDetail)
  const [totalPlatillo, setTotalPlatillo] = useState(0)

  /* Cálculo del total de toda la comanda */
  const totalComanda = useMemo(() => {
    let total = 0;
    for (let i = 0; i < saleDetail.length; i++) {
      const precio = saleDetail[i].precio
      const cantidad = cantidades[i] || 0
      total += precio * cantidad
    }
    return total
  }, [saleDetail, cantidades])
  
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
            Subtotal
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
            onChange={(e) => handleCantidadChange(index, e)}
          />
            <button 
              className="btn-plus"
              // onClick={() => handleCantidadChangePlus(index, 1)}
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
                // onClick={() => handleCantidadChangeMinus(index, 1)}
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
          Q.{(cantidades[index] * platillo.precio).toFixed(2)}
          <button
            onClick={() => {
              handleDelete(platillo.idPlatillo)
              handleCantidadDelete(index)
            }}
              
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
      <td>Q.{totalComanda.toFixed(2)}</td>
    </tr>
   
  </tbody>
    </Table>
  )
}

export default TableSale