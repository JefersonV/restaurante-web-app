import React, { useState } from 'react'
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap'
import { FormGroup, Label, Col, Input } from 'reactstrap'
import { Formik, Form, Field, ErrorMessage } from 'formik';
import { BiEditAlt, BiFoodMenu } from 'react-icons/bi'
import { TbTruck } from 'react-icons/tb'
import Swal from 'sweetalert2'
import '../../styles/Formulario.scss'

function ModalAdd (props) {
  const [modal, setModal] = useState(false);
  const toggle = () => setModal(!modal);
  /* Formik */
  const [formularioEnviado, cambiarFormularioEnviado] = useState(false);

	const bodyProvider = {
		platillo: "",
		precio: ""	
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
        <ModalHeader toggle={toggle}><BiFoodMenu size={30}/> Ingreso de nuevo Platillo</ModalHeader>
        <ModalBody>
        <Formik
				initialValues={{
					platillo: '',
					precio: ''
				}}
				validate={(valores) => {
					let errores = {};
					// Validacion platillo
					if(!valores.platillo){
						errores.platillo = 'Por favor ingresa un platillo'
					} else if(!/^[a-zA-ZÀ-ÿ\s.]{1,25}$/.test(valores.platillo)){
						errores.platillo = 'El platillo debe tener un máximo de 25 caracteres, solo puede contener letras y espacios'
					}
					// Validacion precio
					if(!valores.precio){
						errores.precio = 'Por favor ingresa un número telefónico'
						/* !/^[0-9]{9}$/.test(valores.precio) */ 
					} else if(!/^\d{1,5}(\.\d{1,2})?$/.test(valores.precio)){
						errores.precio = 'El teléfono debe tener un máximo de 8 números, y debe escribirse sin espacios ni guiones'
					}
					return errores;
				}}
				onSubmit={async (valores, {resetForm}) => {
					console.log(valores)
					/* Captura de la data que se va a enviar con post */
					bodyProvider.platillo = valores.platillo
					bodyProvider.precio = valores.precio
					console.warn('json')
					console.log(bodyProvider)

					/* Método Post */
					try {
						const response = await fetch('http://localhost:5173/api/Menu', {
							method: 'POST',
							headers: {
								'Authorization': `Bearer ${localStorage.token}`,
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
							props.actualizarListaMenu()
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
								for="input-platillo"
								sm={2}
								
							>
								Platillo
							</Label>
							<Col sm={10}>
								<Input 
									type="text"
									id="input-platillo"
									name="platillo"
									placeholder="Típico"
									autoComplete="off"
									value={values.platillo }
									onChange={handleChange}
									onBlur={handleBlur}
									/* Feedback para el usuario con el prop valid o invalid de reactstrap */
									valid={touched.platillo && !errors.platillo && values.platillo.length > 0}
									invalid={touched.platillo && !!errors.platillo}
									/>
									{touched.platillo && errors.platillo && <div className="error">{errors.platillo}</div>}
								{/* <ErrorMessage name="platillo" component={() => (<div className="error">{errors.nombre}</div>)} /> */}
							</Col>
						</FormGroup>
						<FormGroup row>
							<Label 
									for="input-precio"
									sm={2}
									>
									Precio
								</Label>
								<Col sm={10}>
									<Input 
										type="text"
										id="input-precio"
										name="precio"
										placeholder="28"
										autoComplete="off"
										value={values.precio}
										onBlur={handleBlur}
										onChange={handleChange}
										/* Feedback para el usuario con el prop valid o invalid de reactstrap */
										valid={touched.precio && !errors.precio && values.precio.length > 0}
										invalid={touched.precio && !!errors.precio}

									/>
									
									{/* <ErrorMessage name="precio" component={() => (<div className="error">{errors.precio}</div>)} /> */}
									{touched.precio && errors.precio && <div className="error">{errors.precio}</div>}
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