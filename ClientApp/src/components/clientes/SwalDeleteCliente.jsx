import React, { useState, useEffect} from 'react'
import Swal from 'sweetalert2'
import { BsFillTrashFill } from 'react-icons/bs';

function SwalDeleteCliente(props) {
  const {idCliente, actualizarListaClientes} = props
  const deleteSweet = (id) => {
    Swal.fire({
      title: "Eliminar registro",
      text: "¿Estás seguro que quieres eliminar el registro?",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#007bff",
      cancelButtonColor: "#d33",
      confirmButtonText: "Si, eliminalo!",
      cancelButtonText: "Cancelar"
    }).then((result) => {
      if (result.isConfirmed) {
        Swal.fire("Eliminado!", "El registro se ha elimando", "success");
        providerDelete(id)
      }
    });
  };

  const [data, setData] = useState([])

  const providerDelete = async (id) => {
    console.log("click -> Id: ", id);
    const response = await fetch(
      `${import.meta.env.VITE_BACKEND_URL}/api/Cliente/${id}`,
      {
        method: "DELETE",
        headers: {
          Authorization: `Bearer ${localStorage.token}`,
        },
      }
    );
    // setData(data.filter((data) => data.id_cliente !== id));
    if(response.ok) {
      /* Prop para actualizar la tabla en tiempo real, después de eliminar el registro. */
      actualizarListaClientes()
    }
  };

  return (
    <>
      <button onClick={() => deleteSweet(idCliente)}>
        <BsFillTrashFill
          className="icon-action icon-action--delete"
          title="Eliminar venta individual"
          size={22}
        />
      </button>
    </>
  )
}

export default SwalDeleteCliente