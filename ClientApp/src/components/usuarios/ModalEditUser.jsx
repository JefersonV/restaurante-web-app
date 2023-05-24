import React, { useState, useEffect } from 'react';
import { FormGroup, Label, Col, Input, Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import { Formik, ErrorMessage } from 'formik';
import Swal from 'sweetalert2';
import { BiEditAlt, BiHide, BiShow } from 'react-icons/bi';
import { TbTruck } from 'react-icons/tb';

const ModalEditUser = (props) => {
  const [modal, setModal] = useState(false);
  const toggleModal = () => setModal(!modal);
  const [formularioEnviado, cambiarFormularioEnviado] = useState(false);
  const { idUsuario, actualizarListaUsuario } = props;
  const [data, setData] = useState({
    nombre: '',
    contrasenia: '',
    confirmarContrasenia: '',
    idTipoUsuario: ''
  });
  const [mostrarContrasenia, setMostrarContrasenia] = useState(false);

  const getUsuarioData = async (id) => {
    try {
      const response = await fetch(`http://localhost:5173/api/Account/${id}`, {
        headers: {
          Authorization: `Bearer ${localStorage.token}`,
          'Content-Type': 'application/json'
        }
      });
      const usuarioData = await response.json();
      setData({
        ...data,
        nombre: usuarioData.nombre,
        idTipoUsuario: usuarioData.tipoUsuario
      });
    } catch (error) {
      console.log('Error Message: ' + error.ErrorMessage);
    }
  };

  useEffect(() => {
    if (idUsuario) {
      getUsuarioData(idUsuario);
    }
  }, [idUsuario]);

  const handleSubmit = async (values, { resetForm }) => {
    const bodyTest = {
      idUsuario: idUsuario,
      nombre: values.nombre,
      contrasenia: values.contrasenia ? values.contrasenia : '',
      idTipoUsuario: values.idTipoUsuario
    };

    cambiarFormularioEnviado(true);
    setTimeout(() => cambiarFormularioEnviado(false), 5000);
    resetForm();
    console.log(bodyTest);

    try {
      const response = await fetch(`http://localhost:5173/api/Account/update/${idUsuario}`, {
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
        actualizarListaUsuario();
      }
    } catch (error) {
      console.log(error);
    }
  };

  const handleMostrarContrasenia = () => {
    setMostrarContrasenia(!mostrarContrasenia);
  };

  return (
    <>
      <BiEditAlt size={22} onClick={toggleModal} />
      <Modal isOpen={modal} fade={false} toggle={toggleModal} centered={true}>
        <ModalHeader toggle={toggleModal}>
          <TbTruck size={30} /> Editar Usuario
        </ModalHeader>
        <ModalBody>
          <Formik
            initialValues={{
              nombre: data.nombre,
              contrasenia: data.contrasenia,
              confirmarContrasenia: data.confirmarContrasenia,
              // idTipoUsuario: data.idTipoUsuario
            }}
            enableReinitialize={true}
            validate={(values) => {
              let errors = {};
              if (!values.nombre) {
                errors.nombre = 'Por favor ingresa un nombre';
              } else if (!/^[a-zA-ZÀ-ÿ\s.]{1,25}$/.test(values.nombre)) {
                errors.nombre =
                  'El nombre debe tener un máximo de 25 caracteres y solo puede contener letras';
              }
              // if (!values.contrasenia) {
              //   errors.contrasenia = 'Por favor ingresa una contraseña';
              // } else if (values.contrasenia.length < 6) {
              //   errors.contrasenia = 'La contraseña debe tener al menos 6 caracteres';
              // }
              if (values.contrasenia !== values.confirmarContrasenia) {
                errors.confirmarContrasenia = 'Las contraseñas no coinciden';
              }
              return errors;
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
                      autoComplete='off'
                      value={data.nombre}
                      onChange={props.handleChange}
                      onBlur={props.handleBlur}
                      valid={props.touched.nombre && !props.errors.nombre && props.values.nombre.length > 0}
                      invalid={props.touched.nombre && props.errors.nombre}
                    />
                    <ErrorMessage name="nombre" component="div" className="field-error text-danger" />
                  </Col>
                </FormGroup>
                <FormGroup row>
                  <Label for="contrasenia" sm={3}>
                    Contraseña
                  </Label>
                  <Col sm={9}>
                    <Input
                      type={mostrarContrasenia ? 'text' : 'password'}
                      name="contrasenia"
                      id="contrasenia"
                      placeholder='Ingrese la nueva contraseña'
                      value={props.values.contrasenia}
                      onChange={props.handleChange}
                      onBlur={props.handleBlur}
                      invalid={props.touched.contrasenia && props.errors.contrasenia}
                    />
                    <BiShow
                      className="eye-icon"
                      onClick={handleMostrarContrasenia}
                      style={{ display: mostrarContrasenia ? 'none' : 'block' }}
                    />
                    <BiHide
                      className="eye-icon"
                      onClick={handleMostrarContrasenia}
                      style={{ display: mostrarContrasenia ? 'block' : 'none' }}
                    />
                    <ErrorMessage name="contrasenia" component="div" className="field-error text-danger" />
                  </Col>
                </FormGroup>
                <FormGroup row>
                  <Label for="confirmarContrasenia" sm={3}>
                    Confirmar Contraseña
                  </Label>
                  <Col sm={9}>
                    <Input
                      type={mostrarContrasenia ? 'text' : 'password'}
                      name="confirmarContrasenia"
                      id="confirmarContrasenia"
                      placeholder='Confirme su contraseña'
                      value={props.values.confirmarContrasenia}
                      onChange={props.handleChange}
                      onBlur={props.handleBlur}
                      invalid={props.touched.confirmarContrasenia && props.errors.confirmarContrasenia}
                    />
                    <BiShow
                      className="eye-icon"
                      onClick={handleMostrarContrasenia}
                      style={{ display: mostrarContrasenia ? 'none' : 'block' }}
                    />
                    <BiHide
                      className="eye-icon"
                      onClick={handleMostrarContrasenia}
                      style={{ display: mostrarContrasenia ? 'block' : 'none' }}
                    />
                    <ErrorMessage
                      name="confirmarContrasenia"
                      component="div"
                      className="field-error text-danger"
                    />
                  </Col>
                </FormGroup>
                <FormGroup row>
                  <Label for="idTipoUsuario" sm={3}>
                    Tipo de Usuario
                  </Label>
                  <Col sm={9}>
                    <Input
                      type="select"
                      name="idTipoUsuario"
                      id="idTipoUsuario"
                      value={props.values.idTipoUsuario || ""}
                      onChange={props.handleChange}
                      onBlur={props.handleBlur}
                      invalid={props.touched.idTipoUsuario && props.errors.idTipoUsuario}
                    >
                      <option value="" disabled>
                        {data.idTipoUsuario ? data.idTipoUsuario : 'Seleccionar tipo de usuario'}
                      </option>
                      <option value="1">Administrador</option>
                      <option value="2">Invitado</option>
                    </Input>
                    <ErrorMessage name="idTipoUsuario" component="div" className="field-error text-danger" />
                  </Col>
                </FormGroup>
                <ModalFooter>
                  <Button color="primary" type="submit" disabled={formularioEnviado}>
                    {formularioEnviado ? 'Enviando...' : 'Guardar Cambios'}
                  </Button>{' '}
                  <Button color="secondary" onClick={toggleModal}>
                    Cancelar
                  </Button>
                </ModalFooter>
              </form>
            )}
          </Formik>
        </ModalBody>
      </Modal>
    </>
  );
};

export default ModalEditUser;
