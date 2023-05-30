import React, { useState, useEffect } from "react";
import { useStore } from "../providers/GlobalProvider";
import Searchbar from "../components/Searchbar";
import { FcPlus, FcPrint } from "react-icons/fc";
import { Button } from "reactstrap";
import TableCliente from "../components/clientes/TableCliente"
import ModalAddCliente from "../components/clientes/ModalAddCliente";

function Customers(props) {
  /* isOpen (globalstate) -> para que el contenido se ajuste según el ancho de la sidebar (navegación) */
  const isOpen = useStore((state) => state.sidebar);
  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Clientes");
  }, []);

  /* ------ Fetch */
  const [dataApi, setDataApi] = useState([]);
  const getData = async () => {
    try {
      const response = await fetch(
        `${import.meta.env.VITE_BACKEND_URL}/api/Cliente`,
        {
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${localStorage.token}`,
          },
        }
      );
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
        item.nombreApellido.toLowerCase().includes(search.toLocaleLowerCase()) /* ||
        item.institucion.toLowerCase().includes(search.toLocaleLowerCase()) ||
        item.puesto.toLowerCase().includes(search.toLocaleLowerCase()) */
      );

  return (
    <div className={isOpen ? "wrapper" : "side"}>
      <div className="container-fluid mt-4">
        <div className="row">
          <div className="col">
            <Searchbar searcher={searcher} />
          </div>
        <div className="col-6">
          
          <ModalAddCliente
            actualizarListaClientes={getData}
          />
          {/* <Button 
            color="primary"
            outline
            >
            Imprimir lista 
            <FcPrint />
          </Button> */}
        </div>
        </div>
        <div className="row">
          <div className="col">
            <TableCliente 
              data={results}
              actualizarListaClientes={getData}
            />
          </div>
        </div>
      </div>
    </div>
  );
}

export default Customers;
