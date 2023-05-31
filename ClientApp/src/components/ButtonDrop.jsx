import React from 'react'
import { ButtonGroup, UncontrolledDropdown, DropdownToggle, DropdownMenu, DropdownItem } from 'reactstrap'
import { NavLink } from 'react-router-dom'

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

            <NavLink to="/reports/week"
             style={{ textDecoration: 'none' }}
           >
            <DropdownItem>
            Reporte Semanal
            </DropdownItem>
            </NavLink>
            

            <DropdownItem divider />
            
            <NavLink to="/reports/month"
             style={{ textDecoration: 'none' }}>
            <DropdownItem>
            Reporte Mensual
            </DropdownItem>
            </NavLink>

            <DropdownItem divider />
            <NavLink to="/reports/all"
             style={{ textDecoration: 'none' }}>
            <DropdownItem>
            Reporte Anual
            </DropdownItem>
            </NavLink>

        </DropdownMenu>
        </UncontrolledDropdown>
      </ButtonGroup> */}
    </>
  )
}

export default ButtonDrop