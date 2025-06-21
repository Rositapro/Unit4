
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using ScottPlot;
using ScottPlot.TickGenerators;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using OpenXmlWord = DocumentFormat.OpenXml.Wordprocessing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestDoc = QuestPDF.Fluent.Document;
using QColors = QuestPDF.Helpers.Colors;
using QuestPDF.Infrastructure;

namespace Unit4
{
    public partial class Form1 : Form
    {
        private List<Venta> ventas = new List<Venta>();

        public Form1()
        {
            QuestPDF.Settings.License = LicenseType.Community;
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

            using (WordprocessingDocument doc = WordprocessingDocument.Create(ruta, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = doc.AddMainDocumentPart();
                mainPart.Document = new OpenXmlWord.Document();
                OpenXmlWord.Body body = mainPart.Document.AppendChild(new OpenXmlWord.Body());

                var title = new OpenXmlWord.Paragraph(new OpenXmlWord.Run(new OpenXmlWord.Text("REPORTE DE VENTAS")))
                {
                    ParagraphProperties = new OpenXmlWord.ParagraphProperties(
                        new OpenXmlWord.Justification() { Val = OpenXmlWord.JustificationValues.Center })
                };
                body.Append(title);

                OpenXmlWord.Table table = new OpenXmlWord.Table();
                OpenXmlWord.TableRow headerRow = new OpenXmlWord.TableRow();
                string[] headers = { "Producto", "Categoría", "Cantidad", "Precio", "Total" };
                foreach (string header in headers)
                {
                    var cell = new OpenXmlWord.TableCell(
                        new OpenXmlWord.Paragraph(
                            new OpenXmlWord.Run(new OpenXmlWord.Text(header))));
                    headerRow.Append(cell);
                }
                table.Append(headerRow);

                foreach (var v in ventas)
                {
                    var row = new OpenXmlWord.TableRow();
                    row.Append(new OpenXmlWord.TableCell(new OpenXmlWord.Paragraph(new OpenXmlWord.Run(new OpenXmlWord.Text(v.Producto)))));
                    row.Append(new OpenXmlWord.TableCell(new OpenXmlWord.Paragraph(new OpenXmlWord.Run(new OpenXmlWord.Text(v.Categoria)))));
                    row.Append(new OpenXmlWord.TableCell(new OpenXmlWord.Paragraph(new OpenXmlWord.Run(new OpenXmlWord.Text(v.Cantidad.ToString())))));
                    row.Append(new OpenXmlWord.TableCell(new OpenXmlWord.Paragraph(new OpenXmlWord.Run(new OpenXmlWord.Text(v.Precio.ToString("C"))))));
                    row.Append(new OpenXmlWord.TableCell(new OpenXmlWord.Paragraph(new OpenXmlWord.Run(new OpenXmlWord.Text(v.Total.ToString("C"))))));
                    table.Append(row);
                }

                body.Append(table);
            }

            MessageBox.Show("Exportado a Word (OpenXML) correctamente.");
        }

        private void btnExportarPDF_Click(object sender, EventArgs e)
        {

            try
            {
                string ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ReporteVentas_QuestPDF.pdf");

                QuestDoc.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(50);
                        page.PageColor(QColors.White);
                        page.DefaultTextStyle(x => x.FontSize(12));

                        page.Header().Text("REPORTE DE VENTAS").SemiBold().FontSize(20).FontColor(QColors.Blue.Medium);

                        page.Content().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Producto").Bold();
                                header.Cell().Element(CellStyle).Text("Categoría").Bold();
                                header.Cell().Element(CellStyle).Text("Cantidad").Bold();
                                header.Cell().Element(CellStyle).Text("Precio").Bold();
                                header.Cell().Element(CellStyle).Text("Total").Bold();
                            });

                            foreach (var v in ventas)
                            {
                                table.Cell().Element(CellStyle).Text(v.Producto);
                                table.Cell().Element(CellStyle).Text(v.Categoria);
                                table.Cell().Element(CellStyle).Text(v.Cantidad.ToString());
                                table.Cell().Element(CellStyle).Text(v.Precio.ToString("C"));
                                table.Cell().Element(CellStyle).Text(v.Total.ToString("C"));
                            }

                            static IContainer CellStyle(IContainer container) =>
                                container.PaddingVertical(5).PaddingHorizontal(5);
                        });

                        page.Footer().AlignCenter().Text(x =>
                        {
                            x.Span("Generado con QuestPDF - ");
                            x.Span(DateTime.Now.ToString("dd/MM/yyyy")).SemiBold();
                        });
                    });
                })
                .GeneratePdf(ruta);

                MessageBox.Show("✅ PDF exportado correctamente en el escritorio.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error al exportar a PDF:\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
