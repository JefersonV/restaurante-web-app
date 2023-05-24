import { useEffect, useState } from "react";
import { useStore } from "../providers/GlobalProvider";
import { Button, Table } from "reactstrap";
import { HiPencilAlt } from "react-icons/hi";
import { RiDeleteBin6Line } from "react-icons/ri";
import { FormGroup, Input } from "reactstrap";
import TablePurchases from "../components/purchases/TablePurchases";
import ModalAdd from '../components/purchases/ModalAddPurchase'
import "../styles/Select.scss";

function Purchases(props) {
  /* isOpen (globalstate) -> para que el contenido se ajuste según el ancho de la sidebar (navegación) */
  const isOpen = useStore((state) => state.sidebar);

  //Estados necesarios
  const [reporteCompras, setReporteCompras] = useState([]);
  const [datosTabla, setDatosTabla] = useState([]);
  const [selectedOption, setSelectedOption] = useState("");

  //Consultas a la api
  //Consulta para obtener todas las compras
  const consultaDeCompras = async () => {
    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.token}`,
      },
    };
    const response = await fetch(
      "http://localhost:5173/api/Gasto",
      requestOptions
    );
    const data = await response.json();
    setReporteCompras(data);
    setDatosTabla(filterDataByMonth(data));
    setSelectedOption(1)
  };

  //Funciones necesarias para el sistema
  const filterDataByCurrentDate = (data) => {
    const currentDate = new Date().toISOString().split("T")[0];
    //const currentDate = new Date();
    //currentDate.setDate(currentDate.getDate() - 1);
    //const yesterday = currentDate.toISOString().split("T")[0];
    //console.log(yesterday);
    const filteredData = data.filter((item) => item.fecha === currentDate);
    return filteredData;
  };
  const filterDataByDateRange = (data) => {
    const startDate = new Date(); // Fecha de inicio de la semana actual
    startDate.setDate(startDate.getDate() - startDate.getDay());

    const endDate = new Date(); // Fecha de cierre de la semana actual
    endDate.setDate(endDate.getDate() + (6 - endDate.getDay()));

    const filteredData = data.filter((item) => {
      const itemDate = new Date(item.fecha);
      return itemDate >= startDate && itemDate <= endDate;
    });

    return filteredData;
  };
  const filterDataByMonth = (jsonData) => {
    const currentDate = new Date();
    const startOfMonth = new Date(
      currentDate.getFullYear(),
      currentDate.getMonth(),
      1
    );
    const endOfMonth = new Date(
      currentDate.getFullYear(),
      currentDate.getMonth() + 1,
      0
    );

    const filteredData = jsonData.filter((item) => {
      const itemDate = new Date(item.fecha);
      return itemDate >= startOfMonth && itemDate <= endOfMonth;
    });

    return filteredData;
  };
  const filterDataBy3Months = (jsonData) => {
    const currentDate = new Date();
    const currentMonth = currentDate.getMonth();
    const currentYear = currentDate.getFullYear();

    const endOfCurrentMonth = new Date(currentYear, currentMonth + 1, 0);

    const startOfTwoMonthsAgo = new Date(currentYear, currentMonth - 2, 1);

    return jsonData.filter((item) => {
      const itemDate = new Date(item.fecha);
      return itemDate >= startOfTwoMonthsAgo && itemDate <= endOfCurrentMonth;
    });
  };
  const filterDataByYear = (jsonData) => {
    const currentDate = new Date();
    const currentYear = currentDate.getFullYear();

    const startOfYear = new Date(currentYear, 0, 1);
    const endOfYear = new Date(currentYear, 11, 31);

    return jsonData.filter(({ fecha }) => {
      const itemDate = new Date(fecha);
      return itemDate >= startOfYear && itemDate <= endOfYear;
    });
  };

  //Funciones para detectar la selección
  const handleChange = (event) => {
    setSelectedOption(event.target.value);

    // Verificar el valor seleccionado y ejecutar la función correspondiente
    if (event.target.value === "1") {
      handleOption1();
    }
    if (event.target.value === "2") {
      handleOption2();
    }
    if (event.target.value === "3") {
      handleOption3();
    }
    if (event.target.value === "4") {
      handleOption4();
    }
    if (event.target.value === "5") {
      handleOption5();
    }
    if (event.target.value === "6") {
      handleOption6();
    }
  };
  const handleOption1 = () => {
    // Función para la opción número 1
    setDatosTabla(filterDataByMonth(reporteCompras));
  };
  const handleOption2 = () => {
    // Función para la opción número 2
    setDatosTabla(filterDataByCurrentDate(reporteCompras));
  };
  const handleOption3 = () => {
    // Función para la opción número 2
    setDatosTabla(filterDataByDateRange(reporteCompras));
  };
  const handleOption4 = () => {
    // Función para la opción número 2
    setDatosTabla(filterDataBy3Months(reporteCompras));
  };
  const handleOption5 = () => {
    // Función para la opción número 2
    setDatosTabla(filterDataByYear(reporteCompras));
  };
  const handleOption6 = () => {
    // Función para la opción número 2
    setDatosTabla(reporteCompras);
  };

  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Gastos");
    consultaDeCompras();
  }, []);

  return (
    <div className={isOpen ? "wrapper" : "side"}>
      <div className="container-fluid mt-4">
        <div className="row">
          <div className="col">
            <h3>Barra para buscar entre fechas</h3>
          </div>
        </div>
        <div className="row d-flex justify-content-center align-items-center">
          <div className="col">
            <FormGroup style={{ maxWidth: "500px" }}>
              <Input
                id="exampleSelect"
                name="select"
                type="select"
                value={selectedOption}
                onChange={handleChange}
              >
                <option value="1">Resumen de gastos del mes</option>
                <option value="2">Gastos de hoy</option>
                <option value="3">Gastos de la semana</option>
                <option value="4">Gastos de los ultimos 3 meses</option>
                <option value="5">Gastos del año</option>
                <option value="6">Todos los gastos</option>
              </Input>
            </FormGroup>
          </div>
          <div className="col">
            <ModalAdd actualizarListaMenu={consultaDeCompras} />
          </div>
          {/* <div className="col">
            <button>Icono reporte</button>
          </div> */}
        </div>
        <div className="row">
          <div className="col">
            <TablePurchases
              data={datosTabla}
              actualizarListaCompras={consultaDeCompras}
            />
          </div>
        </div>
      </div>
    </div>
  );
}

export default Purchases;
