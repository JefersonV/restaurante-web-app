import React from 'react'
import Swal from 'sweetalert2'
import { AiFillDelete } from 'react-icons/ai';

function SwalDelete() {
  const deleteSweet = () => {
    Swal.fire({
      title: "Eliminar registro",
      text: "¿Estás seguro que quieres eliminar el registro?",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Si, eliminalo!",
    }).then((result) => {
      if (result.isConfirmed) {
        Swal.fire("Eliminado!", "El registro se ha elimando", "success");
        // productDelete(id);
      }
    });
  };

  return (
    <>
      <button onClick={deleteSweet}>
        <AiFillDelete
          className="icon-action icon-action--delete"
          title="Eliminar venta individual"
        />
      </button>
    </>
  )
}

export default SwalDelete