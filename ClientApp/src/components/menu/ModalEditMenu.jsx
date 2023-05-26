import React, { useState, useEffect } from 'react'
import { FormGroup, Label, Col, Input } from 'reactstrap'
import { Formik, Form, Field, ErrorMessage } from 'formik';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap'
import Swal from 'sweetalert2';
import { BiEditAlt, BiFoodMenu } from 'react-icons/bi'
import { TbTruck } from 'react-icons/tb';

function ModalEditMenu(props) {
  const [modal, setModal] = useState(false);
  const toggle = () => setModal(!modal)
  const [formularioEnviado, cambiarFormularioEnviado] = useState(false);
  /* Identifica a cuál de los items de la tabla se le hizo click */
  const [itemId, setItemId] = useState("")
	//console.log('elemento', itemId  )

  const [data, setData] = useState({
		platillo: "",
		precio: ""
	})
	
	/* Objeto que se enviará en la solicitud PUT */
	let bodyTest = {
		idPlatillo: "",
		platillo: "",
		precio: ""
	}

/* Traer la data del item seleccionado */
  const getDataId = async (id) => {
		try {
			const response = await fetch(
        `${import.meta.env.VITE_BACKEND_URL}/api/Menu/${id}`,
        {
          headers: {
            Authorization: `Bearer ${localStorage.token}`,
            "Content-Type": "application/json",
          },
        }
      );
			const dataFetch = await response.json()
			console.warn(dataFetch)
			setData({
				...data,
				platillo: dataFetch.platillo,
				precio: dataFetch.precio
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
          setItemId(props.idPlatillo)
        }} />
      <Modal isOpen={modal} fade={false} toggle={toggle} centered={true}>
        <ModalHeader toggle={toggle}><BiFoodMenu size={30} /> Editar Platillo</ModalHeader>
        <ModalBody>
        <Formik
				initialValues={{
					platillo: data.platillo,
					precio: data.precio
					// data
				}}
				enableReinitialize={true}
				validate={(valores) => {
					let errores = {};
					console.log(valores)
					// Validacion platillo
					if(!valores.platillo){
						errores.platillo = 'Por favor ingresa un platillo'
					} else if(!/^[a-zA-ZÀ-ÿ\s.]{1,35}$/.test(valores.platillo)){
						errores.platillo = 'El platillo debe tener un máximo de 35 caracteres, solo puede contener letras y espacios'
					}
					// Validacion precio
					if(!valores.precio){
						errores.precio = 'Por favor ingresa un número telefónico'
						/* !/^[0-9]{9}$/.test(valores.precio) */ 
					} else if(!/^\d+(\.\d{1,2})?$/.test(valores.precio)){
						errores.precio = 'El precio debe contener una cantidad y opcional dos decimales separados por .'
					}
					
					return errores;
				}}
				onSubmit={async  (valores, {resetForm}) => {
					console.warn(' dd')
					console.log(valores)

					bodyTest.idPlatillo = itemId,
					bodyTest.platillo = valores.platillo,
					bodyTest.precio = valores.precio

          cambiarFormularioEnviado(true);
					setTimeout(() => cambiarFormularioEnviado(false), 5000);
					resetForm();
					console.log(bodyTest)

					try {
						const response = await fetch(
              `${import.meta.env.VITE_BACKEND_URL}/api/Menu/${itemId}`,
              {
                method: "PUT",
                body: JSON.stringify(bodyTest),
                headers: {
                  Authorization: `Bearer ${localStorage.token}`,
                  "Content-Type": "application/json",
                },
              }
            );
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
									placeholder="Nombre del Platillo"
									autoComplete="off"
									value={ values.platillo }
									onChange={handleChange}
									onBlur={handleBlur}
									/* Feedback para el usuario con el prop valid o invalid de reactstrap */
									valid={!errors.platillo && values.platillo.length > 0}
									invalid={!!errors.platillo}
									/>
								<ErrorMessage name="platillo" component={() => (<div className="error">{errors.platillo}</div>)} />
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
										placeholder="00.00"
										autoComplete="off"
										value={values.precio}
										onBlur={handleBlur}
										onChange={handleChange}
										/* Feedback para el usuario con el prop valid o invalid de reactstrap */
										valid={!errors.precio && values.precio.length > 0}
										invalid={!!errors.precio}
									/>
									<ErrorMessage name="precio" component={() => (<div className="error">{errors.precio}</div>)} />
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

export default ModalEditMenu
