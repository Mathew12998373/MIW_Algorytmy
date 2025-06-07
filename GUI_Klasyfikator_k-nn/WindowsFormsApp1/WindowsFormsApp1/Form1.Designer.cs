namespace KlasyfikacjaApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnKlasyfikuj;
        private System.Windows.Forms.TextBox txtWynik;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnKlasyfikuj = new System.Windows.Forms.Button();
            this.txtWynik = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnKlasyfikuj
            // 
            this.btnKlasyfikuj.Font = new System.Drawing.Font("Arial Narrow", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnKlasyfikuj.Location = new System.Drawing.Point(139, 29);
            this.btnKlasyfikuj.Name = "btnKlasyfikuj";
            this.btnKlasyfikuj.Size = new System.Drawing.Size(283, 61);
            this.btnKlasyfikuj.TabIndex = 2;
            this.btnKlasyfikuj.Text = "Kliknij, aby zobaczyć wynik";
            this.btnKlasyfikuj.Click += new System.EventHandler(this.btnKlasyfikuj_Click);
            // 
            // txtWynik
            // 
            this.txtWynik.BackColor = System.Drawing.SystemColors.ControlDark;
            this.txtWynik.Font = new System.Drawing.Font("Arial Narrow", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txtWynik.Location = new System.Drawing.Point(81, 117);
            this.txtWynik.Multiline = true;
            this.txtWynik.Name = "txtWynik";
            this.txtWynik.ReadOnly = true;
            this.txtWynik.Size = new System.Drawing.Size(412, 188);
            this.txtWynik.TabIndex = 3;
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(578, 344);
            this.Controls.Add(this.btnKlasyfikuj);
            this.Controls.Add(this.txtWynik);
            this.Name = "Form1";
            this.Text = "Klasyfikator KNN";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
