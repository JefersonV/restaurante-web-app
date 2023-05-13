import React, { useState, useEffect } from 'react'
import { useStore } from '../providers/GlobalProvider'
import { FormGroup, Label, Col, Input } from 'reactstrap'
import TableSale from '../components/TableSale'
import '../styles/IndividualSale.scss'
import SearchBarDrop from '../components/menu-search-drop/SearchBarDrop'
function IndividualSale(props) {
  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Nueva Venta");
  }, []);
   /* isOpen (globalstate) -> para que el contenido se ajuste según el ancho de la sidebar (navegación) */
  const isOpen = useStore((state) => state.sidebar)
  // DataApi
  const [menu, setMenu] = useState([]);
  // 
  const [noDataMenu, setNoDataMenu] = useState(false);
  const [numeroComanda, setNumeroComanda] = useState("")
  // Item seleccionado de <SearchBarDrop />
  const [itemSelected, setItemSelected] = useState(null)

  // Función que se manda como prop al componente nieto :v
  const setItemSelectedList = (id) => {
    // Si el item ya existe en el array saleDetail, corta la secuencia de este bloque de código
    if(saleDetail.find((item) => item.id === id)) return
    // Busca los items que coincidan entre dataApi e ItemSelected
    const selectedItem = menu.find((item) => item.id === id)
    console.log(selectedItem)

    setItemSelected(id)
    // Agregamos al array el nuevo item, y además no se repite ningún item
    setSaleDetail((prevSaleDetail) => [...prevSaleDetail, selectedItem])  
  }
  
  // Platillos seleccionados
  const [saleDetail, setSaleDetail] = useState([])
  /*
  {
    idPlatillo: '',
    nombre: '',
    precio: '',
  }
  */
  useEffect(() => {
    console.warn('Final -->')
    console.log(saleDetail)
  }, [setItemSelectedList])
  
  const getData = async () => {
    try {
      // if(!searchQuery || searchQuery.trim() === "") return
      // https://jsonplaceholder.typicode.com/comments
      // setLoading(true)
      setNoDataMenu(false)
      // if(response.data && response.data.length === 0) setNoDataMenu(true)
      const response = await fetch('https://jsonplaceholder.typicode.com/users')
      const json = await response.json() 
      setMenu(json)
      console.log(json)
    }
    catch(err) {
      console.error(err)
    }
  }

  useEffect(() => {
    getData()
  }, [])

  // console.log(menu)
  
  const handleChange = (e) => {
    e.preventDefault()
    setNumeroComanda(e.target.value)
  }
  return (
    <div className={ isOpen ? "wrapper" : "side" }>
      <section className="comanda">
        <h2>Comanda digital</h2>
        <form className="comanda-form">
        <div className="container">
          <div className="row">
            <div className="col-3">
              <div className="form-floating mb-3">
                <input type="number" className="form-control" id="input-comanda" placeholder="1332" onChange={handleChange}/>
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
          saleDetail={saleDetail}
        />
      </section>
    </div>
  )
}

export default IndividualSale