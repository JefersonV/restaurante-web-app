import React from 'react'
/* icons */
import { AiFillHome } from "react-icons/ai"
import { FaCashRegister, FaCartPlus,FaUserCog, FaHouseUser } from "react-icons/fa"
import { MdOutlineInventory } from "react-icons/md"
import { TbBrandShopee } from "react-icons/tb"
import { VscGraph } from "react-icons/vsc"
import { GrConfigure } from "react-icons/gr"
import { BiFoodMenu } from "react-icons/bi"
import { FiTruck } from "react-icons/fi"
import { RiArrowDownSFill } from 'react-icons/ri'
import { RiArrowUpSFill } from 'react-icons/ri'
/* Arrows */
import { RiArrowUpSLine, RiArrowDownSLine } from "react-icons/ri"
/* 
  MÓDULOS
  Dashboard
  Inventario (sub módulos --> compras ventas)
  caja
  Usuarios
  Reportes
  Config (sub módulos --> menu, proveedores clientes)
  */

export const SidebarData = [
  {
    title: "Inicio",
    path: "/",
    icon: <AiFillHome />,
  },
  {
    title: "Inventario",
    path: "/inventory",
    icon: <MdOutlineInventory />,
    iconClosed: <RiArrowDownSFill />,
    iconOpened: <RiArrowUpSFill />,
    subNav: [
      {
        title: "Ventas",
        path: "/sales",
        icon: <FaCartPlus/>,
        class: "nav-subItem",
      },
      {
        title: "Compras",
        path: "/purchases",
        icon: <TbBrandShopee/>,
        class: "nav-subItem",
      }
    ]
  },
  {
    title: "Caja",
    path: "/cash-box",
    icon: <FaCashRegister />,
  },
  {
    title: "Usuarios",
    path: "/users",
    icon: <FaUserCog />,
  },
  {
    title: "Reportes",
    path: "/reports",
    icon: <VscGraph />,
  },
  {
    title: "Configuración",
    path: "/config",
    icon: <GrConfigure />,
    iconClosed: <RiArrowDownSFill />,
    iconOpened: <RiArrowUpSFill />,
    subNav: [
      {
        title: "Menú",
        path: "/menu",
        icon: <BiFoodMenu />,
        class: "nav-subItem",
      },
      {
        title: "Proveedores",
        path: "/providers",
        icon: <FiTruck />,
        class: "nav-subItem",
      },
      {
        title: "Clientes",
        path: "/customers",
        icon: <FaHouseUser />,
        class: "nav-subItem",
      },
      {
        title: "Meseros",
        path: "/waiters",
        icon: <FaHouseUser />,
        class: "nav-subItem",
      },
    ]
  }
];