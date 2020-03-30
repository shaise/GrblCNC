namespace GrblCNC
{
    partial class GcodeParamViewer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridGCodeConf = new System.Windows.Forms.DataGridView();
            this.buttConfProg = new System.Windows.Forms.Button();
            this.buttConfLoad = new System.Windows.Forms.Button();
            this.buttConfSave = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.GCodeCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColZ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridGCodeConf)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridGCodeConf
            // 
            this.dataGridGCodeConf.AllowUserToAddRows = false;
            this.dataGridGCodeConf.AllowUserToDeleteRows = false;
            this.dataGridGCodeConf.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridGCodeConf.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridGCodeConf.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.GCodeCode,
            this.ColX,
            this.ColY,
            this.ColZ,
            this.ColA,
            this.ColB});
            this.dataGridGCodeConf.Location = new System.Drawing.Point(4, 4);
            this.dataGridGCodeConf.Name = "dataGridGCodeConf";
            this.dataGridGCodeConf.RowHeadersVisible = false;
            this.dataGridGCodeConf.Size = new System.Drawing.Size(283, 392);
            this.dataGridGCodeConf.TabIndex = 1;
            this.dataGridGCodeConf.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridGCodeConf_CellValueChanged);
            // 
            // buttConfProg
            // 
            this.buttConfProg.Location = new System.Drawing.Point(199, 402);
            this.buttConfProg.Name = "buttConfProg";
            this.buttConfProg.Size = new System.Drawing.Size(88, 23);
            this.buttConfProg.TabIndex = 6;
            this.buttConfProg.Text = "Program";
            this.buttConfProg.UseVisualStyleBackColor = true;
            this.buttConfProg.Click += new System.EventHandler(this.buttConfProg_Click);
            // 
            // buttConfLoad
            // 
            this.buttConfLoad.Location = new System.Drawing.Point(65, 402);
            this.buttConfLoad.Name = "buttConfLoad";
            this.buttConfLoad.Size = new System.Drawing.Size(56, 23);
            this.buttConfLoad.TabIndex = 5;
            this.buttConfLoad.Text = "Load";
            this.buttConfLoad.UseVisualStyleBackColor = true;
            this.buttConfLoad.Click += new System.EventHandler(this.buttConfLoad_Click);
            // 
            // buttConfSave
            // 
            this.buttConfSave.Location = new System.Drawing.Point(3, 402);
            this.buttConfSave.Name = "buttConfSave";
            this.buttConfSave.Size = new System.Drawing.Size(56, 23);
            this.buttConfSave.TabIndex = 4;
            this.buttConfSave.Text = "Save";
            this.buttConfSave.UseVisualStyleBackColor = true;
            this.buttConfSave.Click += new System.EventHandler(this.buttConfSave_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "GCode Configuration file|*.txt";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "GCode Configuration file|*.txt";
            // 
            // GCodeCode
            // 
            this.GCodeCode.HeaderText = "Code";
            this.GCodeCode.Name = "GCodeCode";
            this.GCodeCode.ReadOnly = true;
            this.GCodeCode.ToolTipText = "GCode Internal parameter code";
            this.GCodeCode.Width = 40;
            // 
            // ColX
            // 
            this.ColX.HeaderText = "X";
            this.ColX.Name = "ColX";
            this.ColX.Width = 60;
            // 
            // ColY
            // 
            this.ColY.HeaderText = "Y";
            this.ColY.Name = "ColY";
            this.ColY.Width = 60;
            // 
            // ColZ
            // 
            this.ColZ.HeaderText = "Z";
            this.ColZ.Name = "ColZ";
            this.ColZ.Width = 60;
            // 
            // ColA
            // 
            this.ColA.HeaderText = "A";
            this.ColA.Name = "ColA";
            this.ColA.Width = 60;
            // 
            // ColB
            // 
            this.ColB.HeaderText = "B";
            this.ColB.Name = "ColB";
            this.ColB.Width = 60;
            // 
            // GcodeParamViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttConfProg);
            this.Controls.Add(this.buttConfLoad);
            this.Controls.Add(this.buttConfSave);
            this.Controls.Add(this.dataGridGCodeConf);
            this.Name = "GcodeParamViewer";
            this.Size = new System.Drawing.Size(290, 430);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridGCodeConf)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridGCodeConf;
        private System.Windows.Forms.Button buttConfProg;
        private System.Windows.Forms.Button buttConfLoad;
        private System.Windows.Forms.Button buttConfSave;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.DataGridViewTextBoxColumn GCodeCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColX;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColY;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColZ;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColA;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColB;
    }
}
