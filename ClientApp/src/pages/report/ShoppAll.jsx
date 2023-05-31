import React, { useEffect, useState, useRef } from 'react'
import { useStore } from '../../providers/GlobalProvider'
import DatePicker from '../../components/DatePicker';
import Searchbar from '../../components/Searchbar';
// import Select from '../../components/Select';
// import Select from '../../components/report/SelecttReport';
import Select from '../../components/report/SelectReportShopp';
// import Tablaprueba from '../components/tprueba';
import ButtonDrop from '../../components/ButtonDrop';
import { Col, Button, Label, Input, Spinner, Alert} from 'reactstrap'
// import Tablep from '../../components/report/tprueba';
import Tablep from '../../components/report/tshopping';
// import { Row, Col,  Button } from "reactstrap";
import { FcPrint } from 'react-icons/fc';
import ModalNewSale from '../../components/modales/ModalNewSale';
import SelectOPT from '../../components/report/SelectReportShopp';
import Selectcompras from '../../components/report/SelecttReportC';


function ShoppAll (props)  {
  const URL = import.meta.env.VITE_BACKEND_URL;

  const isOpen = useStore((state) => state.sidebar)
  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Compras: Reporte Semanal");
  }, []);
  //   /* isOpen (globalstate) -> para que el contenido se ajuste según el ancho de la sidebar (navegación) */
  // const isOpen = useStore((state) => state.sidebar)
  // const [seledia, setseledia] = useState([]);


  const [loading1, setLoading1] = useState(false);

  const generarPdf = () => {//http://localhost:5188/api/pdf/reportweek
    setLoading1(true)
    const url = `${URL}/api/pdfCost/Costyear`;
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

  const currentDate = new Date();
  const initialMonth = currentDate.getMonth() + 1;
  const initialWeekNumber = getWeekNumber(currentDate);
  const formRef = useRef([]);

  // const [month, setMonth] = useState(initialMonth);
  // const [weekNumber, setWeekNumber] = useState(initialWeekNumber);

const [month, setMonth] = useState(initialMonth);
  const [weekNumber, setWeekNumber] = useState(initialWeekNumber);
  const [salesData, setSalesData] = useState(null);
  const [loading, setLoading] = useState(false);




  // const handleSubmit = async (e) => {
  //   e.preventDefault();
  //   setLoading(true);

  //   try {
  //     const response = await fetch("http://localhost:5173/api/ReportCost/year",
  //     {
  //       headers: {
  //         'Authorization': `Bearer ${localStorage.token}`,
  //       }
  //     });
  //     if (response.ok) {
  //       const data = await response.json();
  //       setSalesData(data);
  //     } else {
  //       console.error('Error al obtener los datos');
  //     }
  //   } catch (error) {
  //     console.error('Error en la solicitud:', error);
  //   }

  //   setLoading(false);
  // };



    
    const fetchYearData = async () => {
      setLoading(true);
      try {
        const response = await fetch(`${URL}/api/ReportCost/year`, {
          headers: {
            'Authorization': `Bearer ${localStorage.token}`,
          }
        });
        const data = await response.json();
        setSalesData(data);
        setLoading(false);
      } catch (error) {
        console.error('Error al obtener los datos del año:', error);
        setLoading(false);
      }
      // setLoading(false);
    };
   
    useEffect(() => { 
    fetchYearData();
  }, []);

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
  // Si se ha ingresado información al input, que la compare a los criterios
  : salesData.filter((item) =>
  item.proveedor.nombre.toLowerCase().includes(search.toLocaleLowerCase()) ||
  item.gastos[0].concepto.toLowerCase().includes(search.toLocaleLowerCase())
  // item.platillo.toLowerCase().includes(search.toLocaleLowerCase())
  )
// FIN BUSCAR



  
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
        <Selectcompras />
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

        <div>
      <h3>Datos Generales</h3>
      {/* <Button type='submit' onChange={handleSubmit}>Ver</Button>} */}

      {loading ? (
        <Spinner
        className="m-5"
        color="warning"
      >
        Loading...
      </Spinner>
      ) : salesData ? (
      
      <Tablep data={results} />

      ) : (
        <Alert color="danger">No se econtraron compras.</Alert>
        // <Tablep data={results} />
      )}
    </div>
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

export default ShoppAll;
