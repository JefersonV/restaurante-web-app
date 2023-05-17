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
        public IActionResult GeneratePdf()
        {

            

            DateTime today = DateTime.Today;
            DateTime weekStart = today.AddDays(-(today.DayOfWeek - DayOfWeek.Monday + 7) % 7);
            DateTime weekEnd = weekStart.AddDays(6);

            // Convert to DateOnly
            DateOnly weekStartDate = DateOnly.FromDateTime(weekStart);
            DateOnly weekEndDate = DateOnly.FromDateTime(weekEnd);

            // Get the data from the database for the current week
            var data = from c in _dbContext.Clientes
                       join v in _dbContext.Ventas on c.IdCliente equals v.IdCliente
                       join d in _dbContext.DetalleVenta on v.IdVenta equals d.IdVenta
                       join m in _dbContext.Menus on d.IdPlatillo equals m.IdPlatillo
                       where v.Fecha >= weekStartDate && v.Fecha < weekEndDate
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
            var groupedData = data.GroupBy(item => item.IdCliente);

            // Calculate the total sales for the week
            //decimal totalVentas = groupedData.Sum(group => group.Sum(item => item.Subtotal));

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

            // Define the default font style
            //FontFactory.Register(openSansFontPath, "Roboto Mono");

            // Open the PDF document
            document.Open();

            // Loop through the groups and add a new page for each customer
            foreach (var group in groupedData)
            {
                // Add the customer details to the page
                Paragraph customer = new Paragraph($"Cliente: {group.Key}, {group.First().Institucion}, {group.First().NombreApellido}, {group.First().Fecha}");
                customer.Alignment = Element.ALIGN_LEFT;
                document.Add(customer);
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

            // Add the total sales for the week
            Paragraph totalSales = new Paragraph($"Total de ventas de la semana: {totalVentas.ToString("C")}");
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

