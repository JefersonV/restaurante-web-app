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

const  Selectmonthbox = (props) => {
  const {  sele } = props;


  const navigate = useNavigate();
  // const [selectedOption, setSelectedOption] = useState('Resumen de ventas de hoy');
  const [selectedOption, setSelectedOption] = useState('');




  const options = [

    {
      value: 'enero',
      label: 'Enero',

    },
    {
      value: 'febrero',
      label: 'Febrero',
      
    },
    {
      value: 'marzo',
      label: 'Marzo',
    },
    {
      value: 'Abril',
      label: 'Abril',
    },
    {
      value: 'Mayo',
      label: 'Mayo',
    },
    {
      value: 'Junio',
      label: 'Junio',
    },
    {
      value: 'Julio',
      label: 'Julio',
    },
    {
      value: 'Agosto',
      label: 'Agosto',
    },
    {
      value: 'Septiembre',
      label: 'Septiembre',
    },
    {
      value: 'Octubre',
      label: 'Octubre',
    },
    {
      value: 'Noviembre',
      label: 'Noviembre',
    },
    {
      value: 'Diciembre',
      label: 'Diciembre',
    },
    // {
    //   value: '1',
    //   label: 'Enero',

    // },
    // {
    //   value: '2',
    //   label: 'Febrero',
      
    // },
    // {
    //   value: '3',
    //   label: 'Marzo',
    // },
    // {
    //   value: '4',
    //   label: 'Abril',
    // },
    // {
    //   value: '5',
    //   label: 'Mayo',
    // },
    // {
    //   value: '6',
    //   label: 'Junio',
    // },
    // {
    //   value: '7',
    //   label: 'Julio',
    // },
    // {
    //   value: '8',
    //   label: 'Agosto',
    // },
    // {
    //   value: '9',
    //   label: 'Septiembre',
    // },
    // {
    //   value: '10',
    //   label: 'Octubre',
    // },
    // {
    //   value: '11',
    //   label: 'Noviembre',
    // },
    // {
    //   value: '12',
    //   label: 'Diciembre',
    // },
    
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
    //  className=" text-white   rounded-3"

        onChange={sele} 
        id="exampleSelect"
        name="select"
        type="select">
     <option value="">Seleccionar mes</option>
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

export default Selectmonthbox;
