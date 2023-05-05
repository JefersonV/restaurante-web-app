import jsPDF from 'jspdf';  
import autoTable from 'jspdf-autotable'
const generatePDF = (dato) => {
      // crea un nuevo objeto `Date`
  var today = new Date();
 
  // obtener la fecha y la hora
  var now = today.toLocaleString();
  console.log(now);
  /*
      Resultado: 1/27/2020, 9:30:00 PM
  */
 // Obtiene los datos de la base de datos
 
 // Crea un nuevo objeto PDF
 const doc = new jsPDF();

 // Agrega un encabezado al documento
 doc.text("Listado de proveedores", 10, 10);

 // Define las columnas de la tabla
 const columns = ["IDPROVEEDOR","NOMBRE", "TELEFONO"];
     

 // Define las filas de la tabla
 const rows = [];
     
 // Itera sobre los datos de la tabla y agrega cada fila a la matriz "rows"
 dato.forEach((item) => {
   const rowData = [
     item.idProveedor,
     item.nombre,
     item.telefono
   ];
   rows.push(rowData);
 });
 
  autoTable(doc, {
    head: [columns],
    body: rows,
    
  })
 
   // Guarda el archivo PDF
   // doc.save('data.pdf');
   doc.save(`Reporte_${now}.pdf`);  

}

export default generatePDF;