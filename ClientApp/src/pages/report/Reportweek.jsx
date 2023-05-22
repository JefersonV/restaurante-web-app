import React, { useEffect, useState } from 'react'
import { useStore } from '../../providers/GlobalProvider'
import DatePicker from '../../components/DatePicker';
import Searchbar from '../../components/Searchbar';
// import Select from '../../components/Select';
import Select from '../../components/report/SelecttReport';
import TableData from '../../components/TableData';
// import Tablaprueba from '../components/tprueba';
import ButtonDrop from '../../components/ButtonDrop';
import { Col, Button, Label, Input, } from 'reactstrap'
import Tablep from '../../components/tprueba';
// import { Row, Col,  Button } from "reactstrap";
import { FcPrint } from 'react-icons/fc';
import ModalNewSale from '../../components/modales/ModalNewSale';
// function Reports(props) {
//   /* isOpen (globalstate) -> para que el contenido se ajuste según el ancho de la sidebar (navegación) */
//   const isOpen = useStore((state) => state.sidebar)
  

//   //****
//   const [selectedDate, setSelectedDate] = useState('');

//   const handleDateChange = (event) => {
//     setSelectedDate(event.target.value);
//   };


//   const handleGeneratePdf = async () => {
//     try {
//       const response = await fetch(`http://localhost:5173/api/generate-pdf?date=${selectedDate}`, {
//         method: 'GET',
//         responseType: 'blob', // Importante: establecer el tipo de respuesta como blob
//       });

//       // Obtener el contenido del PDF
//       const pdfBlob = await response.blob();

//       // Crear un objeto URL del blob
//       const pdfUrl = URL.createObjectURL(pdfBlob);

//       // Abrir el PDF en una nueva ventana o pestaña del navegador
//       window.open(pdfUrl, '_blank');
//     } catch (error) {
//       console.error(error);
//     }
//   };

//   //*/
//   return (
//     <div>
//     <label htmlFor="dateInput">Fecha:</label>
//     <input type="date" id="dateInput" value={selectedDate} onChange={handleDateChange} />
//     <button onClick={handleGeneratePdf}>Generar PDF</button>
//   </div>
//   )
// }



// export default Reports



function Reportsweek (props)  {
  const isOpen = useStore((state) => state.sidebar)
  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Reporte Semanal");
  });
  //   /* isOpen (globalstate) -> para que el contenido se ajuste según el ancho de la sidebar (navegación) */
  // const isOpen = useStore((state) => state.sidebar)
  const [seledia, setseledia] = useState([]);

  useEffect(() => {
    // Obtener la fecha actual en formato 'YYYY-MM-DD'
    // const today = new Date().toISOString().split('T')[0];
  
    // setseledia(today);

    const today = new Date();
today.setDate(today.getDate() - 1);
const formattedDate = today.toISOString().split('T')[0];

setseledia(formattedDate);
    console.log("Fecha: ",today)
  }, []);

  const today = new Date(); console.log(today)
  const now = today.toLocaleDateString();
  console.log(now)
  const ManejoCambioFecha = (event) => {
    
    setseledia(event.target.value);
  };
  

  // const handleGeneratePdf = () => {
  //   const url = `http://localhost:5173/api/pdf?date=${selectedDate}`;
  //   fetch(url, {
  //     method: 'GET',
  //     headers: {
  //       'Content-Type': 'application/pdf',
  //     },
  //     responseType: 'blob',
  //   })
  //     .then((response) => response.blob())
  //     .then((blob) => {
  //       const url = window.URL.createObjectURL(blob);
  //       const link = document.createElement('a');
  //       link.href = url;
  //       // window.open(blob, '_blank');
  //       link.setAttribute('dowload', 'ventas.pdf');
  //       document.body.appendChild(link);
  //       link.click();
  //       link.remove();
  //     })
  //     .catch((error) => {
  //       console.error('Error:', error);
  //     });
  // };

  const generarPdf = () => {//http://localhost:5188/api/pdf/reportweek
    const url = `http://localhost:5188/api/pdf/reportweek`;
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

  // ++++++++++******************
  // const apiurl = 'http://localhost:5173/api/Proveedor';
  // const columna = ['Nombre', 'Dirección'];

  const info = {
    apiurl: 'http://localhost:5173/api/Venta',
    columna: ['Index','Nombre', 'Dirección'],
  };


  // *************************

    /* ----- Buscador */
  // state para buscador
  // const [search, setSearch] = useState("")
  // // buscador, captura de datos
  // const searcher = (e) => {
  //   console.log(e.target.value)
  //   setSearch(e.target.value)
  //   console.log(e.target.value.length)
  // }
  // //metodo de filtrado del buscador
  // // Si state search es null (no sea ha ingresado nada en el input) results = dataApi
  // const results = !search ? dataApi 
  // // Si se ha ingresado información al input, que la compare a los criterios
  // : dataApi.filter((item) =>
  //   item.email.toLowerCase().includes(search.toLocaleLowerCase())
  // )
// FIN BUSCAR




  

  
  return (
    


  //   <Row className="justify-content-center">
  //   <Col xs="auto">
  //     <div>
  //       <Label htmlFor="dateInput">Fecha:</Label>
  //       <Input
  //         type="date"
  //         id="dateInput"
  //         value={seledia}
  //         onChange={ManejoCambioFecha}
  //       />
  //       <Button onClick={generarPdf}>Generar PDF</Button>
  //     </div>
  //   </Col>
  // </Row>

  <div className={ isOpen ? "wrapper" : "side" }>
  <div className="container-fluid mt-4">
    <div className="row">
      {/* <div className="col">
        <DatePicker />
      </div> */}
      <div className="col">
        {/* <Searchbar searcher={searcher}/> */}search
      </div>
    </div>
    <div className="row d-flex justify-content-center align-items-center">
      <div className="col">
        <Select />
      </div>
    
    <Col className="border p-3" >
        {/* <ButtonDrop>
          <FcPrint />
        </ButtonDrop> */}
        <Label htmlFor="dateInput">Seleccionar fecha:</Label>
        <Button
          onClick={generarPdf}
          color="primary"
          outline
        >
          Generar reporte
          <FcPrint />
        </Button>
      </Col>
    
      
    </div>
    <div className="row">
      <div className="col">
        {/* <TableData data={results} /> */}
        {/* <Tablep info={info}/> */}
      </div>
    </div>
  </div>
</div>





    
  );
};

export default Reportsweek;
