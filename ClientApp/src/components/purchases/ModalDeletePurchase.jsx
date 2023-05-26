import React, { useState, useEffect } from "react";
import Swal from "sweetalert2";
import { BsFillTrashFill } from "react-icons/bs";

function SwalDelete({ idplatillo, actualizarListaMenu }) {
  const deleteSweet = (id) => {
    Swal.fire({
      title: "Eliminar registro",
      text: "¿Estás seguro que quieres eliminar el registro?",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#007bff",
      cancelButtonColor: "#d33",
      confirmButtonText: "Si, eliminalo!",
      cancelButtonText: "Cancelar",
    }).then((result) => {
      if (result.isConfirmed) {
        Swal.fire("Eliminado!", "El registro se ha elimando", "success");
        providerDelete(id);
      }
    });
  };

  const [data, setData] = useState([]);

  const providerDelete = async (id) => {
    console.log("click -> Id: ", id);
    const response = await fetch(
      `${import.meta.env.VITE_BACKEND_URL}/api/Gasto/${id}`,
      {
        method: "DELETE",
        headers: {
          Authorization: `Bearer ${localStorage.token}`,
          "Content-Type": "application/json",
        },
      }
    );
    // setData(data.filter((data) => data.id_cliente !== id));
    if (response.ok) {
      /* Prop para actualizar la tabla en tiempo real, después de eliminar el registro. */
      actualizarListaMenu();
    }
  };

  return (
    <>
      <button onClick={() => deleteSweet(idplatillo)}>
        <BsFillTrashFill
          className="icon-action icon-action--delete"
          title="Eliminar gasto"
          size={22}
        />
      </button>
    </>
  );
}

export default SwalDelete;
