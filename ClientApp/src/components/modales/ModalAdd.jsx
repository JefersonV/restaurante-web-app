import React, { useState } from 'react'
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap'
import { FormGroup, Label, Col, Input } from 'reactstrap'
import { Formik, Form, Field, ErrorMessage } from 'formik';
import { BiEditAlt } from 'react-icons/bi'
import { TbTruck } from 'react-icons/tb'
import Swal from 'sweetalert2'
import '../../styles/Formulario.scss'

function ModalAdd (props) {
  const [modal, setModal] = useState(false);
  const toggle = () => setModal(!modal);
  /* Formik */
  const [formularioEnviado, cambiarFormularioEnviado] = useState(false);

	const bodyProvider = {
		nombre: "",
    	telefono: ""
	}

  return (
		<>
		<Button 
			color="danger"
			outline
			onClick={toggle}
		> 
			Registra Nuevo
			<BiEditAlt  />
		</Button>
      <Modal isOpen={modal} fade={false} toggle={toggle} centered={true}>
        <ModalHeader toggle={toggle}><TbTruck size={30}/> Ingreso de nuevo Proveedor</ModalHeader>
        <ModalBody>
        <Formik
				initialValues={{
					nombre: '',
					telefono: ''
				}}
				validate={(valores) => {
					let errores = {};
					// Validacion nombre
					if(!valores.nombre){
						errores.nombre = 'Por favor ingresa un nombre'
					} else if(!/^[a-zA-ZÀ-ÿ\s.]{1,25}$/.test(valores.nombre)){
						errores.nombre = 'El nombre debe tener un máximo de 25 caracteres, solo puede contener letras y espacios'
					}
					// Validacion telefono
					if(!valores.telefono){
						errores.telefono = 'Por favor ingresa un número telefónico'
						/* !/^[0-9]{9}$/.test(valores.telefono) */ 
					} else if(!/^[0-9]{8}$/.test(valores.telefono)){
						errores.telefono = 'El teléfono debe tener un máximo de 8 números, y debe escribirse sin espacios ni guiones'
					}
					return errores;
				}}
				onSubmit={async (valores, {resetForm}) => {
					console.log(valores)
					/* Captura de la data que se va a enviar con post */
					bodyProvider.nombre = valores.nombre
					bodyProvider.telefono = valores.telefono
					console.warn('json')
					console.log(bodyProvider)

					/* Método Post */
					try {
						const response = await fetch('http://localhost:5173/api/Proveedor', {
							method: 'POST',
							headers: {
								'Content-Type': 'application/json'
							},
							body: JSON.stringify(bodyProvider)
						});
						if (response.ok) {
							Swal.fire({
								position: 'center',
								icon: 'success',
								title: 'Registro agregado correctamente',
								showConfirmButton: false,
								timer: 1500
							})
							/* Si post fue exitoso se actualiza la data de la tabla */
							props.actualizarListaProveedores()
							console.log('Formulario enviado con éxito')
							// State 
							cambiarFormularioEnviado(true);
							setTimeout(() => cambiarFormularioEnviado(false), 5000);
							resetForm();
						} else {
							console.log('Ha ocurrido un error al enviar el formulario')
						}
					} catch (error) {
						console.log('Ha ocurrido un error al enviar el formulario')
						console.log(error)
					}
				}}
			>
				{( {values, handleSubmit, handleChange, handleBlur, errors, touched, setTouched } ) => (
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
									placeholder="Magnus S.A."
									autoComplete="off"
									value={values.nombre }
									onChange={handleChange}
									onBlur={handleBlur}
									/* Feedback para el usuario con el prop valid o invalid de reactstrap */
									valid={touched.nombre && !errors.nombre && values.nombre.length > 0}
									invalid={touched.nombre && !!errors.nombre}
									/>
									{touched.nombre && errors.nombre && <div className="error">{errors.nombre}</div>}
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
										placeholder="77623030"
										autoComplete="off"
										value={values.telefono}
										onBlur={handleBlur}
										onChange={handleChange}
										/* Feedback para el usuario con el prop valid o invalid de reactstrap */
										valid={touched.telefono && !errors.telefono && values.telefono.length > 0}
										invalid={touched.telefono && !!errors.telefono}

									/>
									
									{/* <ErrorMessage name="telefono" component={() => (<div className="error">{errors.telefono}</div>)} /> */}
									{touched.telefono && errors.telefono && <div className="error">{errors.telefono}</div>}
								</Col>
						</FormGroup>
						{/* <button type="submit" >enviar</button> */}
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