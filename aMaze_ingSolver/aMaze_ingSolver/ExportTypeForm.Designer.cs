namespace aMaze_ingSolver
{
    partial class ExportTypeForm
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
            this.rbWhole = new System.Windows.Forms.RadioButton();
            this.rbPathOnly = new System.Windows.Forms.RadioButton();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rbWhole
            // 
            this.rbWhole.AutoSize = true;
            this.rbWhole.Location = new System.Drawing.Point(30, 12);
            this.rbWhole.Name = "rbWhole";
            this.rbWhole.Size = new System.Drawing.Size(110, 21);
            this.rbWhole.TabIndex = 0;
            this.rbWhole.TabStop = true;
            this.rbWhole.Text = "Whole graph";
            this.rbWhole.UseVisualStyleBackColor = true;
            // 
            // rbPathOnly
            // 
            this.rbPathOnly.AutoSize = true;
            this.rbPathOnly.Location = new System.Drawing.Point(30, 39);
            this.rbPathOnly.Name = "rbPathOnly";
            this.rbPathOnly.Size = new System.Drawing.Size(90, 21);
            this.rbPathOnly.TabIndex = 1;
            this.rbPathOnly.TabStop = true;
            this.rbPathOnly.Text = "Only path";
            this.rbPathOnly.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(45, 71);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // ExportTypeForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(186, 106);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.rbPathOnly);
            this.Controls.Add(this.rbWhole);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ExportTypeForm";
            this.Text = "Export type";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.RadioButton rbWhole;
        public System.Windows.Forms.RadioButton rbPathOnly;
        private System.Windows.Forms.Button btnOk;
    }
}