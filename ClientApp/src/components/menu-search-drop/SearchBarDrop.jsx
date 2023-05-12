import React, { useState, useEffect, useRef } from 'react'
import { IoClose, IoSearch } from "react-icons/io5";
import { AnimatePresence, motion } from 'framer-motion'
import { MoonLoader } from 'react-spinners';
import '../../styles/Searchbar.scss'
import DataMenuDrop from './DataMenuDrop';
import { SearchBarContainer } from './SearchBarContainer';
import CloseIcon from './CloseIcon';
import { useDebounce } from '../../Hooks/UseDebounce';
import { useClickOutside } from '../../Hooks/UseClickOutside';

function SearchBarDrop(props) {
  // Expande u oculta el componente DataMenuDrop
  const [isExpanded, setExpanded] = useState(false);
  // hook que detecta si se dió click por fuera de DataMenuDrop
  const [parentRef, isClickedOutside] = useClickOutside();
  // Referencia para limpiar el input de búsqueda
  const inputRef = useRef();
  // Valor capturado del input
  const [searchQuery, setSearchQuery] = useState("");
  const [isLoading, setLoading] = useState(false);
  
  const isEmpty = !props.menuState || props.menuState.length === 0;

  const changeHandler = (e) => {
    e.preventDefault();
    // Si el elemento está vacío o solo contiene espacios en blanco
    if (e.target.value.trim() === "") props.setNoDataMenu(false);

    setSearchQuery(e.target.value);
    console.log('Search Query: ', typeof e.target.value)
  };

  const expandContainer = () => {
    setExpanded(true);
  };

  const collapseContainer = () => {
    setExpanded(false);
    setSearchQuery("");
    setLoading(false);
    props.setNoDataMenu(false)
    // setMenu([]);
    if (inputRef.current) inputRef.current.value = "";
  };

  useEffect(() => {
    // Si se le da por fuera del SearchContent
    if (isClickedOutside) collapseContainer();
  }, [isClickedOutside]);

  // Filtrado
  const filtradoMenu = !searchQuery ? props.menuState
  // Si se ha ingresado información al input, que la compare a los criterios y los filtre
  : props.menuState.filter((item) =>
    item.name.toLowerCase().includes(searchQuery.toLocaleLowerCase() ||
    parseInt(item.index).toLocaleLowerCase().includes(parseInt(searchQuery).toLocaleLowerCase()))
  )

  
  /* Framer motion */
  const containerVariants = {
    expanded: {
      height: "30em",
    },
    collapsed: {
      height: "3.8em",
    },
  };

  const containerTransition = { type: "spring", damping: 22, stiffness: 150 };

  // useDebounce(searchQuery, 500, showMenu)
  return (
    <>
      <SearchBarContainer
        animate={isExpanded ? "expanded" : "collapsed"}
        variants={containerVariants}
        transition={containerTransition}
        ref={parentRef}
      >
        <div className="SearchInputContainer">
          <span className="SearchIcon">
            <IoSearch />
          </span>
          <input 
            type="text" 
            className="SearchInput" 
            onFocus={expandContainer}
            ref={inputRef}
            onChange={changeHandler}
            value={searchQuery}
            placeholder="Nombre o # de platillo del menú"
          />
          <AnimatePresence>
            {isExpanded && (
            <div className="CloseIcon" onClick={collapseContainer}>
              <IoClose />
            </div>

            )}
          </AnimatePresence>
        </div>
        {isExpanded && (
          <div className="SearchContent">

            {isEmpty && !props.noDataMenu && (
              <div className="LoadingWrapper">
                <span className="WarningMessage">Escribe el nombre del item</span>
              </div>
            )}
            {props.noDataMenu && (
              <div className="LoadingWrapper">
                <span className="WarningMessage">No se encontró el item</span>
              </div>
            )}
            {!isEmpty && (
              <>
                {/* Componente dropdown de búsqueda */}
                {filtradoMenu.map((item, index) => 
                  <DataMenuDrop
                    itemSelected={props.itemSelected}
                    setItemSelectedList={props.setItemSelectedList}
                    key={item.id}
                    menuName={item.name}
                    index={index}
                    id={item.id}
                  />
                )}
              </>
            )}
          </div>
        )}
      </SearchBarContainer>
    </>
  )
}

export default SearchBarDrop