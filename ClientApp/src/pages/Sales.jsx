import React, { useState, useEffect } from "react";
import { useStore } from "../providers/GlobalProvider";
import DatePicker from "../components/purchases/DatePicker";
import Searchbar from "../components/Searchbar";
import Select from "../components/Select";
import TableData from "../components/TableData";
import SummaryTableSale from "../components/sales/SummaryTableSale";
import ButtonDrop from "../components/ButtonDrop";
import { FcPrint } from "react-icons/fc";
import ModalNewSale from "../components/modales/ModalNewSale";
import { FormGroup, Input, Button } from "reactstrap";
import dayjs from "dayjs";
import addDays from "date-fns/addDays";
import weekOfYear from "dayjs/plugin/weekOfYear";
import isToday from "dayjs/plugin/isToday";
import weekday from "dayjs/plugin/weekday";

dayjs.extend(weekOfYear);
dayjs.extend(isToday);
dayjs.extend(weekday);
// dayjs.extend(startOfMonth);
// dayjs.extend(endOfMonth);

function Sales(props) {
  /* isOpen (globalstate) -> para que el contenido se ajuste según el ancho de la sidebar (navegación) */
  const isOpen = useStore((state) => state.sidebar);
  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Ventas");
  }, []);

  const [selectedDateRange, setSelectedDateRange] = useState([
    {
      startDate: new Date(),
      endDate: addDays(new Date(), 7),
      key: "selection",
    },
  ]);
  useEffect(() => {
    //console.log('rango de fechas')
    //console.log(dayjs(selectedDateRange[0].startDate).format('DD/MM/YYYY'))
    //console.log(dayjs(selectedDateRange[0].endDate).format('DD/MM/YYYY'))
  }, [selectedDateRange]);
  const handleDateChange = (ranges) => {
    setSelectedDateRange([ranges.selection]);
  };

  /* ------ Fetch */
  const [dataApi, setDataApi] = useState([]);
  const [datosTabla, setDatosTabla] = useState("");
  const [selectedOption, setSelectedOption] = useState("");

  const getData = async () => {
    try {
      const response = await fetch(
        `${import.meta.env.VITE_BACKEND_URL}/api/Venta`,
        {
          headers: {
            Authorization: `Bearer ${localStorage.token}`,
          },
        }
      );
      const json = await response.json();
      setDataApi(json);
      setDatosTabla(filterDataByMonth(json));
      setSelectedOption(1);
    } catch (error) {
      console.log(error);
    }
  };

  useEffect(() => {
    console.log(dataApi);
    getData();
  }, []);

  /* ----- Buscador */
  // state para buscador
  const [search, setSearch] = useState("");
  // buscador, captura de datos
  const searcher = (e) => {
    console.log(e.target.value);
    setSearch(e.target.value);
    console.log(e.target.value.length);
  };
  //metodo de filtrado del buscador
  // Si state search es null (no sea ha ingresado nada en el input) results = dataApi
  const results = !search
    ? dataApi
    : // Si se ha ingresado información al input, que la compare a los criterios
      dataApi.filter((item) =>
        item.cliente.toLowerCase().includes(search.toLocaleLowerCase())
      );

  //Funciones necesarias para el sistema
  const filterDataByCurrentDate = (data) => {
    const fechaActual = dayjs();
    const fechaFormateada = fechaActual.format("YYYY-MM-DD");
    //const currentDate = new Date();
    //currentDate.setDate(currentDate.getDate() - 1);
    //const yesterday = currentDate.toISOString().split("T")[0];
    //console.log(yesterday);
    const filteredData = data.filter((item) => item.fecha === fechaFormateada);
    return filteredData;
  };
  const filterDataByDateRange = (data) => {
    const fechaActual = dayjs();
    const numeroSemana = fechaActual.week();
    const fechaInicioSemana = fechaActual.startOf("week");
    const fechaFinSemana = fechaInicioSemana.add(6, "day");
    const fechaInicioFormateada = fechaInicioSemana.format("YYYY-MM-DD");
    const fechaFinFormateada = fechaFinSemana.format("YYYY-MM-DD");

    const filteredData = data.filter((item) => {
      const itemDate = item.fecha;
      return (
        itemDate >= fechaInicioFormateada && itemDate <= fechaFinFormateada
      );
    });

    return filteredData;
  };
  const filterDataByMonth = (jsonData) => {
    const fechaActual = dayjs();
    const fechaInicioMes = fechaActual.startOf("month");
    const fechaFinMes = fechaActual.endOf("month");
    const fechaInicioFormateada = fechaInicioMes.format("YYYY-MM-DD");
    const fechaFinFormateada = fechaFinMes.format("YYYY-MM-DD");

    const filteredData = jsonData.filter((item) => {
      const itemDate = item.fecha;
      return (
        itemDate >= fechaInicioFormateada && itemDate <= fechaFinFormateada
      );
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
  const filterDataByCustomDateRange = (data, startDate, endDate) => {
    // const startDate = new Date(); // Fecha de inicio de la semana actual
    // startDate.setDate(startDate.getDate() - startDate.getDay());

    // const endDate = new Date(); // Fecha de cierre de la semana actual
    // endDate.setDate(endDate.getDate() + (6 - endDate.getDay()));

    const filteredData = data.filter((item) => {
      const itemDate = item.fecha;
      return itemDate >= startDate && itemDate <= endDate;
    });

    return filteredData;
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
    setDatosTabla(filterDataByMonth(dataApi));
  };
  const handleOption2 = () => {
    // Función para la opción número 2
    setDatosTabla(filterDataByCurrentDate(dataApi));
  };
  const handleOption3 = () => {
    // Función para la opción número 2
    setDatosTabla(filterDataByDateRange(dataApi));
  };
  const handleOption4 = () => {
    // Función para la opción número 2
    setDatosTabla(filterDataBy3Months(dataApi));
  };
  const handleOption5 = () => {
    // Función para la opción número 2
    setDatosTabla(filterDataByYear(dataApi));
  };
  const handleOption6 = () => {
    // Función para la opción número 2
    setDatosTabla(dataApi);
  };
  const handleOption7 = () => {
    // Función para la opción número 2
    setDatosTabla(
      filterDataByCustomDateRange(
        dataApi,
        dayjs(selectedDateRange[0].startDate).format("YYYY-MM-DD"),
        dayjs(selectedDateRange[0].endDate).format("YYYY-MM-DD")
      )
    );
  };

  return (
    <div className={isOpen ? "wrapper" : "side"}>
      <div className="container-fluid mt-4">
        <div className="row">
          <div className="col">
            <DatePicker
              selectedDateRange={selectedDateRange}
              handleDateChange={handleDateChange}
            />
          </div>
          <div className="col">
            <Button outline color="info" onClick={handleOption7}>
              Buscar
            </Button>
          </div>
          {/* <div className="col">
            <Searchbar searcher={searcher} />
          </div> */}
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
                <option value="1">Resumen de ventas del mes</option>
                <option value="2">Ventas de hoy</option>
                <option value="3">Ventas de la semana</option>
                <option value="4">Ventas de los ultimos 3 meses</option>
                <option value="5">Ventas del año</option>
                <option value="6">Todos los ventas</option>
              </Input>
            </FormGroup>
          </div>
          <div className="col">
            <ModalNewSale />
          </div>
          <div className="col">
            <ButtonDrop>
              <FcPrint />
            </ButtonDrop>
          </div>
        </div>
        <div className="row">
          <div className="col">
            <SummaryTableSale
              data={datosTabla}
              actualizarListaVentas={getData}
            />
          </div>
        </div>
      </div>
    </div>
  );
}

export default Sales;
