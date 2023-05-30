import React, { useState } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, FormGroup, Label, Col, Input } from 'reactstrap';
import { Formik, Form } from 'formik';
import { BiEditAlt, BiFoodMenu } from 'react-icons/bi';
import Swal from 'sweetalert2';
import '../../styles/Formulario.scss';

function ModalAdd(props) {
  const [modal, setModal] = useState(false);
  const toggle = () => setModal(!modal);

  const [formularioEnviado, cambiarFormularioEnviado] = useState(false);

  const bodyProvider = {
    nombre: '',
  };

  const validate = (valores) => {
    let errores = {};
    if (!valores.nombre) {
      errores.nombre = 'Por favor ingresa un nombre';
    } else if (!/^[a-zA-ZÀ-ÿ\s]{1,25}$/.test(valores.nombre)) {
      errores.nombre =
        'El nombre debe tener un máximo de 25 caracteres y solo puede contener letras y espacios';
    }
    return errores;
  };

  const handleSubmit = async (valores, { resetForm }) => {
    bodyProvider.nombre = valores.nombre;

    try {
      const response = await fetch(
        `${import.meta.env.VITE_BACKEND_URL}/api/Mesero`,
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
          position: 'center',
          icon: 'success',
          title: 'Registro agregado correctamente',
          showConfirmButton: false,
          timer: 1500
        });

        props.actualizarListaMesero();
        cambiarFormularioEnviado(true);
        setTimeout(() => cambiarFormularioEnviado(false), 5000);
        resetForm();
      } else {
        console.log('Ha ocurrido un error al enviar el formulario');
      }
    } catch (error) {
      console.log('Ha ocurrido un error al enviar el formulario');
      console.log(error);
    }
  };

  return (
    <>
      <Button color="danger" outline onClick={toggle}>
        <BiEditAlt /> Registrar Nuevo 
      </Button>
      <Modal isOpen={modal} fade={false} toggle={toggle} centered={true}>
        <ModalHeader toggle={toggle}>
          <BiFoodMenu size={30} /> Ingreso de nuevo Mesero
        </ModalHeader>
        <ModalBody>
          <Formik
            initialValues={{
              nombre: '',
            }}
            validate={validate}
            onSubmit={handleSubmit}
          >
            {({ values, handleSubmit, handleChange, handleBlur, errors, touched, setTouched }) => (
              <Form className="formulario" onSubmit={handleSubmit}>
                <FormGroup row>
                  <Label for="input-nombre" sm={2}>
                    Nombre
                  </Label>
                  <Col sm={10}>
                    <Input
                      type="text"
                      id="input-nombre"
                      name="nombre"
                      placeholder="Juan Pérez"
                      autoComplete="off"
                      value={values.nombre}
                      onChange={handleChange}
                      onBlur={handleBlur}
                      valid={touched.nombre && !errors.nombre && values.nombre.length > 0}
                      invalid={touched.nombre && !!errors.nombre}
                    />
                    {touched.nombre && errors.nombre && <div className="error">{errors.nombre}</div>}
                  </Col>
                </FormGroup>
                <Button type="submit" color="primary" outline>
                  Registrar
                </Button>
                <Button color="secondary" onClick={toggle}>
                  Cancelar
                </Button>
                {formularioEnviado && <p className="exito">Formulario enviado con éxito</p>}
              </Form>
            )}
          </Formik>
        </ModalBody>
        <ModalFooter></ModalFooter>
      </Modal>
    </>
  );
}

export default ModalAdd;