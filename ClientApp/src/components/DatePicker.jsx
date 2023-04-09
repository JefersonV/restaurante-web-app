import React, { useState, useEffect, useRef } from 'react'
import dayjs from 'dayjs';
import 'react-date-range/dist/styles.css'; // main style file
import 'react-date-range/dist/theme/default.css'; // theme css file
import { DateRange } from 'react-date-range';
import es from 'date-fns/locale/es';
import addDays from 'date-fns/addDays';
import '../styles/DatePicker.scss'

function DatePicker() {
  const [selectedDateRange, setSelectedDateRange] = useState([
    {
      startDate: new Date(),
      endDate: addDays(new Date(),7),
      key: 'selection',
    },
  ]);
  /* Rango seleccionado */
  useEffect(() => {
    console.log('rango de fechas')
    console.log(dayjs(selectedDateRange[0].startDate).format('DD/MM/YYYY'))
    console.log(dayjs(selectedDateRange[0].endDate).format('DD/MM/YYYY'))
  }, [selectedDateRange])
  /* Abrir y cerrar el datepicker */
  const [openDate, setOpenDate] = useState(false)
  // get the element to toggle
  const refOne = useRef(null)

  /* EVENTOS PARA OCULTAR EL DATE PICKER */
  const hideOnClickOutside = e => {
    if(refOne.current && !refOne.current.contains(e.target)) {
      setOpenDate(false)
    }
  }

  const hideOnEscape = (e) => {
    console.log(e.key)
    if(e.key === "Escape") {
      setOpenDate(false)
    }
  }

  useEffect(() => {
    document.addEventListener('keydown', hideOnEscape, true)
    document.addEventListener('click', hideOnClickOutside, true)
  }, [])

  const handleDateChange = (ranges) => {
    setSelectedDateRange([ranges.selection]);
  };
  
  /* Opciones del datepicker range */
  const options = {
    locale: es,
    ranges: selectedDateRange,
    onChange: handleDateChange,
    months: 2,
    direction: "horizontal",
    /* moveRangeOnFirstSelection: false, */
  }; 
  return (
    <div>
      <input 
        
        value={ `Fecha desde ${dayjs(selectedDateRange[0].startDate).format('DD/MM/YYYY')} hasta ${dayjs(selectedDateRange[0].startDate).format('DD/MM/YYYY')}` }
        className='form-control inputBox'
        readOnly
        onClick={() => setOpenDate(prevState => !prevState)}
      />
      <div className="calendarContainer" ref={refOne}>
        {openDate && 
          <DateRange
            {...options}
          />
        }
      </div>
    </div>
  )
}

export default DatePicker