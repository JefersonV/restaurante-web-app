import React, { useState ,useEffect } from 'react'
import { useStore } from '../providers/GlobalProvider'
import Searchbar from '../components/Searchbar'
import { FcPlus, FcPrint } from 'react-icons/fc'
import { Button } from 'reactstrap'
import TableData from '../components/TableData'
import ModalAdd from '../components/modales/ModalAdd'
import Skeleton, { SkeletonTheme } from 'react-loading-skeleton'
import CardSkeleton from '../components/skeletonUI/CardSkeleton'

function Providers(props) {
  /* isOpen (globalstate) -> para que el contenido se ajuste según el ancho de la sidebar (navegación) */
  const isOpen = useStore((state) => state.sidebar)
  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Proveedores");
  });
  /* Skeleton para la UI, mientras se carga la data de la API */
  const [isLoading, setIsLoading] = useState(true)
  /* ------ Fetch */
  const [dataApi, setDataApi] = useState([])
  const token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiamVmZmVyc29uIiwicm9sZSI6IkFkbWluaXN0cmFkb3IiLCJuYmYiOjE2ODMwMDA5NTksImV4cCI6MTY4MzAzNjk1OCwiaWF0IjoxNjgzMDAwOTU5LCJpc3MiOiJHb0VhdCIsImF1ZCI6IkxhQ2VudGVuYXJpYSJ9.Vww8GeQr1Dp8-LMMUncyvQQozJVxxHL4_pLXy74X0ro'
  const getData = async () => {
    try {
      // https://jsonplaceholder.typicode.com/comments
      const response = await fetch('http://localhost:5173/api/Proveedor', {
        headers: {
          'Authorization': `Bearer ${token}`
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
   
  /* ----- Buscador */
  // state para buscador
  const [search, setSearch] = useState("")
  // buscador, captura de datos
  const searcher = (e) => {
    console.log(e.target.value)
    setSearch(e.target.value)
    console.log(e.target.value.length)
  }
  //metodo de filtrado del buscador
  // Si state search es null (no sea ha ingresado nada en el input) results = dataApi
  const results = !search ? dataApi 
  // Si se ha ingresado información al input, que la compare a los criterios y los filtre
  : dataApi.filter((item) =>
    item.nombre.toLowerCase().includes(search.toLocaleLowerCase()) ||
    item.telefono.toLowerCase().includes(search.toLocaleLowerCase())
  )

  return (
    <div className={ isOpen ? "wrapper" : "side" }>
      <div className="container mt-4">
        <div className="row">
          <div className="col-6">
            <Searchbar searcher={searcher}/>
          </div>
          <div className="col-6">
            {/* Prop para actualizar la data después de confirmar el envío de post */}
            <ModalAdd actualizarListaProveedores={getData} />
      
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
            <TableData 
              data={results} 
              actualizarListaProveedores={getData} 
            />
          </div>
        </div>
      </div>
    </div>
  )
}

export default Providers