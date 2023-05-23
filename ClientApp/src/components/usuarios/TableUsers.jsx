import React, { useState, useEffect } from "react";
import { Table, Button } from "reactstrap";
import { BiEditAlt } from "react-icons/bi";
import { BsFillTrashFill } from "react-icons/bs";
import ModalEditUser from "./ModalEditUser";
import SwalDelete from '../usuarios/SwalDeleteUser'

function TableUsers(props) {
  const [modal, setModal] = useState(false);
  const { actualizarListaUsuario } = props
  const showModal = () => !modal;

  return (
    <>
      <div className="container">
        <div className="col">
          <div className="row">
            <Table hover bordered>
              <thead>
                <tr>
                  <th>#</th>
                  <th>Nombre</th>
                  <th>Usuario</th>
                  <th>Tipo Usuario</th>
                  <th>Acciones</th>
                </tr>
              </thead>
              <tbody>
                {props.data.map((data, index) => (
                  <tr key={index}>
                    
                    <th scope="row">{index + 1}</th>
                    <td>{data.nombre}</td>
                    <td>{data.usuario1}</td>
                    <td>{data.tipoUsuario}</td>
                    <td>
                      <ModalEditUser
                        idUsuario={data.idUsuario}
                        actualizarListaUsuario={actualizarListaUsuario}
                      />
                      <SwalDelete 
                        idUsuario={data.idUsuario} 
                        actualizarListaUsuario={actualizarListaUsuario} 
                      />
                    </td>
                  </tr>
                ))}
              </tbody>
            </Table>
          </div>
        </div>
      </div>
    </>
  );
}

export default TableUsers;
