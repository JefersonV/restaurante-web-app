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
            document.Open();


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

            Font headerFont = new Font(Font.FontFamily.COURIER, 12, Font.BOLD);

            // Fuente para el contenido de la tabla (normal)
            Font contentFont = new Font(Font.FontFamily.COURIER, 12, Font.NORMAL);

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




        [HttpGet("dailyweek")]
        public IActionResult weeBoxRepor()
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

            if (cajasSemanales.Count == 0)
            {
                return NotFound("No se encontraron cajas diarias para la semana actual.");
            }

            List<object> response = new List<object>();

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

                decimal ingresoTotalSemana = cajasSemanales.Sum(c => c.SaldoFinal) ?? 0;

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

                response.Add(cajaResponse);
            }

            //decimal totalEentas = response.Sum(item => item.cajaDaria.Estado ?? 0);

            //return Ok(response);
            // Crear documento PDF
            Document document = new Document(PageSize.A4.Rotate());
            //Document document = new Document();
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);



            document.Open();

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

            Font headerFont = new Font(Font.FontFamily.COURIER, 12, Font.BOLD);

            // Fuente para el contenido de la tabla (normal)
            Font contentFont = new Font(Font.FontFamily.COURIER, 12, Font.NORMAL);

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



           

            // Agregar contenido al documento
            foreach (var item in response)
            {
                dynamic cajaResponse = item;




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
                cajaTable.AddCell(new PdfPCell(new Phrase(cajaResponse.CajaDiaria.Fecha.ToString(), contentFont)));
                cajaTable.AddCell(new PdfPCell(new Phrase(cajaResponse.CajaDiaria.SaldoInicial.ToString(), contentFont)));
                cajaTable.AddCell(new PdfPCell(new Phrase(cajaResponse.CajaDiaria.SaldoFinal.ToString(), contentFont)));
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
                PdfPCell headerCell4 = new PdfPCell(new Phrase("IdTipoMovimiento", headerFont));
                movimientosTable.AddCell(headerCell4);
                headerCell4 = new PdfPCell(new Phrase("Concepto", headerFont));
                movimientosTable.AddCell(headerCell4);
                headerCell4 = new PdfPCell(new Phrase("Total", headerFont));
                movimientosTable.AddCell(headerCell4);

                foreach (var movimiento in cajaResponse.MovimientosCaja)
                {
                    //movimientosTable.AddCell(movimiento.IdMovimiento.ToString());
                    movimientosTable.AddCell(new PdfPCell(new Phrase(movimiento.IdTipoMovimiento.ToString(),contentFont)));
                    movimientosTable.AddCell(new PdfPCell(new Phrase(movimiento.Concepto.ToString(), contentFont)));
                    movimientosTable.AddCell(new PdfPCell(new Phrase(movimiento.Total.ToString(), contentFont)));
                    //movimientosTable.AddCell(movimiento.Concepto.ToString());
                    //movimientosTable.AddCell(movimiento.Total.ToString());
                }

                document.Add(movimientosTable);

                

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

                foreach (var movimiento in cajaResponse.MovimientosCaja)
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

                foreach (var movimiento in cajaResponse.MovimientosCaja)
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


                //*******************

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

                totalesTable.AddCell(new PdfPCell(new Phrase(cajaResponse.TotalVentas.ToString(), contentFont)));
                totalesTable.AddCell(new PdfPCell(new Phrase(cajaResponse.TotalGastos.ToString(), contentFont)));

                

                document.Add(totalesTable);



                document.NewPage();



            }

            //document.Add(new Paragraph($"Informacion de caja: {cajaResponse.ingresoTotalSemana.ToString}", headerFont));

            

            document.Close();

            //// Descargar el archivo PDF
            //Response.ContentType = "application/pdf";
            //Response.Headers.Add("content-disposition", "attachment;filename=reporte.pdf");
            //Response.Body.Write(memoryStream.ToArray(), 0, memoryStream.ToArray().Length);

            //return new FileStreamResult(memoryStream, "application/pdf");



            //document.Close();
            writer.Close();

            byte[] pdfBytes = memoryStream.ToArray();

            // Devolver el archivo PDF
            return File(pdfBytes, "application/pdf");





        }



    }
}




