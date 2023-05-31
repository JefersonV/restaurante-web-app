import React, { useEffect, useState } from 'react'
import { useStore } from '../../providers/GlobalProvider'
import DatePicker from '../../components/DatePicker';
import Searchbar from '../../components/Searchbar';
// import Select from '../../components/Select';
// import Select from '../../components/report/SelecttReport';
// import Select from '../../components/report/SelectReportShopp';
// import Selectc from '../../components/report/SelecttReportC';
import SelectOPT from '../../components/report/SelectReportShopp';
import Tablep from '../../components/report/tshopping';
import TableData from '../../components/TableData';
// import Tablaprueba from '../components/tprueba';
import ButtonDrop from '../../components/ButtonDrop';
import Selectcompras from '../../components/report/SelecttReportC';
import { Col, Button, Label, Input, Table, Alert,  Spinner} from 'reactstrap'
import { Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
// import Tablep from '../../components/report/tprueba';
// import { Row, Col,  Button } from "reactstrap";
import { FcPrint } from 'react-icons/fc';
import ModalNewSale from '../../components/modales/ModalNewSale';



function ShoppDay (props)  {
  const URL = import.meta.env.VITE_BACKEND_URL;

    //   /* isOpen (globalstate) -> para que el contenido se ajuste según el ancho de la sidebar (navegación) */
  const isOpen = useStore((state) => state.sidebar)
  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Compras: Reporte Diario");
  }, []);

  const sele = [{value: '1',label: 'hoy',href: '/reports/pu'},{value: '2',label: 'semanal',href: '/reports/week',},
  {value: '3',label: 'rango',href: '/reports/rango',},{value: '4',label: 'Mensual',href: '/reports/month',
  }];

  const defaultDateString = () => {
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  const [selectedDate1, setSelectedDate1] = useState(defaultDateString());
  const [salesData, setSalesData] = useState([]);
  const [hasSales, setHasSales] = useState(null);
  const [loading, setLoading] = useState(false);
  const [loading1, setLoading1] = useState(false)

// const getDataday = async () => {

//   try {
//     const response = await fetch(`http://localhost:5188/api/ReportDay/day?fecha=${selectedDate}`);
//     const json = await response.json();

//     setDataApi(json);
//     // setIsLoading(false);

//     // if (json.length === 0) {
//     //   setModalOpen(true);
//     // }
//   } catch (error) {
//     console.error(error);
//   }
// }
// useEffect(() => {
//   getDataday()
// }, [])

// const toggleModal = () => {
//   setModalOpen(!modalOpen);
// };

// *****************************************fin MOSTRAR VENTAS DIARIOS TABLA */


//   *****************************************GENERAR PDF */

  const generarPdf = () => {
    setLoading1(true)
    const url = `${URL}/api/pdfCost/Costday?date=${selectedDate1}`;
    fetch(url, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/pdf',
      },
      responseType: 'blob',
    })
      .then((response) => response.blob())
      .then((blob) => {
        const url = window.URL.createObjectURL(blob);
        const newWindow = window.open(url, '_blank');
        if (!newWindow) {
          throw new Error('No se pudo abrir el PDF en una pestaña nueva.');
        }
        setLoading1(false)
      })
      .catch((error) => {
        console.error('Error:', error);
        setLoading1(false)
      });
  };
  // *****************************************FIN GENERAR PDF */


  
  useEffect(() => {
    checkSales(new Date(selectedDate1));
  }, []); // Ejecutar una vez al cargar la página
  
  const checkSales = (date) => {
    setLoading(true);
    if (date) {
      const formattedDate = date.toISOString().split('T')[0];
      
      // Realizar la solicitud a la API para obtener los datos de ventas en la fecha seleccionada
      fetch(`${URL}/api/ReportCost/day?fecha=${formattedDate}`,{
        headers: {
          'Authorization': `Bearer ${localStorage.token}`,
        }
      })
        .then((response) => response.json())
        .then((data) => {
          // Verificar la respuesta de la API y actualizar los datos de ventas y el estado hasSales en consecuencia
          setSalesData(data);
          setHasSales(data.length > 0);
          setLoading(false);
        })
        .catch((error) => {
          console.error('Error:', error);
          setLoading(false);
        });
    }
    // setLoading(false)
  };
  
  const handleDateChange1 = (event) => {
    const dateValue = event.target.value;
    const date = dateValue ? new Date(dateValue) : null;
    setSelectedDate1(dateValue);
    checkSales(date);
  };

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
  const results = !search ? salesData 
  // Si se ha ingresado información al input, que la compare a los criterios y los filtre
  : salesData.filter((item) =>
    item.proveedor.nombre.toLowerCase().includes(search.toLocaleLowerCase()) ||
    item.gastos[0].concepto.toLowerCase().includes(search.toLocaleLowerCase()) 
    // item.platillo.toLowerCase().includes(search.toLocaleLowerCase())
  )
// FIN BUSCAR




// const [selectedDate1, setSelectedDate1] = useState(null);
//   const [hasSales, setHasSales] = useState(false);

  

//   const checkSales = (date) => {
//     if (date) {
//       // Realizar la verificación de las ventas en la fecha seleccionada
//       // Puedes utilizar una función o hacer una solicitud a tu API aquí

//       // Ejemplo de verificación de ventas
//       if (date.toISOString().split('T')[0] === '2023-05-18') {
//         setHasSales(true);
//       } else {
//         setHasSales(false);
//       }
//     }
//   };

//   const handle = (event) => {
//     const dateValue = event.target.value;
//     const date = dateValue ? new Date(dateValue) : null;
//     setSelectedDate1(date);
//     checkSales(date);
//   };





  



  
 
  return (

  <div className={ isOpen ? "wrapper" : "side" }>
      <div className="container-fluid mt-4">
        <div className="row">
          {/* <div className="col">
            <DatePicker />
          </div> */}
          <div className="col">
            <Searchbar searcher={searcher}/>


          </div>
          <div className="col">
            <SelectOPT/>


          </div>
        </div>
        <div className="row ">
          <div className='col '>
            <Selectcompras />
            
            
          </div> 
          <div className='col '>
              <Button color="primary" disabled={loading1} onClick={generarPdf} >
                
                {loading1 ? (
                  <>
                    <Spinner size="sm" />
                    <span>Generando</span>
                  </>
                ) : (
                  <>
                    Imprimir
                    <FcPrint />
                  </>
                )}
              </Button>
            
          </div> 

          <div className="col ">
          <Input
            type="date"
            id="dateInput"
            value={selectedDate1}
            onChange={handleDateChange1}
          />

      </div>
    
        </div>
        
        <div className="row">
          <div className="col">
      {loading ? (
        <Spinner
        className="m-5"
        color="warning"
      >
        Loading...
      </Spinner>
      ):
      hasSales  ? (
        // <Alert color="success">Hay ventas en la fecha seleccionada.</Alert>
        <Tablep data={results} />
        // <Alert color="warning">No hay ventas en la fecha seleccionada.</Alert>
      ) : (
        <Alert color="danger">No hay compras en la fecha seleccionada.</Alert>
        // <Tablep data={results} />
      )}

          </div>
        </div>
      </div>
    </div>



    
  );


  
};

export default ShoppDay;



