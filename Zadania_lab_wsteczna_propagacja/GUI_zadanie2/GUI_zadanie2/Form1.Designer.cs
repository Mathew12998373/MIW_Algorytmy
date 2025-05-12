namespace SiecNeuronowaGUI
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnWyswietl;
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
            this.btnWyswietl = new System.Windows.Forms.Button();
            this.outputBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnWyswietl
            // 
            this.btnWyswietl.BackColor = System.Drawing.Color.Azure;
            this.btnWyswietl.Font = new System.Drawing.Font("Book Antiqua", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWyswietl.Location = new System.Drawing.Point(150, 33);
            this.btnWyswietl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnWyswietl.Name = "btnWyswietl";
            this.btnWyswietl.Size = new System.Drawing.Size(384, 59);
            this.btnWyswietl.TabIndex = 0;
            this.btnWyswietl.Text = "Kliknij, aby wyświetlić wyniki";
            this.btnWyswietl.UseVisualStyleBackColor = false;
            this.btnWyswietl.Click += new System.EventHandler(this.btnWyswietl_Click);
            // 
            // outputBox
            // 
            this.outputBox.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.outputBox.Font = new System.Drawing.Font("Perpetua", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outputBox.Location = new System.Drawing.Point(40, 110);
            this.outputBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.outputBox.Multiline = true;
            this.outputBox.Name = "outputBox";
            this.outputBox.ReadOnly = true;
            this.outputBox.Size = new System.Drawing.Size(614, 354);
            this.outputBox.TabIndex = 1;
            this.outputBox.TextChanged += new System.EventHandler(this.outputBox_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(700, 500);
            this.Controls.Add(this.btnWyswietl);
            this.Controls.Add(this.outputBox);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "Zadanie2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
