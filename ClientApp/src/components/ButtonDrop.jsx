import React from 'react'
import { ButtonGroup, UncontrolledDropdown, DropdownToggle, DropdownMenu, DropdownItem } from 'reactstrap'

function ButtonDrop({children}) {
  return (
    <>
      {/* <ButtonGroup>
        <UncontrolledDropdown>
        <DropdownToggle caret
          color="primary"
          outline
        >
            {children}
            Imprimir Reporte
        </DropdownToggle>
        <DropdownMenu>
            <DropdownItem header>
            Opciones de impresión
            <DropdownItem divider />
            </DropdownItem>
            <DropdownItem>
            Reporte mensual
            </DropdownItem>
            <DropdownItem divider />
            <DropdownItem>
            Reporte trimestral
            </DropdownItem>
            <DropdownItem divider />
            <DropdownItem>
            Reporte Anual
            </DropdownItem>
        </DropdownMenu>
        </UncontrolledDropdown>
      </ButtonGroup> */}
    </>
  )
}

export default ButtonDrop