import React, { useEffect, useState } from 'react'
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
import { Col, Button, Label, Input, Table, Alert,  Spinner} from 'reactstrap'
import { ToastContainer, toast } from "react-toastify";
import Tablebox from '../../components/report/tablebox';
import { Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import Tablep from '../../components/report/tprueba';
// import { Row, Col,  Button } from "reactstrap";
import { FcPrint } from 'react-icons/fc';
import ModalNewSale from '../../components/modales/ModalNewSale';
import Selectc from '../../components/report/SelecttReportC';
import { tr } from 'date-fns/locale';

import Tableboxweek from '../../components/report/tableboxweek';



function BoxWeek (props)  {
  const URL = import.meta.env.VITE_BACKEND_URL;
    //   /* isOpen (globalstate) -> para que el contenido se ajuste según el ancho de la sidebar (navegación) */
  const isOpen = useStore((state) => state.sidebar)
  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Caja: Reporte Semanal");
  }, []);



  //*********************************** */
  // const [cajaDiaria, setCajaDiaria] = useState(null);



//   *****************************************GENERAR PDF */

  const generarPdf = () => {
    const url = `${URL}/api/pdfBox/dailyweek`;
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
      })
      .catch((error) => {
        console.error('Error:', error);
      });
  };
  // *****************************************FIN GENERAR PDF */


  const [cajasSemanales, setCajasSemanales] = useState([]);

  useEffect(() => {
    fetch(`${URL}/api/ReportBox/dailyweek`,{
      headers: {
        'Authorization': `Bearer ${localStorage.token}`,
      }
    })
      .then(response => response.json())
      .then(data => {
        setCajasSemanales(data);
      })
      .catch(error => {
        console.error('Error:', error);
      });
  }, []);

  if (cajasSemanales.length === 0) {
    return 
      <Spinner
        className="m-5"
        color="warning"
      >
        Loading...
      </Spinner>
    
  }
  


  
 
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
          <Button
              onClick={generarPdf}
              color="primary"
              outline
            >
              Generar reporte
              <FcPrint />
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
                {/* ******************************************** */}

              <Tableboxweek cajasSemanales={cajasSemanales}></Tableboxweek>




                {/* *************************************************** */}



            </div>
    
 

    </div>
      

      <div>

          </div>
      </div>
    </div>


     </div>



    
  );


  
};

export default BoxWeek;