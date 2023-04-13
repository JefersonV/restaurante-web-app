import React from 'react'
import { Label } from 'reactstrap'
import { FormGroup } from 'reactstrap'
import { Input } from 'reactstrap'
import '../styles/Select.scss'

export default function Select() {
  return (
    <FormGroup>
      <Input
        id="exampleSelect"
        name="select"
        type="select"
      >
      <option value="1">Resumen de ventas de hoy</option>
      <option value="2">Resumen de ventas semanal</option>
      <option value="3">Resumen de ventas mensual</option>
      <option value="4">Resumen de ventas trimestral</option>
      <option value="5">Resumen de todas las ventas</option>
      </Input>
   </FormGroup>

  )
}
