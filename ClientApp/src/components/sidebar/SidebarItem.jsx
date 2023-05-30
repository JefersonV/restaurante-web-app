import React from 'react'
import {useState} from 'react'
import { Link } from 'react-router-dom'
// import { useSidebarOpenContext } from '../../providers/GlobalProvider'
import { useStore, useSubItem } from '../../providers/GlobalProvider';
function SidebarItem(props) {
  const isOpen = useStore()
  /* item -> prop para recorrer la data ... isOpen state de la sidebar (true / false) */
  const { item } = props
  /*subNav state para desplegar las opciones de la subnav  */
  const [subNav, setSubNav] = useState(false)
  const showSubNav = () => setSubNav(!subNav)
  return (
    <>
      {/* Menú  */}
      {/* Items */}
      <Link to={item.path} className="link" activeclassname="active" onClick={item.subNav && showSubNav} title={item.title}>
        <div className="icon">{item.icon}</div>
        <span style={{display: isOpen ? "block" : "none"}} className="link_text">{item.title}</span>
        <div className="sidebar-arrow">
          {/* Si existe un item subnav (item.subNav) 
            Y si subNav (state === true) */}
          {item.subNav && subNav
            ? item.iconOpened
            : item.subNav
            ? item.iconClosed
            : null
          }
        </div>
      </Link>
      
      {/* Sub items */}
      {subNav && item.subNav.map((item, index) => {
        return(

          <Link to={item.path} key={index} className="nav-subItem link" >
            <div className="icon">{item.icon}</div>
            <span className="link_text" style={{display: subNav && isOpen ? "block" : "none"}}>{item.title}</span>
          </Link>
        )
      })}
    </>
  )
}

export default SidebarItem