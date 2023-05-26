import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom"
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from "reactstrap";
import { FormGroup, Label, Col, Input, Table } from "reactstrap";
import { Formik, FieldArray } from "formik";
import Swal from "sweetalert2";
import { FaCashRegister } from "react-icons/fa";
import { AiOutlineClose } from "react-icons/ai"

function ModalBoxClose(props) {
  const [modal, setModal] = useState(false);
  const toggle = () => setModal(!modal);
  /* Estado para el resultante de caja */
  const [resultadoFinal, setResultadoFinal] = useState(0);

  /* Identificar el activo para cerra la caja */
  const getDataCashBoxId = props.cashData.filter((item) => item.estado === true);
  /* Id del que tiene el estado activo */
  const IdActivo = getDataCashBoxId[0]?.idCajaDiaria;  
  /* Objeto que irá en el body de la solicitud put */
  console.info(getDataCashBoxId)
  let dataCerrar = {
    cantidadSacar: ""
  }

  const navigate = useNavigate();
  return (
    <>
      <Button 
			color="danger"
			outline
			onClick={toggle}
		> 
      <AiOutlineClose />
			Cerrar caja
		</Button>
     
      <Modal isOpen={modal} fade={false} toggle={toggle} centered={true}>
        <ModalHeader toggle={toggle}>
          <FaCashRegister size={30} /> Cerrar Caja
        </ModalHeader>
        <ModalBody>
          <Formik
            initialValues={{
              montoInicial: "",
            }}
            validate={(valores) => {
              // Validaciones...
              let errores = {};
              // Validacion montoInicial
              if (!valores.montoInicial) {
                errores.montoInicial = "Por favor ingresa el monto para abrir caja";
                /* !/^[0-9]{9}$/.test(valores.montoInicial) */
              } else if (!/^(\d{1,4}(\.\d{1,2})?)$/.test(valores.montoInicial)) {
                errores.montoInicial =
                  "El monto inicial debe tener un máximo de 4 números y 2 decimales opcionales";
              }
              return errores;
            }}
            onSubmit={async (valores, { resetForm }) => {
              // Captura de la data que se va a enviar...
              dataCerrar.cantidadSacar = resultadoFinal
              // Método PUT para cerra caja
              try {
                const response = await fetch(
                  `http://localhost:5173/api/Caja/${IdActivo}`,
                  {
                    method: "PUT",
                    body: JSON.stringify(dataCerrar),
                    headers: {
                      Authorization: `Bearer ${localStorage.token}`,
                      "Content-Type": "application/json",
                    },
                  });
                if(response.ok) {
                  /* Ocultar el modal */
                  setModal(false)
                  Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: 'Caja Cerrada',
                    showConfirmButton: false,
                    timer: 1000
                  })
                  /* Eliminar el valor de localStorage */
                  localStorage.removeItem("cajaAbierta")
                  navigate('/')
                }  
              } catch (error) {
                console.log(error);
              }
              // Resetear el formulario y mostrar mensaje de éxito
            
            }}
          >
            {({ values, handleSubmit, handleChange, handleBlur, errors, touched }) => (
              <form className="formulario" onSubmit={handleSubmit}>
                <FormGroup row>
                  <Label for="input-montoInicial" sm={4}>
                    Monto a retirar
                  </Label>
                  <Col sm={7}>
                    <Input
                      type="text"
                      id="input-montoInicial"
                      name="montoInicial"
                      placeholder="250"
                      autoComplete="off"
                      value={values.montoInicial}
                      onBlur={handleBlur}
                      onChange={handleChange}
                      /* Feedback para el usuario con el prop valid o invalid de reactstrap */
                      valid={
                        touched.montoInicial &&
                        !errors.montoInicial &&
                        values.montoInicial.length > 0
                      }
                      invalid={touched.montoInicial && !!errors.montoInicial}
                    />

                    {touched.montoInicial && errors.montoInicial && (
                      <div className="error">{errors.montoInicial}</div>
                      )}
                  </Col>
                </FormGroup>
                <FieldArray name="caja">
                  
                  {({ insert, remove, push }) => (
                    <>
                      {/* Campos del formulario */}
                      <Table className="mt-3" bordered striped>
                        <thead>
                          <tr>
                            <th>Caja</th>
                            <th>Totales</th>
                          </tr>
                        </thead>
                        <tbody>
                          <tr>
                            <td title="saldo entregado en la caja anterior">
                              Saldo actual en caja
                            </td>
                            <td>{getDataCashBoxId[0]?.caja || 0}</td>
                          </tr>
                          <tr>
                            <td>Monto a retirar</td>
                            <td>{values.montoInicial || 0}</td>
                          </tr>
                          <tr>
                            <th>Resultante en caja</th>
                            {/* Cálculo del monto inicial de la jornada */}
                            <th className="d-none">
                              {/* getDataCashBoxId[0]?.caja es lo que hay actualmente en caja */}
                              {setResultadoFinal(getDataCashBoxId[0]?.caja - parseFloat(values.montoInicial))
                              || getDataCashBoxId[0]?.caja || 0}
                            </th>
                            <th>{resultadoFinal || getDataCashBoxId[0]?.caja || 0}</th>
                          </tr>
                        </tbody>
                      </Table>
                    </>
                  )}
                </FieldArray>

                <Button type="submit" color="primary" outline>
                  Registrar
                </Button>
                <Button color="secondary" onClick={toggle}>
                  Cancelar
                </Button>
               {/*  {formularioEnviado && (
                  <p className="exito">Formulario enviado con éxito!</p>
                )} */}
              </form>
            )}
          </Formik>
        </ModalBody>
        <ModalFooter></ModalFooter>
      </Modal>
    </>
  );
}

export default ModalBoxClose;
