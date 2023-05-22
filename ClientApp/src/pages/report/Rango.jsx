import React, { useEffect, useState } from 'react';
import { Table, Form, FormGroup, Label, Input, Button } from 'reactstrap';

import { useStore } from '../../providers/GlobalProvider'
import DatePicker from '../../components/DatePicker';
import Searchbar from '../../components/Searchbar';
// import Select from '../../components/Select';
import Select from '../../components/report/SelecttReport';
import TableData from '../../components/TableData';
// import Tablaprueba from '../components/tprueba';
import ButtonDrop from '../../components/ButtonDrop';
// import { Col, Button, Label, Input, Table, Alert,  Spinner} from 'reactstrap'
import { Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import Tablep from '../../components/tprueba';

function Rango(props)  {

  const isOpen = useStore((state) => state.sidebar)
  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Reporte Diario");
  });


  const [data, setData] = useState([]);
  // const [fechaDesde, setFechaDesde] = useState(new Date().toISOString().split('T')[0]);
  // const [fechaHasta, setFechaHasta] = useState(new Date().toISOString().split('T')[0]);
    const [fechaDesde, setFechaDesde] = useState("");
  const [fechaHasta, setFechaHasta] = useState("");

  const fetchData = () => {
    // Realizar la solicitud a la API para obtener los datos del rango de fechas seleccionado
    // fetch(`http://api-url.com/reports/day?fechaDesde=${fechaDesde}&fechaHasta=${fechaHasta}`)
    fetch(`http://localhost:5188/api/ReportDay/rango?fechaDesde=${fechaDesde}&fechaHasta=${fechaHasta}`)
    // `http://localhost:5188/api/ReportDay/day?fechaDesde=${fechaDesde}&fechaHasta=${fechaHasta}`
      .then((response) => response.json())
      .then((data) => {
        setData(data);
        // console.log("casa",data)
      })
      .catch((error) => {
        console.error('Error:', error);
      });
  };
  console.log("dsafsd", data)

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






    return (
      <div className={ isOpen ? "wrapper" : "side" }>
        <div className="container mt-4">
          <div className="row">
            <div className="col-6">
            {/* <DatePicker /> */}
              {/* <Searchbar searcher={searcher}/> */}
            </div>
            <div className="col-6">
              {/* Prop para actualizar la data después de confirmar el envío de post */}
              {/* <ModalAdd actualizarListaProveedores={getData} /> */}
        
              {/* <Button 
                onClick = {handleDownload} 
                // onClick={() => {
                //   generatePDF(
                //     dataApi
                //   )}}
                color="primary"
                outline
                >
                Imprimir lista 
                <FcPrint />
              </Button> */}
   
            </div>
          </div>
          
          <div className="row">
            <div className="col">

            <Form >
        <FormGroup>
          <Label for="fechaDesde">Fecha Desde:</Label>
          <Input type="date" id="fechaDesde" value={fechaDesde} onChange={handleFechaDesdeChange} />
        </FormGroup>
        <FormGroup>
          <Label for="fechaHasta">Fecha Hasta:</Label>
          <Input type="date" id="fechaHasta" value={fechaHasta} onChange={handleFechaHastaChange} />
        </FormGroup>
        <Button color="primary" onClick={fetchData} disabled={!fechaDesde || !fechaHasta}>
          Generar Reporte
        </Button>
      </Form>
      
      {data.length > 0 ? (
        <Table>
          <thead>
            <tr>
              <th>Cliente</th>
              <th>Total</th>
              <th>Ventas</th>
              <th>Fecha</th>
            </tr>
          </thead>
          <tbody>
            {data.map((item, index) => (
              <tr key={index}>
                <td>{item.cliente.nombreApellido}</td>
                <td>{item.total}</td>
                <td>
                  <ul>
                    {item.ventas.map((venta, ind) => (
                      <li key={ind}>
                        {venta.platillo} - Cantidad: {venta.cantidad} - Subtotal: {venta.subtotal}
                      </li>
                    ))}
                  </ul>
                </td>
                
                <td>
                {item.ventas.length > 0 && item.ventas[0].fecha}
                </td>
              </tr>
            ))}
          </tbody>
        </Table>
      ) : (
        <p>No hay datos disponibles.</p>
      )}
              
            </div>
          </div>
        </div>
      </div>
    )
};

export default Rango;
