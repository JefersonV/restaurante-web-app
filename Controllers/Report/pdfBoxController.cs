using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using restaurante_web_app.Models;

using iTextSharp.text;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using iTextSharp;
using iTextSharp.text.pdf.draw;
using System.Drawing;
using System.Data;
using System;
using static iTextSharp.text.pdf.AcroFields;
using Microsoft.EntityFrameworkCore;
using System.IO;
using iText.Kernel.Events;
using Newtonsoft.Json;


namespace restaurante_web_app.Controllers.Report
{
    [Route("api/[controller]")]
    [ApiController]
    public class pdfBoxController : ControllerBase
    {
        public readonly GoeatContext _dbContext;

        public pdfBoxController(GoeatContext dbContext)
        {
            _dbContext = dbContext;
        }

        public class HeaderFooter : PdfPageEventHelper
        {
            public override void OnStartPage(PdfWriter writer, Document document)
            {

                // Crear el encabezado
                PdfPTable headerTable = new PdfPTable(3);
                headerTable.WidthPercentage = 100;

                // Agregar la imagen al encabezado
                string imagePath = "https://res.cloudinary.com/djcaqjlqx/image/upload/v1684894683/Centenario_uhiubm.png"; // Reemplaza con la ruta de tu imagen
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
                PdfPCell descriptionCell = new PdfPCell(new Phrase($"Reporte Diario de Caja\n\nFecha de Impresión:\n{fechaHoy}", new Font(Font.FontFamily.COURIER, 10, Font.NORMAL)));
                descriptionCell.HorizontalAlignment = Element.ALIGN_CENTER;
                descriptionCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                descriptionCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                headerTable.AddCell(descriptionCell);

                // Agregar el encabezado al documento
                document.Add(headerTable);


            }
        }

        public class HeaderFooter1 : PdfPageEventHelper
        {
            public override void OnStartPage(PdfWriter writer, Document document)
            {

                // Crear el encabezado
                PdfPTable headerTable = new PdfPTable(3);
                headerTable.WidthPercentage = 100;

                // Agregar la imagen al encabezado
                string imagePath = "https://res.cloudinary.com/djcaqjlqx/image/upload/v1684894683/Centenario_uhiubm.png"; // Reemplaza con la ruta de tu imagen
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
                PdfPCell descriptionCell = new PdfPCell(new Phrase($"Reporte Semanal de Caja\n\nFecha de Impresión:\n{fechaHoy}", new Font(Font.FontFamily.COURIER, 10, Font.NORMAL)));
                descriptionCell.HorizontalAlignment = Element.ALIGN_CENTER;
                descriptionCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                descriptionCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                headerTable.AddCell(descriptionCell);

                // Agregar el encabezado al documento
                document.Add(headerTable);


            }
        }

        public class HeaderFooter2 : PdfPageEventHelper
        {
            public override void OnStartPage(PdfWriter writer, Document document)
            {

                // Crear el encabezado
                PdfPTable headerTable = new PdfPTable(3);
                headerTable.WidthPercentage = 100;

                // Agregar la imagen al encabezado
                string imagePath = "https://res.cloudinary.com/djcaqjlqx/image/upload/v1684894683/Centenario_uhiubm.png"; // Reemplaza con la ruta de tu imagen
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
                PdfPCell descriptionCell = new PdfPCell(new Phrase($"Reporte Mensual de Caja\n\nFecha de Impresión:\n{fechaHoy}", new Font(Font.FontFamily.COURIER, 10, Font.NORMAL)));
                descriptionCell.HorizontalAlignment = Element.ALIGN_CENTER;
                descriptionCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                descriptionCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                headerTable.AddCell(descriptionCell);

                // Agregar el encabezado al documento
                document.Add(headerTable);


            }
        }

        [HttpGet("boxday")]
        public IActionResult GenerarPDF(DateTime fecha)
        {
            DateOnly currentDate = DateOnly.FromDateTime(fecha);

            CajaDiaria cajaDiaria = _dbContext.CajaDiaria.FirstOrDefault(c => c.Fecha == currentDate);

            if (cajaDiaria == null)
            {
                return NotFound("No se encontró la caja diaria para la fecha especificada.");
            }

            var movimientosCaja = (from mc in _dbContext.MovimientoCajas
                                   where mc.IdCajaDiaria == cajaDiaria.IdCajaDiaria
                                   select new
                                   {
                                       mc.IdMovimiento,
                                       mc.IdTipoMovimiento,
                                       mc.Concepto,
                                       mc.Total,
                                       Ventas = (from vmc in _dbContext.VentaMovimientoCajas
                                                 join v in _dbContext.Ventas on vmc.IdVenta equals v.IdVenta
                                                 where vmc.IdMovimientoCaja == mc.IdMovimiento
                                                 select new
                                                 {
                                                     v.IdVenta,
                                                     v.NumeroComanda,
                                                     v.Fecha,
                                                     v.Total,
                                                     v.IdMesero,
                                                     v.IdCliente
                                                 }).ToList(),
                                       Gastos = (from gmc in _dbContext.GastosMovimientoCajas
                                                 join g in _dbContext.Gastos on gmc.IdGasto equals g.IdGasto
                                                 where gmc.IdMovimientoCaja == mc.IdMovimiento
                                                 select new
                                                 {
                                                     g.IdGasto,
                                                     g.NumeroDocumento,
                                                     g.Fecha,
                                                     g.Concepto,
                                                     g.Total,
                                                     g.IdProveedor
                                                 }).ToList()
                                   }).ToList();

            decimal totalVentas = movimientosCaja.SelectMany(m => m.Ventas).Sum(v => v.Total) ?? 0;
            decimal totalGastos = movimientosCaja.SelectMany(m => m.Gastos).Sum(g => g.Total) ?? 0;

            // Realizar cálculos solo si el estado de la caja es verdadero (true)
            //if (!cajaDiaria.Estado)
            //{
            //    cajaDiaria.SaldoInicial += totalVentas;
            //    //totalGastos -= totalVentas;
            //}
            decimal subtotal = (cajaDiaria.SaldoInicial + totalVentas) ?? 0;
            decimal saldoFinal = ((cajaDiaria.SaldoInicial + totalVentas) - totalGastos) ?? 0;
            
            cajaDiaria.SaldoFinal = saldoFinal;

            var response = new
            {
                CajaDiaria = new
                {
                    cajaDiaria.IdCajaDiaria,
                    cajaDiaria.Fecha,
                    cajaDiaria.SaldoInicial,
                    cajaDiaria.SaldoFinal,
                    cajaDiaria.Estado
                },
                MovimientosCaja = movimientosCaja,
                TotalVentas = totalVentas,
                TotalGastos = totalGastos
            };

            if (movimientosCaja == null)
            {
                return NotFound("No se encontraron datos para la fecha especificada.");
            }

            // Generar el archivo PDF
            Document document = new Document();
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);

            HeaderFooter eventHandler = new HeaderFooter();
            writer.PageEvent = eventHandler;

            document.Open();

            Font headerFont = new Font(Font.FontFamily.COURIER, 12, Font.BOLD);

            // Fuente para el contenido de la tabla (normal)
            Font contentFont = new Font(Font.FontFamily.COURIER, 12, Font.NORMAL);

            // Linea separador
            LineSeparator line = new LineSeparator();
            line.LineColor = BaseColor.BLACK; // Color de la línea
            line.LineWidth = 1f; // Ancho de la línea
                                 // Agregar la línea al documento
            document.Add(new Chunk(line));

            // Crear la tabla principal que contendrá las tablas de ventas y gastos
            PdfPTable mainTable = new PdfPTable(2);
            mainTable.WidthPercentage = 100;
            mainTable.DefaultCell.BackgroundColor = new BaseColor(240, 240, 240);

            // Tabla de ventas
            PdfPTable ventasTable = new PdfPTable(3);
            ventasTable.WidthPercentage = 100;
            //ventasTable.AddCell("Número de Comanda");
            //ventasTable.AddCell("Fecha");
            //ventasTable.AddCell("Total");

            PdfPCell headerCell1 = new PdfPCell(new Phrase("Número de Comanda", headerFont));
            ventasTable.AddCell(headerCell1);
            headerCell1 = new PdfPCell(new Phrase("Fecha", headerFont));
            ventasTable.AddCell(headerCell1);
            headerCell1 = new PdfPCell(new Phrase("Total", headerFont));
            ventasTable.AddCell(headerCell1);

            foreach (var movimiento in movimientosCaja)
            {
                foreach (var venta in movimiento.Ventas)
                {
                    //ventasTable.AddCell(venta.NumeroComanda);
                    //ventasTable.AddCell(venta.Fecha.ToString());
                    //ventasTable.AddCell(venta.Total.ToString());
                    ventasTable.AddCell(new PdfPCell(new Phrase(venta.NumeroComanda, contentFont)));
                    ventasTable.AddCell(new PdfPCell(new Phrase(venta.Fecha.ToString(), contentFont)));
                    ventasTable.AddCell(new PdfPCell(new Phrase(venta.Total.ToString(), contentFont)));
                    //gastosTable.AddCell(new PdfPCell(new Phrase(gasto.Total.ToString(), contentFont)));
                }
            }
            // Fuente para el encabezado (negrita)
           

            // Tabla de gastos
            PdfPTable gastosTable = new PdfPTable(4);
            //gastosTable.WidthPercentage = 100;
            //gastosTable.AddCell("Número de Documento");
            //gastosTable.AddCell("Fecha");
            //gastosTable.AddCell("Concepto");
            //gastosTable.AddCell("Total");
            // Encabezado de la tabla en negrita
            PdfPCell headerCell = new PdfPCell(new Phrase("No.Documento", headerFont));
            gastosTable.AddCell(headerCell);
            headerCell = new PdfPCell(new Phrase("Fecha", headerFont));
            gastosTable.AddCell(headerCell);
            headerCell = new PdfPCell(new Phrase("Concepto", headerFont));
            gastosTable.AddCell(headerCell);
            headerCell = new PdfPCell(new Phrase("Total", headerFont));
            gastosTable.AddCell(headerCell);

            foreach (var movimiento in movimientosCaja)
            {
                foreach (var gasto in movimiento.Gastos)
                {
                    //gastosTable.AddCell(gasto.NumeroDocumento, contentFont);
                    gastosTable.AddCell(new PdfPCell(new Phrase(gasto.NumeroDocumento, contentFont)));
                    gastosTable.AddCell(new PdfPCell(new Phrase(gasto.Fecha.ToString(), contentFont)));
                    gastosTable.AddCell(new PdfPCell(new Phrase(gasto.Concepto, contentFont)));
                    gastosTable.AddCell(new PdfPCell(new Phrase(gasto.Total.ToString(), contentFont)));
                    //gastosTable.AddCell(gasto.Fecha.ToString(), contentFont);
                    //gastosTable.AddCell(gasto.Concepto, contentFont);
                    //gastosTable.AddCell(gasto.Total.ToString());
                }
            }

            // Agregar las tablas a la tabla principal
            mainTable.AddCell(new PdfPCell(ventasTable) { Border = PdfPCell.NO_BORDER });
            mainTable.AddCell(new PdfPCell(gastosTable) { Border = PdfPCell.NO_BORDER });


            document.Add(new Paragraph("Información de Caja: \n" +
                                        "               Ventas                            Gastos \n\n", headerFont));
            document.Add(mainTable);


            document.Add(new Paragraph("+ Saldo Inicial: " + cajaDiaria.SaldoInicial.ToString(), headerFont));
            document.Add(new Paragraph("+ Total Ventas: " + totalVentas.ToString(), headerFont));

            document.Add(new Paragraph("\n+ Subtotal: " + subtotal.ToString(), headerFont));
            document.Add(new Paragraph("- Total Gastos: " + totalGastos.ToString(), headerFont));

            document.Add(new Paragraph("\nSALDO FINAL: " + cajaDiaria.SaldoFinal.ToString(), headerFont));

            // Linea separador
            LineSeparator line3 = new LineSeparator();
            line3.LineColor = BaseColor.BLACK; // Color de la línea
            line3.LineWidth = 1f; // Ancho de la línea
                                 // Agregar la línea al documento
            document.Add(new Chunk(line3));

            document.Close();
            writer.Close();

            byte[] pdfBytes = stream.ToArray();

            // Devolver el archivo PDF
            return File(pdfBytes, "application/pdf");
        }




        //[HttpGet("dailyweek")]
        //public IActionResult weeBoxRepor()
        //{

        //    DayOfWeek currentDayOfWeek = DateTime.Today.DayOfWeek;
        //    int daysUntilStartOfWeek = ((int)currentDayOfWeek - 1 + 7) % 7;
        //    DateTime startOfWeek = DateTime.Today.AddDays(-daysUntilStartOfWeek).Date;
        //    DateTime endOfWeek = startOfWeek.AddDays(7).Date;

        //    DateOnly startOfWeekDateOnly = DateOnly.FromDateTime(startOfWeek);
        //    DateOnly endOfWeekDateOnly = DateOnly.FromDateTime(endOfWeek);

        //    List<CajaDiaria> cajasSemanales = _dbContext.CajaDiaria
        //        .Where(c => c.Fecha >= startOfWeekDateOnly && c.Fecha <= endOfWeekDateOnly)
        //        .ToList();

        //    if (cajasSemanales.Count == 0)
        //    {
        //        return NotFound("No se encontraron cajas diarias para la semana actual.");
        //    }

        //    List<object> response = new List<object>();

        //    foreach (CajaDiaria cajaDiaria in cajasSemanales)
        //    {
        //        var movimientosCaja = (from mc in _dbContext.MovimientoCajas
        //                               where mc.IdCajaDiaria == cajaDiaria.IdCajaDiaria
        //                               select new
        //                               {
        //                                   mc.IdMovimiento,
        //                                   mc.IdTipoMovimiento,
        //                                   mc.Concepto,
        //                                   mc.Total,
        //                                   Ventas = (from vmc in _dbContext.VentaMovimientoCajas
        //                                             join v in _dbContext.Ventas on vmc.IdVenta equals v.IdVenta
        //                                             where vmc.IdMovimientoCaja == mc.IdMovimiento
        //                                             select new
        //                                             {
        //                                                 v.IdVenta,
        //                                                 v.NumeroComanda,
        //                                                 v.Fecha,
        //                                                 v.Total,
        //                                                 v.IdMesero,
        //                                                 v.IdCliente
        //                                             }).ToList(),
        //                                   Gastos = (from gmc in _dbContext.GastosMovimientoCajas
        //                                             join g in _dbContext.Gastos on gmc.IdGasto equals g.IdGasto
        //                                             where gmc.IdMovimientoCaja == mc.IdMovimiento
        //                                             select new
        //                                             {
        //                                                 g.IdGasto,
        //                                                 g.NumeroDocumento,
        //                                                 g.Fecha,
        //                                                 g.Concepto,
        //                                                 g.Total,
        //                                                 g.IdProveedor
        //                                             }).ToList()
        //                               }).ToList();

        //        decimal totalVentas = movimientosCaja.SelectMany(m => m.Ventas).Sum(v => v.Total) ?? 0;
        //        decimal totalGastos = movimientosCaja.SelectMany(m => m.Gastos).Sum(g => g.Total) ?? 0;

        //        decimal saldoFinal = ((cajaDiaria.SaldoInicial + totalVentas) - totalGastos) ?? 0;
        //        cajaDiaria.SaldoFinal = saldoFinal;

        //        decimal ingresoTotalSemana = cajasSemanales.Sum(c => c.SaldoFinal) ?? 0;

        //        var cajaResponse = new
        //        {
        //            CajaDiaria = new
        //            {
        //                cajaDiaria.IdCajaDiaria,
        //                cajaDiaria.Fecha,
        //                cajaDiaria.SaldoInicial,
        //                cajaDiaria.SaldoFinal,
        //                cajaDiaria.Estado
        //            },
        //            MovimientosCaja = movimientosCaja,
        //            TotalVentas = totalVentas,
        //            TotalGastos = totalGastos
        //        };

        //        response.Add(cajaResponse);
        //    }

        //    //decimal totalEentas = response.Sum(item => item.cajaDaria.Estado ?? 0);

        //    //return Ok(response);
        //    // Crear documento PDF
        //    Document document = new Document(PageSize.A4.Rotate());
        //    //Document document = new Document();
        //    MemoryStream memoryStream = new MemoryStream();
        //    PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);



        //    document.Open();

        //    // Crear el encabezado
        //    PdfPTable headerTable = new PdfPTable(3);
        //    headerTable.WidthPercentage = 100;

        //    // Agregar la imagen al encabezado
        //    string imagePath = "https://res.cloudinary.com/djcaqjlqx/image/upload/v1684894683/Centenario_uhiubm.png"; // Reemplaza con la ruta de tu imagen
        //    Image image = Image.GetInstance(imagePath);
        //    image.ScaleAbsolute(100, 100); // Ajustar el tamaño de la imagen
        //    PdfPCell imageCell = new PdfPCell(image);
        //    imageCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    imageCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //    headerTable.AddCell(imageCell);

        //    // Agregar el título al encabezado
        //    //PdfPCell titleCell = new PdfPCell(new Phrase("Título del documento"));
        //    PdfPCell titleCell = new PdfPCell(new Phrase("Cafe y Restaurante La Centenaria", new Font(Font.FontFamily.COURIER, 16, Font.BOLD)));
        //    titleCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    titleCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    titleCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //    headerTable.AddCell(titleCell);

        //    Font headerFont = new Font(Font.FontFamily.COURIER, 12, Font.BOLD);

        //    // Fuente para el contenido de la tabla (normal)
        //    Font contentFont = new Font(Font.FontFamily.COURIER, 12, Font.NORMAL);

        //    // Obtener la fecha de hoy
        //    DateTime daynow = DateTime.Now;
        //    string fechaHoy = daynow.ToString();

        //    // Agregar la descripción al encabezado
        //    PdfPCell descriptionCell = new PdfPCell(new Phrase($"Reporte Semanal de Caja\n\nFecha de Impresión:\n{fechaHoy}", new Font(Font.FontFamily.COURIER, 10, Font.NORMAL)));
        //    descriptionCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    descriptionCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    descriptionCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //    headerTable.AddCell(descriptionCell);

        //    // Agregar el encabezado al documento
        //    document.Add(headerTable);



           

        //    // Agregar contenido al documento
        //    foreach (var item in response)
        //    {
        //        dynamic cajaResponse = item;




        //        // Agregar tabla para los datos de la caja diaria
        //        PdfPTable cajaTable = new PdfPTable(3);
        //        cajaTable.WidthPercentage = 100;
        //        cajaTable.SpacingAfter = 10f;

        //        PdfPCell headerCell5 = new PdfPCell(new Phrase("Fecha", headerFont));
        //        cajaTable.AddCell(headerCell5);
        //        headerCell5 = new PdfPCell(new Phrase("Saldo Inicial", headerFont));
        //        cajaTable.AddCell(headerCell5);
        //        headerCell5 = new PdfPCell(new Phrase("Saldo Final", headerFont));
        //        cajaTable.AddCell(headerCell5);
              

        //        //cajaTable.AddCell(cajaResponse.CajaDiaria.IdCajaDiaria.ToString());
        //        //cajaTable.AddCell(cajaResponse.CajaDiaria.Fecha.ToString());
        //        cajaTable.AddCell(new PdfPCell(new Phrase(cajaResponse.CajaDiaria.Fecha.ToString(), contentFont)));
        //        cajaTable.AddCell(new PdfPCell(new Phrase(cajaResponse.CajaDiaria.SaldoInicial.ToString(), contentFont)));
        //        cajaTable.AddCell(new PdfPCell(new Phrase(cajaResponse.CajaDiaria.SaldoFinal.ToString(), contentFont)));
        //        //cajaTable.AddCell(cajaResponse.CajaDiaria.SaldoInicial.ToString());
        //        //cajaTable.AddCell(cajaResponse.CajaDiaria.SaldoFinal.ToString());
        //        //cajaTable.AddCell(cajaResponse.CajaDiaria.Estado.ToString());

        //        LineSeparator line = new LineSeparator();
        //        line.LineColor = BaseColor.BLACK; // Color de la línea
        //        line.LineWidth = 1f; // Ancho de la línea
        //                             // Agregar la línea al documento
        //        document.Add(new Chunk(line));

        //        document.Add(new Paragraph("Informacion de caja", headerFont));
        //        document.Add(new Chunk("\n"));

        //        document.Add(cajaTable);
        //        // Linea separador
               

               

        //        // Agregar tabla para los datos de movimientos de caja, gastos y ventas
        //        PdfPTable movimientosTable = new PdfPTable(3);
        //        movimientosTable.WidthPercentage = 100;
        //        movimientosTable.SpacingAfter = 10f;

        //        //movimientosTable.AddCell("IdMovimiento");
        //        PdfPCell headerCell4 = new PdfPCell(new Phrase("IdTipoMovimiento", headerFont));
        //        movimientosTable.AddCell(headerCell4);
        //        headerCell4 = new PdfPCell(new Phrase("Concepto", headerFont));
        //        movimientosTable.AddCell(headerCell4);
        //        headerCell4 = new PdfPCell(new Phrase("Total", headerFont));
        //        movimientosTable.AddCell(headerCell4);

        //        foreach (var movimiento in cajaResponse.MovimientosCaja)
        //        {
        //            //movimientosTable.AddCell(movimiento.IdMovimiento.ToString());
        //            movimientosTable.AddCell(new PdfPCell(new Phrase(movimiento.IdTipoMovimiento.ToString(),contentFont)));
        //            movimientosTable.AddCell(new PdfPCell(new Phrase(movimiento.Concepto.ToString(), contentFont)));
        //            movimientosTable.AddCell(new PdfPCell(new Phrase(movimiento.Total.ToString(), contentFont)));
        //            //movimientosTable.AddCell(movimiento.Concepto.ToString());
        //            //movimientosTable.AddCell(movimiento.Total.ToString());
        //        }

        //        document.Add(movimientosTable);

                

        //        //**************************
        //        // Crear la tabla principal que contendrá las tablas de ventas y gastos
        //        PdfPTable mainTable = new PdfPTable(2);
        //        mainTable.WidthPercentage = 100;
        //        mainTable.DefaultCell.BackgroundColor = new BaseColor(240, 240, 240);

        //        // Tabla de ventas
        //        PdfPTable ventasTable = new PdfPTable(3);
        //        ventasTable.WidthPercentage = 100;
        //        //ventasTable.AddCell("Número de Comanda");
        //        //ventasTable.AddCell("Fecha");
        //        //ventasTable.AddCell("Total");

        //        PdfPCell headerCell1 = new PdfPCell(new Phrase("Número de Comanda", headerFont));
        //        ventasTable.AddCell(headerCell1);
        //        headerCell1 = new PdfPCell(new Phrase("Fecha", headerFont));
        //        ventasTable.AddCell(headerCell1);
        //        headerCell1 = new PdfPCell(new Phrase("Total", headerFont));
        //        ventasTable.AddCell(headerCell1);

        //        foreach (var movimiento in cajaResponse.MovimientosCaja)
        //        {
        //            foreach (var venta in movimiento.Ventas)
        //            {
        //                //ventasTable.AddCell(venta.NumeroComanda);
        //                //ventasTable.AddCell(venta.Fecha.ToString());
        //                //ventasTable.AddCell(venta.Total.ToString());
        //                ventasTable.AddCell(new PdfPCell(new Phrase(venta.NumeroComanda, contentFont)));
        //                ventasTable.AddCell(new PdfPCell(new Phrase(venta.Fecha.ToString(), contentFont)));
        //                ventasTable.AddCell(new PdfPCell(new Phrase(venta.Total.ToString(), contentFont)));
        //                //gastosTable.AddCell(new PdfPCell(new Phrase(gasto.Total.ToString(), contentFont)));
        //            }
        //        }
        //        // Fuente para el encabezado (negrita)


        //        // Tabla de gastos
        //        PdfPTable gastosTable = new PdfPTable(4);
        //        //gastosTable.WidthPercentage = 100;
        //        //gastosTable.AddCell("Número de Documento");
        //        //gastosTable.AddCell("Fecha");
        //        //gastosTable.AddCell("Concepto");
        //        //gastosTable.AddCell("Total");
        //        // Encabezado de la tabla en negrita
        //        PdfPCell headerCell = new PdfPCell(new Phrase("No.Documento", headerFont));
        //        gastosTable.AddCell(headerCell);
        //        headerCell = new PdfPCell(new Phrase("Fecha", headerFont));
        //        gastosTable.AddCell(headerCell);
        //        headerCell = new PdfPCell(new Phrase("Concepto", headerFont));
        //        gastosTable.AddCell(headerCell);
        //        headerCell = new PdfPCell(new Phrase("Total", headerFont));
        //        gastosTable.AddCell(headerCell);

        //        foreach (var movimiento in cajaResponse.MovimientosCaja)
        //        {
        //            foreach (var gasto in movimiento.Gastos)
        //            {
        //                //gastosTable.AddCell(gasto.NumeroDocumento, contentFont);
        //                gastosTable.AddCell(new PdfPCell(new Phrase(gasto.NumeroDocumento, contentFont)));
        //                gastosTable.AddCell(new PdfPCell(new Phrase(gasto.Fecha.ToString(), contentFont)));
        //                gastosTable.AddCell(new PdfPCell(new Phrase(gasto.Concepto, contentFont)));
        //                gastosTable.AddCell(new PdfPCell(new Phrase(gasto.Total.ToString(), contentFont)));
        //                //gastosTable.AddCell(gasto.Fecha.ToString(), contentFont);
        //                //gastosTable.AddCell(gasto.Concepto, contentFont);
        //                //gastosTable.AddCell(gasto.Total.ToString());
        //            }
        //        }

        //        // Agregar las tablas a la tabla principal
        //        mainTable.AddCell(new PdfPCell(ventasTable) { Border = PdfPCell.NO_BORDER });
        //        mainTable.AddCell(new PdfPCell(gastosTable) { Border = PdfPCell.NO_BORDER });

        //        //document.Add(new Paragraph("Información de Caja: \n Gastos \n\n", headerFont));

        //        document.Add(mainTable);


        //        //*******************

        //        // Agregar totales
        //        PdfPTable totalesTable = new PdfPTable(2);
        //        totalesTable.WidthPercentage = 100;
        //        totalesTable.SpacingAfter = 10f;

        //        //totalesTable.AddCell("Total Ventas");
        //        //totalesTable.AddCell("Total Gastos");

                
        //        PdfPCell headerCell6 = new PdfPCell(new Phrase("Total Ventas", headerFont));
        //        totalesTable.AddCell(headerCell6);
        //        headerCell6 = new PdfPCell(new Phrase("Total Gastos", headerFont));
        //        totalesTable.AddCell(headerCell6);

        //        totalesTable.AddCell(new PdfPCell(new Phrase(cajaResponse.TotalVentas.ToString(), contentFont)));
        //        totalesTable.AddCell(new PdfPCell(new Phrase(cajaResponse.TotalGastos.ToString(), contentFont)));

                

        //        document.Add(totalesTable);



        //        document.NewPage();



        //    }

        //    //document.Add(new Paragraph($"Informacion de caja: {cajaResponse.ingresoTotalSemana.ToString}", headerFont));

            

        //    document.Close();

        //    //// Descargar el archivo PDF
        //    //Response.ContentType = "application/pdf";
        //    //Response.Headers.Add("content-disposition", "attachment;filename=reporte.pdf");
        //    //Response.Body.Write(memoryStream.ToArray(), 0, memoryStream.ToArray().Length);

        //    //return new FileStreamResult(memoryStream, "application/pdf");



        //    //document.Close();
        //    writer.Close();

        //    byte[] pdfBytes = memoryStream.ToArray();

        //    // Devolver el archivo PDF
        //    return File(pdfBytes, "application/pdf");





        //}



        //************************************


[HttpGet("weeklypdf")]
    public IActionResult GenerateklyBoxReportPDF()
    {
        DayOfWeek currentDayOfWeek = DateTime.Today.DayOfWeek;
        int daysUntilStartOfWeek = ((int)currentDayOfWeek - 1 + 7) % 7;
        DateTime startOfWeek = DateTime.Today.AddDays(-daysUntilStartOfWeek).Date;
        DateTime endOfWeek = startOfWeek.AddDays(7).Date;

        DateOnly startOfWeekDateOnly = DateOnly.FromDateTime(startOfWeek);
        DateOnly endOfWeekDateOnly = DateOnly.FromDateTime(endOfWeek);

        List<CajaDiaria> cajasSemanales = _dbContext.CajaDiaria
            .Where(c => c.Fecha >= startOfWeekDateOnly && c.Fecha <= endOfWeekDateOnly)
            .ToList();
            cajasSemanales = cajasSemanales.OrderByDescending(c => c.Fecha).ToList();


            if (cajasSemanales.Count == 0)
        {
            return NotFound("No se encontraron cajas diarias para la semana actual.");
        }

        decimal totalIngresosSemana = 0;
        decimal totalVentasSemana = 0;
        decimal totalComprasSemana = 0;
        decimal totalSaldoFinalSemana = 0;

            decimal totalVentas = 0;
            decimal totalGastos = 0;

            Document document = new Document();
        MemoryStream memoryStream = new MemoryStream();
        PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

            HeaderFooter1 eventHandler = new HeaderFooter1();
            writer.PageEvent = eventHandler;


            document.Open();




            Font headerFont = new Font(Font.FontFamily.COURIER, 12, Font.BOLD);

            // Fuente para el contenido de la tabla (normal)
            Font contentFont = new Font(Font.FontFamily.COURIER, 12, Font.NORMAL);


            

            foreach (CajaDiaria cajaDiaria in cajasSemanales)
        {
                
                var movimientosCaja = (from mc in _dbContext.MovimientoCajas
                                   where mc.IdCajaDiaria == cajaDiaria.IdCajaDiaria
                                   select new
                                   {
                                       mc.IdMovimiento,
                                       mc.IdTipoMovimiento,
                                       mc.Concepto,
                                       mc.Total,
                                       Ventas = (from vmc in _dbContext.VentaMovimientoCajas
                                                 join v in _dbContext.Ventas on vmc.IdVenta equals v.IdVenta
                                                 where vmc.IdMovimientoCaja == mc.IdMovimiento
                                                 select new
                                                 {
                                                     v.IdVenta,
                                                     v.NumeroComanda,
                                                     v.Fecha,
                                                     v.Total,
                                                     v.IdMesero,
                                                     v.IdCliente
                                                 }).ToList(),
                                       Gastos = (from gmc in _dbContext.GastosMovimientoCajas
                                                 join g in _dbContext.Gastos on gmc.IdGasto equals g.IdGasto
                                                 where gmc.IdMovimientoCaja == mc.IdMovimiento
                                                 select new
                                                 {
                                                     g.IdGasto,
                                                     g.NumeroDocumento,
                                                     g.Fecha,
                                                     g.Concepto,
                                                     g.Total,
                                                     g.IdProveedor
                                                 }).ToList()
                                   }).ToList();

            totalVentas = movimientosCaja.SelectMany(m => m.Ventas).Sum(v => v.Total) ?? 0;
            totalGastos = movimientosCaja.SelectMany(m => m.Gastos).Sum(g => g.Total) ?? 0;

            decimal saldoFinal = ((cajaDiaria.SaldoInicial + totalVentas) - totalGastos) ?? 0;
            cajaDiaria.SaldoFinal = saldoFinal;

            totalIngresosSemana += totalVentas;
            totalVentasSemana += totalVentas;
            totalComprasSemana += totalGastos;
            totalSaldoFinalSemana += saldoFinal;

                




                // Agregar tabla para los datos de la caja diaria
                PdfPTable cajaTable = new PdfPTable(3);
                cajaTable.WidthPercentage = 100;
                cajaTable.SpacingAfter = 10f;

                PdfPCell headerCell5 = new PdfPCell(new Phrase("Fecha", headerFont));
                cajaTable.AddCell(headerCell5);
                headerCell5 = new PdfPCell(new Phrase("Saldo Inicial", headerFont));
                cajaTable.AddCell(headerCell5);
                headerCell5 = new PdfPCell(new Phrase("Saldo Final", headerFont));
                cajaTable.AddCell(headerCell5);


                //cajaTable.AddCell(cajaResponse.CajaDiaria.IdCajaDiaria.ToString());
                //cajaTable.AddCell(cajaResponse.CajaDiaria.Fecha.ToString());
                cajaTable.AddCell(new PdfPCell(new Phrase(cajaDiaria.Fecha.ToString(), contentFont)));
                cajaTable.AddCell(new PdfPCell(new Phrase(cajaDiaria.SaldoInicial.ToString(), contentFont)));
                cajaTable.AddCell(new PdfPCell(new Phrase(cajaDiaria.SaldoFinal.ToString(), contentFont)));
                //cajaTable.AddCell(cajaResponse.CajaDiaria.SaldoInicial.ToString());
                //cajaTable.AddCell(cajaResponse.CajaDiaria.SaldoFinal.ToString());
                //cajaTable.AddCell(cajaResponse.CajaDiaria.Estado.ToString());

                LineSeparator line = new LineSeparator();
                line.LineColor = BaseColor.BLACK; // Color de la línea
                line.LineWidth = 1f; // Ancho de la línea
                                     // Agregar la línea al documento
                document.Add(new Chunk(line));

                document.Add(new Paragraph("Informacion de caja", headerFont));
                document.Add(new Chunk("\n"));

                document.Add(cajaTable);
                // Linea separador



                // Agregar tabla para los datos de movimientos de caja, gastos y ventas
                PdfPTable movimientosTable = new PdfPTable(3);
                movimientosTable.WidthPercentage = 100;
                movimientosTable.SpacingAfter = 10f;

                //movimientosTable.AddCell("IdMovimiento");
                PdfPCell headerCell4 = new PdfPCell(new Phrase("Cod. Movimiento", headerFont));
                movimientosTable.AddCell(headerCell4);
                headerCell4 = new PdfPCell(new Phrase("Concepto", headerFont));
                movimientosTable.AddCell(headerCell4);
                headerCell4 = new PdfPCell(new Phrase("Total", headerFont));
                movimientosTable.AddCell(headerCell4);

                foreach (var movimiento in movimientosCaja)
                {
                    //movimientosTable.AddCell(movimiento.IdMovimiento.ToString());
                    movimientosTable.AddCell(new PdfPCell(new Phrase(movimiento.IdMovimiento.ToString(), contentFont)));
                    movimientosTable.AddCell(new PdfPCell(new Phrase(movimiento.Concepto.ToString(), contentFont)));
                    movimientosTable.AddCell(new PdfPCell(new Phrase(movimiento.Total.ToString(), contentFont)));
                    //movimientosTable.AddCell(movimiento.Concepto.ToString());
                    //movimientosTable.AddCell(movimiento.Total.ToString());
                }

                document.Add(movimientosTable);




                //document.Add(new Chunk("\n"));




                //**************************
                // Crear la tabla principal que contendrá las tablas de ventas y gastos
                PdfPTable mainTable = new PdfPTable(2);
                mainTable.WidthPercentage = 100;
                mainTable.DefaultCell.BackgroundColor = new BaseColor(240, 240, 240);

                // Tabla de ventas
                PdfPTable ventasTable = new PdfPTable(3);
                ventasTable.WidthPercentage = 100;
                //ventasTable.AddCell("Número de Comanda");
                //ventasTable.AddCell("Fecha");
                //ventasTable.AddCell("Total");

                PdfPCell headerCell1 = new PdfPCell(new Phrase("Número de Comanda", headerFont));
                ventasTable.AddCell(headerCell1);
                headerCell1 = new PdfPCell(new Phrase("Fecha", headerFont));
                ventasTable.AddCell(headerCell1);
                headerCell1 = new PdfPCell(new Phrase("Total", headerFont));
                ventasTable.AddCell(headerCell1);

                foreach (var movimiento in movimientosCaja)
                {
                    foreach (var venta in movimiento.Ventas)
                    {
                        //ventasTable.AddCell(venta.NumeroComanda);
                        //ventasTable.AddCell(venta.Fecha.ToString());
                        //ventasTable.AddCell(venta.Total.ToString());
                        ventasTable.AddCell(new PdfPCell(new Phrase(venta.NumeroComanda, contentFont)));
                        ventasTable.AddCell(new PdfPCell(new Phrase(venta.Fecha.ToString(), contentFont)));
                        ventasTable.AddCell(new PdfPCell(new Phrase(venta.Total.ToString(), contentFont)));
                        //gastosTable.AddCell(new PdfPCell(new Phrase(gasto.Total.ToString(), contentFont)));
                    }
                }
                // Fuente para el encabezado (negrita)


                // Tabla de gastos
                PdfPTable gastosTable = new PdfPTable(4);
                //gastosTable.WidthPercentage = 100;
                //gastosTable.AddCell("Número de Documento");
                //gastosTable.AddCell("Fecha");
                //gastosTable.AddCell("Concepto");
                //gastosTable.AddCell("Total");
                // Encabezado de la tabla en negrita
                PdfPCell headerCell = new PdfPCell(new Phrase("No.Documento", headerFont));
                gastosTable.AddCell(headerCell);
                headerCell = new PdfPCell(new Phrase("Fecha", headerFont));
                gastosTable.AddCell(headerCell);
                headerCell = new PdfPCell(new Phrase("Concepto", headerFont));
                gastosTable.AddCell(headerCell);
                headerCell = new PdfPCell(new Phrase("Total", headerFont));
                gastosTable.AddCell(headerCell);

                foreach (var movimiento in movimientosCaja)
                {
                    foreach (var gasto in movimiento.Gastos)
                    {
                        //gastosTable.AddCell(gasto.NumeroDocumento, contentFont);
                        gastosTable.AddCell(new PdfPCell(new Phrase(gasto.NumeroDocumento, contentFont)));
                        gastosTable.AddCell(new PdfPCell(new Phrase(gasto.Fecha.ToString(), contentFont)));
                        gastosTable.AddCell(new PdfPCell(new Phrase(gasto.Concepto, contentFont)));
                        gastosTable.AddCell(new PdfPCell(new Phrase(gasto.Total.ToString(), contentFont)));
                        //gastosTable.AddCell(gasto.Fecha.ToString(), contentFont);
                        //gastosTable.AddCell(gasto.Concepto, contentFont);
                        //gastosTable.AddCell(gasto.Total.ToString());
                    }
                }

                // Agregar las tablas a la tabla principal
                mainTable.AddCell(new PdfPCell(ventasTable) { Border = PdfPCell.NO_BORDER });
                mainTable.AddCell(new PdfPCell(gastosTable) { Border = PdfPCell.NO_BORDER });

                //document.Add(new Paragraph("Información de Caja: \n Gastos \n\n", headerFont));

                document.Add(mainTable);

                    //********************************

                // Agregar totales
                PdfPTable totalesTable = new PdfPTable(2);
                totalesTable.WidthPercentage = 100;
                totalesTable.SpacingAfter = 10f;

                //totalesTable.AddCell("Total Ventas");
                //totalesTable.AddCell("Total Gastos");


                PdfPCell headerCell6 = new PdfPCell(new Phrase("Total Ventas", headerFont));
                totalesTable.AddCell(headerCell6);
                headerCell6 = new PdfPCell(new Phrase("Total Gastos", headerFont));
                totalesTable.AddCell(headerCell6);

                totalesTable.AddCell(new PdfPCell(new Phrase(totalVentas.ToString(), contentFont)));
                totalesTable.AddCell(new PdfPCell(new Phrase(totalGastos.ToString(), contentFont)));

                document.Add(totalesTable);



                document.NewPage();

               


                //*******************

                //*************************
            }
            //document.NewPage();

            // Crear una tabla de 3x3
            PdfPTable table = new PdfPTable(4);
            table.WidthPercentage = 100;

            // Agregar la descripción de las columnas
            table.AddCell(new PdfPCell(new Phrase("")));
            table.AddCell(new PdfPCell(new Phrase("Perdidas(-)", headerFont)));
            table.AddCell(new PdfPCell(new Phrase("Ganancias(+)", headerFont)));
            table.AddCell(new PdfPCell(new Phrase("Saldo semanal", headerFont)));

            // Agregar el título "Entrada" en la primera fila
            table.AddCell(new PdfPCell(new Phrase("Ventas", headerFont)));
            table.AddCell(new PdfPCell(new Phrase($"", contentFont)));
            table.AddCell(new PdfPCell(new Phrase($"{totalVentasSemana}", contentFont)));
            table.AddCell(new PdfPCell(new Phrase("")));

            // Agregar el título "Salidas" en la segunda fila
            table.AddCell(new PdfPCell(new Phrase("Compras", headerFont)));
            table.AddCell(new PdfPCell(new Phrase($"{totalComprasSemana}", contentFont)));
            table.AddCell(new PdfPCell(new Phrase($"", contentFont)));
            table.AddCell(new PdfPCell(new Phrase("", contentFont)));

            // Agregar el título "Salidas" en la segunda fila
            table.AddCell(new PdfPCell(new Phrase("Total", headerFont)));
            table.AddCell(new PdfPCell(new Phrase("", contentFont)));
            table.AddCell(new PdfPCell(new Phrase("", contentFont)));
            table.AddCell(new PdfPCell(new Phrase($"{totalSaldoFinalSemana}", contentFont)));

            document.Add(new Paragraph("A continuacion visualiza un pequeño resumen de todas las ventas y todos los gastos, " +
                "además, se realiza la operacion para obtener el total de Saldo de la semana", headerFont));
            document.Add(new Chunk("\n"));
            // Agregar la tabla al documento
            document.Add(table);

            






            document.Close();


            byte[] bytes = memoryStream.ToArray();

            // Prepare the HTTP response
            //var result = new FileContentResult(bytes, "application/pdf");

            //// Liberar recursos
            //writer.Close();
            //memoryStream.Close();
            //memoryStream.Dispose();

            return File(bytes, "application/pdf");
        }



        //****************************************************








        [HttpGet("montss")]
        public IActionResult MokthlyBdj(string month)
        {
            DateTime currentDate = DateTime.Today;
            DateTime requestedDate;

            try
            {
                requestedDate = DateTime.ParseExact(month, "MMMM", CultureInfo.CurrentCulture);
            }
            catch (FormatException)
            {
                return BadRequest("El mes ingresado no es válido.");
            }

            if (requestedDate > currentDate)
            {
                return BadRequest("El mes solicitado es mayor al mes actual.");
            }

            DateTime startOfMonth = new DateTime(requestedDate.Year, requestedDate.Month, 1);
            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            DateOnly startOfMonthDateOnly = DateOnly.FromDateTime(startOfMonth);
            DateOnly endOfMonthDateOnly = DateOnly.FromDateTime(endOfMonth);

            List<CajaDiaria> cajasMensuales = _dbContext.CajaDiaria
                .Where(c => c.Fecha >= startOfMonthDateOnly && c.Fecha <= endOfMonthDateOnly)
                .OrderByDescending(c => c.Fecha) // Ordenar por fecha en orden descendente
                .ToList();

            if (cajasMensuales.Count == 0)
            {
                return NotFound("No se encontraron cajas diarias para el mes solicitado.");
            }

            decimal totalIngresosMes = 0;
            decimal totalVentasMes = 0;
            decimal totalComprasMes = 0;
            decimal totalSaldoFinalMes = 0;

            List<object> response = new List<object>();

            DateTime startOfWeek = startOfMonth;
            DateTime endOfWeek = startOfWeek.AddDays(6);

            while (startOfWeek <= endOfMonth)
            {
                DateOnly startOfWeekDateOnly = DateOnly.FromDateTime(startOfWeek);
                DateOnly endOfWeekDateOnly = DateOnly.FromDateTime(endOfWeek);

                List<CajaDiaria> cajasSemanales = cajasMensuales
                    .Where(c => c.Fecha >= startOfWeekDateOnly && c.Fecha <= endOfWeekDateOnly)
                    .ToList();

                decimal totalVentasSemana = 0;
                decimal totalComprasSemana = 0;
                decimal totalSaldoFinalSemana = 0;

                List<object> semanaResponse = new List<object>();

                foreach (CajaDiaria cajaDiaria in cajasSemanales)
                {
                    var movimientosCaja = (from mc in _dbContext.MovimientoCajas
                                           where mc.IdCajaDiaria == cajaDiaria.IdCajaDiaria
                                           select new
                                           {
                                               mc.IdMovimiento,
                                               mc.IdTipoMovimiento,
                                               mc.Concepto,
                                               mc.Total,
                                               Ventas = (from vmc in _dbContext.VentaMovimientoCajas
                                                         join v in _dbContext.Ventas on vmc.IdVenta equals v.IdVenta
                                                         where vmc.IdMovimientoCaja == mc.IdMovimiento
                                                         select new
                                                         {
                                                             v.IdVenta,
                                                             v.NumeroComanda,
                                                             v.Fecha,
                                                             v.Total,
                                                             v.IdMesero,
                                                             v.IdCliente
                                                         }).ToList(),
                                               Gastos = (from gmc in _dbContext.GastosMovimientoCajas
                                                         join g in _dbContext.Gastos on gmc.IdGasto equals g.IdGasto
                                                         where gmc.IdMovimientoCaja == mc.IdMovimiento
                                                         select new
                                                         {
                                                             g.IdGasto,
                                                             g.NumeroDocumento,
                                                             g.Fecha,
                                                             g.Concepto,
                                                             g.Total,
                                                             g.IdProveedor
                                                         }).ToList()
                                           }).ToList();

                    decimal totalVentas = movimientosCaja.SelectMany(m => m.Ventas).Sum(v => v.Total) ?? 0;
                    decimal totalGastos = movimientosCaja.SelectMany(m => m.Gastos).Sum(g => g.Total) ?? 0;

                    decimal saldoFinal = ((cajaDiaria.SaldoInicial + totalVentas) - totalGastos) ?? 0;
                    cajaDiaria.SaldoFinal = saldoFinal;

                    totalVentasSemana += totalVentas;
                    totalComprasSemana += totalGastos;
                    totalSaldoFinalSemana += saldoFinal;

                    var cajaResponse = new
                    {
                        CajaDiaria = new
                        {
                            cajaDiaria.IdCajaDiaria,
                            cajaDiaria.Fecha,
                            cajaDiaria.SaldoInicial,
                            cajaDiaria.SaldoFinal,
                            cajaDiaria.Estado
                        },
                        MovimientosCaja = movimientosCaja,
                        TotalVentas = totalVentas,
                        TotalGastos = totalGastos
                    };

                    semanaResponse.Add(cajaResponse);
                }

                var semanaData = new
                {
                    StartDate = startOfWeek,
                    EndDate = endOfWeek,
                    TotalVentasSemana = totalVentasSemana,
                    TotalComprasSemana = totalComprasSemana,
                    TotalSaldoFinalSemana = totalSaldoFinalSemana,
                    CajasSemanales = semanaResponse
                };

                response.Add(semanaData);

                totalVentasMes += totalVentasSemana;
                totalComprasMes += totalComprasSemana;
                totalSaldoFinalMes += totalSaldoFinalSemana;

                startOfWeek = startOfWeek.AddDays(7);
                endOfWeek = endOfWeek.AddDays(7);

                if (endOfWeek > endOfMonth)
                {
                    endOfWeek = endOfMonth;
                }
            }

            var mesResponse = new
            {
                TotalIngresosMes = totalIngresosMes,
                TotalVentasMes = totalVentasMes,
                TotalComprasMes = totalComprasMes,
                TotalSaldoFinalMes = totalSaldoFinalMes,
                Semanas = response
            };

            // Generar el PDF con iTextSharp
            Document document = new Document();
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

            HeaderFooter2 eventHandler = new HeaderFooter2();
            writer.PageEvent = eventHandler;

            document.Open();

            
            Font headerFont = new Font(Font.FontFamily.COURIER, 12, Font.BOLD);
            Font headerFont1 = new Font(Font.FontFamily.COURIER, 12, Font.BOLD, BaseColor.RED);

            // Fuente para el contenido de la tabla (normal)
            Font contentFont = new Font(Font.FontFamily.COURIER, 12, Font.NORMAL);
            Font contentFont1 = new Font(Font.FontFamily.COURIER, 12, Font.NORMAL, BaseColor.RED);

            // Obtén la respuesta como una cadena JSON
            string mesResponseJson = JsonConvert.SerializeObject(mesResponse);

            // Deserializa la respuesta en el tipo de objeto adecuado
            dynamic mesResponseData = JsonConvert.DeserializeObject<dynamic>(mesResponseJson);



            //document.Add(cajasSemanalesTable);
            // Recorrer las semanas
            foreach (dynamic semanaData in mesResponse.Semanas)
            {
                DateTime startDate = DateTime.Parse(semanaData.StartDate.ToString());
                DateTime endDate = DateTime.Parse(semanaData.EndDate.ToString());
                decimal totalVentasSemana = decimal.Parse(semanaData.TotalVentasSemana.ToString());
                decimal totalComprasSemana = decimal.Parse(semanaData.TotalComprasSemana.ToString());
                decimal totalSaldoFinalSemana = decimal.Parse(semanaData.TotalSaldoFinalSemana.ToString());

                //document.Add(new Paragraph($"Fecha de inicio de la semana: {semanaData.StartDate.ToShortDateString()}"));
                //document.Add(new Paragraph($"Fecha de fin de la semana: {semanaData.EndDate.ToShortDateString()}"));
                //document.Add(new Paragraph($"Total ventas de la semana: {semanaData.TotalVentasSemana}"));
                //document.Add(new Paragraph($"Total compras de la semana: {semanaData.TotalComprasSemana}"));
                //document.Add(new Paragraph($"Total saldo final de la semana: {semanaData.TotalSaldoFinalSemana}"));

                // Agregar tabla para los datos de la caja diaria
                PdfPTable inicioTable = new PdfPTable(5);
                inicioTable.WidthPercentage = 100;
                //inicioTable.DefaultCell.BackgroundColor = new BaseColor(0, 0, 255); // Establece el color de fondo de la tabla como rojo
                //inicioTable.BackgroundColor = new BaseColor(255, 0, 0);
                //inicioTable.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 0, 0); // Red
                inicioTable.SpacingAfter = 10f;

                PdfPCell headerCell5 = new PdfPCell(new Phrase("Fecha inicio de la semana", headerFont1));
                inicioTable.AddCell(headerCell5);
                headerCell5 = new PdfPCell(new Phrase("Fecha fin de la semana", headerFont1));
                inicioTable.AddCell(headerCell5);
                headerCell5 = new PdfPCell(new Phrase("Total de ventas de la semana", headerFont1));
                inicioTable.AddCell(headerCell5);
                headerCell5 = new PdfPCell(new Phrase("Total de compras de la semana", headerFont1));
                inicioTable.AddCell(headerCell5);
                headerCell5 = new PdfPCell(new Phrase("Saldo final de la semana", headerFont1));
                inicioTable.AddCell(headerCell5);


                inicioTable.AddCell(new PdfPCell(new Phrase(semanaData.StartDate.ToShortDateString(), contentFont1)));
                inicioTable.AddCell(new PdfPCell(new Phrase(semanaData.EndDate.ToShortDateString(), contentFont1)));
                inicioTable.AddCell(new PdfPCell(new Phrase(semanaData.TotalVentasSemana.ToString(), contentFont1)));
                inicioTable.AddCell(new PdfPCell(new Phrase(semanaData.TotalComprasSemana.ToString(), contentFont1)));
                inicioTable.AddCell(new PdfPCell(new Phrase(semanaData.TotalSaldoFinalSemana.ToString(), contentFont1)));

                document.Add(Chunk.NEWLINE);
                LineSeparator line = new LineSeparator();
                line.LineColor = BaseColor.BLACK; // Color de la línea
                line.LineWidth = 1f; // Ancho de la línea
                                     // Agregar la línea al documento
                document.Add(new Chunk(line));

                document.Add(new Paragraph("Informacion de caja", headerFont));
                //document.Add(new Chunk("\n"));
                document.Add(Chunk.NEWLINE);
                document.Add(inicioTable);





                // Recorrer las cajas semanales
                foreach (var cajaSemana in semanaData.CajasSemanales)
                {
                    //document.Add(new Paragraph($"ID de la caja diaria: {cajaSemana.CajaDiaria.IdCajaDiaria}"));
                    //document.Add(new Paragraph($"Fecha de la caja diaria: {cajaSemana.CajaDiaria.Fecha.ToShortDateString()}"));
                    //document.Add(new Paragraph($"Saldo inicial de la caja diaria: {cajaSemana.CajaDiaria.SaldoInicial}"));
                    //document.Add(new Paragraph($"Saldo final de la caja diaria: {cajaSemana.CajaDiaria.SaldoFinal}"));
                    //document.Add(new Paragraph($"Estado de la caja diaria: {cajaSemana.CajaDiaria.Estado}"));
                    //document.Add(Chunk.NEWLINE);
                    // Agregar tabla para los datos de la caja diaria
                    PdfPTable inicioCaja = new PdfPTable(5);
                    inicioCaja.WidthPercentage = 100;
                    inicioCaja.SpacingAfter = 10f;

                    PdfPCell headerCell1 = new PdfPCell(new Phrase("No. Caja Diaria", headerFont));
                    inicioCaja.AddCell(headerCell1);
                    headerCell1 = new PdfPCell(new Phrase("Fecha", headerFont));
                    inicioCaja.AddCell(headerCell1);
                    headerCell1 = new PdfPCell(new Phrase("Saldo Inicial", headerFont));
                    inicioCaja.AddCell(headerCell1);
                    headerCell1 = new PdfPCell(new Phrase("Saldo Final", headerFont));
                    inicioCaja.AddCell(headerCell1);
                    headerCell1 = new PdfPCell(new Phrase("Estado", headerFont));
                    inicioCaja.AddCell(headerCell1);


                    inicioCaja.AddCell(new PdfPCell(new Phrase(cajaSemana.CajaDiaria.IdCajaDiaria.ToString(), contentFont)));
                    inicioCaja.AddCell(new PdfPCell(new Phrase(cajaSemana.CajaDiaria.Fecha.ToString(), contentFont)));
                    inicioCaja.AddCell(new PdfPCell(new Phrase(cajaSemana.CajaDiaria.SaldoInicial.ToString(), contentFont)));
                    inicioCaja.AddCell(new PdfPCell(new Phrase(cajaSemana.CajaDiaria.SaldoFinal.ToString(), contentFont)));
                    inicioCaja.AddCell(new PdfPCell(new Phrase(cajaSemana.CajaDiaria.Estado.ToString(), contentFont)));

                    //document.Add(new Chunk("\n"));

                    document.Add(inicioCaja);
                    //document.Add(Chunk.NEWLINE);



                    // Recorrer los movimientos de caja
                    //foreach (var movimientoCaja in cajaSemana.MovimientosCaja)
                    //{
                    //    document.Add(new Paragraph($"ID del movimiento: {movimientoCaja.IdMovimiento}"));
                    //    document.Add(new Paragraph($"ID del tipo de movimiento: {movimientoCaja.IdTipoMovimiento}"));
                    //    document.Add(new Paragraph($"Concepto del movimiento: {movimientoCaja.Concepto}"));
                    //    document.Add(new Paragraph($"Total del movimiento: {movimientoCaja.Total}"));
                    //    document.Add(Chunk.NEWLINE);
                    //}

                    // Crear la tabla principal que contendrá las tablas de ventas y gastos
                    PdfPTable mainTable = new PdfPTable(2);
                    mainTable.WidthPercentage = 100;
                    mainTable.DefaultCell.BackgroundColor = new BaseColor(240, 240, 240);

                    // Tabla de ventas
                    PdfPTable ventasTable = new PdfPTable(3);
                    ventasTable.WidthPercentage = 100;

                    PdfPCell headerCel = new PdfPCell(new Phrase("Número de Comanda", headerFont));
                    ventasTable.AddCell(headerCel);
                    headerCel = new PdfPCell(new Phrase("Fecha", headerFont));
                    ventasTable.AddCell(headerCel);
                    headerCel = new PdfPCell(new Phrase("Total", headerFont));
                    ventasTable.AddCell(headerCel);

                    //foreach (var movimientoCaja in cajaSemana.movimientosCaja)
                        foreach (var movimientoCaja in cajaSemana.MovimientosCaja)
                     {
                        foreach (var venta in movimientoCaja.Ventas)
                        {

                            ventasTable.AddCell(new PdfPCell(new Phrase(venta.NumeroComanda, contentFont)));
                            ventasTable.AddCell(new PdfPCell(new Phrase(venta.Fecha.ToString(), contentFont)));
                            ventasTable.AddCell(new PdfPCell(new Phrase(venta.Total.ToString(), contentFont)));
                            //gastosTable.AddCell(new PdfPCell(new Phrase(gasto.Total.ToString(), contentFont)));
                        }
                    }


                    // Fuente para el encabezado (negrita)


                    // Tabla de gastos
                    PdfPTable gastosTable = new PdfPTable(3);
                    gastosTable.WidthPercentage = 100;
                    // Encabezado de la tabla en negrita
                    PdfPCell headerCell = new PdfPCell(new Phrase("No.Documento", headerFont));
                    gastosTable.AddCell(headerCell);
                    headerCell = new PdfPCell(new Phrase("Concepto", headerFont));
                    gastosTable.AddCell(headerCell);
                    headerCell = new PdfPCell(new Phrase("Total", headerFont));
                    gastosTable.AddCell(headerCell);

                    foreach (var movimientoCaja in cajaSemana.MovimientosCaja)
                    {
                        foreach (var gasto in movimientoCaja.Gastos)
                        {
                            //gastosTable.AddCell(gasto.NumeroDocumento, contentFont);
                            gastosTable.AddCell(new PdfPCell(new Phrase(gasto.NumeroDocumento.ToString(), contentFont)));
                            //gastosTable.AddCell(new PdfPCell(new Phrase(gasto.Fecha.ToString(), contentFont)));
                            gastosTable.AddCell(new PdfPCell(new Phrase(gasto.Concepto, contentFont)));
                            gastosTable.AddCell(new PdfPCell(new Phrase(gasto.Total.ToString(), contentFont)));

                        }
                    }

                    // Agregar las tablas a la tabla principal
                    mainTable.AddCell(new PdfPCell(ventasTable) { Border = PdfPCell.NO_BORDER });
                    mainTable.AddCell(new PdfPCell(gastosTable) { Border = PdfPCell.NO_BORDER });

                    //document.Add(new Paragraph("Información de Caja: \n Gastos \n\n", headerFont));

                    document.Add(mainTable);


                    // Agregar totales
                    PdfPTable totalesTable = new PdfPTable(2);
                    totalesTable.WidthPercentage = 100;
                    totalesTable.SpacingAfter = 10f;

                    //totalesTable.AddCell("Total Ventas");
                    //totalesTable.AddCell("Total Gastos");


                    PdfPCell headerCell6 = new PdfPCell(new Phrase("Total Ventas", headerFont));
                    totalesTable.AddCell(headerCell6);
                    headerCell6 = new PdfPCell(new Phrase("Total Gastos", headerFont));
                    totalesTable.AddCell(headerCell6);

                    totalesTable.AddCell(new PdfPCell(new Phrase(cajaSemana.TotalVentas.ToString(), contentFont)));
                    totalesTable.AddCell(new PdfPCell(new Phrase(cajaSemana.TotalGastos.ToString(), contentFont)));

                    document.Add(totalesTable);

                    //document.Add(new Paragraph($"Total de ventas: {cajaSemana.TotalVentas}"));
                    //document.Add(new Paragraph($"Total de gastos: {cajaSemana.TotalGastos}"));
                    //document.Add(Chunk.NEWLINE);
                }

                document.NewPage();
            }

            //document.Close();



            document.Close();
            writer.Close();

            byte[] pdfBytes = memoryStream.ToArray();

            // Devolver el archivo PDF
            return File(pdfBytes, "application/pdf");
        }





    }
}




