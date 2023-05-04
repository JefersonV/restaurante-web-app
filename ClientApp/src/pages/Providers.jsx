import React, { useState ,useEffect } from 'react'
import { useStore } from '../providers/GlobalProvider'
import Searchbar from '../components/Searchbar'
import { FcPlus, FcPrint } from 'react-icons/fc'
import { Button } from 'reactstrap'
import TableData from '../components/TableData'
import ModalAdd from '../components/modales/ModalAdd'
import Skeleton, { SkeletonTheme } from 'react-loading-skeleton'
import CardSkeleton from '../components/skeletonUI/CardSkeleton'
import jsPDF from 'jspdf';  
import autoTable from 'jspdf-autotable'

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
  const getData = async () => {
    try {
      // https://jsonplaceholder.typicode.com/comments
      // const response = await fetch('http://localhost:5188/api/Proveedor')
      const response = await fetch('http://localhost:5188/api/Proveedor')
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


  // *********++++++++++++++++++++++++++++++++++++++++++++++++PRUEBA PARA IMPRIMIR

// // ********************************PRUEBA 4: FUNCIONAL
function generatePDF() {
  // crea un nuevo objeto `Date`
  var today = new Date();
 
  // obtener la fecha y la hora
  var now = today.toLocaleString();
  console.log(now);
  /*
      Resultado: 1/27/2020, 9:30:00 PM
  */
 // Obtiene los datos de la base de datos
 let data = dataApi;
 // Crea un nuevo objeto PDF
 const doc = new jsPDF();

 // Define las columnas de la tabla
 const columns = ["IDPROVEEDOR","NOMBRE", "TELEFONO"];
     

 // Define las filas de la tabla
 const rows = [];
     
 // Itera sobre los datos de la tabla y agrega cada fila a la matriz "rows"
 data.forEach((item) => {
   const rowData = [
     item.idProveedor,
     item.nombre,
     item.telefono
   ];
   rows.push(rowData);
 });
 
     autoTable(doc, {
     head: [columns],
     body: rows
     })
 
   // Guarda el archivo PDF
   // doc.save('data.pdf');
   doc.save(`Reporte_${now}.pdf`);  


};

// // ********************************PRUEBA 4: FIN FUNCIONAL

// +++++++++++++++++++++++++++++++FIN PRUEBA DE IMPRIMIR

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
              onClick = {generatePDF} 
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