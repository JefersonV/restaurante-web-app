import React, { useState, useEffect } from 'react';
import { useStore } from '../providers/GlobalProvider';
import { FormGroup, Label, Col, Input } from 'reactstrap';
import TableSale from '../components/TableSale';
import '../styles/IndividualSale.scss';
import SearchBarDrop from '../components/menu-search-drop/SearchBarDrop';

function IndividualSale(props) {
  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Nueva Venta");
  }, []);

  /* isOpen (globalstate) -> para que el contenido se ajuste según el ancho de la sidebar (navegación) */
  const isOpen = useStore((state) => state.sidebar);
  // DataApi
  const [menu, setMenu] = useState([]);
  //
  const [noDataMenu, setNoDataMenu] = useState(false);
  const [numeroComanda, setNumeroComanda] = useState("");
  // Item seleccionado prop de <SearchBarDrop />
  const [itemSelected, setItemSelected] = useState(null);
  // Platillos seleccionados
  const [saleDetail, setSaleDetail] = useState([]);
  // Cantidades del input del componente <TableSale/>
  const [cantidades, setCantidades] = useState([]);

  // Identifica el item de <searchbardrop> seleccionado -> prop al componente nieto :v
  const setItemSelectedList = (id) => {
    // Sin repetidos: si el item ya existe en el array saleDetail, corta la secuencia de este bloque de código
    if (saleDetail.find((item) => item.idPlatillo === id)) return;
    // Busca los items que coincidan entre dataApi e ItemSelected
    const selectedItem = menu.find((item) => item.idPlatillo === id);
    console.log(selectedItem);

    setItemSelected(id);
    // Agregamos al array el nuevo item, y además no se repite ningún item
    setSaleDetail((prevSaleDetail) => [...prevSaleDetail, selectedItem]);
    // Inicializamos el valor en 1
    setCantidades((prevCantidades) => [...prevCantidades, 1]);
  };

  /* Eliminar un item de la venta */
  const deleteSaleDetail = (id) => {
    setSaleDetail((prevSaleDetail) => {
      // Nuevos elementos, elimina los items que no coincidan con el id propocionado como parámetro
      const nuevosElementos = prevSaleDetail.filter((item) => item.idPlatillo !== id);
      return nuevosElementos;
    });
  };

  /*  useEffect(() => {
    console.warn('Final -->')
    console.log(saleDetail)
  }, [setItemSelectedList]) */

  const getData = async () => {
    try {
      // if(!searchQuery || searchQuery.trim() === "") return
      // https://jsonplaceholder.typicode.com/comments
      // setLoading(true)
      setNoDataMenu(false);
      // if(response.data && response.data.length === 0) setNoDataMenu(true)
      const response = await fetch('http://localhost:5173/api/Menu', {
        headers: {
          'Authorization': `Bearer ${localStorage.token}`,
        },
      });
      const json = await response.json();
      setMenu(json);
      console.log(json);
    } catch (err) {
      console.error(err);
    }
  };

  useEffect(() => {
    getData();
  }, []);

  // console.log(menu)
  // Manejador del número de comanda
  const handleChange = (e) => {
    e.preventDefault();
    setNumeroComanda(e.target.value);
  };

  // Actualiza la cantidad asociada al item agregado a la comanda, si el item 
  const updateCantidades = (index, nuevaCantidad) => {
    setCantidades((prevCantidades) => {
      // Nuevas cantidades es una copia de prevCantidades [2,3,43 ... etc] cada número simboliza el value de cada input
      const nuevasCantidades = [...prevCantidades];
      // index representa la posición del elemento en el array de cantidades que queremos modificar.
      nuevasCantidades[index] = nuevaCantidad;
      // nuevasCantidades es el nuevo valor de la cantidad que se ha ingresado en el input
      return nuevasCantidades;
    });
  };

  const updateCantidadesDelete = (indexItem) => {
    setCantidades((prevCantidades) => {
      const cantidadesActualizadas = prevCantidades.filter((_, index) => index !== indexItem);
      return cantidadesActualizadas;
    });
  };

  return (
    <div className={isOpen ? "wrapper" : "side"}>
      <section className="comanda">
        <h2>Comanda digital</h2>
        <form className="comanda-form">
          <div className="container">
            <div className="row">
              <div className="col-3">
                <div className="form-floating mb-3">
                  <input
                    type="number"
                    className="form-control"
                    id="input-comanda"
                    placeholder="1332"
                    onChange={handleChange}
                  />
                  <label htmlFor="input-comanda">No. de Comanda</label>
                </div>
              </div>
              <div className="col-9">
                <SearchBarDrop
                  itemSelected={itemSelected}
                  setItemSelectedList={setItemSelectedList}
                  menuState={menu}
                  setNoDataMenu={setNoDataMenu}
                  noDataMenu={noDataMenu}
                />
              </div>
            </div>
          </div>
        </form>
      </section>
      <section className="comanda-table">
        <TableSale
          noComanda={numeroComanda}
          /* Nuevo array con los items seleccionados */
          cantidades={cantidades}
          saleDetail={saleDetail}
          deleteSaleDetail={deleteSaleDetail}
          updateCantidades={updateCantidades}
          updateCantidadesDelete={updateCantidadesDelete}
        />
      </section>
    </div>
  );
}

export default IndividualSale;
