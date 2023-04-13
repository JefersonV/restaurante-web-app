import React, { useState } from 'react'
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap'
import { FormGroup, Label, Col, Input } from 'reactstrap'
import { Formik, Form, Field, ErrorMessage } from 'formik';
import { BiEditAlt } from 'react-icons/bi'
import { TbTruck } from 'react-icons/tb'
import '../styles/Formulario.scss'

function ModalAdd () {
  const [modal, setModal] = useState(false);
  const toggle = () => setModal(!modal);
  /* Formik */
  const [formularioEnviado, cambiarFormularioEnviado] = useState(false);
  return (
		<>
      <BiEditAlt onClick={toggle} />
      <Modal isOpen={modal} fade={false} toggle={toggle} centered={true}>
        <ModalHeader toggle={toggle}><TbTruck size={30}/> Ingreso de nuevo Proveedor</ModalHeader>
        <ModalBody>
        <Formik
				initialValues={{
					nombre: '',
					telefono: ''
				}}
				/* validate={(valores) => {
					let errores = {};
					// Validacion nombre
					if(!valores.nombre){
						errores.nombre = 'Por favor ingresa un nombre'
					} else if(!/^[a-zA-ZÀ-ÿ\s]{1,40}$/.test(valores.nombre)){
						errores.nombre = 'El nombre solo puede contener letras y espacios'
					}
					// Validacion telefono
					if(!valores.telefono){
						errores.telefono = 'Por favor ingresa un número telefónico' 
					} else if(/^[\p{L}\d\s.,'-]+$/u.test(valores.telefono)){
						errores.telefono = 'El telefono solo puede tener números y un máximo de 13 caracteres'
					}
					return errores;
				}} */
				onSubmit={(valores) => {
					console.log(valores)
					console.log('Formulario enviado')
				}}
				/* onSubmit={({resetForm, valores}) => {
					console.log(valores)
					resetForm();
					console.log('Formulario enviado');
					// State 
					cambiarFormularioEnviado(true);
					setTimeout(() => cambiarFormularioEnviado(false), 5000);
				}} */
			>
				{( {values, handleSubmit, handleChange, handleBlur } ) => (
					<form className="formulario" onSubmit={handleSubmit}>
						<FormGroup row>
							<Label 
								for="input-nombre"
								sm={2}
							>
								Nombre
							</Label>
							<Col sm={10}>
								<Input 
									type="text"
									id="input-nombre"
									name="nombre"
									placeholder="with a placeholder"
									autoComplete="off"
									value={values.nombre }
									onChange={handleChange}
									onBlur={handleBlur}
									/>
								{/* <ErrorMessage name="nombre" component={() => (<div className="error">{errors.nombre}</div>)} /> */}
							</Col>
						</FormGroup>
						<FormGroup row>
							<Label 
									for="input-telefono"
									sm={2}
									>
									Telefono
								</Label>
								<Col sm={10}>
									<Input 
										type="text"
										id="input-telefono"
										name="telefono"
										placeholder="with a placeholder"
										autoComplete="off"
										value={values.telefono}
										onBlur={handleBlur}
										onChange={handleChange}
									/>
									{/* <ErrorMessage name="telefono" component={() => (<div className="error">{errors.telefono}</div>)} /> */}
								</Col>
						</FormGroup>
						<button type="submit" >enviar</button>
						{/* <Button 
							type="submit"
							color="primary"
							outline
							>
							Registrar
          	</Button> */}
						<Button color="secondary" onClick={toggle}>
							Cancelar
						</Button>
						{formularioEnviado && <p className="exito">Formulario enviado con exito!</p>}
					</form>
				)}
			</Formik>
        </ModalBody>
        <ModalFooter>
						
        </ModalFooter>
      </Modal>
    </>
  )
}

export default ModalAdd