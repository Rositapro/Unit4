using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using ScottPlot;
using ScottPlot.TickGenerators;
using System.Data;

namespace Unit4
{
    public partial class Form1 : Form
    {
        private List<Venta> ventas = new List<Venta>();

        public Form1()
        {
            InitializeComponent();
            btnCargarDatos.Click += btnCargarDatos_Click;
            btnGraficar.Click += btnGraficar_Click;
            btnExportarWord.Click += btnExportarWord_Click;
            btnExportarPDF.Click += btnExportarPDF_Click;
        }

        private void btnCargarDatos_Click(object sender, EventArgs e)
        {
            ventas = new List<Venta>
            {
                new Venta("Laptop", "Electrónica", 2, 12000),
                new Venta("Mouse", "Electrónica", 5, 300),
                new Venta("Escritorio", "Muebles", 1, 5000),
                new Venta("Silla", "Muebles", 3, 1500),
                new Venta("Audífonos", "Electrónica", 4, 800)
            };

            var dt = new System.Data.DataTable();
            dt.Columns.Add("Producto");
            dt.Columns.Add("Categoría");
            dt.Columns.Add("Cantidad");
            dt.Columns.Add("Precio Unitario");
            dt.Columns.Add("Total");

            foreach (var v in ventas)
                dt.Rows.Add(v.Producto, v.Categoria, v.Cantidad, v.Precio.ToString("C"), v.Total.ToString("C"));

            dgv.DataSource = dt;
        }

        private void btnGraficar_Click(object sender, EventArgs e)
        {
            var productos = ventas.Select(v => v.Producto).ToArray();
            var totales = ventas.Select(v => (double)v.Total).ToArray();

            var plt = formsPlot.Plot;
            plt.Clear();

            // 1. Agregar barras
            plt.Add.Bars(values: totales);

            // 2. Crear ticks personalizados
            double[] posiciones = Enumerable.Range(0, productos.Length).Select(i => (double)i).ToArray();
            var ticks = posiciones.Zip(productos, (pos, label) => new Tick(pos, label)).ToArray();
            plt.Axes.Bottom.TickGenerator = new NumericManual(ticks);

            // 3. Rotar etiquetas: aquí está la corrección
            var estiloTicks = plt.Axes.Bottom.TickLabelStyle;
            estiloTicks.Rotation = 45;
            plt.Axes.Bottom.TickLabelStyle = estiloTicks;

            // 4. Títulos
            plt.Title("Ventas por Producto");
            plt.Axes.Left.Label.Text = "Monto ($)";
            plt.Axes.Bottom.Label.Text = "Producto";

            formsPlot.Refresh();
        }

        private void btnExportarWord_Click(object sender, EventArgs e)
        {
            string ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ReporteVentas_OpenXML.docx");

            using (WordprocessingDocument doc = WordprocessingDocument.Create(ruta, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = doc.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());

                // Título
                body.AppendChild(new Paragraph(new Run(new Text("REPORTE DE VENTAS"))) { ParagraphProperties = new ParagraphProperties(new Justification() { Val = JustificationValues.Center }) });

                // Tabla
                Table table = new Table();

                // Encabezados
                TableRow headerRow = new TableRow();
                string[] headers = { "Producto", "Categoría", "Cantidad", "Precio", "Total" };
                foreach (string header in headers)
                {
                    TableCell cell = new TableCell(new Paragraph(new Run(new Text(header))));
                    headerRow.Append(cell);
                }
                table.Append(headerRow);

                // Datos
                foreach (var v in ventas)
                {
                    TableRow row = new TableRow();
                    row.Append(new TableCell(new Paragraph(new Run(new Text(v.Producto)))));
                    row.Append(new TableCell(new Paragraph(new Run(new Text(v.Categoria)))));
                    row.Append(new TableCell(new Paragraph(new Run(new Text(v.Cantidad.ToString())))));
                    row.Append(new TableCell(new Paragraph(new Run(new Text(v.Precio.ToString("C"))))));
                    row.Append(new TableCell(new Paragraph(new Run(new Text(v.Total.ToString("C"))))));
                    table.Append(row);
                }

                body.Append(table);
            }

            MessageBox.Show("Exportado a Word (OpenXML) correctamente.");
        }

        private void btnExportarPDF_Click(object sender, EventArgs e)
        {
            //string ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ReporteVentas.pdf");

            //PdfDocument doc = new PdfDocument();
            //PdfPage page = doc.AddPage();
            //XGraphics gfx = XGraphics.FromPdfPage(page);

            //XFont titleFont = new XFont("Arial", 16, XFontStyle.Bold);
            //XFont font = new XFont("Arial", 12, XFontStyle.Regular);

            //int y = 40;
            //gfx.DrawString("REPORTE DE VENTAS", titleFont, XBrushes.Black, new XPoint(40, y));
            //y += 30;

            //foreach (var v in ventas)
            //{
            //    string linea = $"{v.Producto} | {v.Categoria} | {v.Cantidad} | {v.Precio:C} | {v.Total:C}";
            //    gfx.DrawString(linea, font, XBrushes.Black, new XPoint(40, y));
            //    y += 20;
            //}

            //doc.Save(ruta);
            //MessageBox.Show("Exportado a PDF correctamente.");
        }
        public class Venta
        {
            public string Producto { get; set; }
            public string Categoria { get; set; }
            public int Cantidad { get; set; }
            public decimal Precio { get; set; }
            public decimal Total => Cantidad * Precio;

            public Venta(string prod, string cat, int cant, decimal precio)
            {
                Producto = prod;
                Categoria = cat;
                Cantidad = cant;
                Precio = precio;
            }
        }
    }
}
