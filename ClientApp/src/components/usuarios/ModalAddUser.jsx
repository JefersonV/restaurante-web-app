import React, { useState } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, FormGroup, Label, Col, Input } from 'reactstrap';
import { Formik, Form } from 'formik';
import { BiEditAlt, BiFoodMenu } from 'react-icons/bi';
import { AiFillEye, AiFillEyeInvisible } from 'react-icons/ai';
import Swal from 'sweetalert2';
import '../../styles/Formulario.scss';

function ModalAdd(props) {
  const [modal, setModal] = useState(false);
  const toggle = () => setModal(!modal);

  const [formularioEnviado, cambiarFormularioEnviado] = useState(false);
  const [mostrarContrasenia, setMostrarContrasenia] = useState(false);

  const bodyProvider = {
    nombre: '',
    contrasenia: '',
    idTipoUsuario: '',
  };

  const validate = (valores) => {
    let errores = {};
    if (!valores.nombre) {
      errores.nombre = 'Por favor ingresa un nombre';
    } else if (!/^[a-zA-ZÀ-ÿ\s]{1,25}$/.test(valores.nombre)) {
      errores.nombre =
        'El nombre debe tener un máximo de 25 caracteres y solo puede contener letras y espacios';
    }
    if (!valores.contrasenia) {
      errores.contrasenia = 'Por favor ingresa una contrasenia';
    }
    if (!valores.idTipoUsuario) {
      errores.idTipoUsuario = 'Por favor selecciona un tipo de usuario';
    }
    return errores;
  };

  const handleSubmit = async (valores, { resetForm }) => {
    bodyProvider.nombre = valores.nombre;
    bodyProvider.contrasenia = valores.contrasenia;
    bodyProvider.idTipoUsuario = valores.idTipoUsuario;

    try {
      const response = await fetch('http://localhost:5173/api/Account/register', {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${localStorage.token}`,
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(bodyProvider)
      });

      if (response.status === 400) {
        Swal.fire({
          position: 'center',
          icon: 'warning',
          title: 'El nombre de usuario ya está registrado',
          showConfirmButton: false,
          timer: 2000
        });
      }

      if (response.ok) {
        Swal.fire({
          position: 'center',
          icon: 'success',
          title: 'Registro agregado correctamente',
          showConfirmButton: false,
          timer: 1500
        });

        props.actualizarListaUsuario();
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
        Registra Nuevo <BiEditAlt />
      </Button>
      <Modal isOpen={modal} fade={false} toggle={toggle} centered={true}>
        <ModalHeader toggle={toggle}>
          <BiFoodMenu size={30} /> Ingreso de nuevo Usuario
        </ModalHeader>
        <ModalBody>
          <Formik
            initialValues={{
              nombre: '',
              contrasenia: '',
              idTipoUsuario: '',
            }}
            validate={validate}
            onSubmit={handleSubmit}
          >
            {({ values, handleSubmit, handleChange, handleBlur, errors, touched }) => (
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
                <FormGroup row>
                  <Label for="input-contrasenia" sm={2}>
                    Contraseña
                  </Label>
                  <Col sm={8}>
                    <Input
                      type={mostrarContrasenia ? 'text' : 'password'}
                      id="input-contrasenia"
                      name="contrasenia"
                      autoComplete="new-password"
                      value={values.contrasenia}
                      onChange={handleChange}
                      onBlur={handleBlur}
                      valid={touched.contrasenia && !errors.contrasenia && values.contrasenia.length > 0}
                      invalid={touched.contrasenia && !!errors.contrasenia}
                    />
                    {touched.contrasenia && errors.contrasenia && <div className="error">{errors.contrasenia}</div>}
                  </Col>
                  <Col sm={2}>
                    {mostrarContrasenia ? (
                      <AiFillEyeInvisible size={20} onClick={() => setMostrarContrasenia(false)} />
                    ) : (
                      <AiFillEye size={20} onClick={() => setMostrarContrasenia(true)} />
                    )}
                  </Col>
                </FormGroup>
                <FormGroup row>
                  <Label for="select-tipo-usuario" sm={2}>
                    Tipo de usuario
                  </Label>
                  <Col sm={10}>
                    <Input
                      type="select"
                      id="select-tipo-usuario"
                      name="idTipoUsuario"
                      value={values.idTipoUsuario}
                      onChange={handleChange}
                      onBlur={handleBlur}
                      valid={touched.idTipoUsuario && !errors.idTipoUsuario}
                      invalid={touched.idTipoUsuario && !!errors.idTipoUsuario}
                    >
                      <option value="" disabled>Seleccione un tipo de usuario</option>
                      <option value="1">Administrador</option>
                      <option value="2">Invitado</option>
                    </Input>
                    {touched.idTipoUsuario && errors.idTipoUsuario && (
                      <div className="error">{errors.idTipoUsuario}</div>
                    )}
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
