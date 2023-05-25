import React, { useEffect, useState, useMemo } from "react";
import { Table, Input } from "reactstrap";
import dayjs from "dayjs";
import Skeleton from "react-loading-skeleton";
import { AiFillPlusSquare, AiFillMinusSquare, AiOutlinePlus, AiOutlineMinus } from "react-icons/ai";
import { BsFillTrashFill } from "react-icons/bs";


function TableSale({
  // PROPS
  noComanda,
  defaultDate,
  changeDate,
  saleDetail,
  deleteSaleDetail,
  cantidades,
  updateCantidades,
  updateCantidadesDelete,
  changeCantidad,
}) {
  // State para validar el formulario
  const [isFormValid, setIsFormValid] = useState(true);
  
  useEffect(() => {
    setIsFormValid(saleDetail.length > 0);
  }, [saleDetail]);
  /* ------ Función para manejar el cambio de cantidad de un elemento -------*/

  /* Nota: el state cantidades está definido en el componente padre, y se recibe como prop */
  // Manejador del <input /> de cantidad de la tabla, identifica que input está siendo modificado
  const handleCantidadChange = (index, e) => {
      // Elimina los espacios en blanco antes o después del número
      const inputValue = e.target.value.trim();
      // Item que se agregará al array cantidades = [1,2,....]
      let nuevaCantidad
      // Si es un número válido
      if(/^\d+$/.test(inputValue)) {
        // parsea la cantidad
        nuevaCantidad = parseInt(inputValue);
      } else {
        // si es cualquier otra cosa como una letra o símbolo
        nuevaCantidad = 0
      }

      // Array de cantidades 
      updateCantidades(index, nuevaCantidad);
  };

  // Problema: al eliminar el item del array saleDetail [{props}] se debe de eliminar también la cantidad asociada =[10,2,3 ....] en la tabla
  // Derivado
  // Problema 2: cuando el <input /> tiene undefined o null, y se elimina toma el valor que tenía el anterior que fue eliminado, porque el array cantidades solo almacena los que ya tienen definido su value
  /* Eliminar la cantidad asociada */

  const handleCantidadDelete = (indexItem) => {
    // Actualiza la cantidad asociada de la venta, evalúa si está vacía entonces le asigna "", para que aunque esté vacía se elimine junto con su item asociado
    updateCantidadesDelete(indexItem);
  };

  const handleDelete = (id, index) => {
    console.log(id);
    // Array que almacena los items seleccionados del componente de búsqueda

    deleteSaleDetail(id);
    // Otra función
    handleCantidadDelete(index);
    console.log(saleDetail);
  };

  /* Cálculo del total de toda la comanda */
  const totalComanda = useMemo(() => {
    let total = 0;
    for (let i = 0; i < saleDetail.length; i++) {
      // saleDetail es el array que tiene los items seleccionados del buscador
      const precio = saleDetail[i].precio;
      // cantidades es el array que tiene las cantidades asociadas a cada item
      const cantidad = cantidades[i] || 0;
      total += precio * cantidad;
    }
    return total;
  }, [saleDetail, cantidades]);

  return (
    <div className="comanda-digital">
      <Table bordered responsive>
        <thead>
          <tr>
            <th>Platillo</th>
            <th>Cantidad</th>
            <th>Precio Unitario</th>
            <th>Subtotal</th>
          </tr>
        </thead>
        <tbody>
        {saleDetail.length === 0 ? (
          <tr>
            <td colSpan="5" align="center">
              Debes seleccionar los items, da click en el buscador 😊
            </td>
          </tr>
        ) : (
          saleDetail.map((platillo, index) => (
            <tr key={index}>
                <td>{platillo.platillo}</td>
                <td>
                  <div className="table-buttons">
                    <button type="button" className="btn-minus"
                      // onClick={() => changeCantidad(index, 1, "disminuir")}
                      onClick={() => changeCantidad(platillo.idPlatillo, 1, "disminuir")}
                    >
                      <AiOutlineMinus
                        size={20}
                        color="#8b1e3f"
                        strokeWidth={75}
                      />
                    </button>
                    <input
                      className="form-control input-cantidad"
                      name={`input-${index}`}
                      id="cantidad-platillo"
                      type="text"
                      tabIndex={index + 1}
                      value={cantidades[index]}
                      /* pattern="/^(\d{1,4})$/" */
                      onChange={(e) => handleCantidadChange(index, e)}
                    />
                    <button
                      type="button"
                      className="btn-plus"
                      // Aumenta la cantidad + 1
                      // onClick={() => changeCantidad(index, 1, "aumentar")}
                      onClick={() => changeCantidad(platillo.idPlatillo, 1, "aumentar")}
                    >
                      <AiOutlinePlus 
                        className="svg"
                        size={20}
                        color="#8b1e3f"
                        strokeWidth={75}
                      />
                    </button>
                    
                  </div>
                </td>
                <td>Q.{platillo.precio.toFixed(2)}</td>
                <td>
                  Q.{(cantidades[index] * platillo.precio).toFixed(2)}
                  <button
                    type="button"
                    onClick={() => {
                      handleDelete(platillo.idPlatillo);
                      handleCantidadDelete(index);
                    }}
                  >
                    <BsFillTrashFill color="#FF0000" />
                  </button>
                </td>
              </tr>
          ))
        )}
        </tbody>
        {/* Tabla de resumen de comanda */}
      </Table>
      <div className="resumen-comanda">
        <table className="table responsive table-resumen">
          <thead>
            <tr>
              <th className="resumen-title">Resumen comanda</th>
            </tr>
          </thead>
          <tbody className="table-resumen-body">
            <tr className="row-resumen">
              <th >
                No. {noComanda}
              </th>
            </tr>
            <tr>
              <td className="row-resumen">Fecha: 
                <input 
                  type="date" 
                  value={defaultDate}
                  onChange={changeDate}
                /></td>
            </tr>
            <tr className="row-resumen">
              <td>Tipo: 1</td>
            </tr>
            <tr className="row-resumen">
              <td className="resumen-total"><span className="total-comanda"> Total: Q.{totalComanda.toFixed(2)}</span></td>
            </tr>
            <tr className="row-resumen">
              <td>
                <input className="btn btn-primary" type="submit" value="Guardar" disabled={!isFormValid} />
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default TableSale;
