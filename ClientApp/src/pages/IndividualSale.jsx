import React, { useState, useEffect, Children } from 'react'
import { useStore } from '../providers/GlobalProvider'
import { FormGroup, Label, Col, Input } from 'reactstrap'
import TableSale from '../components/TableSale'
import '../styles/IndividualSale.scss'
function IndividualSale(props) {
  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Nueva Venta");
  });
   /* isOpen (globalstate) -> para que el contenido se ajuste según el ancho de la sidebar (navegación) */
  const isOpen = useStore((state) => state.sidebar)

  return (
    <div className={ isOpen ? "wrapper" : "side" }>
      <section className="comanda">
        <h2>Comanda digital</h2>
        <form className="comanda-form">
          <FormGroup row>
            <Label htmlFor="input-comanda" sm={3}>No. Comanda</Label>
            <Col sm={6}>
              <Input type="number" id="input-comanda"/>
            </Col>
          </FormGroup>
          <FormGroup row>
            <Label htmlFor="input-platillo" sm={3}>Platillo</Label>
            <Col sm={6}>
              <Input type="search" id="input-platillo"/>
            </Col>
            <Col sm={2}>
              <span className="d-block">Precio</span>
              <span>Q.50.00</span>
            </Col>
          </FormGroup>
        </form>
      </section>
      <section className="comanda-table">
        <TableSale/>
      </section>
    </div>
  )
}

export default IndividualSale