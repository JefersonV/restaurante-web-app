import React, { useEffect, useState, useRef } from 'react'
import { useStore } from '../../providers/GlobalProvider'
import DatePicker from '../../components/DatePicker';
import Searchbar from '../../components/Searchbar';
// import Select from '../../components/Select';
import SelectBox from '../../components/report/SelectBox';
// import Select from '../../components/report/SelectReportShopp';
import SelectOPT from '../../components/report/SelectReportShopp';
import TableData from '../../components/TableData';
// import Tablaprueba from '../components/tprueba';
import ButtonDrop from '../../components/ButtonDrop';
import { Form, Button, Label, Input, Table, Alert,  Spinner} from 'reactstrap'
import { ToastContainer, toast } from "react-toastify";
import Tablebox from '../../components/report/tablebox';
import { Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import Tablep from '../../components/report/tprueba';
// import { Row, Col,  Button } from "reactstrap";
import { FcPrint } from 'react-icons/fc';
import Tableboxmonth from '../../components/report/tableboxMonth';
import { tr } from 'date-fns/locale';
import Selectmonthbox from '../../components/report/Selectmbox';

import Tableboxweek from '../../components/report/tableboxweek';



function BoxMonth (props)  {
  const URL = import.meta.env.VITE_BACKEND_URL;
    //   /* isOpen (globalstate) -> para que el contenido se ajuste según el ancho de la sidebar (navegación) */
  const isOpen = useStore((state) => state.sidebar)
  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Caja: Reporte Mensual");
  }, []);



  //*********************************** */
  // const [cajaDiaria, setCajaDiaria] = useState(null);



//   *****************************************GENERAR PDF */
const [loading1, setLoading1] = useState(false)

  const generarPdf = () => {
    setLoading1(true)
    const url = `${URL}/api/pdfBox/montss?month=${month}`;
    //=2023-5-20
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


  const currentDate = new Date();
  const initialMonth = currentDate.getMonth() + 1;
  const initialWeekNumber = getWeekNumber(currentDate);
  const formRef = useRef([]);


  const [month, setMonth] = useState(initialMonth);
  
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [sem, setsem] = useState(null)

  // const handleMonthChange = (event) => {
  //   setMonth(event.target.value);
  // };

  // const handleSubmit = async (event) => {
  //   event.preventDefault();
  //   setLoading(true);

  //   try { 
  //     const response = await fetch();
  //     const jsonData = await response.json();
  //     setData(jsonData);
  //     setLoading(false);
  //   } catch (error) {
  //     setError('Error al obtener los datos.');
  //     setLoading(false);
  //   }
  // };

  const handleMonthChange = (event) => {
    setMonth(event.target.value);
  };

  // const [selectedMonth, setSelectedMonth] = useState('');

  // const handleMonthChange = (event) => {
  //   setSelectedMonth(event.target.value);
  // };
  const fetchMonthData = async () => {
    setLoading(true)
    try {
      const response = await fetch(`${URL}/api/ReportBox/monts?month=${month}`,{
        headers: {
          'Authorization': `Bearer ${localStorage.token}`,
        }
      });

      // ?month=may
      const jsonData = await response.json();
      setData(jsonData);
      // setsem(data.semanas )
      setLoading(false)
    } catch (error) {
      console.error('No sé encontraron datos:', error);
      alert('No se encontraron  los datos ');
      // <Alert> No se econtraron los datos</Alert>

    
      setLoading(false)
      // setError(error)
    }
    setLoading(false)
    
  };
  useEffect(() => {
    
      fetchMonthData();
    
  }, []);

  const handleSubmit = (event) => {
    event.preventDefault();
    fetchMonthData();
  };

  console.log(data)

  // if (data.semanas.length === 0) {
  //   return <Spinner
  //       className="m-5"
  //       color="warning"
  //     >
  //       Loading...
  //     </Spinner>
    
  // }

  



 
  return (

  <div className={ isOpen ? "wrapper" : "side" }>
      <div className="container-fluid mt-4">
        <div className="row">
          {/* <div className="col">
            <DatePicker />
          </div> */}
          <div className="col">
            {/* <Searchbar searcher={searcher}/> */}


          </div>
          <div className="col">
            <SelectOPT />


          </div>
        </div>
        <div className="row ">
          <div className='col '>
            <SelectBox />
            
            
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
            {/* <Button onClick={fetchDailyReport}>Ver</Button> */}
          {/* <Input
            type="date"
            id="dateInput"
            value={selectedDate1}
            onChange={handleDateChange1}
          /> */}

      </div>
    
        </div>
        
        <div className="row">
          <div className="col">

            <div>



                {/* *************************************************** */}

          




<div>
      <Form onSubmit={handleSubmit}>
        {/* <label> */}
          
          {/* <input type="number" value={month} onChange={handleMonthChange} /> */}
          <div className='row'>
            <div className='col'>
            <Selectmonthbox value={month}  sele={handleMonthChange} ></Selectmonthbox>
            </div>
            <div className='col'>
            <Button color='primary' type="submit" outline >Obtener Datos</Button>


            </div>
          </div>
        {/* </label> */}
      </Form>



      {loading && <Spinner
        className="m-5"
        color="warning"
      >
        Loading...
      </Spinner>}
      {error && <p>{error}</p>}
      {data !== null  ? (
        // <p>no hay</p>
        <div >
        {/* ******************** */}
        <Tableboxmonth data={data}></Tableboxmonth>

          
        </div>
        
      ) : (
        <p>no hay datos</p>
        
      )
    }
    </div>


<div>
      

      

      <div>
        
        
      </div>
    </div>







  

{/*         ++++++++******************************************+ */}


            </div>
    
 

    </div>
      

      <div>

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

export default BoxMonth;