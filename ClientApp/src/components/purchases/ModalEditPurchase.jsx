import React, { useState, useEffect } from "react";
import { FormGroup, Label, Col, Input } from "reactstrap";
import { Formik, Form, Field, ErrorMessage } from "formik";
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from "reactstrap";
import Swal from "sweetalert2";
import { BiEditAlt } from "react-icons/bi";
import { TbTruck } from "react-icons/tb";

function ModalEdit(props) {
  const [modal, setModal] = useState(false);
  const toggle = () => setModal(!modal);
  const [formularioEnviado, cambiarFormularioEnviado] = useState(false);
  const [itemId, setItemId] = useState("");
  const [data, setData] = useState({
    numeroDocumento: "",
    fecha: "",
    concepto: "",
    total: "",
    idProveedor: "",
  });
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

  const bodyTest = {
    idGasto: "",
    numeroDocumento: "",
    fecha: "",
    concepto: "",
    total: "",
    idProveedor: "",
  };

  const getDataId = async (id) => {
    try {
      const response = await fetch(`http://localhost:5173/api/Gasto/${id}`, {
        headers: {
          Authorization: `Bearer ${localStorage.token}`,
          "Content-Type": "application/json",
        },
      });
      const dataFetch = await response.json();
      console.warn(dataFetch);
      const { idProveedor} = proveedores.find(
        (proveedor) => proveedor.nombre == dataFetch.proveedor
      );
      console.log(idProveedor);
      setData({
        ...data,
        numeroDocumento: dataFetch.numeroDocumento,
        fecha: dataFetch.fecha,
        concepto: dataFetch.concepto,
        total: dataFetch.total,
        idProveedor: idProveedor,
      });
      
    } catch (error) {
      console.log("Error Message: " + error.ErrorMessage);
    }
  };

  useEffect(() => {
    if (itemId) {
      getDataId(itemId);
    }
  }, [itemId]);

  return (
    <>
      <BiEditAlt
        size={22}
        onClick={() => {
          toggle();
          setItemId(props.idPlatillo);
        }}
      />
      <Modal isOpen={modal} fade={false} toggle={toggle} centered={true}>
        <ModalHeader toggle={toggle}>
          <TbTruck size={30} /> Editar Gasto
        </ModalHeader>
        <ModalBody>
          <Formik
            initialValues={data}
            enableReinitialize={true}
            validate={(valores) => {
              let errores = {};
              // Validacion numeroDocumento
              if (!valores.numeroDocumento) {
                errores.numeroDocumento =
                  "Por favor ingresa un número de documento";
              } else if (!/^[a-zA-Z0-9]+$/.test(valores.numeroDocumento)) {
                errores.numeroDocumento =
                  "El número de documento solo puede contener letras y dígitos";
              }
              // Validacion fecha
              if (!valores.fecha) {
                errores.fecha = "Por favor ingresa una fecha";
              }
              // Validacion concepto
              if (!valores.concepto) {
                errores.concepto = "Por favor ingresa un concepto";
              } else if (!/^[a-zA-Z\s]+$/.test(valores.concepto)) {
                errores.concepto =
                  "El concepto solo puede contener letras y espacios";
              }
              // Validacion total
              if (!valores.total) {
                errores.total = "Por favor ingresa un total";
              } else if (!/^\d+(\.\d{1,2})?$/.test(valores.total)) {
                errores.total =
                  "El total debe ser un número válido con hasta 2 decimales";
              }
              // Validacion idProveedor
              if (!valores.idProveedor) {
                errores.idProveedor = "Por favor selecciona un proveedor";
              }

              return errores;
            }}
            onSubmit={async (valores, { resetForm }) => {
              bodyTest.idGasto = itemId;
              bodyTest.numeroDocumento = valores.numeroDocumento;
              bodyTest.fecha = valores.fecha;
              bodyTest.concepto = valores.concepto;
              bodyTest.total = valores.total;
              bodyTest.idProveedor = valores.idProveedor;

              cambiarFormularioEnviado(true);
              setTimeout(() => cambiarFormularioEnviado(false), 5000);
              resetForm();

              try {
                const response = await fetch(
                  `http://localhost:5173/api/Gasto/${itemId}`,
                  {
                    method: "PUT",
                    body: JSON.stringify(bodyTest),
                    headers: {
                      Authorization: `Bearer ${localStorage.token}`,
                      "Content-Type": "application/json",
                    },
                  }
                );
                if (response.ok) {
                  setModal(false);
                  Swal.fire({
                    position: "center",
                    icon: "success",
                    title: "Registro actualizado correctamente",
                    showConfirmButton: false,
                    timer: 1500,
                  });
                  props.actualizarListaMenu();
                }
              } catch (error) {
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
              setFieldValue,
            }) => (
              <form className="formulario" onSubmit={handleSubmit}>
                <FormGroup row>
                  <Label for="input-numeroDocumento" sm={2}>
                    Número de documento
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
                        !errors.numeroDocumento &&
                        values.numeroDocumento.length > 0
                      }
                      invalid={!!errors.numeroDocumento}
                    />
                    <ErrorMessage
                      name="numeroDocumento"
                      component={() => (
                        <div className="error">{errors.numeroDocumento}</div>
                      )}
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
                      valid={!errors.fecha && values.fecha.length > 0}
                      invalid={!!errors.fecha}
                    />
                    <ErrorMessage
                      name="fecha"
                      component={() => (
                        <div className="error">{errors.fecha}</div>
                      )}
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
                      onChange={handleChange}
                      onBlur={handleBlur}
                      valid={!errors.concepto && values.concepto.length > 0}
                      invalid={!!errors.concepto}
                    />
                    <ErrorMessage
                      name="concepto"
                      component={() => (
                        <div className="error">{errors.concepto}</div>
                      )}
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
                      step="0.01"
                      id="input-total"
                      name="total"
                      placeholder="Total"
                      autoComplete="off"
                      value={values.total}
                      onChange={handleChange}
                      onBlur={handleBlur}
                      valid={!errors.total && values.total.length > 0}
                      invalid={!!errors.total}
                    />
                    <ErrorMessage
                      name="total"
                      component={() => (
                        <div className="error">{errors.total}</div>
                      )}
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
                      onChange={handleChange}
                      onBlur={handleBlur}
                      valid={
                        !errors.idProveedor && values.idProveedor.length > 0
                      }
                      invalid={!!errors.idProveedor}
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
                      name="proveedor"
                      component={() => (
                        <div className="error">{errors.idProveedor}</div>
                      )}
                    />
                  </Col>
                </FormGroup>
                <Button type="submit" color="primary" outline>
                  Registrar
                </Button>
                <Button type="button" color="secondary" onClick={toggle}>
                  Cancelar
                </Button>
                {formularioEnviado && (
                  <p className="exito">Formulario enviado con éxito!</p>
                )}
              </form>
            )}
          </Formik>
        </ModalBody>
      </Modal>
    </>
  );
}

export default ModalEdit;
