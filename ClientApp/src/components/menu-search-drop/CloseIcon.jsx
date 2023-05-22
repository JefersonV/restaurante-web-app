import React from 'react'
import { motion } from 'framer-motion'
function CloseIcon({children}) {
  return (
    <motion.span
      style={{
        color: "#bebebe",
        fontSize: "27px",
        verticalAlign: "middle",
        transition: "all 200ms ease-in-out",
        cursor: "pointer",
        /* Es necesario agregar esta propiedad para que se propague el evento */
        pointerEvents: "none",
        "&:hover": {
          color: "#dfdfdf"
        }
      }}
    >
      {children}
    </motion.span>
  )
}

export default CloseIcon