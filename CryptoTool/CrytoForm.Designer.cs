namespace CryptoKonverter
{
    partial class CrytoForm
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
            button1 = new Button();
            But_Crypt = new Button();
            But_Decrypt = new Button();
            TB_Clear = new TextBox();
            label1 = new Label();
            label2 = new Label();
            TB_Crypt = new TextBox();
            label3 = new Label();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(178, 283);
            button1.Name = "button1";
            button1.Size = new Size(112, 34);
            button1.TabIndex = 4;
            button1.Text = "Exit";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // But_Crypt
            // 
            But_Crypt.Location = new Point(26, 123);
            But_Crypt.Name = "But_Crypt";
            But_Crypt.Size = new Size(178, 34);
            But_Crypt.TabIndex = 1;
            But_Crypt.Text = "Verschlüsseln";
            But_Crypt.UseVisualStyleBackColor = true;
            But_Crypt.Click += But_Crypt_Click;
            // 
            // But_Decrypt
            // 
            But_Decrypt.Location = new Point(262, 123);
            But_Decrypt.Name = "But_Decrypt";
            But_Decrypt.Size = new Size(178, 34);
            But_Decrypt.TabIndex = 2;
            But_Decrypt.Text = "Entschlüsseln";
            But_Decrypt.UseVisualStyleBackColor = true;
            But_Decrypt.Click += But_Decrypt_Click;
            // 
            // TB_Clear
            // 
            TB_Clear.AllowDrop = true;
            TB_Clear.Location = new Point(26, 63);
            TB_Clear.Name = "TB_Clear";
            TB_Clear.Size = new Size(414, 31);
            TB_Clear.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(26, 20);
            label1.Name = "label1";
            label1.Size = new Size(70, 25);
            label1.TabIndex = 100;
            label1.Text = "Klartext";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(26, 169);
            label2.Name = "label2";
            label2.Size = new Size(0, 25);
            label2.TabIndex = 6;
            // 
            // TB_Crypt
            // 
            TB_Crypt.AllowDrop = true;
            TB_Crypt.Location = new Point(26, 212);
            TB_Crypt.Name = "TB_Crypt";
            TB_Crypt.Size = new Size(414, 31);
            TB_Crypt.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(26, 169);
            label3.Name = "label3";
            label3.Size = new Size(217, 25);
            label3.TabIndex = 7;
            label3.Text = "Verschlüssseltes Password";
            // 
            // CrytoForm
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(487, 346);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(TB_Crypt);
            Controls.Add(label1);
            Controls.Add(TB_Clear);
            Controls.Add(But_Decrypt);
            Controls.Add(But_Crypt);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CrytoForm";
            Text = "CryptoTool";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button But_Crypt;
        private Button But_Decrypt;
        private TextBox TB_Clear;
        private Label label1;
        private Label label2;
        private TextBox TB_Crypt;
        private Label label3;
    }
}