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

const  Select = () => {


  const navigate = useNavigate();
  // const [selectedOption, setSelectedOption] = useState('Resumen de ventas de hoy');
  const [selectedOption, setSelectedOption] = useState('');




  const options = [
    {
      value: '1',
      label: 'hoy',
      href: '/reports',
    },
    {
      value: '2',
      label: 'semanal',
      href: '/reports/week',
      
    },
    {
      value: '3',
      label: 'rango',
      href: '/reports/rango',
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

   {/* <div>
<select onChange={handle}>
<option value="">Seleccionar opción</option>
{options.map((option) => (
<option key={option.value} value={option.value}>
{option.label}
</option>
))}
</select>
</div>  */}
     </FormGroup> 

  //   <Box sx={{ minWidth: 120 }}>
  //   <FormControl fullWidth>
  //     {/* <InputLabel variant="standard" htmlFor="uncontrolled-native">
  //       Age
  //     </InputLabel> */}
  //     <NativeSelect

  //       onChange={handle}
        
  //       defaultValue={1}
  //       inputProps={{
  //         name: 'age',
  //         id: 'uncontrolled-native',
  //       }}
        
  //     >
  //       {options.map((option) => (
          
  //       <option key={option.value} value={option.value}>
  //       {option.label}
  //       </option>
  //       ))}
  //       {/* <option value={10}>Ten</option> */}
  //       {/* <option value={20}>Twenty</option> */}
  //       {/* <option value={30}>Thirty</option> */}
  //     </NativeSelect>
  //   </FormControl>
  // </Box>
    

   

  )
}

export default Select;
