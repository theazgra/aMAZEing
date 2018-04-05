namespace aMaze_ingSolver
{
    partial class MazeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MazeForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.miLoadMaze = new System.Windows.Forms.ToolStripButton();
            this.imgBox = new System.Windows.Forms.PictureBox();
            this.settingsPanel = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.scaleFactor = new System.Windows.Forms.TrackBar();
            this.lbScaleFactor = new System.Windows.Forms.Label();
            this.chbShowVertices = new System.Windows.Forms.CheckBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.chbShowEdges = new System.Windows.Forms.CheckBox();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgBox)).BeginInit();
            this.settingsPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scaleFactor)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miLoadMaze});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(875, 27);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // miLoadMaze
            // 
            this.miLoadMaze.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.miLoadMaze.Image = ((System.Drawing.Image)(resources.GetObject("miLoadMaze.Image")));
            this.miLoadMaze.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.miLoadMaze.Name = "miLoadMaze";
            this.miLoadMaze.Size = new System.Drawing.Size(86, 24);
            this.miLoadMaze.Text = "Load maze";
            this.miLoadMaze.Click += new System.EventHandler(this.miLoadMaze_Click);
            // 
            // imgBox
            // 
            this.imgBox.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.imgBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.imgBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgBox.Location = new System.Drawing.Point(258, 27);
            this.imgBox.Name = "imgBox";
            this.imgBox.Size = new System.Drawing.Size(617, 545);
            this.imgBox.TabIndex = 1;
            this.imgBox.TabStop = false;
            // 
            // settingsPanel
            // 
            this.settingsPanel.Controls.Add(this.groupBox1);
            this.settingsPanel.Controls.Add(this.chbShowEdges);
            this.settingsPanel.Controls.Add(this.chbShowVertices);
            this.settingsPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.settingsPanel.Location = new System.Drawing.Point(0, 27);
            this.settingsPanel.Name = "settingsPanel";
            this.settingsPanel.Size = new System.Drawing.Size(258, 545);
            this.settingsPanel.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.scaleFactor);
            this.groupBox1.Controls.Add(this.lbScaleFactor);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(258, 115);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Image";
            // 
            // scaleFactor
            // 
            this.scaleFactor.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.scaleFactor.Location = new System.Drawing.Point(3, 56);
            this.scaleFactor.Maximum = 100;
            this.scaleFactor.Minimum = 1;
            this.scaleFactor.Name = "scaleFactor";
            this.scaleFactor.Size = new System.Drawing.Size(252, 56);
            this.scaleFactor.SmallChange = 10;
            this.scaleFactor.TabIndex = 1;
            this.scaleFactor.Value = 1;
            this.scaleFactor.ValueChanged += new System.EventHandler(this.ScaleFactor_ValueChanged);
            // 
            // lbScaleFactor
            // 
            this.lbScaleFactor.AutoSize = true;
            this.lbScaleFactor.Location = new System.Drawing.Point(12, 31);
            this.lbScaleFactor.Name = "lbScaleFactor";
            this.lbScaleFactor.Size = new System.Drawing.Size(87, 18);
            this.lbScaleFactor.TabIndex = 0;
            this.lbScaleFactor.Text = "Scale factor";
            // 
            // chbShowVertices
            // 
            this.chbShowVertices.AutoSize = true;
            this.chbShowVertices.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.chbShowVertices.Location = new System.Drawing.Point(12, 157);
            this.chbShowVertices.Name = "chbShowVertices";
            this.chbShowVertices.Size = new System.Drawing.Size(136, 24);
            this.chbShowVertices.TabIndex = 0;
            this.chbShowVertices.Text = "Show vertices";
            this.chbShowVertices.UseVisualStyleBackColor = true;
            this.chbShowVertices.CheckedChanged += new System.EventHandler(this.ChbShowVertices_CheckedChanged);
            // 
            // splitter1
            // 
            this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitter1.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.splitter1.Location = new System.Drawing.Point(258, 27);
            this.splitter1.MinSize = 200;
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(10, 545);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // chbShowEdges
            // 
            this.chbShowEdges.AutoSize = true;
            this.chbShowEdges.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.chbShowEdges.Location = new System.Drawing.Point(12, 187);
            this.chbShowEdges.Name = "chbShowEdges";
            this.chbShowEdges.Size = new System.Drawing.Size(122, 24);
            this.chbShowEdges.TabIndex = 0;
            this.chbShowEdges.Text = "Show edges";
            this.chbShowEdges.UseVisualStyleBackColor = true;
            this.chbShowEdges.CheckedChanged += new System.EventHandler(this.ChbShowVertices_CheckedChanged);
            // 
            // MazeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(875, 572);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.imgBox);
            this.Controls.Add(this.settingsPanel);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MazeForm";
            this.Text = "Form1";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgBox)).EndInit();
            this.settingsPanel.ResumeLayout(false);
            this.settingsPanel.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scaleFactor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton miLoadMaze;
        private System.Windows.Forms.PictureBox imgBox;
        private System.Windows.Forms.Panel settingsPanel;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.CheckBox chbShowVertices;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbScaleFactor;
        private System.Windows.Forms.TrackBar scaleFactor;
        private System.Windows.Forms.CheckBox chbShowEdges;
    }
}

