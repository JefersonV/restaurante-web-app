import React from 'react'
import Paper from "@mui/material/Paper";
import InputBase from "@mui/material/InputBase";
import Divider from "@mui/material/Divider";
import IconButton from "@mui/material/IconButton";
import SearchIcon from "@mui/icons-material/Search";

function Searchbar() {
  return (
    <div>
      <Paper
      component="form"
      sx={{ p: "2px 4px", display: "flex", alignItems: "center", width: 400 }}
      >
      <IconButton sx={{ p: "10px" }} aria-label="menu">
          {/* <MenuIcon /> */}
      </IconButton>
      <InputBase
          sx={{ ml: 1, flex: 1 }}
          placeholder="Buscar por ..."
          inputProps={{ "aria-label": "search google maps" }}
      />
      <IconButton type="button" sx={{ p: "10px" }} aria-label="search">
          <SearchIcon />
      </IconButton>
      <Divider sx={{ height: 28, m: 0.5 }} orientation="vertical" />
      <IconButton color="primary" sx={{ p: "10px" }} aria-label="directions">
          {/* <DirectionsIcon /> */}
      </IconButton>
      </Paper>
  </div>
  )
}

export default Searchbar