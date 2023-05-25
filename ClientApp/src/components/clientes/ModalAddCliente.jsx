import React, { useState } from "react";
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from "reactstrap";
import { FormGroup, Label, Col, Input } from "reactstrap";
import { Formik, Form, Field, ErrorMessage } from "formik";
import { BiEditAlt } from "react-icons/bi";
import { BsFillPersonVcardFill } from "react-icons/bs";
import Swal from "sweetalert2";
import "../../styles/Formulario.scss";

function ModalAddCliente(props) {
  const [modal, setModal] = useState(false);
  const toggle = () => setModal(!modal);
  /* Formik */
  const [formularioEnviado, cambiarFormularioEnviado] = useState(false);

  const bodyProvider = {
    nombreApellido: "",
    fecha: "",
    institucion: "",
    puesto: "",
    venta: [],
  };

  return (
    <>
      <Button color="danger" outline onClick={toggle}>
        Registra Nuevo
        <BiEditAlt />
      </Button>
      <Modal isOpen={modal} fade={false} toggle={toggle} centered={true}>
        <ModalHeader toggle={toggle}>
          <BsFillPersonVcardFill size={30} /> Ingreso de nuevo Cliente
        </ModalHeader>
        <ModalBody>
          <Formik
            initialValues={{
              nombreApellido: "",
              fecha: "",
              institucion: "",
              puesto: "",
              venta: [],
            }}
            validate={(valores) => {
              let errores = {};
              // Validacion nombre
              if (!valores.nombreApellido) {
                errores.nombreApellido = "Por favor ingresa un nombre";
              } else if (!/^[a-zA-ZÀ-ÿ\s.]{1,25}$/.test(valores.nombre)) {
                errores.nombreApellido =
                  "El nombre debe tener un máximo de 25 caracteres, solo puede contener letras y espacios";
              }
              // Validacion telefono
              if (!valores.fecha) {
                errores.fecha = "Por favor ingresa un número telefónico";
                /* !/^[0-9]{9}$/.test(valores.fecha) */
              } 
              if (!valores.institucion) {
                errores.institucion =
                  "Por favor ingresa un nombre de institución";
              } else if (!/^[a-zA-ZÀ-\s.]{1,25}$/.test(valores.institucion)) {
                errores.institucion =
                  "El nombre de la institución debe tener un máximo de 25 caracteres, solo puede contener letras y espacios";
              }
              if (!valores.puesto) {
                errores.puesto = "Por favor ingresa un puesto de trabajo";
              } else if (!/^[a-zA-ZÀ-\s.]{1,25}$/.test(valores.puesto)) {
                errores.puesto =
                  "El nombre de la institución debe tener un máximo de 25 caracteres, solo puede contener letras y espacios";
              }
              return errores;
            }}
            onSubmit={async (valores, { resetForm }) => {
              console.log(valores);
              /* Captura de la data que se va a enviar con post */
              bodyProvider.nombreApellido = valores.nombreApellido;
              bodyProvider.fecha = valores.fecha;
              bodyProvider.institucion = valores.institucion;
              bodyProvider.puesto = valores.puesto;
              console.warn("json");
              console.log(bodyProvider);

              /* Método Post */
              try {
                const response = await fetch(
                  "http://localhost:5173/api/Cliente",
                  {
                    method: "POST",
                    headers: {
                      Authorization: `Bearer ${localStorage.token}`,
                      "Content-Type": "application/json",
                    },
                    body: JSON.stringify(bodyProvider),
                  }
                );
                if (response.ok) {
                  Swal.fire({
                    position: "center",
                    icon: "success",
                    title: "Registro agregado correctamente",
                    showConfirmButton: false,
                    timer: 1500,
                  });
                  /* Si post fue exitoso se actualiza la data de la tabla */
                  props.actualizarListaClientes();
                  console.log("Formulario enviado con éxito");
                  // State
                  cambiarFormularioEnviado(true);
                  setTimeout(() => cambiarFormularioEnviado(false), 5000);
                  resetForm();
                } else {
                  console.log("Ha ocurrido un error al enviar el formulario");
                }
              } catch (error) {
                console.log("Ha ocurrido un error al enviar el formulario");
                console.log(error);
              }
            }}
          >
            {({
              values,
              handleSubmit,
              handleChange,
              handleBlur,
              errors,
              touched,
              setTouched,
            }) => (
              <form className="formulario" onSubmit={handleSubmit}>
                <FormGroup row>
                  <Label for="input-nombre" sm={2}>
                    Nombre
                  </Label>
                  <Col sm={10}>
                    <Input
                      type="text"
                      id="input-nombre"
                      name="nombreApellido"
                      placeholder="Magnus S.A."
                      autoComplete="off"
                      value={values.nombreApellido}
                      onChange={handleChange}
                      onBlur={handleBlur}
                      /* Feedback para el usuario con el prop valid o invalid de reactstrap */
                      valid={
                        touched.nombreApellido &&
                        !errors.nombreApellido &&
                        values.nombreApellido.length > 0
                      }
                      invalid={
                        touched.nombreApellido && !!errors.nombreApellido
                      }
                    />
                    {touched.nombreApellido && errors.nombreApellido && (
                      <div className="error">{errors.nombreApellido}</div>
                    )}
                    {/* <ErrorMessage name="nombre" component={() => (<div className="error">{errors.nombre}</div>)} /> */}
                  </Col>
                </FormGroup>
                <FormGroup row>
                  <Label for="input-telefono" sm={2}>
                    Fecha
                  </Label>
                  <Col sm={10}>
                    <Input
                      type="date"
                      id="input-fecha"
                      name="fecha"
                      placeholder="77623030"
                      autoComplete="off"
                      value={values.fecha}
                      onBlur={handleBlur}
                      onChange={handleChange}
                      /* Feedback para el usuario con el prop valid o invalid de reactstrap */
                      valid={
                        touched.fecha &&
                        !errors.fecha &&
                        values.fecha.length > 0
                      }
                      invalid={touched.fecha && !!errors.fecha}
                    />

                    {/* <ErrorMessage name="fecha" component={() => (<div className="error">{errors.fecha}</div>)} /> */}
                    {touched.fecha && errors.fecha && (
                      <div className="error">{errors.fecha}</div>
                    )}
                  </Col>
                </FormGroup>

				<FormGroup row>
                  <Label for="input-institucion" sm={2}>
                    Institución
                  </Label>
                  <Col sm={10}>
                    <Input
                      type="text"
                      id="input-institucion"
                      name="institucion"
                      placeholder="77623030"
                      autoComplete="off"
                      value={values.institucion}
                      onBlur={handleBlur}
                      onChange={handleChange}
                      /* Feedback para el usuario con el prop valid o invalid de reactstrap */
                      valid={
                        touched.institucion &&
                        !errors.institucion &&
                        values.institucion.length > 0
                      }
                      invalid={touched.institucion && !!errors.institucion}
                    />

                    {/* <ErrorMessage name="institucion" component={() => (<div className="error">{errors.institucion}</div>)} /> */}
                    {touched.institucion && errors.institucion && (
                      <div className="error">{errors.institucion}</div>
                    )}
                  </Col>
                </FormGroup>

				<FormGroup row>
                  <Label for="input-telefono" sm={2}>
                    Puesto
                  </Label>
                  <Col sm={10}>
                    <Input
                      type="text"
                      id="input-puesto"
                      name="puesto"
                      placeholder="77623030"
                      autoComplete="off"
                      value={values.puesto}
                      onBlur={handleBlur}
                      onChange={handleChange}
                      /* Feedback para el usuario con el prop valid o invalid de reactstrap */
                      valid={
                        touched.puesto &&
                        !errors.puesto &&
                        values.puesto.length > 0
                      }
                      invalid={touched.puesto && !!errors.puesto}
                    />

                    {/* <ErrorMessage name="puesto" component={() => (<div className="error">{errors.puesto}</div>)} /> */}
                    {touched.puesto && errors.puesto && (
                      <div className="error">{errors.puesto}</div>
                    )}
                  </Col>
                </FormGroup>
                {/* <button type="submit" >enviar</button> */}
                <Button type="submit" color="primary" outline>
                  Registrar
                </Button>
                <Button color="secondary" onClick={toggle}>
                  Cancelar
                </Button>
                {formularioEnviado && (
                  <p className="exito">Formulario enviado con exito!</p>
                )}
              </form>
            )}
          </Formik>
        </ModalBody>
        <ModalFooter></ModalFooter>
      </Modal>
    </>
  );
}

export default ModalAddCliente;
