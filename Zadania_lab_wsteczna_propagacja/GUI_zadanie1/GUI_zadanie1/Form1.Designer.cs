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
            // 
            // btnWyswietl
            // 
            btnWyswietl.BackColor = Color.AliceBlue;
            btnWyswietl.Font = new Font("Century", 11F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            btnWyswietl.Location = new Point(137, 41);
            btnWyswietl.Margin = new Padding(4, 5, 4, 5);
            btnWyswietl.Name = "btnWyswietl";
            btnWyswietl.Size = new Size(435, 47);
            btnWyswietl.TabIndex = 0;
            btnWyswietl.Text = "Kliknij, aby wyświetlić wyniki";
            btnWyswietl.UseVisualStyleBackColor = false;
            btnWyswietl.Click += btnWyswietl_Click;
            // 
            // outputBox
            // 
            outputBox.BackColor = Color.FromArgb(224, 224, 224);
            outputBox.Font = new Font("Franklin Gothic Book", 10F, FontStyle.Bold, GraphicsUnit.Point, 238);
            outputBox.Location = new Point(54, 119);
            outputBox.Margin = new Padding(4, 5, 4, 5);
            outputBox.Multiline = true;
            outputBox.Name = "outputBox";
            outputBox.ReadOnly = true;
            outputBox.Size = new Size(596, 352);
            outputBox.TabIndex = 1;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Azure;
            ClientSize = new Size(700, 500);
            Controls.Add(btnWyswietl);
            Controls.Add(outputBox);
            Margin = new Padding(4, 5, 4, 5);
            Name = "Form1";
            Text = "Zadanie1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
