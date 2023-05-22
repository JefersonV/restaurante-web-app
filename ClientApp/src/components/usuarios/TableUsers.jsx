import React, { useState, useEffect } from "react";
import { Table, Button } from "reactstrap";
import { BiEditAlt } from "react-icons/bi";
import { BsFillTrashFill } from "react-icons/bs";
import { GrUserAdd } from "react-icons/gr";
function TableUsers(props) {
  const [modal, setModal] = useState(false);
  const showModal = () => !modal;

  return (
    <>
      <div className="container">
        <div className="col-3 mt-3 mb-3">
          <div className="row d-flex justify-content-end">
            <Button color="primary">
              <GrUserAdd color="#FFFFFF" />
              Registra Nuevo Usuario
            </Button>
          </div>
        </div>
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
                  <tr>
                    <th scope="row">{index + 1}</th>
                    <td>{data.nombre}</td>
                    <td>{data.usuario1}</td>
                    <td>{data.tipoUsuario}</td>
                    <td>
                      <button onClick={showModal}>
                        <BiEditAlt
													size={24}
                        /* className="svg"
										size={20}
										color="#8b1e3f"
										strokeWidth={75} */
                        />
                      </button>
                      <button>
                        <BsFillTrashFill 
													size={24}
													color="#FF0000"
												/>
                      </button>
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
