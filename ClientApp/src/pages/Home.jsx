import React, { useEffect } from 'react'
import { useStore } from '../providers/GlobalProvider'
function Home (props) {
  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Dashboard");
  });
  /* isOpen (globalstate) -> para que el contenido se ajuste según el ancho de la sidebar (navegación) */
  const isOpen = useStore((state) => state.sidebar)
  
  return (
    <div className= { isOpen ? "wrapper" : "side" }>
      <h1>Dashboard</h1> 
    </div>
  )
}

export default Home