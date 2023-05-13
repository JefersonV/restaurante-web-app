import { motion } from 'framer-motion';
import React, { forwardRef } from 'react';

// Forward ref -> sirve para pasar referencias a el componente padre
export const SearchBarContainer = forwardRef(({ isExpanded, containerVariants, containerTransition, children }, ref) => {
  return (
    <motion.div
      ref={ref}
      style={{
        display: "flex",
        flexDirection: "column",
        width: "34em",
        height: "3.6em",
        backgroundColor: "#fff",
        borderRadius: "6px",
        boxShadow: "0px 2px 12px 3px rgba(0, 0, 0, 0.14)"
      }}
    >
      {/* Contenido del motion */}
      {children}
    </motion.div>
  );
});
