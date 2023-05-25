import React, { useState, useEffect } from 'react';
import { FormGroup, Label, Col, Input, Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import { Formik, ErrorMessage } from 'formik';
import Swal from 'sweetalert2';
import { BiEditAlt } from 'react-icons/bi';
import { TbTruck } from 'react-icons/tb';

const ModalEditWaiter = ({ idMesero, actualizarListaMesero }) => {
  const [modal, setModal] = useState(false);
  const toggleModal = () => setModal(!modal);
  const [formularioEnviado, cambiarFormularioEnviado] = useState(false);
  const [data, setData] = useState({
    nombre: ''
  });

  const getWaiterData = async (id) => {
    try {
      const response = await fetch(`http://localhost:5173/api/Mesero/${id}`, {
        headers: {
          Authorization: `Bearer ${localStorage.token}`,
          'Content-Type': 'application/json'
        }
      });
      const waiterData = await response.json();
      setData({
        ...data,
        nombre: waiterData.nombre
      });
    } catch (error) {
      console.log('Error Message: ' + error.ErrorMessage);
    }
  };

  useEffect(() => {
    if (idMesero) {
      getWaiterData(idMesero);
    }
  }, [idMesero]);

  const handleSubmit = async (values, { resetForm }) => {
    const bodyTest = {
      idMesero: idMesero,
      nombre: values.nombre
    };

    cambiarFormularioEnviado(true);
    setTimeout(() => cambiarFormularioEnviado(false), 5000);
    resetForm();
    console.log(bodyTest);

    try {
      const response = await fetch(`http://localhost:5173/api/Mesero/${idMesero}`, {
        method: 'PUT',
        body: JSON.stringify(bodyTest),
        headers: {
          Authorization: `Bearer ${localStorage.token}`,
          'Content-Type': 'application/json'
        }
      });
      console.log(response);
      console.log(response.ok);
      if (response.ok) {
        setModal(false);
        /* Alert */
        Swal.fire({
          position: 'center',
          icon: 'success',
          title: 'Registro actualizado correctamente',
          showConfirmButton: false,
          timer: 1500
        });
        /* Prop para actualizar la data de la tabla */
        actualizarListaMesero();
      }
    } catch (error) {
      console.log(error);
    }
  };

  return (
    <>
      <BiEditAlt size={22} onClick={toggleModal} />
      <Modal isOpen={modal} fade={false} toggle={toggleModal} centered={true}>
        <ModalHeader toggle={toggleModal}>
          <TbTruck size={30} /> Editar Mesero
        </ModalHeader>
        <ModalBody>
          <Formik
            initialValues={{
              nombre: data.nombre
            }}
            enableReinitialize={true}
            validate={(valores) => {
              let errores = {};
              if (!valores.nombre) {
                errores.nombre = 'Por favor ingresa un nombre';
              } else if (!/^[a-zA-ZÀ-ÿ\s.]{1,25}$/.test(valores.nombre)) {
                errores.nombre =
                  'El nombre debe tener un máximo de 25 caracteres y solo puede contener letras';
              }
              return errores;
            }}
            onSubmit={handleSubmit}
          >
            {(props) => (
              <form onSubmit={props.handleSubmit}>
                <FormGroup row>
                  <Label for="nombre" sm={3}>
                    Nombre
                  </Label>
                  <Col sm={9}>
                    <Input
                      type="text"
                      name="nombre"
                      id="nombre"
                      value={props.values.nombre}
                      onChange={props.handleChange}
                      onBlur={props.handleBlur}
                      invalid={props.touched.nombre && props.errors.nombre}
                    />
                    <ErrorMessage name="nombre" component="div" className="field-error text-danger" />
                  </Col>
                </FormGroup>
                <Button type="submit" color="primary" outline>
                  Registrar
                </Button>
                <Button color="secondary" onClick={toggleModal}>
                  Cancelar
                </Button>
                {formularioEnviado && <p className="exito">Formulario enviado con exito!</p>}
              </form>
            )}
          </Formik>
        </ModalBody>
      </Modal>
    </>
  );
};

export default ModalEditWaiter;