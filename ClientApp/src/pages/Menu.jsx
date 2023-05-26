import React, { useState ,useEffect } from 'react'
import { useStore } from '../providers/GlobalProvider'
import Searchbar from '../components/Searchbar'
import { FcPlus, FcPrint } from 'react-icons/fc'
import { Button } from 'reactstrap'
import TableMenu from '../components/menu/TableMenu'
import ModalAddMenu from '../components/menu/ModalAddMenu'

function Menu(props) {
  /* isOpen (globalstate) -> para que el contenido se ajuste según el ancho de la sidebar (navegación) */
  const isOpen = useStore((state) => state.sidebar);
  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Menú de platillos");
  }, []);

  /* ------ Fetch */
   const [dataApi, setDataApi] = useState([])
   const getData = async () => {
     try {
       const response = await fetch(
         `${import.meta.env.VITE_BACKEND_URL}/api/Menu`,
         {
           headers: {
             Authorization: `Bearer ${localStorage.token}`,
           },
         }
       );
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
 
  /* ----- Buscador */
  // state para buscador
  const [search, setSearch] = useState("");
  // buscador, captura de datos
  const searcher = (e) => {
    console.log(e.target.value);
    setSearch(e.target.value);
    console.log(e.target.value.length);
  };
  //metodo de filtrado del buscador
  // Si state search es null (no sea ha ingresado nada en el input) results = dataApi
  const results = !search
    ? dataApi
    : // Si se ha ingresado información al input, que la compare a los criterios y los filtre
      dataApi.filter((item) =>
        item.platillo.toLowerCase().includes(search.toLocaleLowerCase())
      );

  return (
    <div className={isOpen ? "wrapper" : "side"}>
      <div className="container-fluid mt-4">
        <div className="row">
          <div className="col">
            <Searchbar searcher={searcher} />
          </div>
        </div>
        <div className="row d-flex justify-content-center align-items-center">
          <div className="col">
          <ModalAddMenu
            actualizarListaMenu={getData}
          />
          <Button 
            color="primary"
            outline
            >
            Imprimir lista 
            <FcPrint />
          </Button>
          </div>
        </div>
        <div className="row">
          <div className="col">
            {/* <TableData data={results} /> */}
            <TableMenu 
              data={results}
              actualizarListaMenu={getData} />
          </div>
        </div>
      </div>
    </div>
  );
}

export default Menu;
