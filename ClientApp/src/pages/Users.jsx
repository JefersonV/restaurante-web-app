import React, { useState, useEffect } from 'react'
import { useStore } from '../providers/GlobalProvider'
import TableUsers from '../components/usuarios/TableUsers';
function Users(props) {
  /* isOpen (globalstate) -> para que el contenido se ajuste según el ancho de la sidebar (navegación) */
  const isOpen = useStore((state) => state.sidebar);
  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Control de Usuarios");
  }, []);
  
  const [dataApi, setDataApi] = useState([]);
  const getData = async () => {
    try {
      // https://jsonplaceholder.typicode.com/comments
      const response = await fetch("http://localhost:5173/api/Account", {
        headers: {
          Authorization: `Bearer ${localStorage.token}`,
        },
      });
      const json = await response.json();
      setDataApi(json);
      console.log(json);
    } catch (err) {
      console.error(err);
    }
  };
  useEffect(() => {
    getData();
  }, []);

  return (
    <div className={ isOpen ? "wrapper" : "side" }>
      <TableUsers 
        data={dataApi}
      />
    </div>
  );
}

export default Users;
