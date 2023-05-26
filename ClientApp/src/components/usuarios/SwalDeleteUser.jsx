import React from 'react';
import Swal from 'sweetalert2';
import { BsFillTrashFill } from 'react-icons/bs';

const SwalDelete = ({ idUsuario, actualizarListaUsuario }) => {
  const deleteSweet = () => {
    Swal.fire({
      title: 'Eliminar registro',
      text: '¿Estás seguro que quieres eliminar el registro?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#007bff',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Sí, elimínalo',
      cancelButtonText: 'Cancelar',
    }).then((result) => {
      if (result.isConfirmed) {
        Swal.fire('Eliminado', 'El registro se ha eliminado', 'success');
        console.log(idUsuario);
        providerDelete(idUsuario);
      }
    });
  };

  const providerDelete = async (id) => {
    try {
      const response = await fetch(
        `${import.meta.env.VITE_BACKEND_URL}/api/Account/${id}`,
        {
          method: "DELETE",
          headers: {
            Authorization: `Bearer ${localStorage.token}`,
          },
        }
      );
      if (response.ok) {
        /* Prop para actualizar la tabla en tiempo real después de eliminar el registro */
        actualizarListaUsuario();
      } else {
        Swal.fire('Error', 'No se pudo eliminar el registro', 'error');
      }
    } catch (error) {
      console.log(error);
      Swal.fire('Error', 'Ocurrió un error al eliminar el registro', 'error');
    }
  };

  return (
    <>
      <button onClick={deleteSweet}>
        <BsFillTrashFill className="icon-action icon-action--delete" title="Eliminar usuario" size={22} />
      </button>
    </>
  );
};

export default SwalDelete;
