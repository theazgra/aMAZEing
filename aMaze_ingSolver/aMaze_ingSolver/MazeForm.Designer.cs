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
            this.pbSolve = new System.Windows.Forms.ProgressBar();
            this.gbSolvers = new System.Windows.Forms.GroupBox();
            this.lbPathSize = new System.Windows.Forms.Label();
            this.lbSolveTime = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSaveGraph = new System.Windows.Forms.Button();
            this.chbShowResult = new System.Windows.Forms.CheckBox();
            this.btnSolve = new System.Windows.Forms.Button();
            this.solverSelection = new System.Windows.Forms.CheckedListBox();
            this.gbThreadCount = new System.Windows.Forms.GroupBox();
            this.tbThreadCount = new System.Windows.Forms.TextBox();
            this.chbParallel = new System.Windows.Forms.CheckBox();
            this.lbMatrixInfo = new System.Windows.Forms.Label();
            this.lbInfo = new System.Windows.Forms.Label();
            this.chbShowStartEnd = new System.Windows.Forms.CheckBox();
            this.chbShowVertices = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbScale = new System.Windows.Forms.Label();
            this.scaleUp = new System.Windows.Forms.Button();
            this.scaleDown = new System.Windows.Forms.Button();
            this.lbScaleFactor = new System.Windows.Forms.Label();
            this.chbInvoke = new System.Windows.Forms.CheckBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.imgPanel = new System.Windows.Forms.Panel();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgBox)).BeginInit();
            this.settingsPanel.SuspendLayout();
            this.gbSolvers.SuspendLayout();
            this.gbThreadCount.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.imgPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miLoadMaze});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1087, 27);
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
            this.imgBox.Location = new System.Drawing.Point(10, 10);
            this.imgBox.Name = "imgBox";
            this.imgBox.Size = new System.Drawing.Size(809, 610);
            this.imgBox.TabIndex = 1;
            this.imgBox.TabStop = false;
            // 
            // settingsPanel
            // 
            this.settingsPanel.Controls.Add(this.pbSolve);
            this.settingsPanel.Controls.Add(this.gbSolvers);
            this.settingsPanel.Controls.Add(this.lbMatrixInfo);
            this.settingsPanel.Controls.Add(this.lbInfo);
            this.settingsPanel.Controls.Add(this.chbShowStartEnd);
            this.settingsPanel.Controls.Add(this.chbShowVertices);
            this.settingsPanel.Controls.Add(this.groupBox1);
            this.settingsPanel.Controls.Add(this.chbInvoke);
            this.settingsPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.settingsPanel.Location = new System.Drawing.Point(0, 27);
            this.settingsPanel.Name = "settingsPanel";
            this.settingsPanel.Padding = new System.Windows.Forms.Padding(5);
            this.settingsPanel.Size = new System.Drawing.Size(258, 630);
            this.settingsPanel.TabIndex = 2;
            // 
            // pbSolve
            // 
            this.pbSolve.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pbSolve.Location = new System.Drawing.Point(5, 568);
            this.pbSolve.Name = "pbSolve";
            this.pbSolve.Size = new System.Drawing.Size(248, 23);
            this.pbSolve.Step = 1;
            this.pbSolve.TabIndex = 7;
            // 
            // gbSolvers
            // 
            this.gbSolvers.Controls.Add(this.lbPathSize);
            this.gbSolvers.Controls.Add(this.lbSolveTime);
            this.gbSolvers.Controls.Add(this.btnClear);
            this.gbSolvers.Controls.Add(this.btnSave);
            this.gbSolvers.Controls.Add(this.btnSaveGraph);
            this.gbSolvers.Controls.Add(this.chbShowResult);
            this.gbSolvers.Controls.Add(this.btnSolve);
            this.gbSolvers.Controls.Add(this.solverSelection);
            this.gbSolvers.Controls.Add(this.gbThreadCount);
            this.gbSolvers.Controls.Add(this.chbParallel);
            this.gbSolvers.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbSolvers.Location = new System.Drawing.Point(5, 175);
            this.gbSolvers.Name = "gbSolvers";
            this.gbSolvers.Size = new System.Drawing.Size(248, 359);
            this.gbSolvers.TabIndex = 5;
            this.gbSolvers.TabStop = false;
            this.gbSolvers.Text = "Solvers";
            // 
            // lbPathSize
            // 
            this.lbPathSize.AutoSize = true;
            this.lbPathSize.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbPathSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbPathSize.Location = new System.Drawing.Point(3, 261);
            this.lbPathSize.Name = "lbPathSize";
            this.lbPathSize.Size = new System.Drawing.Size(0, 17);
            this.lbPathSize.TabIndex = 8;
            // 
            // lbSolveTime
            // 
            this.lbSolveTime.AutoSize = true;
            this.lbSolveTime.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbSolveTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbSolveTime.Location = new System.Drawing.Point(3, 244);
            this.lbSolveTime.Name = "lbSolveTime";
            this.lbSolveTime.Size = new System.Drawing.Size(0, 17);
            this.lbSolveTime.TabIndex = 6;
            // 
            // btnClear
            // 
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnClear.Location = new System.Drawing.Point(3, 275);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(242, 27);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "Reset solver";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.ResetClick);
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnSave.Location = new System.Drawing.Point(3, 302);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(242, 27);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save result";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSaveGraph
            // 
            this.btnSaveGraph.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnSaveGraph.Location = new System.Drawing.Point(3, 329);
            this.btnSaveGraph.Name = "btnSaveGraph";
            this.btnSaveGraph.Size = new System.Drawing.Size(242, 27);
            this.btnSaveGraph.TabIndex = 12;
            this.btnSaveGraph.Text = "Save graph";
            this.btnSaveGraph.UseVisualStyleBackColor = true;
            this.btnSaveGraph.Click += new System.EventHandler(this.BtnSaveGraph);
            // 
            // chbShowResult
            // 
            this.chbShowResult.AutoSize = true;
            this.chbShowResult.Dock = System.Windows.Forms.DockStyle.Top;
            this.chbShowResult.Enabled = false;
            this.chbShowResult.Location = new System.Drawing.Point(3, 223);
            this.chbShowResult.Name = "chbShowResult";
            this.chbShowResult.Size = new System.Drawing.Size(242, 21);
            this.chbShowResult.TabIndex = 6;
            this.chbShowResult.Text = "Show result";
            this.chbShowResult.UseVisualStyleBackColor = true;
            this.chbShowResult.Click += new System.EventHandler(this.ShowResult);
            // 
            // btnSolve
            // 
            this.btnSolve.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSolve.Location = new System.Drawing.Point(3, 192);
            this.btnSolve.Name = "btnSolve";
            this.btnSolve.Size = new System.Drawing.Size(242, 31);
            this.btnSolve.TabIndex = 5;
            this.btnSolve.Text = "Solve";
            this.btnSolve.UseVisualStyleBackColor = true;
            this.btnSolve.Click += new System.EventHandler(this.SolveUsingSolver);
            // 
            // solverSelection
            // 
            this.solverSelection.CheckOnClick = true;
            this.solverSelection.Dock = System.Windows.Forms.DockStyle.Top;
            this.solverSelection.FormattingEnabled = true;
            this.solverSelection.Location = new System.Drawing.Point(3, 86);
            this.solverSelection.Name = "solverSelection";
            this.solverSelection.Size = new System.Drawing.Size(242, 106);
            this.solverSelection.TabIndex = 4;
            this.solverSelection.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.solverSelection_ItemCheck);
            this.solverSelection.SelectedValueChanged += new System.EventHandler(this.SelectionChanged);
            // 
            // gbThreadCount
            // 
            this.gbThreadCount.Controls.Add(this.tbThreadCount);
            this.gbThreadCount.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbThreadCount.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.gbThreadCount.Location = new System.Drawing.Point(3, 39);
            this.gbThreadCount.Name = "gbThreadCount";
            this.gbThreadCount.Size = new System.Drawing.Size(242, 47);
            this.gbThreadCount.TabIndex = 11;
            this.gbThreadCount.TabStop = false;
            this.gbThreadCount.Text = "Thread count";
            // 
            // tbThreadCount
            // 
            this.tbThreadCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbThreadCount.Location = new System.Drawing.Point(3, 18);
            this.tbThreadCount.Name = "tbThreadCount";
            this.tbThreadCount.Size = new System.Drawing.Size(236, 22);
            this.tbThreadCount.TabIndex = 10;
            this.tbThreadCount.Text = "2";
            this.tbThreadCount.TextChanged += new System.EventHandler(this.UpdateThreadCount);
            this.tbThreadCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbThreadCount_KeyPress);
            // 
            // chbParallel
            // 
            this.chbParallel.AutoSize = true;
            this.chbParallel.Checked = true;
            this.chbParallel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbParallel.Dock = System.Windows.Forms.DockStyle.Top;
            this.chbParallel.Location = new System.Drawing.Point(3, 18);
            this.chbParallel.Name = "chbParallel";
            this.chbParallel.Size = new System.Drawing.Size(242, 21);
            this.chbParallel.TabIndex = 9;
            this.chbParallel.Text = "Parallel";
            this.chbParallel.UseVisualStyleBackColor = true;
            this.chbParallel.Click += new System.EventHandler(this.chbParallel_Click);
            // 
            // lbMatrixInfo
            // 
            this.lbMatrixInfo.AutoSize = true;
            this.lbMatrixInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbMatrixInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbMatrixInfo.Location = new System.Drawing.Point(5, 591);
            this.lbMatrixInfo.Name = "lbMatrixInfo";
            this.lbMatrixInfo.Size = new System.Drawing.Size(0, 17);
            this.lbMatrixInfo.TabIndex = 3;
            // 
            // lbInfo
            // 
            this.lbInfo.AutoSize = true;
            this.lbInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbInfo.Location = new System.Drawing.Point(5, 608);
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.Size = new System.Drawing.Size(0, 17);
            this.lbInfo.TabIndex = 2;
            // 
            // chbShowStartEnd
            // 
            this.chbShowStartEnd.AutoSize = true;
            this.chbShowStartEnd.Dock = System.Windows.Forms.DockStyle.Top;
            this.chbShowStartEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.chbShowStartEnd.Location = new System.Drawing.Point(5, 151);
            this.chbShowStartEnd.Name = "chbShowStartEnd";
            this.chbShowStartEnd.Size = new System.Drawing.Size(248, 24);
            this.chbShowStartEnd.TabIndex = 0;
            this.chbShowStartEnd.Text = "Show start and end";
            this.chbShowStartEnd.UseVisualStyleBackColor = true;
            this.chbShowStartEnd.CheckedChanged += new System.EventHandler(this.ChbShowVertices_CheckedChanged);
            // 
            // chbShowVertices
            // 
            this.chbShowVertices.AutoSize = true;
            this.chbShowVertices.Dock = System.Windows.Forms.DockStyle.Top;
            this.chbShowVertices.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.chbShowVertices.Location = new System.Drawing.Point(5, 127);
            this.chbShowVertices.Name = "chbShowVertices";
            this.chbShowVertices.Size = new System.Drawing.Size(248, 24);
            this.chbShowVertices.TabIndex = 0;
            this.chbShowVertices.Text = "Show vertices";
            this.chbShowVertices.UseVisualStyleBackColor = true;
            this.chbShowVertices.CheckedChanged += new System.EventHandler(this.ChbShowVertices_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbScale);
            this.groupBox1.Controls.Add(this.scaleUp);
            this.groupBox1.Controls.Add(this.scaleDown);
            this.groupBox1.Controls.Add(this.lbScaleFactor);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupBox1.Location = new System.Drawing.Point(5, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(248, 98);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Image";
            // 
            // lbScale
            // 
            this.lbScale.AutoSize = true;
            this.lbScale.Location = new System.Drawing.Point(115, 31);
            this.lbScale.Name = "lbScale";
            this.lbScale.Size = new System.Drawing.Size(28, 18);
            this.lbScale.TabIndex = 3;
            this.lbScale.Text = "1.0";
            // 
            // scaleUp
            // 
            this.scaleUp.Location = new System.Drawing.Point(189, 63);
            this.scaleUp.Name = "scaleUp";
            this.scaleUp.Size = new System.Drawing.Size(53, 23);
            this.scaleUp.TabIndex = 2;
            this.scaleUp.Text = "+";
            this.scaleUp.UseVisualStyleBackColor = true;
            this.scaleUp.Click += new System.EventHandler(this.scaleUp_Click);
            // 
            // scaleDown
            // 
            this.scaleDown.Location = new System.Drawing.Point(17, 63);
            this.scaleDown.Name = "scaleDown";
            this.scaleDown.Size = new System.Drawing.Size(50, 23);
            this.scaleDown.TabIndex = 2;
            this.scaleDown.Text = "-";
            this.scaleDown.UseVisualStyleBackColor = true;
            this.scaleDown.Click += new System.EventHandler(this.scaleDown_Click);
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
            // chbInvoke
            // 
            this.chbInvoke.AutoSize = true;
            this.chbInvoke.Dock = System.Windows.Forms.DockStyle.Top;
            this.chbInvoke.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.chbInvoke.Location = new System.Drawing.Point(5, 5);
            this.chbInvoke.Name = "chbInvoke";
            this.chbInvoke.Size = new System.Drawing.Size(248, 24);
            this.chbInvoke.TabIndex = 0;
            this.chbInvoke.Text = "Invoke info print";
            this.chbInvoke.UseVisualStyleBackColor = true;
            this.chbInvoke.CheckedChanged += new System.EventHandler(this.chbInvoke_CheckedChanged);
            // 
            // splitter1
            // 
            this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitter1.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.splitter1.Location = new System.Drawing.Point(258, 27);
            this.splitter1.MinSize = 200;
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(10, 630);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // imgPanel
            // 
            this.imgPanel.AutoScroll = true;
            this.imgPanel.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.imgPanel.Controls.Add(this.imgBox);
            this.imgPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgPanel.Location = new System.Drawing.Point(258, 27);
            this.imgPanel.Name = "imgPanel";
            this.imgPanel.Padding = new System.Windows.Forms.Padding(10);
            this.imgPanel.Size = new System.Drawing.Size(829, 630);
            this.imgPanel.TabIndex = 4;
            // 
            // MazeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1087, 657);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.imgPanel);
            this.Controls.Add(this.settingsPanel);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MazeForm";
            this.Text = "aMAZEing";
            this.Load += new System.EventHandler(this.MazeForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgBox)).EndInit();
            this.settingsPanel.ResumeLayout(false);
            this.settingsPanel.PerformLayout();
            this.gbSolvers.ResumeLayout(false);
            this.gbSolvers.PerformLayout();
            this.gbThreadCount.ResumeLayout(false);
            this.gbThreadCount.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.imgPanel.ResumeLayout(false);
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
        private System.Windows.Forms.Button scaleUp;
        private System.Windows.Forms.Button scaleDown;
        private System.Windows.Forms.Label lbInfo;
        private System.Windows.Forms.Panel imgPanel;
        private System.Windows.Forms.CheckBox chbShowStartEnd;
        private System.Windows.Forms.CheckBox chbInvoke;
        private System.Windows.Forms.Label lbMatrixInfo;
        private System.Windows.Forms.GroupBox gbSolvers;
        private System.Windows.Forms.CheckedListBox solverSelection;
        private System.Windows.Forms.Button btnSolve;
        private System.Windows.Forms.CheckBox chbShowResult;
        private System.Windows.Forms.Label lbSolveTime;
        private System.Windows.Forms.Label lbScale;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lbPathSize;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.CheckBox chbParallel;
        private System.Windows.Forms.GroupBox gbThreadCount;
        private System.Windows.Forms.TextBox tbThreadCount;
        private System.Windows.Forms.ProgressBar pbSolve;
        private System.Windows.Forms.Button btnSaveGraph;
    }
}

