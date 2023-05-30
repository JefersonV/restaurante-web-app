import React, { useState, useEffect } from 'react'
import { useStore } from '../providers/GlobalProvider'
import DatePicker from '../components/DatePicker';
import Searchbar from '../components/Searchbar';
import SelectOption from '../components/SelectOption';
import TableInventario from '../components/TableInventario';

function Inventory(props) {
  /* isOpen (globalstate) -> para que el contenido se ajuste según el ancho de la sidebar (navegación) */
  const isOpen = useStore((state) => state.sidebar);
  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Inventario");
  }, []);

   /* ------ Fetch */
   const [dataApi, setDataApi] = useState([])
   const getData = async () => {
     try {
       const response = await fetch('http://localhost:5173/api/Inventario', {
        headers: {
          'Authorization': `Bearer ${localStorage.token}`
        }
       })
       const json = await response.json() 
       setDataApi(json)
       console.log(json)
     }
     catch(err) {
       console.error(err)
     }
   }
   useEffect(() => {
     getData()
   }, [])
  

   
  return (
    <div className={ isOpen ? "wrapper" : "side" }>
      <div className="container-fluid mt-4">
        <h1 style={{ textAlign: "center", color: "#8b1e3f" }}>Movimientos del mes</h1>
      <div className="row">
          <div className="col">
            {/* <TableData data={results} /> */}
            <TableInventario 
              data={dataApi} />
          </div>
        </div>
        </div>
    </div>
  );
}

export default Inventory;
