namespace GeneticAlgorithmGUI
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button BtnUruchom;
        private System.Windows.Forms.ListBox listBoxWyniki;
        private System.Windows.Forms.Label labelNajlepszy;
        private System.Windows.Forms.Label labelSrednia;

        private void InitializeComponent()
        {
            this.BtnUruchom = new System.Windows.Forms.Button();
            this.listBoxWyniki = new System.Windows.Forms.ListBox();
            this.labelNajlepszy = new System.Windows.Forms.Label();
            this.labelSrednia = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // Uruchomienie algorytmu
            this.BtnUruchom.Font = new System.Drawing.Font("Baskerville Old Face", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnUruchom.ForeColor = System.Drawing.SystemColors.InfoText;
            this.BtnUruchom.Location = new System.Drawing.Point(202, 14);
            this.BtnUruchom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BtnUruchom.Name = "BtnUruchom";
            this.BtnUruchom.Size = new System.Drawing.Size(233, 46);
            this.BtnUruchom.TabIndex = 0;
            this.BtnUruchom.Text = "Uruchom algorytm";
            this.BtnUruchom.UseVisualStyleBackColor = true;
            this.BtnUruchom.Click += new System.EventHandler(this.BtnUruchom_Click);
            // Wyniki
            this.listBoxWyniki.FormattingEnabled = true;
            this.listBoxWyniki.ItemHeight = 20;
            this.listBoxWyniki.Location = new System.Drawing.Point(18, 77);
            this.listBoxWyniki.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.listBoxWyniki.Name = "listBoxWyniki";
            this.listBoxWyniki.Size = new System.Drawing.Size(523, 304);
            this.listBoxWyniki.TabIndex = 1;
            // Najlepszy osobnik
            this.labelNajlepszy.AutoSize = true;
            this.labelNajlepszy.Font = new System.Drawing.Font("Algerian", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNajlepszy.Location = new System.Drawing.Point(18, 400);
            this.labelNajlepszy.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNajlepszy.Name = "labelNajlepszy";
            this.labelNajlepszy.Size = new System.Drawing.Size(140, 22);
            this.labelNajlepszy.TabIndex = 2;
            this.labelNajlepszy.Text = "Najlepszy: ";
            // Srednia
            this.labelSrednia.AutoSize = true;
            this.labelSrednia.Font = new System.Drawing.Font("Algerian", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSrednia.Location = new System.Drawing.Point(18, 446);
            this.labelSrednia.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSrednia.Name = "labelSrednia";
            this.labelSrednia.Size = new System.Drawing.Size(274, 22);
            this.labelSrednia.TabIndex = 3;
            this.labelSrednia.Text = "Średnia dostosowania: ";
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSlateGray;
            this.ClientSize = new System.Drawing.Size(600, 508);
            this.Controls.Add(this.BtnUruchom);
            this.Controls.Add(this.listBoxWyniki);
            this.Controls.Add(this.labelNajlepszy);
            this.Controls.Add(this.labelSrednia);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainForm";
            this.Text = "Algorytm Genetyczny";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
