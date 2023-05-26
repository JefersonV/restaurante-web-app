import React, { useState, useEffect } from "react";
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from "reactstrap";
import { FormGroup, Label, Col, Input, Table } from "reactstrap";
import { Formik, FieldArray } from "formik";
import { TbTruck } from "react-icons/tb";
import Swal from "sweetalert2";
import { FaCashRegister } from "react-icons/fa";
import "../../styles/Formulario.scss";

function ModalBox(props) {
  const [modal, setModal] = useState(false);
  const toggle = () => setModal(!modal);
  // Saldo registrado en la API
  const [saldoCajaAnterior, setSaldoCajaAnterior] = useState(250);
  // Saldo para iniciar la jornada -> saldoCajaAnt + valor del input
  const [resultadoFinal, setResultadoFinal] = useState(0);

  props.setCajaSesion("false")
  /* Session */
  if (props.cajaAbierta === 'true') {
    // La caja está abierta
    console.log('La caja está abierta');
  } else {
    // La caja está cerrada o el valor no está establecido
    console.log('La caja está cerrada');
  }

  return (
    <>
      <button onClick={toggle} id="abrir">
        <label htmlFor="Abrir">Click para Aperturar</label>
      </button>
      <span>
        <FaCashRegister />
      </span>
      <Modal isOpen={modal} fade={false} toggle={toggle} centered={true}>
        <ModalHeader toggle={toggle}>
          <FaCashRegister size={30} /> Apertura de Caja
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
              const dataCajaPost = {
                "saldoInicial": resultadoFinal
              }
              // Método POST y otras operaciones...
              try {
                const response = await fetch('http://localhost:5173/api/Caja', {
                  method: 'POST',
                  headers: {
                    'Authorization': `Bearer ${localStorage.token}`,
                    'Content-Type': 'application/json'
                  },
                  body: JSON.stringify(dataCajaPost)
                });
                if (response.ok) {
                  Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: 'Caja abierta',
                    showConfirmButton: false,
                    timer: 1000
                  })
                  // Validación de caja en localStorage si se creó la caja entonces
                  // Establecer el estado de apertura de la caja
                  localStorage.setItem('cajaAbierta', true);

                  // Obtener el estado de apertura de la caja
                  props.setCajaSesion(localStorage.getItem('cajaAbierta'))
                  
                  // cajaAbierta = localStorage.getItem('cajaAbierta');

                  resetForm();
                } else {
                  console.log('Ha ocurrido un error al enviar el formulario')
                }
              } catch (error) {
                console.log('Ha ocurrido un error al enviar el formulario')
                console.log(error)
              }
              // Resetear el formulario y mostrar mensaje de éxito
              resetForm();
            }}
          >
            {({ values, handleSubmit, handleChange, handleBlur, errors, touched }) => (
              <form className="formulario" onSubmit={handleSubmit}>
                <FormGroup row>
                  <Label for="input-montoInicial" sm={4}>
                    Monto por añadir
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
                              Saldo en Caja
                            </td>
                            <td>{saldoCajaAnterior}</td>
                          </tr>
                          <tr>
                            <td>Monto añadido</td>
                            <td>{values.montoInicial || 0}</td>
                          </tr>
                          <tr>
                            <th>Monto inicial jornada</th>
                            {/* Cálculo del monto inicial de la jornada */}
                            <th className="d-none">{setResultadoFinal(saldoCajaAnterior + parseFloat(values.montoInicial)) || saldoCajaAnterior || 0}</th>
                            <th>{resultadoFinal || saldoCajaAnterior || 0}</th>
                          </tr>
                        </tbody>
                      </Table>
                    </>
                  )}
                </FieldArray>
                </FormGroup>
                <Button 
                  type="submit" 
                  color="primary" 
                  outline
                  >
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

export default ModalBox;
