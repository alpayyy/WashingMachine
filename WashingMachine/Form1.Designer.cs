namespace WashingMachine
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
            lblHassaslik = new Label();
            textBox1 = new TextBox();
            lblMiktar = new Label();
            lblKirlilik = new Label();
            SuspendLayout();
            // 
            // lblHassaslik
            // 
            lblHassaslik.AutoSize = true;
            lblHassaslik.Location = new Point(80, 165);
            lblHassaslik.Name = "lblHassaslik";
            lblHassaslik.Size = new Size(118, 20);
            lblHassaslik.TabIndex = 1;
            lblHassaslik.Text = "Hassaslık Değeri";
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI", 28.2F, FontStyle.Bold, GraphicsUnit.Point);
            textBox1.Location = new Point(161, 28);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(545, 70);
            textBox1.TabIndex = 0;
            textBox1.Text = "  WASHİNG MACHİNE";
            // 
            // lblMiktar
            // 
            lblMiktar.AutoSize = true;
            lblMiktar.Location = new Point(80, 227);
            lblMiktar.Name = "lblMiktar";
            lblMiktar.Size = new Size(51, 20);
            lblMiktar.TabIndex = 2;
            lblMiktar.Text = "Miktar";
            // 
            // lblKirlilik
            // 
            lblKirlilik.AutoSize = true;
            lblKirlilik.Location = new Point(80, 307);
            lblKirlilik.Name = "lblKirlilik";
            lblKirlilik.Size = new Size(50, 20);
            lblKirlilik.TabIndex = 3;
            lblKirlilik.Text = "Kirlilik";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(869, 451);
            Controls.Add(lblKirlilik);
            Controls.Add(lblMiktar);
            Controls.Add(textBox1);
            Controls.Add(lblHassaslik);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblDetergentAmount;
        private Label lblDuration;
        private Label lblSpinSpeed;


        #endregion
        private Label lblHassaslik;
        private TextBox textBox1;
        private Label lblMiktar;
        private Label lblKirlilik;
    }
}