namespace Zadanie3
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnTrenuj;
        private System.Windows.Forms.TextBox outputBox;

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
            this.btnTrenuj = new System.Windows.Forms.Button();
            this.outputBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();

            this.btnTrenuj.Font = new System.Drawing.Font("Perpetua Titling MT", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTrenuj.Location = new System.Drawing.Point(112, 44);
            this.btnTrenuj.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnTrenuj.Name = "btnTrenuj";
            this.btnTrenuj.Size = new System.Drawing.Size(464, 38);
            this.btnTrenuj.TabIndex = 0;
            this.btnTrenuj.Text = "Kliknij, aby wyświetlić wynik";
            this.btnTrenuj.UseVisualStyleBackColor = true;
            this.btnTrenuj.Click += new System.EventHandler(this.btnWyswietl_Click);

            this.outputBox.BackColor = System.Drawing.Color.LightGray;
            this.outputBox.Location = new System.Drawing.Point(57, 113);
            this.outputBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.outputBox.Multiline = true;
            this.outputBox.Name = "outputBox";
            this.outputBox.ReadOnly = true;
            this.outputBox.Size = new System.Drawing.Size(564, 377);
            this.outputBox.TabIndex = 2;
            this.outputBox.TextChanged += new System.EventHandler(this.outputBox_TextChanged);

            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(678, 544);
            this.Controls.Add(this.btnTrenuj);
            this.Controls.Add(this.outputBox);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "Zadanie3";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
