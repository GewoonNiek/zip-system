namespace zipNiek
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnUnzip = new System.Windows.Forms.Button();
            this.btnZip = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnUnzip
            // 
            this.btnUnzip.Location = new System.Drawing.Point(841, 231);
            this.btnUnzip.Name = "btnUnzip";
            this.btnUnzip.Size = new System.Drawing.Size(388, 205);
            this.btnUnzip.TabIndex = 0;
            this.btnUnzip.Text = "unzip";
            this.btnUnzip.UseVisualStyleBackColor = true;
            this.btnUnzip.Click += new System.EventHandler(this.btnUnzip_Click);
            // 
            // btnZip
            // 
            this.btnZip.Location = new System.Drawing.Point(350, 231);
            this.btnZip.Name = "btnZip";
            this.btnZip.Size = new System.Drawing.Size(388, 205);
            this.btnZip.TabIndex = 1;
            this.btnZip.Text = "Zip";
            this.btnZip.UseVisualStyleBackColor = true;
            this.btnZip.Click += new System.EventHandler(this.btnZip_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2056, 991);
            this.Controls.Add(this.btnZip);
            this.Controls.Add(this.btnUnzip);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnUnzip;
        private System.Windows.Forms.Button btnZip;
    }
}

