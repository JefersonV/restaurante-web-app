import React, { useState } from 'react'
import { Label } from 'reactstrap'
import { FormGroup } from 'reactstrap'
import { Input } from 'reactstrap'
import { useNavigate } from 'react-router-dom'
// import Box from '@mui/material/Box';
// import InputLabel from '@mui/material/InputLabel';
// import FormControl from '@mui/material/FormControl';
// import NativeSelect from '@mui/material/NativeSelect';
// import '../styles/Select.scss'
// import '../styles/Select.scss'

const  SelectBox = () => {


  const navigate = useNavigate();
  // const [selectedOption, setSelectedOption] = useState('Resumen de ventas de hoy');
  const [selectedOption, setSelectedOption] = useState('');

 


  const options = [
  
    {
      value: '1',
      label: 'hoy',
      href: '/boxday',
    },
    {
      value: '2',
      label: 'semanal',
      href: '/boxweek',
      
    },
    {
      value: '3',
      label: 'Mensual',
      href: '/boxmonth',
    },

    
  ];

  // const history = useHistory();

const handle = (event) => {

  // Prevenir la actualización del valor seleccionado
  event.preventDefault();
    

  
  
  
const selectedOption = event.target.value;
const selectedOptionData = options.find((option) => option.value === selectedOption);
// Actualizar el estado con el valor seleccionado

if (selectedOptionData) {
  navigate(selectedOptionData.href, {replace: true});
}

};




  
  return (
     <FormGroup> 
     <Input 
        onChange={handle} 
        id="exampleSelect"
        name="select"
        type="select">
     <option value="">Seleccionar opción</option>
     {options.map((option) => (
      <option key={option.value} value={option.value}>
      {option.label}
      </option>
      ))}
      
     
     </Input>

     </FormGroup> 

    

   

  )
}

export default SelectBox;
