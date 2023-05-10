using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restaurante_web_app.Models;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;

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
            // Obtener los datos de la tabla Proveedores desde la base de datos
            var proveedores = _dbContext.Proveedores.ToList();

            // Obtener los datos de la tabla Compras desde la base de datos
            var clientes = _dbContext.Clientes.ToList();

            // Crear un documento PDF
            Document document = new Document();

            // Crear un MemoryStream para guardar el archivo PDF
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);


            // Agregar metadatos
            document.AddTitle("Lista de clientes");
            document.AddSubject("Lista de clientes de la base de datos");
            document.AddKeywords("clientes, base de datos, PDF");
            document.AddCreator("Mi aplicación");

            // Abrir el documento PDF
            document.Open();

            // Agregar un título
            Paragraph title = new Paragraph("Lista de proveedores y Cliente");
            title.Alignment = Element.ALIGN_CENTER;
            document.Add(title);

            // Agregar una descripción
            Paragraph description = new Paragraph("A continuación se muestra una lista de los clientes y proveddores almacenados en la base de datos:");
            description.SpacingBefore = 10;
            document.Add(description);

            // Crear una tabla con los datos de proveedores y compras
            PdfPTable table = new PdfPTable(4);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 1f, 1f, 1f, 1f });
            table.DefaultCell.Padding = 5;
            table.DefaultCell.BackgroundColor = new BaseColor(240, 240, 240);
            table.AddCell("Nombre Proveedor");
            table.AddCell("Teléfono");
            table.AddCell("Cliente");
            table.AddCell("Puesto");

            foreach (var proveedor in proveedores)
            {
                foreach (var compra in clientes.Where(c => c.IdCliente == proveedor.IdProveedor))
                {
                    table.AddCell(proveedor.Nombre);
                    table.AddCell(proveedor.Telefono);
                    table.AddCell(compra.NombreApellido);
                    table.AddCell(compra.Puesto);
                }
            }

            // Agregar la tabla al documento
            document.Add(table);

            // Cerrar el documento
            document.Close();


            // Convertir el documento PDF en un arreglo de bytes
            byte[] bytes = memoryStream.ToArray();

            // Preparar la respuesta HTTP
            var result = new FileContentResult(bytes, "application/pdf");

            // Liberar recursos
            writer.Close();
            memoryStream.Close();
            memoryStream.Dispose();

            return result;

        }
    }
}
