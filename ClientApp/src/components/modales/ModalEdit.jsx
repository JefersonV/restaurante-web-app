import React, { useState, useEffect } from 'react'
import { FormGroup, Label, Col, Input } from 'reactstrap'
import { Formik, Form, Field, ErrorMessage } from 'formik';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap'
import Swal from 'sweetalert2';
import { BiEditAlt } from 'react-icons/bi'
import { TbTruck } from 'react-icons/tb';

function ModalEdit(props) {
  const [modal, setModal] = useState(false);
  const toggle = () => setModal(!modal)
  const [formularioEnviado, cambiarFormularioEnviado] = useState(false);
  /* Identifica a cuál de los items de la tabla se le hizo click */
  const [itemId, setItemId] = useState("")
	//console.log('elemento', itemId  )

  const [data, setData] = useState({
		nombre: "",
		telefono: ""
	})
	
	/* Objeto que se enviará en la solicitud PUT */
	let bodyTest = {
		idProveedor: "",
		nombre: "",
		telefono: ""
	}

/* Traer la data del item seleccionado */
  const getDataId = async (id) => {
		try {
			const response = await fetch(`http://localhost:5173/api/Proveedor/${id}`, {
				headers: {
					'Authorization': `Bearer ${localStorage.token}`,
					'Content-Type': 'application/json'
				}
			})
			const dataFetch = await response.json()
			console.warn(dataFetch)
			setData({
				...data,
				nombre: dataFetch.nombre,
				telefono: dataFetch.telefono
			})
		} catch(error) {
			console.log('Error Message: ' + error.ErrorMessage)
		}
  }

  useEffect(() => {
    if(itemId) {
      getDataId(itemId)
    }
  }, [itemId])

	/* Actualizar el registro */

  return (
    <>
      <BiEditAlt 
        size={22}
        onClick={() => {
					/* Abrir modal */
          toggle()
					/* Captura el item que fue seleccionado */
          setItemId(props.idProveedor)
        }} />
      <Modal isOpen={modal} fade={false} toggle={toggle} centered={true}>
        <ModalHeader toggle={toggle}><TbTruck size={30} /> Editar Proveedor</ModalHeader>
        <ModalBody>
        <Formik
				initialValues={{
					nombre: data.nombre,
					telefono: data.telefono
					// data
				}}
				enableReinitialize={true}
				validate={(valores) => {
					let errores = {};
					console.log(valores)
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
				onSubmit={async  (valores, {resetForm}) => {
					console.warn(' dd')
					console.log(valores)

					bodyTest.idProveedor = itemId,
					bodyTest.nombre = valores.nombre,
					bodyTest.telefono = valores.telefono

          cambiarFormularioEnviado(true);
					setTimeout(() => cambiarFormularioEnviado(false), 5000);
					resetForm();
					console.log(bodyTest)

					try {
						const response = await fetch(`http://localhost:5173/api/Proveedor/${itemId}`, 
						{
							method: "PUT",
							body: JSON.stringify(bodyTest),
							headers: {
								Authorization: `Bearer ${localStorage.token}`,
								"Content-Type": "application/json",
							}
						})
						console.log(response)
						console.log(response.ok)
						if(response.ok) {
							setModal(false)
							/* Alert */
							Swal.fire({
								position: 'center',
								icon: 'success',
								title: 'Registro actualizado Correctamente',
								showConfirmButton: false,
								timer: 1500
							})
							/* Prop para actualizar la data de la tabla */
							props.actualizarListaMenu()
						}
					} catch(error) {
						console.log(error)
					}
				}}

			>
				{( {values, handleSubmit, handleChange, handleBlur, errors, setValues } ) => (
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
									value={ values.nombre }
									onChange={handleChange}
									onBlur={handleBlur}
									/* Feedback para el usuario con el prop valid o invalid de reactstrap */
									valid={!errors.nombre && values.nombre.length > 0}
									invalid={!!errors.nombre}
									/>
								<ErrorMessage name="nombre" component={() => (<div className="error">{errors.nombre}</div>)} />
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
										valid={!errors.telefono && values.telefono.length > 0}
										invalid={!!errors.telefono}
									/>
									<ErrorMessage name="telefono" component={() => (<div className="error">{errors.telefono}</div>)} />
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
          {/* <Button color="primary" onClick={toggle}>
            Do Something
          </Button>{' '}
          <Button color="secondary" onClick={toggle}>
            Cancel
          </Button> */}
        </ModalFooter> 
      </Modal>
    </>
  )
}

export default ModalEdit