import React, { useState, useEffect } from "react";
import {
  Button,
  Modal,
  ModalHeader,
  ModalBody,
  ModalFooter,
  Table,
} from "reactstrap";
import { Formik, FieldArray } from "formik";
import { AiOutlineEdit, AiOutlineMinus, AiOutlinePlus } from "react-icons/ai";
import Swal from "sweetalert2";

function ModalEditSale(props) {
  const { idVenta, actualizarListaVentas } = props;
  const [modal, setModal] = useState(false);
  const [itemId, setItemId] = useState("");
  const [sale, setSale] = useState([]);
  const [formularioEnviado, setFormularioEnviado] = useState(false);
  const toggle = () => setModal(!modal);

  const getDataSale = async (id) => {
    try {
      const response = await fetch(
        `${import.meta.env.VITE_BACKEND_URL}/api/Venta/${id}`,
        {
          headers: {
            Authorization: `Bearer ${localStorage.token}`,
            "Content-Type": "application/json",
          },
        }
      );
      const json = await response.json();
      console.log(json);
      setSale(json);
    } catch (error) {
      console.log(error);
    }
  };

  const [dataPostSale, setDataPostSale] = useState([
    {
      idDetalleVenta: "",
      cantidad: "",
      // idPlatillo: ""
    }
  ]);

  useEffect(() => {
    if (itemId) {
      getDataSale(itemId); 
      console.log(itemId);
    }
  }, [itemId]);

  return (
    <div>
      <AiOutlineEdit
        onClick={() => {
          /* AbrirModal */
          toggle();
          /* item clickado de la tabla */
          setItemId(idVenta);
        }}
        size="20"
      />
      <Modal isOpen={modal} toggle={toggle} centered={true}>
        <ModalHeader toggle={toggle}>
          <AiOutlineEdit size={28} /> Editar venta
        </ModalHeader>
        <ModalBody>
          <Formik
            initialValues={{
              noComanda: sale.numeroComanda,
              total: sale.total,
              detalleVenta: sale.detalleVenta || [],
            }}
            enableReinitialize={true}
            // validate={(valores) => {
            //   const errores = {};
            //   if (!valores.cantidad) {
            //     errores.cantidad = "Por favor ingresa un cantidad";
            //   } else if (!/^[a-zA-ZÀ-ÿ\s.]{1,25}$/.test(valores.cantidad)) {
            //     errores.cantidad =
            //       "El cantidad debe tener un máximo de 25 caracteres, solo puede contener letras y espacios";
            //   }
            //   return errores;
            // }}
            onSubmit={async (valores, { resetForm }) => {
              setFormularioEnviado(true);
              setTimeout(() => setFormularioEnviado(false), 5000);
              resetForm();

              try {
                const response = await fetch(
                  `${import.meta.env.VITE_BACKEND_URL}/api/Venta/${itemId}`,
                  {
                    method: "PUT",
                    body: JSON.stringify(dataPostSale),
                    headers: {
                      Authorization: `Bearer ${localStorage.token}`,
                      "Content-Type": "application/json",
                    },
                  }
                );
                if(response.ok) {
                  setModal(false)

                  Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: 'Registro actualizado Correctamente',
                    showConfirmButton: false,
                    timer: 1500
                  })
                  /* Prop para actualizar la data de la tabla */
                  actualizarListaVentas() 
                }  
              } catch (error) {
                console.log(error);
              }
            }}
          >
            {({ values, handleSubmit, handleChange, handleBlur, errors, setFieldValue }) => (
              <form className="formulario" onSubmit={handleSubmit}>
                <h5>No. de Comanda: {values.noComanda}</h5>
                
                <FieldArray name="detalleVenta">
                  {({ insert, remove, push }) => (
                    <>
                      <Table>
                        <thead>
                          <tr>
                            <th>Platillo</th>
                            <th>Cantidad</th>
                            <th>Precio Unitario</th>
                            <th>Sub total</th>
                          </tr>
                        </thead>
                        <tbody>
                          {values.detalleVenta.map((detalle, index) => {
                            let newDetalleVenta
                            const handleCantidadChange = (e) => {
                              const newCantidad = parseInt(e.target.value, 10);
                              newDetalleVenta = [...values.detalleVenta];
                              newDetalleVenta[index].cantidad = newCantidad;
                              newDetalleVenta[index].subtotal = newCantidad * detalle.precio;

                              let newTotal = 0;
                              newDetalleVenta.forEach((item) => {
                                newTotal += item.subtotal;
                              });
     
                              /* 
                              noComanda: sale.numeroComanda,
                              total: sale.total,
                              detalleVenta: sale.detalleVenta || [],
                              */
                              const newDataPostSale = [...dataPostSale];
                              newDataPostSale[index] = {
                                idDetalleVenta: newDetalleVenta[index].id,
                                cantidad: newDetalleVenta[index].cantidad,
                                // idPlatillo: detalle.idPlatillo
                                // idPlatillo: 4
                              };
                              setDataPostSale(newDataPostSale);
                              setFieldValue(`detalleVenta.${index}.cantidad`, newCantidad);
                              setFieldValue(`detalleVenta.${index}.subtotal`, newCantidad * detalle.precio);
                              setFieldValue(`total`, newTotal);
                            };

                            return (
                              <tr key={index}>
                                <td>{detalle.platillo}</td>
                                <td>
                                  <div className="table-buttons">
                                    <button
                                      type="button"
                                      className="btn btn-plus"
                                      onClick={() => {
                                        handleCantidadChange({
                                          target: {
                                            value: Math.max(detalle.cantidad - 1, 0),
                                          },
                                        });
                                      }}
                                    >
                                      <AiOutlineMinus />
                                    </button>
                                    <input
                                      type="text"
                                      className="form-control input-cantidad-edit"
                                      name={`detalleVenta.${index}.cantidad`}
                                      value={detalle.cantidad}
                                      onChange={handleCantidadChange}
                                    />
                                    <button
                                      type="button"
                                      className="btn btn-minus"
                                      onClick={() => {
                                        handleCantidadChange({
                                          target: {
                                            value: detalle.cantidad + 1,
                                          },
                                        });
                                      }}
                                    >
                                      <AiOutlinePlus />
                                    </button>
                                  </div>
                                </td>
                                <td>{detalle.precio}</td>
                                <td>{detalle.subtotal}</td>
                              </tr>
                            );
                          })}
                        </tbody>
                        <tr>
                          <td colSpan={4}>{values.total}</td>
                        </tr>
                      </Table>
                    </>
                  )}
                </FieldArray>
                <Button 
                  type="submit"
                  color="primary"
                  outline
                >
                  Registrar
                </Button>
                <Button color="secondary" onClick={toggle}>
                  Cancel
                </Button>
            </form>
            )}
          </Formik>
        </ModalBody>
        <ModalFooter>
          
        </ModalFooter>
      </Modal>
    </div>
  );
}

export default ModalEditSale;
