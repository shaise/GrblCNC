namespace GrblCNC.Controls
{
    partial class ToolTableEdit
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
            this.dataGridTools = new System.Windows.Forms.DataGridView();
            this.toolNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pocketNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numAxes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.yOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.zOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.diameter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.decription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttUpdate = new System.Windows.Forms.Button();
            this.buttExport = new System.Windows.Forms.Button();
            this.buttImport = new System.Windows.Forms.Button();
            this.buttRemove = new System.Windows.Forms.Button();
            this.buttAdd = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridTools)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridTools
            // 
            this.dataGridTools.AllowUserToAddRows = false;
            this.dataGridTools.AllowUserToDeleteRows = false;
            this.dataGridTools.AllowUserToResizeRows = false;
            this.dataGridTools.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridTools.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.toolNum,
            this.pocketNum,
            this.numAxes,
            this.xOffset,
            this.yOffset,
            this.zOffset,
            this.aOffset,
            this.bOffset,
            this.diameter,
            this.decription});
            this.dataGridTools.Location = new System.Drawing.Point(3, 3);
            this.dataGridTools.Name = "dataGridTools";
            this.dataGridTools.RowHeadersVisible = false;
            this.dataGridTools.Size = new System.Drawing.Size(594, 269);
            this.dataGridTools.TabIndex = 0;
            this.dataGridTools.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridTools_CellValueChanged);
            // 
            // toolNum
            // 
            this.toolNum.HeaderText = "Tool #";
            this.toolNum.Name = "toolNum";
            this.toolNum.ReadOnly = true;
            this.toolNum.Width = 50;
            // 
            // pocketNum
            // 
            this.pocketNum.HeaderText = "Pocket #";
            this.pocketNum.Name = "pocketNum";
            this.pocketNum.Width = 60;
            // 
            // numAxes
            // 
            this.numAxes.HeaderText = "Num Axes";
            this.numAxes.Name = "numAxes";
            this.numAxes.Visible = false;
            // 
            // xOffset
            // 
            this.xOffset.HeaderText = "X Offs";
            this.xOffset.Name = "xOffset";
            this.xOffset.Width = 50;
            // 
            // yOffset
            // 
            this.yOffset.HeaderText = "Y Offs";
            this.yOffset.Name = "yOffset";
            this.yOffset.Width = 50;
            // 
            // zOffset
            // 
            this.zOffset.HeaderText = "Z Offs";
            this.zOffset.Name = "zOffset";
            this.zOffset.Width = 50;
            // 
            // aOffset
            // 
            this.aOffset.HeaderText = "A Offs";
            this.aOffset.Name = "aOffset";
            this.aOffset.Width = 50;
            // 
            // bOffset
            // 
            this.bOffset.HeaderText = "B Offs";
            this.bOffset.Name = "bOffset";
            this.bOffset.Width = 50;
            // 
            // diameter
            // 
            this.diameter.HeaderText = "Diam";
            this.diameter.Name = "diameter";
            this.diameter.Width = 50;
            // 
            // decription
            // 
            this.decription.HeaderText = "Decription";
            this.decription.Name = "decription";
            this.decription.Width = 180;
            // 
            // buttUpdate
            // 
            this.buttUpdate.Location = new System.Drawing.Point(3, 275);
            this.buttUpdate.Name = "buttUpdate";
            this.buttUpdate.Size = new System.Drawing.Size(51, 23);
            this.buttUpdate.TabIndex = 1;
            this.buttUpdate.Text = "Update";
            this.buttUpdate.UseVisualStyleBackColor = true;
            this.buttUpdate.Click += new System.EventHandler(this.buttUpdate_Click);
            // 
            // buttExport
            // 
            this.buttExport.Location = new System.Drawing.Point(60, 275);
            this.buttExport.Name = "buttExport";
            this.buttExport.Size = new System.Drawing.Size(60, 23);
            this.buttExport.TabIndex = 2;
            this.buttExport.Text = "Export...";
            this.buttExport.UseVisualStyleBackColor = true;
            this.buttExport.Click += new System.EventHandler(this.buttExport_Click);
            // 
            // buttImport
            // 
            this.buttImport.Location = new System.Drawing.Point(126, 275);
            this.buttImport.Name = "buttImport";
            this.buttImport.Size = new System.Drawing.Size(60, 23);
            this.buttImport.TabIndex = 3;
            this.buttImport.Text = "Import...";
            this.buttImport.UseVisualStyleBackColor = true;
            this.buttImport.Click += new System.EventHandler(this.buttImport_Click);
            // 
            // buttRemove
            // 
            this.buttRemove.Location = new System.Drawing.Point(538, 275);
            this.buttRemove.Name = "buttRemove";
            this.buttRemove.Size = new System.Drawing.Size(60, 23);
            this.buttRemove.TabIndex = 4;
            this.buttRemove.Text = "Remove";
            this.buttRemove.UseVisualStyleBackColor = true;
            this.buttRemove.Click += new System.EventHandler(this.buttRemove_Click);
            // 
            // buttAdd
            // 
            this.buttAdd.Location = new System.Drawing.Point(472, 275);
            this.buttAdd.Name = "buttAdd";
            this.buttAdd.Size = new System.Drawing.Size(60, 23);
            this.buttAdd.TabIndex = 5;
            this.buttAdd.Text = "Add";
            this.buttAdd.UseVisualStyleBackColor = true;
            this.buttAdd.Click += new System.EventHandler(this.buttAdd_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "GrblCNC Tool Table (*.gtt)|*.gtt";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "GrblCNC Tool Table (*.gtt)|*.gtt";
            // 
            // ToolTableEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttAdd);
            this.Controls.Add(this.buttRemove);
            this.Controls.Add(this.buttImport);
            this.Controls.Add(this.buttExport);
            this.Controls.Add(this.buttUpdate);
            this.Controls.Add(this.dataGridTools);
            this.Name = "ToolTableEdit";
            this.Size = new System.Drawing.Size(600, 300);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridTools)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridTools;
        private System.Windows.Forms.Button buttUpdate;
        private System.Windows.Forms.Button buttExport;
        private System.Windows.Forms.Button buttImport;
        private System.Windows.Forms.Button buttRemove;
        private System.Windows.Forms.Button buttAdd;
        private System.Windows.Forms.DataGridViewTextBoxColumn toolNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn pocketNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn numAxes;
        private System.Windows.Forms.DataGridViewTextBoxColumn xOffset;
        private System.Windows.Forms.DataGridViewTextBoxColumn yOffset;
        private System.Windows.Forms.DataGridViewTextBoxColumn zOffset;
        private System.Windows.Forms.DataGridViewTextBoxColumn aOffset;
        private System.Windows.Forms.DataGridViewTextBoxColumn bOffset;
        private System.Windows.Forms.DataGridViewTextBoxColumn diameter;
        private System.Windows.Forms.DataGridViewTextBoxColumn decription;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}
