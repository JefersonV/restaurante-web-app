import React, { useState, useEffect } from 'react'
import { useStore } from '../providers/GlobalProvider'
import TableWaiters from '../components/meseros/TableWaiters'

function Waiters(props) {
  /* isOpen (globalstate) -> para que el contenido se ajuste según el ancho de la sidebar (navegación) */
  const isOpen = useStore((state) => state.sidebar);
  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Meseros");
  }, []);
  
  const [dataApi, setDataApi] = useState([]);
  const getData = async () => {
    try {
      const response = await fetch("http://localhost:5173/api/Mesero", {
        headers: {
          Authorization: `Bearer ${localStorage.token}`,
        },
      });
      const json = await response.json();
      setDataApi(json);
    } catch (err) {
      console.error(err);
    }
  };
  useEffect(() => {
    getData();
  }, []);

  return (
    <div className={ isOpen ? "wrapper" : "side" }>
      <TableWaiters 
        data={dataApi}
      />
    </div>
  );
}

export default Waiters;
