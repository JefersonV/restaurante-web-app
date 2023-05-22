import React, { useState, useEffect } from "react";

export function useDebounce(value, timeout, callback) {
  const [timer, setTimer] = useState(null);

  const clearTimer = () => {
    // si hay un temporizador activo lo elimina
    if (timer) clearTimeout(timer);
  };

  useEffect(() => {
    clearTimer();
    // Value es el valor del input del buscador
    // Callback es la respuesta de la función que hace la petición al server
    if (value && callback) {
      const newTimer = setTimeout(callback, timeout);
      setTimer(newTimer);
    }
  }, [value]);
}
