import React, { useState } from 'react'
import { SidebarData } from './SidebarData'
import { FaBars } from "react-icons/fa"
import { Link } from 'react-router-dom';
import SidebarItem from './SidebarItem';
import LogoRestaurant from '../../assets/images/la centenaria.png'
/* Provider (estado global)*/
import { useStore, useSubItem } from '../../providers/GlobalProvider';
import '../../styles/App.scss'


function Sidebar(props) {
  const { children, pageTitle } = props
  /* Estado global de la sidebar */
  const isOpen = useStore((state) => state.sidebar)
  const toggle = useStore((state) => state.showSidebar)
  const subNav = useSubItem((state) => state.subNav)
 
  console.log(isOpen)

  return (
    <>
      <header className="header">
        <div className= {isOpen ? "nav-icon-open nav-icon bars" : "nav-icon-close nav-icon bars"}>
          <FaBars onClick={toggle} />
          <h3 className="page-title">{pageTitle}</h3>
        </div>
        <div className="header-info-user">
            <span>Carmen</span>
            <span>Admin</span>
          </div>
        <div>
          
          <img className="header-avatar" src="https://b2472105.smushcdn.com/2472105/wp-content/uploads/2022/11/10-Poses-para-foto-de-Perfil-Profesional-Mujer-04-2022-7-819x1024.jpg?lossy=1&strip=1&webp=1" width="50px" height="50px" alt="Perfil del usuario" />
        </div>
      </header>
      <div className="container-sidebar">
        <aside className= {isOpen ? "sidebar sidebarOpen" : "sidebar sidebarClose" }>

          <section className="top_section">
              {/* Sidebar completa o incompleta  */}
              {/* isOpen === true -> se mostrará el h1 por defecto, de lo contrario se oculta */}
              <h1 style={{display: isOpen ? "block" : "none"}} className="logo">Logo</h1>
          </section>
          {/* Items de la sidebar */}
          {SidebarData.map((item, index)=> {
            // return <SidebarItem item ={item} isOpen={isOpen} key={index} />;
            return <SidebarItem 
                    item ={item} 
                    key={index} />;
          })}

        </aside>
        {/* Componentes */}
      <main>{children}</main>
      </div>
    </>
  )
}

export default Sidebar