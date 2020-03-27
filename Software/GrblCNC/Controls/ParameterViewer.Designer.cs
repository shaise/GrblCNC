namespace GrblCNC
{
    partial class ParameterViewer
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
            this.dataGridGrblConf = new System.Windows.Forms.DataGridView();
            this.GrblCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GrblDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GrblValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttConfProg = new System.Windows.Forms.Button();
            this.buttConfLoad = new System.Windows.Forms.Button();
            this.buttConfSave = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridGrblConf)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridGrblConf
            // 
            this.dataGridGrblConf.AllowUserToAddRows = false;
            this.dataGridGrblConf.AllowUserToDeleteRows = false;
            this.dataGridGrblConf.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridGrblConf.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridGrblConf.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.GrblCode,
            this.GrblDescription,
            this.GrblValue});
            this.dataGridGrblConf.Location = new System.Drawing.Point(4, 4);
            this.dataGridGrblConf.Name = "dataGridGrblConf";
            this.dataGridGrblConf.RowHeadersVisible = false;
            this.dataGridGrblConf.Size = new System.Drawing.Size(283, 392);
            this.dataGridGrblConf.TabIndex = 1;
            this.dataGridGrblConf.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridGrblConf_CellValueChanged);
            // 
            // GrblCode
            // 
            this.GrblCode.HeaderText = "Code";
            this.GrblCode.Name = "GrblCode";
            this.GrblCode.ReadOnly = true;
            this.GrblCode.ToolTipText = "Grbl Internal parameter code";
            this.GrblCode.Width = 40;
            // 
            // GrblDescription
            // 
            this.GrblDescription.HeaderText = "Description";
            this.GrblDescription.Name = "GrblDescription";
            this.GrblDescription.ReadOnly = true;
            this.GrblDescription.Width = 160;
            // 
            // GrblValue
            // 
            this.GrblValue.HeaderText = "Value";
            this.GrblValue.Name = "GrblValue";
            this.GrblValue.Width = 60;
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
            this.openFileDialog.Filter = "Grbl Configuration file|*.txt";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Grbl Configuration file|*.txt";
            // 
            // ParameterViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttConfProg);
            this.Controls.Add(this.buttConfLoad);
            this.Controls.Add(this.buttConfSave);
            this.Controls.Add(this.dataGridGrblConf);
            this.Name = "ParameterViewer";
            this.Size = new System.Drawing.Size(290, 430);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridGrblConf)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridGrblConf;
        private System.Windows.Forms.Button buttConfProg;
        private System.Windows.Forms.Button buttConfLoad;
        private System.Windows.Forms.Button buttConfSave;
        private System.Windows.Forms.DataGridViewTextBoxColumn GrblCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn GrblDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn GrblValue;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}
