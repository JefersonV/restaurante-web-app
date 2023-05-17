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
        public IActionResult GeneratePdf()
        {

            string openSansFontPath = "https://fonts.googleapis.com/css?family=Roboto"; // Reemplaza con la ruta real del archivo de fuente
            
            // Obtener la fecha actual
            var fechaActual = DateTime.Today;

            // Obtener la fecha actual en PostgreSQL
            //string currentDate = "CURRENT_DATE";
            string currentDate = "CURRENT_DATE";

            DateTime dateTime = DateTime.Now; // Tu objeto DateTime en .NET

            
            //DateTime today = DateTime.Now;

            //DateTime today = DateTime.Today;
            //DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            DateOnly today = DateOnly.FromDateTime(new DateTime(2023, 3, 3)); //2023 - 03 - 03

            // Obtener los datos de las tablas de la base de datos solo para hoy
            var data = from c in _dbContext.Clientes
                       join v in _dbContext.Ventas on c.IdCliente equals v.IdCliente
                       join d in _dbContext.DetalleVenta on v.IdVenta equals d.IdVenta
                       join m in _dbContext.Menus on d.IdPlatillo equals m.IdPlatillo
                       //where v.Fecha == dataOnly.
                       //where v.Fecha == today
                       //where v.Fecha.Year == today.Year && v.Fecha.Month == today.Month && v.Fecha.Day == today.Day
                       where v.Fecha == today
                       //v.Fecha.DateOnly.ToString() == currentDate
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
            FontFactory.Register(openSansFontPath, "Roboto Mono");

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
            PdfPCell titleCell = new PdfPCell(new Phrase("Título del documento"));
            titleCell.HorizontalAlignment = Element.ALIGN_CENTER;
            titleCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            titleCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            headerTable.AddCell(titleCell);



            // Agregar la descripción al encabezado
            PdfPCell descriptionCell = new PdfPCell(new Phrase("Descripción del documento"));
            descriptionCell.HorizontalAlignment = Element.ALIGN_CENTER;
            descriptionCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            descriptionCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            headerTable.AddCell(descriptionCell);

            // Agregar el encabezado al documento
            document.Add(headerTable);




            // Loop through the groups and add a new page for each customer
            foreach (var group in groupedData)
            {

                // Create a new page
                //document.NewPage();

                // Add the customer details to the page
                //Paragraph customer = new Paragraph($"Cliente: {group.Key}");
                //customer.Alignment = Element.ALIGN_LEFT;
                //document.Add(customer);
                LineSeparator line = new LineSeparator();
                line.LineColor = BaseColor.RED; // Color de la línea
                line.LineWidth = 1f; // Ancho de la línea
                // Agregar la línea al documento
                document.Add(new Chunk(line));

                // Add the customer details to the page
                Paragraph customer = new Paragraph($"Cliente: {group.Key}, {group.First().Institucion}, {group.First().NombreApellido}, {group.First().Fecha}");
                customer.Alignment = Element.ALIGN_LEFT;
                document.Add(customer);

                LineSeparator line2 = new LineSeparator();
                line2.LineColor = BaseColor.BLACK; // Color de la línea
                line2.LineWidth = 1f; // Ancho de la línea
                // Agregar la línea al documento
                document.Add(new Chunk(line2));


                // Add the sale items
                PdfPTable table = new PdfPTable(4);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 1f, 1f, 1f, 1f });
                table.DefaultCell.Padding = 5;
                table.DefaultCell.BackgroundColor = new BaseColor(240, 240, 240);
                table.AddCell("Platillo");
                table.AddCell("Cantidad");
                table.AddCell("Precio");
                table.AddCell("Subtotal");

                foreach (var item in group)
                {
                    table.AddCell(item.Platillo);
                    table.AddCell(item.Cantidad.ToString());
                    table.AddCell(item.Precio.ToString());
                    table.AddCell(item.Subtotal.ToString());
                }

                // Add the table to the document
                document.Add(table);

                // Add the total
                Paragraph total = new Paragraph($"Total: {group.Sum(item => item.Subtotal).ToString()}");
                total.Alignment = Element.ALIGN_RIGHT;
                document.Add(total);

                


            }

            LineSeparator line3 = new LineSeparator();
                line3.LineColor = BaseColor.BLUE; // Color de la línea
                line3.LineWidth = 1f; // Ancho de la línea
                // Agregar la línea al documento
                document.Add(new Chunk(line3));

            //// Add the total sales for the day
            //Paragraph totalSales = new Paragraph($"Total de ventas del día: {groupedData.Sum(group => group.Sum(item => item.Total)).ToString()}");
            //totalSales.Alignment = Element.ALIGN_RIGHT;
            //document.Add(totalSales);

            // Add the total sales for the day
            Paragraph totalSales = new Paragraph($"Total de ventas del día: {totalVentas.ToString("C")}");
            totalSales.Alignment = Element.ALIGN_RIGHT;
            document.Add(totalSales);

            // Close the document
            document.Close();

            // Convert the PDF document to an array of bytes
            byte[] bytes = memoryStream.ToArray();

            // Prepare the HTTP response
            var result = new FileContentResult(bytes, "application/pdf");

            // Liberar recursos
            writer.Close();
            memoryStream.Close();
            memoryStream.Dispose();

            return result;


        }
        }
    }

