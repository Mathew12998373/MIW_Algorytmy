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
            btnWyswietl = new Button();
            outputBox = new TextBox();
            SuspendLayout();
            btnWyswietl.BackColor = Color.AliceBlue;
            btnWyswietl.Font = new Font("Century", 11F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            btnWyswietl.Location = new Point(131, 34);
            btnWyswietl.Margin = new Padding(4, 5, 4, 5);
            btnWyswietl.Name = "btnWyswietl";
            btnWyswietl.Size = new Size(372, 47);
            btnWyswietl.TabIndex = 0;
            btnWyswietl.Text = "Kliknij, aby wyświetlić wyniki";
            btnWyswietl.UseVisualStyleBackColor = false;
            btnWyswietl.Click += btnWyswietl_Click;

            outputBox.BackColor = Color.FromArgb(224, 224, 224);
            outputBox.Font = new Font("Franklin Gothic Book", 10F, FontStyle.Bold, GraphicsUnit.Point, 238);
            outputBox.Location = new Point(84, 111);
            outputBox.Margin = new Padding(4, 5, 4, 5);
            outputBox.Multiline = true;
            outputBox.Name = "outputBox";
            outputBox.ReadOnly = true;
            outputBox.Size = new Size(450, 300);
            outputBox.TabIndex = 1;

            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Azure;
            ClientSize = new Size(628, 444);
            Controls.Add(btnWyswietl);
            Controls.Add(outputBox);
            Margin = new Padding(4, 5, 4, 5);
            Name = "Form1";
            Text = "Sieć Neuronowa GUI";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
