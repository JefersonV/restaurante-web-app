// vite.config.js
import { defineConfig } from "file:///C:/Users/jeffe/source/repos/restaurante-web-app/restaurante-web-app/ClientApp/node_modules/vite/dist/node/index.js";
import react from "file:///C:/Users/jeffe/source/repos/restaurante-web-app/restaurante-web-app/ClientApp/node_modules/@vitejs/plugin-react/dist/index.mjs";
var vite_config_default = defineConfig({
  plugins: [react()],
  server: {
    /* Proxy dentro del servidor http servirá todo lo que esté dentro de api */
    proxy: {
      "/api": {
        target: "http://localhost:5188"
      }
    }
  }
});
export {
  vite_config_default as default
};
//# sourceMappingURL=data:application/json;base64,ewogICJ2ZXJzaW9uIjogMywKICAic291cmNlcyI6IFsidml0ZS5jb25maWcuanMiXSwKICAic291cmNlc0NvbnRlbnQiOiBbImNvbnN0IF9fdml0ZV9pbmplY3RlZF9vcmlnaW5hbF9kaXJuYW1lID0gXCJDOlxcXFxVc2Vyc1xcXFxqZWZmZVxcXFxzb3VyY2VcXFxccmVwb3NcXFxccmVzdGF1cmFudGUtd2ViLWFwcFxcXFxyZXN0YXVyYW50ZS13ZWItYXBwXFxcXENsaWVudEFwcFwiO2NvbnN0IF9fdml0ZV9pbmplY3RlZF9vcmlnaW5hbF9maWxlbmFtZSA9IFwiQzpcXFxcVXNlcnNcXFxcamVmZmVcXFxcc291cmNlXFxcXHJlcG9zXFxcXHJlc3RhdXJhbnRlLXdlYi1hcHBcXFxccmVzdGF1cmFudGUtd2ViLWFwcFxcXFxDbGllbnRBcHBcXFxcdml0ZS5jb25maWcuanNcIjtjb25zdCBfX3ZpdGVfaW5qZWN0ZWRfb3JpZ2luYWxfaW1wb3J0X21ldGFfdXJsID0gXCJmaWxlOi8vL0M6L1VzZXJzL2plZmZlL3NvdXJjZS9yZXBvcy9yZXN0YXVyYW50ZS13ZWItYXBwL3Jlc3RhdXJhbnRlLXdlYi1hcHAvQ2xpZW50QXBwL3ZpdGUuY29uZmlnLmpzXCI7aW1wb3J0IHsgZGVmaW5lQ29uZmlnIH0gZnJvbSAndml0ZSdcbmltcG9ydCByZWFjdCBmcm9tICdAdml0ZWpzL3BsdWdpbi1yZWFjdCdcblxuLy8gaHR0cHM6Ly92aXRlanMuZGV2L2NvbmZpZy9cbmV4cG9ydCBkZWZhdWx0IGRlZmluZUNvbmZpZyh7XG4gIHBsdWdpbnM6IFtyZWFjdCgpXSxcbiAgc2VydmVyOiB7XG4gICAgLyogUHJveHkgZGVudHJvIGRlbCBzZXJ2aWRvciBodHRwIHNlcnZpclx1MDBFMSB0b2RvIGxvIHF1ZSBlc3RcdTAwRTkgZGVudHJvIGRlIGFwaSAqL1xuICAgIHByb3h5OiB7XG4gICAgICAnL2FwaSc6IHtcbiAgICAgICAgdGFyZ2V0OiAnaHR0cDovL2xvY2FsaG9zdDo1MTg4JyxcbiAgICAgIH1cbiAgICB9XG4gIH1cbn0pXG4iXSwKICAibWFwcGluZ3MiOiAiO0FBQXlhLFNBQVMsb0JBQW9CO0FBQ3RjLE9BQU8sV0FBVztBQUdsQixJQUFPLHNCQUFRLGFBQWE7QUFBQSxFQUMxQixTQUFTLENBQUMsTUFBTSxDQUFDO0FBQUEsRUFDakIsUUFBUTtBQUFBO0FBQUEsSUFFTixPQUFPO0FBQUEsTUFDTCxRQUFRO0FBQUEsUUFDTixRQUFRO0FBQUEsTUFDVjtBQUFBLElBQ0Y7QUFBQSxFQUNGO0FBQ0YsQ0FBQzsiLAogICJuYW1lcyI6IFtdCn0K
