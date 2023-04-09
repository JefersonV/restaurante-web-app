import React, { useState, useEffect } from 'react'
import { useStore } from '../providers/GlobalProvider'
import DatePicker from '../components/DatePicker';
import Searchbar from '../components/Searchbar';
import SelectOption from '../components/SelectOption';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import { Table } from '@mui/material';


function Inventory(props) {
  /* isOpen (globalstate) -> para que el contenido se ajuste según el ancho de la sidebar (navegación) */
  const isOpen = useStore((state) => state.sidebar)
  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Inventario");
  });
  
  return (
    <div className={ isOpen ? "wrapper" : "side" }>
      <Box sx={{ width: '100%'}}>
        <Grid container rowSpacing={1} columnSpacing={{ xs: 1, sm: 2, md: 3 }}>
          <Grid item xs={4}>
            <DatePicker />
          </Grid>
          <Grid item xs={8}>
            <Searchbar />  
          </Grid>
          <Grid item xs={3}>
            <SelectOption />
          </Grid>
          <Grid item xs={12} >
            <Table />
          </Grid>
        </Grid>
      </Box>
    </div>
  )
}

export default Inventory