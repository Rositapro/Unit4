namespace Unit4
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnCargarDatos = new Button();
            btnExportarPDF = new Button();
            btnExportarWord = new Button();
            btnGraficar = new Button();
            formsPlot = new ScottPlot.WinForms.FormsPlot();
            dgv = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
            SuspendLayout();
            // 
            // btnCargarDatos
            // 
            btnCargarDatos.Location = new Point(62, 74);
            btnCargarDatos.Margin = new Padding(4, 5, 4, 5);
            btnCargarDatos.Name = "btnCargarDatos";
            btnCargarDatos.Size = new Size(107, 38);
            btnCargarDatos.TabIndex = 0;
            btnCargarDatos.Text = "Cargar";
            btnCargarDatos.UseVisualStyleBackColor = true;
            btnCargarDatos.Click += btnCargarDatos_Click;
            // 
            // btnExportarPDF
            // 
            btnExportarPDF.Location = new Point(62, 218);
            btnExportarPDF.Margin = new Padding(4, 5, 4, 5);
            btnExportarPDF.Name = "btnExportarPDF";
            btnExportarPDF.Size = new Size(107, 38);
            btnExportarPDF.TabIndex = 1;
            btnExportarPDF.Text = "PDF";
            btnExportarPDF.UseVisualStyleBackColor = true;
            btnExportarPDF.Click += btnExportarPDF_Click;
            // 
            // btnExportarWord
            // 
            btnExportarWord.Location = new Point(62, 170);
            btnExportarWord.Margin = new Padding(4, 5, 4, 5);
            btnExportarWord.Name = "btnExportarWord";
            btnExportarWord.Size = new Size(107, 38);
            btnExportarWord.TabIndex = 2;
            btnExportarWord.Text = "Word";
            btnExportarWord.UseVisualStyleBackColor = true;
            btnExportarWord.Click += btnExportarWord_Click;
            // 
            // btnGraficar
            // 
            btnGraficar.Location = new Point(62, 122);
            btnGraficar.Margin = new Padding(4, 5, 4, 5);
            btnGraficar.Name = "btnGraficar";
            btnGraficar.Size = new Size(107, 38);
            btnGraficar.TabIndex = 3;
            btnGraficar.Text = "Graficar";
            btnGraficar.UseVisualStyleBackColor = true;
            btnGraficar.Click += btnGraficar_Click;
            // 
            // formsPlot
            // 
            formsPlot.DisplayScale = 1.5F;
            formsPlot.Location = new Point(269, 387);
            formsPlot.Name = "formsPlot";
            formsPlot.Size = new Size(569, 292);
            formsPlot.TabIndex = 4;
            // 
            // dgv
            // 
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.Location = new Point(285, 40);
            dgv.Name = "dgv";
            dgv.RowHeadersWidth = 62;
            dgv.Size = new Size(788, 341);
            dgv.TabIndex = 5;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1143, 750);
            Controls.Add(dgv);
            Controls.Add(formsPlot);
            Controls.Add(btnGraficar);
            Controls.Add(btnExportarWord);
            Controls.Add(btnExportarPDF);
            Controls.Add(btnCargarDatos);
            Margin = new Padding(4, 5, 4, 5);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dgv).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btnCargarDatos;
        private Button btnExportarPDF;
        private Button btnExportarWord;
        private Button btnGraficar;
        private ScottPlot.WinForms.FormsPlot formsPlot;
        private DataGridView dgv;
    }
}
