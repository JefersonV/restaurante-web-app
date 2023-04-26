import React, { useState, useRef, useEffect } from 'react';
import "../styles/Searchbar.scss"
import Swal from 'sweetalert2'

const SearchBarDrop = ({ options, onInputChange, onSubmitCustomer, onSubmitClient }) => {
  const ulRef = useRef();
  const inputRef = useRef();
  useEffect(() => {
    //Evento para el buscador
    inputRef.current.addEventListener('click', (e) => {
      e.stopPropagation();
      ulRef.current.style.display = 'flex';
      onInputChange(e);
    });
    //Evento para que se desaparezca la lista de botones, al hacer click por fuera del elemento
    document.addEventListener('click', (event) => {
      ulRef.current.style.display = 'none';
      // ulRef.current.style={display: 'none'}
    });
  }, []);
  
 
  return (
    <div className="search-bar-dropdown">

    {/* <FcIcons.FcSearch/> */}
    <div className="input-group mb-3">
      <input 
        id="search-bar" 
        type="search" 
        className="form-control" 
        placeholder="Nombre del cliente ..."
        aria-label="Rec safdasf"
        aria-describedby="basic-addon2" 
        ref={inputRef}
        onChange={onInputChange}
        autoComplete="off"
        
      />
      <div className="input-group-append">
        <input type="submit" value="Aceptar" form="CustomerForm" className="btn btn-outline-primary" id="customer-btn"
        />
      </div>
    </div>
      {/* <input type="submit" value="Elegir cliente"  form="CustomerForm" className="btn btn-primary"/> */}
      <form id="CustomerForm" className="list-group" ref={ulRef} onSubmit={onSubmitCustomer}>
        {options.map((option, index) => {
          return (
            <>
              <button
                //Name sirve para saber que número es el que está selccionando
                key={option.id}
                name={index}
                type="button"
                onClick={(e) => {
                  // Aparece el nombre en el value del botón
                  inputRef.current.value = option;
                }}
                className="list-group-item list-group-item-action"
                id="buttons-actions"
                >
                {option}
              </button>
            </>
          );
        })}
      </form>
    </div>
  );
};

export default SearchBarDrop