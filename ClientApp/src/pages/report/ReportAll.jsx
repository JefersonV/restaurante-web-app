import React, { useEffect, useState, useRef } from 'react'
import { useStore } from '../../providers/GlobalProvider'
import DatePicker from '../../components/DatePicker';
import Searchbar from '../../components/Searchbar';
// import Select from '../../components/Select';
import Select from '../../components/report/SelecttReport';
import SelectOPT from '../../components/report/SelectReportShopp';
import Selectmonth from '../../components/report/Selectmonth copy';
import TableData from '../../components/TableData';
// import Tablaprueba from '../components/tprueba';
import ButtonDrop from '../../components/ButtonDrop';
import { Col, Button, Label, Input, Table, Alert, Spinner} from 'reactstrap'
import Tablep from '../../components/report/tprueba';
// import { Row, Col,  Button } from "reactstrap";
import { FcPrint } from 'react-icons/fc';
import ModalNewSale from '../../components/modales/ModalNewSale';
// import Selectc from '../../components/report/SelecttReportC';

function ReportAll (props)  {
  
  const URL = import.meta.env.VITE_BACKEND_URL;

  const isOpen = useStore((state) => state.sidebar)
  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Ventas: Reporte Mensual");
  }, []);


  //   /* isOpen (globalstate) -> para que el contenido se ajuste según el ancho de la sidebar (navegación) */
  // const isOpen = useStore((state) => state.sidebar)
  // const [seledia, setseledia] = useState([]);



  



  const currentDate = new Date();
  const initialMonth = currentDate.getMonth() + 1;
  const initialWeekNumber = getWeekNumber(currentDate);
  const formRef = useRef([]);

  // const [month, setMonth] = useState(initialMonth);
  // const [weekNumber, setWeekNumber] = useState(initialWeekNumber);

const [month, setMonth] = useState(initialMonth);
  // const [weekNumber, setWeekNumber] = useState(initialWeekNumber);
  // const [salesData, setSalesData] = useState(null);
  const [loading, setLoading] = useState(false);
  const [loading1, setLoading1] = useState(false);

  const [data, setData] = useState([]);




  const handleMonthChange = (event) => {
    setMonth(event.target.value);
  };


  useEffect(() => {
    setLoading(true);
    const fetchYearData = async () => {
      try {
        const response = await fetch(`${URL}/api/ReportDay/year`,{
          headers: {
            'Authorization': `Bearer ${localStorage.token}`,
          }
        });
        const data = await response.json();
        setData(data);
      } catch (error) {
        console.error('Error al obtener los datos del año:', error);
      }
    };
    setLoading(false);
    fetchYearData();
  }, []);



  const generarPdf = () => {//http://localhost:5188/api/pdf/reportweek
    setLoading1(true)
    const url = `${URL}/api/pdf/year`;
    
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
  // Si se ha ingresado información al input, que la compare a los criterios
  : data.filter((item) =>
  item.cliente.nombreApellido.toLowerCase().includes(search.toLocaleLowerCase())||
  item.cliente.nombreApellido.toLowerCase().includes(search.toLocaleLowerCase())
  // item.platillo.toLowerCase().includes(search.toLocaleLowerCase())
  )
// FIN BUSCAR


  //accionar hanldesubmit automaticamente
  // const handleInputChange = (e) => {
  //   const { name, value } = e.target;
  //   if (name === 'month') {
  //     setMonth(value);
  //   } else if (name === 'weekNumber') {
  //     setWeekNumber(value);
  //   }
  // };

  // const handleInputsFilled = () => {
  //   return month !== '' && weekNumber !== '';
  // };

  // const handleAutoSubmit = () => {
  //   if (handleInputsFilled()) {
  //     formRef.current.submit();
  //   }
  // };
  //fin

  
  return (
    

  <div className={ isOpen ? "wrapper" : "side" }>
  <div className="container-fluid mt-4">
    <div className="row">
      
      <div className="col">
        <Searchbar searcher={searcher}/>
      </div>
      <div className="col">
            <SelectOPT />
        </div>
    </div>
    <div className="row">
      <div className="col">
        <Select />
      </div>
    
    <div className="col" >
        {/* <ButtonDrop>
          <FcPrint />
        </ButtonDrop> */}
        {/* <Label htmlFor="dateInput">Seleccionar fecha:</Label> */}
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
        {/* <TableData data={results} /> */}
        {/* <Tablep info={info}/> */}

        {/* <div>
      <form  onSubmit={handleSubmit}>
        <Label>
          Numero de mes:
          <Input
            type="number"
            name="month"
            value={month}
            // onChange={(e) => setMonth(e.target.value)}

          
          
        
          onChange={handleInputChange}
          // onBlur={handleAutoSubmit}
          />
        </Label>
        <Label>
          Numero de semana del:
          <Input
            type="number"
            name="weekNumber"
            value={weekNumber}
            // onChange={(e) => setWeekNumber(e.target.value)}

            
   
          onChange={handleInputChange}
          // onBlur={handleAutoSubmit}
          />
        </Label>
        <Button type="submit" disabled={loading}>
          Generar Reporte
        </Button>
      </form>

      {loading ? (
        <p>Cargando...</p>
      ) : salesData ? (
      
      <Tablep data={results} />

      ) : (
        <Alert color="danger">No se econtraron ventas.</Alert>
        // <Tablep data={results} />
      )}
    </div> */}
   <h3>Datos Generales</h3>
  
  {loading ? (
        <Spinner
        className="m-5"
        color="warning"
      >
        Loading...
      </Spinner>
      ) : data ? (
      
      <Tablep data={results} />

      ) : (
        <Alert color="danger">No se econtraron ventas.</Alert>
        // <Tablep data={results} />
      )}
{/* <div>
      <h2>Datos del Mes</h2>
      <form onSubmit={handleSubmit}>
        <label>
          Nombre del Mes:
          <input type="text" value={month} onChange={handleMonthChange} />
        </label>
        <button type="submit">Mostrar Datos</button>
      </form>
      <Table>
        <thead>
          <tr>
            <th>Cliente ID</th>
            <th>Nombre y Apellido</th>
            <th>Institución</th>
            <th>Total</th>
          </tr>
        </thead>
        <tbody>
          {data.map((item, index) => (
            <tr key={index}>
              <td>{item.cliente.idCliente}</td>
              <td>{item.cliente.nombreApellido}</td>
              <td>{item.cliente.institucion}</td>
              <td>{item.total}</td>
            </tr>
          ))}
        </tbody>
      </Table>
    </div> */}
      </div>
    </div>
  </div>
</div>





    
  );
};
// Función auxiliar para obtener el número de semana del mes
const getWeekNumber = (date) => {
  const firstDayOfMonth = new Date(date.getFullYear(), date.getMonth(), 1);
  const firstDayOfWeek = firstDayOfMonth.getDay();
  const weekNumber = Math.ceil((date.getDate() + firstDayOfWeek) / 7);
  return weekNumber;
};

export default ReportAll;
