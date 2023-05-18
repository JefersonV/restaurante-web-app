using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restaurante_web_app.Models;

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;

using System;
using System.IO;
using System.Linq;
using iTextSharp.text.pdf.draw;
using System.Drawing;
using System.Data;
using System.Globalization;

namespace restaurante_web_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class pdfController : ControllerBase
    {
        public readonly GoeatContext _dbContext;

        public pdfController(GoeatContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        //[HttpGet("generate-pdf")]
        public IActionResult GeneratePdf([FromQuery] DateTime date)
        {

            string openSansFontPath = "https://fonts.googleapis.com/css?family=Roboto"; // Reemplaza con la ruta real del archivo de fuente


            DateOnly selectedDate = DateOnly.FromDateTime(date);

            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            //DateOnly today = DateOnly.FromDateTime(new DateTime(2023, 3, 3));

            
            // Obtener el nombre del usuario
            //string userId = User.Identity.Name; // Obtén el nombre del usuario autenticado
            //var usuario = _dbContext.Usuarios.FirstOrDefault(u => u.Usuario1 == userId);
            //string nombreUsuario = usuario != null ? usuario.Usuario1 : "Nombre de usuario desconocido";


            // Obtener los datos de las tablas de la base de datos solo para hoy
            var data = from c in _dbContext.Clientes
                       join v in _dbContext.Ventas on c.IdCliente equals v.IdCliente
                       join d in _dbContext.DetalleVenta on v.IdVenta equals d.IdVenta
                       join m in _dbContext.Menus on d.IdPlatillo equals m.IdPlatillo
                       where v.Fecha == selectedDate
                       select new
                       {
                           v.IdVenta,
                           v.NumeroComanda,
                           v.Fecha,
                           v.Total,
                           v.IdMesero,
                           c.IdCliente,
                           c.NombreApellido,
                           c.Institucion,
                           d.Cantidad,
                           d.Subtotal,
                           m.Platillo,
                           m.Precio
                       };

            decimal totalVentas = data.Sum(item => item.Subtotal ?? 0);



            // Group the data by customer
            //var groupedData = data.GroupBy(item => item.IdCliente);




            // Group the data by customer
            var groupedData = data.GroupBy(item => item.IdCliente);

            //suma de hoy
            //decimal totalVentas = groupedData.Sum(group => group.Sum(item => item.Subtotal ?? 0));


            // Create a PDF document
            Document document = new Document();

            // Create a MemoryStream to save the PDF file
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);


            // Add metadata
            document.AddTitle("Facturas de ventas");
            document.AddSubject("Facturas de ventas de la base de datos");
            document.AddKeywords("ventas, base de datos, PDF");
            document.AddCreator("Mi aplicación");

            // Definir el estilo de fuente predeterminado
            //FontFactory.Register(openSansFontPath, "Roboto Mono");

            // Open the PDF document
            document.Open();

            // Crear el encabezado
            PdfPTable headerTable = new PdfPTable(3);
            headerTable.WidthPercentage = 100;

            // Agregar la imagen al encabezado
            string imagePath = "https://umg.edu.gt/miumg/sesion_files/logo_white.png"; // Reemplaza con la ruta de tu imagen
            Image image = Image.GetInstance(imagePath);
            image.ScaleAbsolute(100, 100); // Ajustar el tamaño de la imagen
            PdfPCell imageCell = new PdfPCell(image);
            imageCell.HorizontalAlignment = Element.ALIGN_LEFT;
            imageCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            headerTable.AddCell(imageCell);

            // Agregar el título al encabezado
            //PdfPCell titleCell = new PdfPCell(new Phrase("Título del documento"));
            PdfPCell titleCell = new PdfPCell(new Phrase("Cafe y Restaurante La Centenaria", new Font(Font.FontFamily.COURIER, 16, Font.BOLD))); 
            titleCell.HorizontalAlignment = Element.ALIGN_CENTER;
            titleCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            titleCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            headerTable.AddCell(titleCell);


            // Obtener la fecha de hoy
            DateTime daynow = DateTime.Now;
            string fechaHoy = daynow.ToString();

            // Agregar la descripción al encabezado
            PdfPCell descriptionCell = new PdfPCell(new Phrase($"Nombre: {nombreUsuario}\nReporte Diario de Ventas\n\nFecha de Impresión:\n{fechaHoy}", new Font(Font.FontFamily.COURIER, 10, Font.NORMAL)));
            descriptionCell.HorizontalAlignment = Element.ALIGN_CENTER;
            descriptionCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            descriptionCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            headerTable.AddCell(descriptionCell);

            // Agregar el encabezado al documento
            document.Add(headerTable);

            // Loop through the groups and add a new page for each customer
            foreach (var group in groupedData)
            {

                // Linea separador
                LineSeparator line = new LineSeparator();
                line.LineColor = BaseColor.BLACK; // Color de la línea
                line.LineWidth = 1f; // Ancho de la línea
                // Agregar la línea al documento
                document.Add(new Chunk(line));

                // Add the customer details to the page
                Paragraph customer = new Paragraph($"Cliente: {group.First().NombreApellido} \n" +
                                                   $"Intitución: {group.First().Institucion} \n" +
                                                   $"Fecha de Registro: {group.First().Fecha} \n\n", new Font(Font.FontFamily.COURIER, 12, Font.BOLD));
                customer.Alignment = Element.ALIGN_LEFT;
                document.Add(customer);



                // Add the sale items
                PdfPTable table = new PdfPTable(4);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 1f, 1f, 1f, 1f });
                table.DefaultCell.Padding = 5;
                table.DefaultCell.BackgroundColor = new BaseColor(240, 240, 240);

                // Fuente para el encabezado (negrita)
                Font headerFont = new Font(Font.FontFamily.COURIER, 12, Font.BOLD);

                // Encabezado de la tabla en negrita
                PdfPCell headerCell = new PdfPCell(new Phrase("Platillo", headerFont));
                table.AddCell(headerCell);
                headerCell = new PdfPCell(new Phrase("Cantidad", headerFont));
                table.AddCell(headerCell);
                headerCell = new PdfPCell(new Phrase("Precio", headerFont));
                table.AddCell(headerCell);
                headerCell = new PdfPCell(new Phrase("Subtotal", headerFont));
                table.AddCell(headerCell);

                // Fuente para el contenido de la tabla (normal)
                Font contentFont = new Font(Font.FontFamily.COURIER, 12, Font.NORMAL);

                foreach (var item in group)
                {
                    table.AddCell(new PdfPCell(new Phrase(item.Platillo, contentFont)));
                    table.AddCell(new PdfPCell(new Phrase(item.Cantidad.ToString(), contentFont)));
                    table.AddCell(new PdfPCell(new Phrase(item.Precio.ToString(), contentFont)));
                    table.AddCell(new PdfPCell(new Phrase(item.Subtotal.ToString(), contentFont)));
                }

                // Add the table to the document
                document.Add(table);

                // Add the total
                Paragraph total = new Paragraph($"Total: {group.Sum(item => item.Subtotal).ToString()}", contentFont);
                total.Alignment = Element.ALIGN_RIGHT;
                document.Add(total);




            }

            //linea separador
            LineSeparator line3 = new LineSeparator();
            line3.LineColor = BaseColor.BLUE; // Color de la línea
            line3.LineWidth = 1f; // Ancho de la línea
                                  // Agregar la línea al documento
            document.Add(new Chunk(line3));

            // Obtener el nombre del día en español
            CultureInfo spanishCulture = new CultureInfo("es-ES");
            string dayOfWeek = spanishCulture.DateTimeFormat.GetDayName(selectedDate.DayOfWeek);

            // Add the total sales for the day
            Paragraph totalSales = new Paragraph($"Total de ventas del día {dayOfWeek}: {totalVentas.ToString("C")}", new Font(Font.FontFamily.COURIER, 14, Font.BOLD));
            totalSales.Alignment = Element.ALIGN_RIGHT;
            document.Add(totalSales);

            // Close the document
            document.Close();

            // Convert the PDF document to an array of bytes
            byte[] bytes = memoryStream.ToArray();

            // Prepare the HTTP response
            //var result = new FileContentResult(bytes, "application/pdf");

            //// Liberar recursos
            //writer.Close();
            //memoryStream.Close();
            //memoryStream.Dispose();

            return File(bytes, "application/pdf"); ;


        }
    }
}