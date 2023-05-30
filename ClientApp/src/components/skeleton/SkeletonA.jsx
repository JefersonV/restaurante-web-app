import React from 'react'

function SkeletonA() {
  const Skeleton = () => {
    return (
    <motion.div
      style={{
        width: "200px",
        height: "20px",
        backgroundColor: "#e0e0e0",
        borderRadius: "4px",
        marginBottom: "10px",
      }}
      animate={{ opacity: [1, 0.5, 1] }}
      transition={{ duration: 1.5, repeat: Infinity }}
    />
    );
    };
}

export default SkeletonA