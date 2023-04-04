import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    /* Proxy dentro del servidor http servirá todo lo que esté dentro de api */
    proxy: {
      '/api': {
        target: 'http://localhost:5188',
      }
    }
  }
})
