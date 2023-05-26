using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using restaurante_web_app.Models;
using System.Globalization;
using iTextSharp;
using iTextSharp.text.pdf.draw;
using System.Drawing;
using System.Data;
using System;


namespace restaurante_web_app.Controllers.Report
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class pdfCostController : ControllerBase
    {
        public readonly GoeatContext _dbContext;

        public pdfCostController(GoeatContext dbContext)
        {
            _dbContext = dbContext;
        }

        //[HttpGet]
        [HttpGet("Costday")]
        public IActionResult GeneratePdfday([FromQuery] DateTime date)
        {
            DateOnly selectedDate = DateOnly.FromDateTime(date);

            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            //DateOnly today = DateOnly.FromDateTime(new DateTime(2023, 3, 3));

            // Obtener los datos de las tablas de la base de datos solo para hoy
            var data = from p in _dbContext.Proveedores
                       join g in _dbContext.Gastos on p.IdProveedor equals g.IdProveedor
                       where g.Fecha == selectedDate
                       select new
                       {
                           g.IdGasto,
                           g.NumeroDocumento,
                           g.Fecha, 
                           g.Concepto,
                           g.Total,
                           g.IdProveedor,
                           p.Nombre,
                           p.Telefono,
                       };

            decimal totalVentas = data.Sum(item => item.Total ?? 0);

            // Group the data by customer
            var groupedData = data.GroupBy(item => item.IdGasto);

            //suma de hoy
            //decimal totalVentas = groupedData.Sum(group => group.Sum(item => item.Subtotal ?? 0));


            // Create a PDF document
            Document document = new Document();

            // Create a MemoryStream to save the PDF file
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);


            // Add metadata
            document.AddTitle("Facturas de gastos");
            document.AddSubject("Facturas de gastos de la base de datos");
            document.AddKeywords("gastos, base de datos, PDF");
            document.AddCreator("Mi aplicación");

            // Definir el estilo de fuente predeterminado
            //FontFactory.Register(openSansFontPath, "Roboto Mono");

            // Open the PDF document
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


            // Obtener la fecha de hoy
            DateTime daynow = DateTime.Now;
            string fechaHoy = daynow.ToString();

            // Agregar la descripción al encabezado
            PdfPCell descriptionCell = new PdfPCell(new Phrase($"Reporte Diario de Gastos\n\nFecha de Impresión:\n{fechaHoy}", new Font(Font.FontFamily.COURIER, 10, Font.NORMAL)));
            descriptionCell.HorizontalAlignment = Element.ALIGN_CENTER;
            descriptionCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            descriptionCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            headerTable.AddCell(descriptionCell);

            // Agregar el encabezado al documento
            document.Add(headerTable);

            // Recorra los grupos y agregue una nueva página para cada cliente
            foreach (var group in groupedData)
            {

                // Linea separador
                LineSeparator line = new LineSeparator();
                line.LineColor = BaseColor.BLACK; // Color de la línea
                line.LineWidth = 1f; // Ancho de la línea
                // Agregar la línea al documento
                document.Add(new Chunk(line));

                
                Paragraph customer = new Paragraph($"Proveedor: {group.First().Nombre} \n" +
                                                   $"No. de Doc.: {group.First().NumeroDocumento} \n" +
                                                   $"Fecha de Registro: {group.First().Fecha} \n\n", new Font(Font.FontFamily.COURIER, 12, Font.BOLD));
                customer.Alignment = Element.ALIGN_LEFT;
                document.Add(customer);



                // añadir items
                PdfPTable table = new PdfPTable(2);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 1f, 1f });
                table.DefaultCell.Padding = 5;
                table.DefaultCell.BackgroundColor = new BaseColor(240, 240, 240);

                // Fuente para el encabezado (negrita)
                Font headerFont = new Font(Font.FontFamily.COURIER, 12, Font.BOLD);

                // Encabezado de la tabla en negrita
                PdfPCell headerCell = new PdfPCell(new Phrase("Concepto", headerFont));
                table.AddCell(headerCell);
                headerCell = new PdfPCell(new Phrase("Subtotal", headerFont));
                table.AddCell(headerCell);
                //headerCell = new PdfPCell(new Phrase("Precio", headerFont));
                //table.AddCell(headerCell);
                //headerCell = new PdfPCell(new Phrase("Subtotal", headerFont));
                //table.AddCell(headerCell);

                // Fuente para el contenido de la tabla (normal)
                Font contentFont = new Font(Font.FontFamily.COURIER, 12, Font.NORMAL);

                foreach (var item in group)
                {
                    table.AddCell(new PdfPCell(new Phrase(item.Concepto, contentFont)));
                    table.AddCell(new PdfPCell(new Phrase(item.Total.ToString(), contentFont)));
                    //table.AddCell(new PdfPCell(new Phrase(item.Precio.ToString(), contentFont)));
                    //table.AddCell(new PdfPCell(new Phrase(item.Subtotal.ToString(), contentFont)));
                }

                // añadir datos a la tabla
                document.Add(table);

                // Add the total
                Paragraph total = new Paragraph($"Total de documento: {group.Sum(item => item.Total).ToString()}", contentFont);
                total.Alignment = Element.ALIGN_RIGHT;
                document.Add(total);

            }

            //linea separador
            LineSeparator line3 = new LineSeparator();
            line3.LineColor = BaseColor.BLUE; // Color de la línea
            line3.LineWidth = 1f; // Ancho de la línea
                                  // Agregar la línea al documento
            document.Add(new Chunk(line3));

            if (totalVentas > 0)
            {
                // Obtener el nombre del día en español
                CultureInfo spanishCulture = new CultureInfo("es-ES");
                string dayOfWeek = spanishCulture.DateTimeFormat.GetDayName(selectedDate.DayOfWeek);

                // añadir total del dia
                Paragraph totalSales = new Paragraph($"Total de gastos del día {dayOfWeek}: {totalVentas.ToString("C")}", new Font(Font.FontFamily.COURIER, 14, Font.BOLD));
                totalSales.Alignment = Element.ALIGN_RIGHT;
                document.Add(totalSales);
            }
            else
            {
                
                Paragraph totalSales1 = new Paragraph($"No hay compras para mostrar");
                totalSales1.Alignment = Element.ALIGN_CENTER;
                document.Add(totalSales1);
            }

            // Close the document
            document.Close();

            // Convierta el documento PDF en una matriz de bytes
            byte[] bytes = memoryStream.ToArray();

            return File(bytes, "application/pdf");
        }



        //[HttpGet]
        [HttpGet("Costweek")]
        public IActionResult GeneratePdfweek(int month, int weekNumber)
        {
            if (month > 12 || weekNumber > 31)
            {
                return BadRequest("No se puede realizar la acción");
            }


            int currentYear = DateTime.Today.Year;

            // Calcular la fecha de inicio de la semana y la fecha de fin de la semana
            DateTime weekStart = GetFirstDayOfWeek(currentYear, month, weekNumber);
            DateTime weekEnd = weekStart.AddDays(7);

            


            // Convert to DateOnly
            DateOnly weekStartDate = DateOnly.FromDateTime(weekStart);
            DateOnly weekEndDate = DateOnly.FromDateTime(weekEnd);


            //DateOnly selectedDate = DateOnly.FromDateTime(date);

            DateOnly selectedDate = DateOnly.FromDateTime(DateTime.Today);
            //DateOnly today = DateOnly.FromDateTime(new DateTime(2023, 3, 3));

            // Obtener los datos de las tablas de la base de datos solo para hoy
            var data = from p in _dbContext.Proveedores
                       join g in _dbContext.Gastos on p.IdProveedor equals g.IdProveedor
                       //where g.Fecha == selectedDate
                       where g.Fecha >= weekStartDate && g.Fecha < weekEndDate
                       select new
                       {
                           g.IdGasto,
                           g.NumeroDocumento,
                           g.Fecha,
                           g.Concepto,
                           g.Total,
                           g.IdProveedor,
                           p.Nombre,
                           p.Telefono,
                       };

            decimal totalVentas = data.Sum(item => item.Total ?? 0);

            // Group the data by customer
            var groupedData = data.GroupBy(item => item.IdGasto);

            //suma de hoy
            //decimal totalVentas = groupedData.Sum(group => group.Sum(item => item.Subtotal ?? 0));


            // Create a PDF document
            Document document = new Document();

            // Create a MemoryStream to save the PDF file
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);


            // Add metadata
            document.AddTitle("Facturas de gastos");
            document.AddSubject("Facturas de gastos de la base de datos");
            document.AddKeywords("gastos, base de datos, PDF");
            document.AddCreator("Mi aplicación");

            // Definir el estilo de fuente predeterminado
            //FontFactory.Register(openSansFontPath, "Roboto Mono");

            // Open the PDF document
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


            // Obtener la fecha de hoy
            DateTime daynow = DateTime.Now;
            string fechaHoy = daynow.ToString();

            // Agregar la descripción al encabezado
            PdfPCell descriptionCell = new PdfPCell(new Phrase($"Reporte Semanal de Gastos\n\nFecha de Impresión:\n{fechaHoy}", new Font(Font.FontFamily.COURIER, 10, Font.NORMAL)));
            descriptionCell.HorizontalAlignment = Element.ALIGN_CENTER;
            descriptionCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            descriptionCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            headerTable.AddCell(descriptionCell);

            // Agregar el encabezado al documento
            document.Add(headerTable);

            // Recorra los grupos y agregue una nueva página para cada cliente
            foreach (var group in groupedData)
            {

                // Linea separador
                LineSeparator line = new LineSeparator();
                line.LineColor = BaseColor.BLACK; // Color de la línea
                line.LineWidth = 1f; // Ancho de la línea
                // Agregar la línea al documento
                document.Add(new Chunk(line));


                Paragraph customer = new Paragraph($"Proveedor: {group.First().Nombre} \n" +
                                                   $"No. de Doc.: {group.First().NumeroDocumento} \n" +
                                                   $"Fecha de Registro: {group.First().Fecha} \n\n", new Font(Font.FontFamily.COURIER, 12, Font.BOLD));
                customer.Alignment = Element.ALIGN_LEFT;
                document.Add(customer);



                // añadir items
                PdfPTable table = new PdfPTable(2);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 1f, 1f });
                table.DefaultCell.Padding = 5;
                table.DefaultCell.BackgroundColor = new BaseColor(240, 240, 240);

                // Fuente para el encabezado (negrita)
                Font headerFont = new Font(Font.FontFamily.COURIER, 12, Font.BOLD);

                // Encabezado de la tabla en negrita
                PdfPCell headerCell = new PdfPCell(new Phrase("Concepto", headerFont));
                table.AddCell(headerCell);
                headerCell = new PdfPCell(new Phrase("Subtotal", headerFont));
                table.AddCell(headerCell);
  

                // Fuente para el contenido de la tabla (normal)
                Font contentFont = new Font(Font.FontFamily.COURIER, 12, Font.NORMAL);

                foreach (var item in group)
                {
                    table.AddCell(new PdfPCell(new Phrase(item.Concepto, contentFont)));
                    table.AddCell(new PdfPCell(new Phrase(item.Total.ToString(), contentFont)));
                   
                }

                // añadir datos a la tabla
                document.Add(table);

                // Add the total
                Paragraph total = new Paragraph($"Total de documento: {group.Sum(item => item.Total).ToString()}", contentFont);
                total.Alignment = Element.ALIGN_RIGHT;
                document.Add(total);

            }

            //linea separador
            LineSeparator line3 = new LineSeparator();
            line3.LineColor = BaseColor.BLUE; // Color de la línea
            line3.LineWidth = 1f; // Ancho de la línea
                                  // Agregar la línea al documento
            document.Add(new Chunk(line3));

            if (totalVentas > 0)
            {
                // Obtener el nombre del día en español
                CultureInfo spanishCulture = new CultureInfo("es-ES");
                string dayOfWeek = spanishCulture.DateTimeFormat.GetDayName(selectedDate.DayOfWeek);

                CultureInfo spanishCulture1 = new CultureInfo("es-ES");
                string nombreMes = spanishCulture1.DateTimeFormat.GetMonthName(month);

                // añadir total del dia
                Paragraph totalSales = new Paragraph($"Total de gastos de la semana {weekNumber} del mes de {nombreMes}: {totalVentas.ToString("C")}", new Font(Font.FontFamily.COURIER, 14, Font.BOLD));
                totalSales.Alignment = Element.ALIGN_RIGHT;
                document.Add(totalSales);
            }
            else
            {

                Paragraph totalSales1 = new Paragraph($"No hay compras para mostrar");
                totalSales1.Alignment = Element.ALIGN_CENTER;
                document.Add(totalSales1);
            }

            // Close the document
            document.Close();

            // Convierta el documento PDF en una matriz de bytes
            byte[] bytes = memoryStream.ToArray();

            return File(bytes, "application/pdf");
        }
        private DateTime GetFirstDayOfWeek(int year, int month, int weekNumber)
        {
            DateTime firstDayOfMonth = new DateTime(year, month, 1);
            DayOfWeek dayOfWeek = firstDayOfMonth.DayOfWeek;
            int daysToAdd = (int)DayOfWeek.Monday - (int)dayOfWeek;
            if (daysToAdd > 0)
                daysToAdd -= 7;

            int weeksToAdd = weekNumber - 1;

            return firstDayOfMonth.AddDays(daysToAdd + 7 * weeksToAdd);
        }


        [HttpGet("Costmonth")]
        public IActionResult GeneratePdfmonth(string month)
        {


            int currentYear = DateTime.Today.Year;

            // Obtener el número del mes a partir del nombre ingresado por el usuario
            int monthNumber = GetMonthNumber(month);

            // Verificar si se encontró un número de mes válido
            if (monthNumber == 0 || monthNumber > 12)
            {
                return BadRequest("Mes inválido");
            }

            // Obtener el primer día del mes seleccionado
            DateTime monthStart = new DateTime(currentYear, monthNumber, 1);

            // Obtener el primer día del mes siguiente
            DateTime nextMonthStart = monthStart.AddMonths(1);

            // Convertir a DateOnly
            DateOnly monthStartDate = DateOnly.FromDateTime(monthStart);
            DateOnly nextMonthStartDate = DateOnly.FromDateTime(nextMonthStart);




            //DateOnly selectedDate = DateOnly.FromDateTime(date);

            DateOnly selectedDate = DateOnly.FromDateTime(DateTime.Today);
            //DateOnly today = DateOnly.FromDateTime(new DateTime(2023, 3, 3));

            // Obtener los datos de las tablas de la base de datos solo para hoy
            var data = from p in _dbContext.Proveedores
                       join g in _dbContext.Gastos on p.IdProveedor equals g.IdProveedor
                       //where g.Fecha == selectedDate
                       where g.Fecha >= monthStartDate && g.Fecha < nextMonthStartDate
                       select new
                       {
                           g.IdGasto,
                           g.NumeroDocumento,
                           g.Fecha,
                           g.Concepto,
                           g.Total,
                           g.IdProveedor,
                           p.Nombre,
                           p.Telefono,
                       };

            decimal totalVentas = data.Sum(item => item.Total ?? 0);

            // Group the data by customer
            var groupedData = data.GroupBy(item => item.IdGasto);

            //suma de hoy
            //decimal totalVentas = groupedData.Sum(group => group.Sum(item => item.Subtotal ?? 0));


            // Create a PDF document
            Document document = new Document();

            // Create a MemoryStream to save the PDF file
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);


            // Add metadata
            document.AddTitle("Facturas de gastos");
            document.AddSubject("Facturas de gastos de la base de datos");
            document.AddKeywords("gastos, base de datos, PDF");
            document.AddCreator("Mi aplicación");

            // Definir el estilo de fuente predeterminado
            //FontFactory.Register(openSansFontPath, "Roboto Mono");

            // Open the PDF document
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


            // Obtener la fecha de hoy
            DateTime daynow = DateTime.Now;
            string fechaHoy = daynow.ToString();

            // Agregar la descripción al encabezado
            PdfPCell descriptionCell = new PdfPCell(new Phrase($"Reporte Mensual de Gastos\n\nFecha de Impresión:\n{fechaHoy}", new Font(Font.FontFamily.COURIER, 10, Font.NORMAL)));
            descriptionCell.HorizontalAlignment = Element.ALIGN_CENTER;
            descriptionCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            descriptionCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            headerTable.AddCell(descriptionCell);

            // Agregar el encabezado al documento
            document.Add(headerTable);

            // Recorra los grupos y agregue una nueva página para cada cliente
            foreach (var group in groupedData)
            {

                // Linea separador
                LineSeparator line = new LineSeparator();
                line.LineColor = BaseColor.BLACK; // Color de la línea
                line.LineWidth = 1f; // Ancho de la línea
                // Agregar la línea al documento
                document.Add(new Chunk(line));


                Paragraph customer = new Paragraph($"Proveedor: {group.First().Nombre} \n" +
                                                   $"No. de Doc.: {group.First().NumeroDocumento} \n" +
                                                   $"Fecha de Registro: {group.First().Fecha} \n\n", new Font(Font.FontFamily.COURIER, 12, Font.BOLD));
                customer.Alignment = Element.ALIGN_LEFT;
                document.Add(customer);



                // añadir items
                PdfPTable table = new PdfPTable(2);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 1f, 1f });
                table.DefaultCell.Padding = 5;
                table.DefaultCell.BackgroundColor = new BaseColor(240, 240, 240);

                // Fuente para el encabezado (negrita)
                Font headerFont = new Font(Font.FontFamily.COURIER, 12, Font.BOLD);

                // Encabezado de la tabla en negrita
                PdfPCell headerCell = new PdfPCell(new Phrase("Concepto", headerFont));
                table.AddCell(headerCell);
                headerCell = new PdfPCell(new Phrase("Subtotal", headerFont));
                table.AddCell(headerCell);
                //headerCell = new PdfPCell(new Phrase("Precio", headerFont));
                //table.AddCell(headerCell);
                //headerCell = new PdfPCell(new Phrase("Subtotal", headerFont));
                //table.AddCell(headerCell);

                // Fuente para el contenido de la tabla (normal)
                Font contentFont = new Font(Font.FontFamily.COURIER, 12, Font.NORMAL);

                foreach (var item in group)
                {
                    table.AddCell(new PdfPCell(new Phrase(item.Concepto, contentFont)));
                    table.AddCell(new PdfPCell(new Phrase(item.Total.ToString(), contentFont)));
                    //table.AddCell(new PdfPCell(new Phrase(item.Precio.ToString(), contentFont)));
                    //table.AddCell(new PdfPCell(new Phrase(item.Subtotal.ToString(), contentFont)));
                }

                // añadir datos a la tabla
                document.Add(table);

                // Add the total
                Paragraph total = new Paragraph($"Total de documento: {group.Sum(item => item.Total).ToString()}", contentFont);
                total.Alignment = Element.ALIGN_RIGHT;
                document.Add(total);

            }

            //linea separador
            LineSeparator line3 = new LineSeparator();
            line3.LineColor = BaseColor.BLUE; // Color de la línea
            line3.LineWidth = 1f; // Ancho de la línea
                                  // Agregar la línea al documento
            document.Add(new Chunk(line3));

            if (totalVentas > 0)
            {
                // Obtener el nombre del día en español
                CultureInfo spanishCulture = new CultureInfo("es-ES");
                string dayOfWeek = spanishCulture.DateTimeFormat.GetDayName(selectedDate.DayOfWeek);

                // Obtener el nombre del mes en español
                CultureInfo spanishCulture1 = new CultureInfo("es-ES");
                string nombreMes = spanishCulture1.DateTimeFormat.GetMonthName(monthNumber);

                

                // añadir total del dia
                Paragraph totalSales = new Paragraph($"Total de gastos del mes de {nombreMes}: {totalVentas.ToString("C")}", new Font(Font.FontFamily.COURIER, 14, Font.BOLD));
                totalSales.Alignment = Element.ALIGN_RIGHT;
                document.Add(totalSales);
            }
            else
            {

                Paragraph totalSales1 = new Paragraph($"No hay compras para mostrar");
                totalSales1.Alignment = Element.ALIGN_CENTER;
                document.Add(totalSales1);
            }

            // Close the document
            document.Close();

            // Convierta el documento PDF en una matriz de bytes
            byte[] bytes = memoryStream.ToArray();

            return File(bytes, "application/pdf");
        }

        private int GetMonthNumber(string month)
        {
            DateTimeFormatInfo dateTimeFormat = new DateTimeFormatInfo();
            List<string> monthNames = dateTimeFormat.MonthNames.ToList();
            int monthNumber = monthNames.FindIndex(m => m.Equals(month, StringComparison.OrdinalIgnoreCase)) + 1;
            return monthNumber;
        }


        [HttpGet("Costrange")]
        public IActionResult GeneratePdfrange([FromQuery] DateTime fechaDesde, DateTime fechaHasta)
        {

            DateOnly selectedDateDesde = DateOnly.FromDateTime(fechaDesde);
            DateOnly selectedDateHasta = DateOnly.FromDateTime(fechaHasta);


            //DateOnly selectedDate = DateOnly.FromDateTime(date);

            DateOnly selectedDate = DateOnly.FromDateTime(DateTime.Today);
            //DateOnly today = DateOnly.FromDateTime(new DateTime(2023, 3, 3));

            // Obtener los datos de las tablas de la base de datos solo para hoy
            var data = from p in _dbContext.Proveedores
                       join g in _dbContext.Gastos on p.IdProveedor equals g.IdProveedor
                       //where g.Fecha == selectedDate
                       //where g.Fecha >= weekStartDate && g.Fecha < weekEndDate
                       where g.Fecha >= selectedDateDesde && g.Fecha <= selectedDateHasta
                       select new
                       {
                           g.IdGasto,
                           g.NumeroDocumento,
                           g.Fecha,
                           g.Concepto,
                           g.Total,
                           g.IdProveedor,
                           p.Nombre,
                           p.Telefono,
                       };

            decimal totalVentas = data.Sum(item => item.Total ?? 0);

            // Group the data by customer
            var groupedData = data.GroupBy(item => item.IdGasto);

            //suma de hoy
            //decimal totalVentas = groupedData.Sum(group => group.Sum(item => item.Subtotal ?? 0));


            // Create a PDF document
            Document document = new Document();

            // Create a MemoryStream to save the PDF file
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);


            // Add metadata
            document.AddTitle("Facturas de gastos");
            document.AddSubject("Facturas de gastos de la base de datos");
            document.AddKeywords("gastos, base de datos, PDF");
            document.AddCreator("Mi aplicación");

            // Definir el estilo de fuente predeterminado
            //FontFactory.Register(openSansFontPath, "Roboto Mono");

            // Open the PDF document
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


            // Obtener la fecha de hoy
            DateTime daynow = DateTime.Now;
            string fechaHoy = daynow.ToString();

            // Agregar la descripción al encabezado
            PdfPCell descriptionCell = new PdfPCell(new Phrase($"Reporte de Gastos generado en el rango de fechas que eligió\n" +
                                                                $"Desde: {selectedDateDesde} \n Hasta: {selectedDateHasta}\n\nFecha de Impresión:\n{fechaHoy}", new Font(Font.FontFamily.COURIER, 10, Font.NORMAL)));
            descriptionCell.HorizontalAlignment = Element.ALIGN_CENTER;
            descriptionCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            descriptionCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            headerTable.AddCell(descriptionCell);

            // Agregar el encabezado al documento
            document.Add(headerTable);

            // Recorra los grupos y agregue una nueva página para cada cliente
            foreach (var group in groupedData)
            {

                // Linea separador
                LineSeparator line = new LineSeparator();
                line.LineColor = BaseColor.BLACK; // Color de la línea
                line.LineWidth = 1f; // Ancho de la línea
                // Agregar la línea al documento
                document.Add(new Chunk(line));


                Paragraph customer = new Paragraph($"Proveedor: {group.First().Nombre} \n" +
                                                   $"No. de Doc.: {group.First().NumeroDocumento} \n" +
                                                   $"Fecha de Registro: {group.First().Fecha} \n\n", new Font(Font.FontFamily.COURIER, 12, Font.BOLD));
                customer.Alignment = Element.ALIGN_LEFT;
                document.Add(customer);



                // añadir items
                PdfPTable table = new PdfPTable(2);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 1f, 1f });
                table.DefaultCell.Padding = 5;
                table.DefaultCell.BackgroundColor = new BaseColor(240, 240, 240);

                // Fuente para el encabezado (negrita)
                Font headerFont = new Font(Font.FontFamily.COURIER, 12, Font.BOLD);

                // Encabezado de la tabla en negrita
                PdfPCell headerCell = new PdfPCell(new Phrase("Concepto", headerFont));
                table.AddCell(headerCell);
                headerCell = new PdfPCell(new Phrase("Subtotal", headerFont));
                table.AddCell(headerCell);
                //headerCell = new PdfPCell(new Phrase("Precio", headerFont));
                //table.AddCell(headerCell);
                //headerCell = new PdfPCell(new Phrase("Subtotal", headerFont));
                //table.AddCell(headerCell);

                // Fuente para el contenido de la tabla (normal)
                Font contentFont = new Font(Font.FontFamily.COURIER, 12, Font.NORMAL);

                foreach (var item in group)
                {
                    table.AddCell(new PdfPCell(new Phrase(item.Concepto, contentFont)));
                    table.AddCell(new PdfPCell(new Phrase(item.Total.ToString(), contentFont)));
                    //table.AddCell(new PdfPCell(new Phrase(item.Precio.ToString(), contentFont)));
                    //table.AddCell(new PdfPCell(new Phrase(item.Subtotal.ToString(), contentFont)));
                }

                // añadir datos a la tabla
                document.Add(table);

                // Add the total
                Paragraph total = new Paragraph($"Total de documento: {group.Sum(item => item.Total).ToString()}", contentFont);
                total.Alignment = Element.ALIGN_RIGHT;
                document.Add(total);

            }

            //linea separador
            LineSeparator line3 = new LineSeparator();
            line3.LineColor = BaseColor.BLUE; // Color de la línea
            line3.LineWidth = 1f; // Ancho de la línea
                                  // Agregar la línea al documento
            document.Add(new Chunk(line3));

            if (totalVentas > 0)
            {
                // Obtener el nombre del día en español
                CultureInfo spanishCulture = new CultureInfo("es-ES");
                string dayOfWeek = spanishCulture.DateTimeFormat.GetDayName(selectedDate.DayOfWeek);

                // añadir total del dia
                Paragraph totalSales = new Paragraph($"Total de gastos desde la fecha {selectedDateDesde} hasta {selectedDateHasta}: {totalVentas.ToString("C")}", new Font(Font.FontFamily.COURIER, 14, Font.BOLD));
                totalSales.Alignment = Element.ALIGN_RIGHT;
                document.Add(totalSales);
            }
            else
            {

                Paragraph totalSales1 = new Paragraph($"No hay compras para mostrar");
                totalSales1.Alignment = Element.ALIGN_CENTER;
                document.Add(totalSales1);
            }

            // Close the document
            document.Close();

            // Convierta el documento PDF en una matriz de bytes
            byte[] bytes = memoryStream.ToArray();

            return File(bytes, "application/pdf");
        }


        [HttpGet("Costyear")]
        public IActionResult GeneratePdfyear()
        {

            int currentYear = DateTime.Today.Year;

            // Obtener la fecha de inicio y fin del año actual
            DateTime yearStart = new DateTime(currentYear, 1, 1);
            DateTime yearEnd = yearStart.AddYears(1);

            // Convertir a DateOnly
            DateOnly yearStartDate = DateOnly.FromDateTime(yearStart);
            DateOnly yearEndDate = DateOnly.FromDateTime(yearEnd);



            //DateOnly selectedDate = DateOnly.FromDateTime(date);

            DateOnly selectedDate = DateOnly.FromDateTime(DateTime.Today);
            //DateOnly today = DateOnly.FromDateTime(new DateTime(2023, 3, 3));

            // Obtener los datos de las tablas de la base de datos solo para hoy
            var data = from p in _dbContext.Proveedores
                       join g in _dbContext.Gastos on p.IdProveedor equals g.IdProveedor
                       //where g.Fecha == selectedDate
                       where g.Fecha >= yearStartDate && g.Fecha < yearEndDate
                       //where g.Fecha >= monthStartDate && g.Fecha < nextMonthStartDate
                       select new
                       {
                           g.IdGasto,
                           g.NumeroDocumento,
                           g.Fecha,
                           g.Concepto,
                           g.Total,
                           g.IdProveedor,
                           p.Nombre,
                           p.Telefono,
                       };

            decimal totalVentas = data.Sum(item => item.Total ?? 0);

            // Group the data by customer
            var groupedData = data.GroupBy(item => item.IdGasto);

            //suma de hoy
            //decimal totalVentas = groupedData.Sum(group => group.Sum(item => item.Subtotal ?? 0));


            // Create a PDF document
            Document document = new Document();

            // Create a MemoryStream to save the PDF file
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);


            // Add metadata
            document.AddTitle("Facturas de gastos");
            document.AddSubject("Facturas de gastos de la base de datos");
            document.AddKeywords("gastos, base de datos, PDF");
            document.AddCreator("Mi aplicación");

            // Definir el estilo de fuente predeterminado
            //FontFactory.Register(openSansFontPath, "Roboto Mono");

            // Open the PDF document
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


            // Obtener la fecha de hoy
            DateTime daynow = DateTime.Now;
            string fechaHoy = daynow.ToString();

            // Agregar la descripción al encabezado
            PdfPCell descriptionCell = new PdfPCell(new Phrase($"Reporte Anual de Gastos\n\nFecha de Impresión:\n{fechaHoy}", new Font(Font.FontFamily.COURIER, 10, Font.NORMAL)));
            descriptionCell.HorizontalAlignment = Element.ALIGN_CENTER;
            descriptionCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            descriptionCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            headerTable.AddCell(descriptionCell);

            // Agregar el encabezado al documento
            document.Add(headerTable);

            // Recorra los grupos y agregue una nueva página para cada cliente
            foreach (var group in groupedData)
            {

                // Linea separador
                LineSeparator line = new LineSeparator();
                line.LineColor = BaseColor.BLACK; // Color de la línea
                line.LineWidth = 1f; // Ancho de la línea
                // Agregar la línea al documento
                document.Add(new Chunk(line));


                Paragraph customer = new Paragraph($"Proveedor: {group.First().Nombre} \n" +
                                                   $"No. de Doc.: {group.First().NumeroDocumento} \n" +
                                                   $"Fecha de Registro: {group.First().Fecha} \n\n", new Font(Font.FontFamily.COURIER, 12, Font.BOLD));
                customer.Alignment = Element.ALIGN_LEFT;
                document.Add(customer);



                // añadir items
                PdfPTable table = new PdfPTable(2);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 1f, 1f });
                table.DefaultCell.Padding = 5;
                table.DefaultCell.BackgroundColor = new BaseColor(240, 240, 240);

                // Fuente para el encabezado (negrita)
                Font headerFont = new Font(Font.FontFamily.COURIER, 12, Font.BOLD);

                // Encabezado de la tabla en negrita
                PdfPCell headerCell = new PdfPCell(new Phrase("Concepto", headerFont));
                table.AddCell(headerCell);
                headerCell = new PdfPCell(new Phrase("Subtotal", headerFont));
                table.AddCell(headerCell);
                //headerCell = new PdfPCell(new Phrase("Precio", headerFont));
                //table.AddCell(headerCell);
                //headerCell = new PdfPCell(new Phrase("Subtotal", headerFont));
                //table.AddCell(headerCell);

                // Fuente para el contenido de la tabla (normal)
                Font contentFont = new Font(Font.FontFamily.COURIER, 12, Font.NORMAL);

                foreach (var item in group)
                {
                    table.AddCell(new PdfPCell(new Phrase(item.Concepto, contentFont)));
                    table.AddCell(new PdfPCell(new Phrase(item.Total.ToString(), contentFont)));
                    //table.AddCell(new PdfPCell(new Phrase(item.Precio.ToString(), contentFont)));
                    //table.AddCell(new PdfPCell(new Phrase(item.Subtotal.ToString(), contentFont)));
                }

                // añadir datos a la tabla
                document.Add(table);

                // Add the total
                Paragraph total = new Paragraph($"Total de documento: {group.Sum(item => item.Total).ToString()}", contentFont);
                total.Alignment = Element.ALIGN_RIGHT;
                document.Add(total);

            }

            //linea separador
            LineSeparator line3 = new LineSeparator();
            line3.LineColor = BaseColor.BLUE; // Color de la línea
            line3.LineWidth = 1f; // Ancho de la línea
                                  // Agregar la línea al documento
            document.Add(new Chunk(line3));

            if (totalVentas > 0)
            {
                // Obtener el nombre del día en español
                CultureInfo spanishCulture = new CultureInfo("es-ES");
                string dayOfWeek = spanishCulture.DateTimeFormat.GetDayName(selectedDate.DayOfWeek);

                // añadir total del dia
                Paragraph totalSales = new Paragraph($"Total de gastos del año {currentYear}: {totalVentas.ToString("C")}", new Font(Font.FontFamily.COURIER, 14, Font.BOLD));
                totalSales.Alignment = Element.ALIGN_RIGHT;
                document.Add(totalSales);
            }
            else
            {

                Paragraph totalSales1 = new Paragraph($"No hay compras para mostrar");
                totalSales1.Alignment = Element.ALIGN_CENTER;
                document.Add(totalSales1);
            }

            // Close the document
            document.Close();

            // Convierta el documento PDF en una matriz de bytes
            byte[] bytes = memoryStream.ToArray();

            return File(bytes, "application/pdf");
        }




    }
}
