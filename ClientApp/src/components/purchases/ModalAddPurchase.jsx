import React, { useState, useEffect } from "react";
import {
  Button,
  Modal,
  ModalHeader,
  ModalBody,
  ModalFooter,
  FormGroup,
  Label,
  Col,
  Input,
} from "reactstrap";
import { Formik, Form, ErrorMessage } from "formik";
import { BiEditAlt, BiFoodMenu } from "react-icons/bi";
import Swal from "sweetalert2";
import "../../styles/Formulario.scss";

function ModalAdd(props) {
  const [modal, setModal] = useState(false);
  const toggle = () => setModal(!modal);
  const [formularioEnviado, setFormularioEnviado] = useState(false);

  const [proveedores, setProveedores] = useState([]);

  const fetchProveedores = async () => {
    try {
      const response = await fetch("http://localhost:5173/api/Proveedor/", {
        method: "GET",
        headers: {
          Authorization: `Bearer ${localStorage.token}`,
        },
      });

      if (response.ok) {
        const data = await response.json();
        setProveedores(data);
      } else {
        console.log("Error fetching proveedores");
      }
    } catch (error) {
      console.log("Error fetching proveedores");
      console.log(error);
    }
  };

  useEffect(() => {
    fetchProveedores();
  }, []);

  const bodyProvider = {
    numeroDocumento: "",
    fecha: "",
    concepto: "",
    total: "",
    idProveedor: "",
  };

  const validate = (valores) => {
    let errores = {};

    if (!valores.numeroDocumento) {
      errores.numeroDocumento = "Por favor ingresa un número de documento";
    } else if (!/^[a-zA-Z0-9]+$/.test(valores.numeroDocumento)) {
      errores.numeroDocumento =
        "El número de documento solo puede contener letras y dígitos";
    }

    if (!valores.fecha) {
      errores.fecha = "Por favor ingresa una fecha";
    }

    if (!valores.concepto) {
      errores.concepto = "Por favor ingresa un concepto";
    } else if (!/^[a-zA-Z\s]+$/.test(valores.concepto)) {
      errores.concepto = "El concepto solo puede contener letras y espacios";
    }

    if (!valores.total) {
      errores.total = "Por favor ingresa un total";
    } else if (!/^\d+(\.\d{1,2})?$/.test(valores.total)) {
      errores.total =
        "El total debe ser un número válido con hasta 2 decimales";
    }

    if (!valores.idProveedor) {
      errores.idProveedor = "Por favor selecciona un proveedor";
    }

    return errores;
  };

  const onSubmit = async (valores, { resetForm }) => {
    bodyProvider.numeroDocumento = valores.numeroDocumento;
    bodyProvider.fecha = valores.fecha;
    bodyProvider.concepto = valores.concepto;
    bodyProvider.total = valores.total;
    bodyProvider.idProveedor = valores.idProveedor;

    try {
      const response = await fetch("http://localhost:5173/api/Gasto/", {
        method: "POST",
        headers: {
          Authorization: `Bearer ${localStorage.token}`,
          "Content-Type": "application/json",
        },
        body: JSON.stringify(bodyProvider),
      });

      if (response.ok) {
        Swal.fire({
          position: "center",
          icon: "success",
          title: "Registro agregado correctamente",
          showConfirmButton: false,
          timer: 1500,
        });

        props.actualizarListaMenu();
        console.log("Formulario enviado con éxito");
        setFormularioEnviado(true);
        setTimeout(() => setFormularioEnviado(false), 5000);
        resetForm();
      } else {
        console.log("Ha ocurrido un error al enviar el formulario");
      }
    } catch (error) {
      console.log("Ha ocurrido un error al enviar el formulario");
      console.log(error);
    }
  };

  return (
    <>
      <Button color="success" outline onClick={toggle}>
        Registrar Gasto
        <BiEditAlt />
      </Button>
      <Modal isOpen={modal} fade={false} toggle={toggle} centered={true}>
        <ModalHeader toggle={toggle}>
          <BiFoodMenu size={30} /> Ingreso de un nuevo gasto
        </ModalHeader>
        <ModalBody>
          <Formik
            initialValues={{
              numeroDocumento: "",
              fecha: "",
              concepto: "",
              total: "",
              idProveedor: "",
            }}
            validate={validate}
            onSubmit={onSubmit}
          >
            {({
              values,
              handleSubmit,
              handleChange,
              handleBlur,
              errors,
              touched,
            }) => (
              <Form className="formulario" onSubmit={handleSubmit}>
                <FormGroup row>
                  <Label for="input-numeroDocumento" sm={2}>
                    Número de Documento
                  </Label>
                  <Col sm={10}>
                    <Input
                      type="text"
                      id="input-numeroDocumento"
                      name="numeroDocumento"
                      placeholder="Documento"
                      autoComplete="off"
                      value={values.numeroDocumento}
                      onChange={handleChange}
                      onBlur={handleBlur}
                      valid={
                        touched.numeroDocumento &&
                        !errors.numeroDocumento &&
                        values.numeroDocumento.length > 0
                      }
                      invalid={
                        touched.numeroDocumento && !!errors.numeroDocumento
                      }
                    />
                    <ErrorMessage
                      name="numeroDocumento"
                      component="div"
                      className="error"
                    />
                  </Col>
                </FormGroup>
                <FormGroup row>
                  <Label for="input-fecha" sm={2}>
                    Fecha
                  </Label>
                  <Col sm={10}>
                    <Input
                      type="date"
                      id="input-fecha"
                      name="fecha"
                      autoComplete="off"
                      value={values.fecha}
                      onBlur={handleBlur}
                      onChange={handleChange}
                      valid={
                        touched.fecha &&
                        !errors.fecha &&
                        values.fecha.length > 0
                      }
                      invalid={touched.fecha && !!errors.fecha}
                    />
                    <ErrorMessage
                      name="fecha"
                      component="div"
                      className="error"
                    />
                  </Col>
                </FormGroup>
                <FormGroup row>
                  <Label for="input-concepto" sm={2}>
                    Concepto
                  </Label>
                  <Col sm={10}>
                    <Input
                      type="text"
                      id="input-concepto"
                      name="concepto"
                      placeholder="Concepto"
                      autoComplete="off"
                      value={values.concepto}
                      onBlur={handleBlur}
                      onChange={handleChange}
                      valid={
                        touched.concepto &&
                        !errors.concepto &&
                        values.concepto.length > 0
                      }
                      invalid={touched.concepto && !!errors.concepto}
                    />
                    <ErrorMessage
                      name="concepto"
                      component="div"
                      className="error"
                    />
                  </Col>
                </FormGroup>
                <FormGroup row>
                  <Label for="input-total" sm={2}>
                    Total
                  </Label>
                  <Col sm={10}>
                    <Input
                      type="number"
                      id="input-total"
                      name="total"
                      placeholder="Total"
                      autoComplete="off"
                      value={values.total}
                      onBlur={handleBlur}
                      onChange={handleChange}
                      valid={
                        touched.total &&
                        !errors.total &&
                        values.total.length > 0
                      }
                      invalid={touched.total && !!errors.total}
                    />
                    <ErrorMessage
                      name="total"
                      component="div"
                      className="error"
                    />
                  </Col>
                </FormGroup>
                <FormGroup row>
                  <Label for="select-idProveedor" sm={2}>
                    Proveedor
                  </Label>
                  <Col sm={10}>
                    <Input
                      type="select"
                      id="select-idProveedor"
                      name="idProveedor"
                      value={values.idProveedor}
                      onBlur={handleBlur}
                      onChange={handleChange}
                      valid={
                        touched.idProveedor &&
                        !errors.idProveedor &&
                        values.idProveedor.length > 0
                      }
                      invalid={touched.idProveedor && !!errors.idProveedor}
                    >
                      <option value="">Selecciona un proveedor</option>
                      {proveedores.map((proveedor) => (
                        <option
                          value={proveedor.idProveedor}
                          key={proveedor.idProveedor}
                        >
                          {proveedor.nombre}
                        </option>
                      ))}
                    </Input>
                    <ErrorMessage
                      name="idProveedor"
                      component="div"
                      className="error"
                    />
                  </Col>
                </FormGroup>
                <Button type="submit" color="success" outline>
                  Registrar
                </Button>
                <Button color="danger" outline onClick={toggle}>
                  Cancelar
                </Button>
                {formularioEnviado && (
                  <p className="exito">Formulario enviado con éxito!</p>
                )}
              </Form>
            )}
          </Formik>
        </ModalBody>
      </Modal>
    </>
  );
}

export default ModalAdd;

/* Helper Functions */