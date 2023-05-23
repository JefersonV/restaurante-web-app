import React, { useState, useEffect } from "react";
import { Table } from "reactstrap";
import ModalEditMesero from "./ModalEditMesero";
import SwalDelete from '../meseros/SwalDeleteMesero'

function TableWaiters(props) {
  const [waiters, setWaiters] = useState([]);
  const { actualizarListaMesero } = props

  useEffect(() => {
    setWaiters(props.data);
  }, [props.data]);

  return (
    <>
      <div className="container">
        <div className="col-3 mt-3 mb-3">
          <div className="row d-flex justify-content-end">
          </div>
        </div>
        <div className="col">
          <div className="row">
            <Table hover bordered>
              <thead>
                <tr>
                  <th>#</th>
                  <th>Nombre</th>
                  <th>Acciones</th>
                </tr>
              </thead>
              <tbody>
                {waiters.map((waiter, index) => (
                  <tr key={index}>
                    <th scope="row">{index + 1}</th>
                    <td>{waiter.nombre}</td>
                    <td>
                      <ModalEditMesero 
                        idMesero={waiter.idMesero} 
                        actualizarListaMesero={actualizarListaMesero} 
                      />
                      <SwalDelete 
                        idMesero={waiter.idMesero} 
                        actualizarListaMesero={actualizarListaMesero} 
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

export default TableWaiters;
