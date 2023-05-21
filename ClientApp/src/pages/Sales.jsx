import React, { useState, useEffect } from "react";
import { useStore } from "../providers/GlobalProvider";
import DatePicker from "../components/DatePicker";
import Searchbar from "../components/Searchbar";
import Select from "../components/Select";
import TableData from "../components/TableData";
import ButtonDrop from "../components/ButtonDrop";
import { FcPrint } from "react-icons/fc";
import ModalNewSale from "../components/modales/ModalNewSale";

function Sales(props) {
  /* isOpen (globalstate) -> para que el contenido se ajuste según el ancho de la sidebar (navegación) */
  const isOpen = useStore((state) => state.sidebar);
  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Ventas");
  }, []);
  /* ------ Fetch */
  const [dataApi, setDataApi] = useState([])

  const getData = async () => {
    try {
      const response = await fetch('http://localhost:5173/api/Venta', {
        headers: {
          'Authorization': `Bearer ${localStorage.token}`
        }
      })
      const json = await response.json() 
      setDataApi(json)
    }
  };
  useEffect(() => {
    console.log(dataApi)
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
    : // Si se ha ingresado información al input, que la compare a los criterios
      dataApi.filter((item) =>
        item.email.toLowerCase().includes(search.toLocaleLowerCase())
      );
  return (
    <div className={isOpen ? "wrapper" : "side"}>
      <div className="container-fluid mt-4">
        <div className="row">
          <div className="col">
            <DatePicker />
          </div>
          <div className="col">
            <Searchbar searcher={searcher} />
          </div>
        </div>
        <div className="row d-flex justify-content-center align-items-center">
          <div className="col">
            <Select />
          </div>
          <div className="col">
            <ModalNewSale />
          </div>
          <div className="col">
            <ButtonDrop>
              <FcPrint />
            </ButtonDrop>
          </div>
        </div>
        <div className="row">
          <div className="col">
            <TableData data={results} />
          </div>
        </div>
      </div>
    </div>
  );
}

export default Sales;
