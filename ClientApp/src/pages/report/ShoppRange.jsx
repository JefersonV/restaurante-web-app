import React, { useEffect, useState } from 'react';
import { Table, Form, Spinner, Label, Input, Button } from 'reactstrap';
import { useStore } from '../../providers/GlobalProvider'
import DatePicker from '../../components/DatePicker';
import Searchbar from '../../components/Searchbar';
// import Select from '../../components/Select';
import Selectcompras from '../../components/report/SelecttReportC';
import SelectOPT from '../../components/report/SelectReportShopp';
import Tablep from '../../components/report/tshopping';
import TableData from '../../components/TableData';
// import Tablaprueba from '../components/tprueba';
import ButtonDrop from '../../components/ButtonDrop';
// import { Col, Button, Label, Input, Table, Alert,  Spinner} from 'reactstrap'
import { Modal, ModalHeader, ModalBody, ModalFooter, Alert, Col } from 'reactstrap';
// import Tablep from '../../components/report/tprueba';
import { FcPrint } from 'react-icons/fc';
import Selectc from '../../components/report/SelecttReportC';

function ShoppRange(props)  {

  const URL = import.meta.env.VITE_BACKEND_URL;

  const isOpen = useStore((state) => state.sidebar)
  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Compras: Reporte Rangos");
  }, []);



  
  
  const [data, setData] = useState(null);
  // const [fechaDesde, setFechaDesde] = useState(new Date().toISOString().split('T')[0]);
  // const [fechaHasta, setFechaHasta] = useState(new Date().toISOString().split('T')[0]);
    const [fechaDesde, setFechaDesde] = useState("");
  const [fechaHasta, setFechaHasta] = useState("");
  const [hasSales, setHasSales] = useState(null);
  const [loading, setLoading] = useState(false);
  const [loading1, setLoading1] = useState(false);


  const generarPdf= () => {
    setLoading1(true)
    const url = `${URL}/api/pdfCost/Costrange?fechaDesde=${fechaDesde}&fechaHasta=${fechaHasta}`;
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

  const fetchData = () => {
    // e.preventDefault();
    setLoading(true);
    // Realizar la solicitud a la API para obtener los datos del rango de fechas seleccionado
    fetch(`${URL}/api/ReportCost/rango?fechaDesde=${fechaDesde}&fechaHasta=${fechaHasta}`, {
      headers: {
        'Authorization': `Bearer ${localStorage.token}`,
      }
    })
    
      .then((response) => response.json())
      .then((data) => {
        setData(data);
        setHasSales(data.length > 0);
        setLoading(false);
        // console.log("casa",data)
      })
      .catch((error) => {
        console.error('Error:', error);
        setLoading(false);
      });
      // setLoading(false);

  };


  useEffect(() => {
    if (fechaDesde && fechaHasta) {
      fetchData();
    }
  }, [fechaDesde, fechaHasta]);

  const handleFechaDesdeChange = (event) => {
    setFechaDesde(event.target.value);
  };

  const handleFechaHastaChange = (event) => {
    setFechaHasta(event.target.value);
  }



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
  const results = !search ? data 
  // Si se ha ingresado información al input, que la compare a los criterios y los filtre
  : data.filter((item) =>
    item.proveedor.nombre.toLowerCase().includes(search.toLocaleLowerCase()) ||
    item.gastos[0].concepto.toLowerCase().includes(search.toLocaleLowerCase())
    // item.platillo.toLowerCase().includes(search.toLocaleLowerCase())
  )
// FIN BUSCAR


    return (
      <div className={ isOpen ? "wrapper" : "side" }>
        <div className="container mt-4">
        <div className="row">
      
          <div className="col">
            <Searchbar searcher={searcher}/>
          </div>
          <div className="col">
            <SelectOPT />


          </div>
        </div>
      <div className="row ">
        <div className="col">
          <Selectcompras/>
        </div>
      
      <div className="col" >
          {/* <ButtonDrop>
            <FcPrint />
          </ButtonDrop> */}
          
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
      
        
      </div>
          
          <div className="row">
            <div className="col">

            <Form onSubmit={fetchData}>
              <div className='row'>

              <div className='col-6'>
                <Label for="fechaDesde">Fecha Desde:</Label>
                <Input type="date" id="fechaDesde" value={fechaDesde} onChange={handleFechaDesdeChange} />
              </div>
              <div className='col-6'>
                <Label for="fechaHasta">Fecha Hasta:</Label>
                <Input type="date" id="fechaHasta" value={fechaHasta} onChange={handleFechaHastaChange} />
              </div>
              
              </div>
            </Form>

      
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
    )
};

export default ShoppRange;
