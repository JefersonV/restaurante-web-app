import React, { useState, useEffect } from 'react'
import '../..//styles/MenuDrop.scss'

function DataMenuDrop(props) {
  const { menuName, index, id, itemSelected, setItemSelectedList, price } = props
  // const [itemSelected, setItemSelected] = useState(null)
  useEffect(() => {
    // id del item seleccionado
    console.log(itemSelected)
  }, [itemSelected])

  const handleClick = (e) => {
    e.preventDefault()
    // Captura el item seleccionado, además ahora está disponible en el componente padre (IndividualSale)
    setItemSelectedList(id) // id -> .map () item.id
  }
  return (
    <>
      <button className="DataContainer" onClick={handleClick}>
        {index + 1}  {menuName} <span className="precio"> Q.{price.toFixed(2)}</span> 
      </button>
    </>
  )
}

export default DataMenuDrop