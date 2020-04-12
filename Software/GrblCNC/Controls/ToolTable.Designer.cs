﻿namespace GrblCNC.Controls
{
    partial class ToolTable
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.toolNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pocketNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.yOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.zOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.diameter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.decription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttSave = new System.Windows.Forms.Button();
            this.buttExport = new System.Windows.Forms.Button();
            this.buttImport = new System.Windows.Forms.Button();
            this.buttRemove = new System.Windows.Forms.Button();
            this.buttAdd = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.toolNum,
            this.pocketNum,
            this.xOffset,
            this.yOffset,
            this.zOffset,
            this.aOffset,
            this.bOffset,
            this.diameter,
            this.decription});
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(619, 263);
            this.dataGridView1.TabIndex = 0;
            // 
            // toolNum
            // 
            this.toolNum.HeaderText = "Tool #";
            this.toolNum.Name = "toolNum";
            this.toolNum.Width = 50;
            // 
            // pocketNum
            // 
            this.pocketNum.HeaderText = "Pocket #";
            this.pocketNum.Name = "pocketNum";
            this.pocketNum.Width = 60;
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
            // buttSave
            // 
            this.buttSave.Location = new System.Drawing.Point(4, 272);
            this.buttSave.Name = "buttSave";
            this.buttSave.Size = new System.Drawing.Size(51, 23);
            this.buttSave.TabIndex = 1;
            this.buttSave.Text = "Save";
            this.buttSave.UseVisualStyleBackColor = true;
            // 
            // buttExport
            // 
            this.buttExport.Location = new System.Drawing.Point(61, 272);
            this.buttExport.Name = "buttExport";
            this.buttExport.Size = new System.Drawing.Size(60, 23);
            this.buttExport.TabIndex = 2;
            this.buttExport.Text = "Export...";
            this.buttExport.UseVisualStyleBackColor = true;
            // 
            // buttImport
            // 
            this.buttImport.Location = new System.Drawing.Point(127, 272);
            this.buttImport.Name = "buttImport";
            this.buttImport.Size = new System.Drawing.Size(60, 23);
            this.buttImport.TabIndex = 3;
            this.buttImport.Text = "Import...";
            this.buttImport.UseVisualStyleBackColor = true;
            // 
            // buttRemove
            // 
            this.buttRemove.Location = new System.Drawing.Point(562, 272);
            this.buttRemove.Name = "buttRemove";
            this.buttRemove.Size = new System.Drawing.Size(60, 23);
            this.buttRemove.TabIndex = 4;
            this.buttRemove.Text = "Remove";
            this.buttRemove.UseVisualStyleBackColor = true;
            // 
            // buttAdd
            // 
            this.buttAdd.Location = new System.Drawing.Point(496, 272);
            this.buttAdd.Name = "buttAdd";
            this.buttAdd.Size = new System.Drawing.Size(60, 23);
            this.buttAdd.TabIndex = 5;
            this.buttAdd.Text = "Add";
            this.buttAdd.UseVisualStyleBackColor = true;
            // 
            // ToolTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttAdd);
            this.Controls.Add(this.buttRemove);
            this.Controls.Add(this.buttImport);
            this.Controls.Add(this.buttExport);
            this.Controls.Add(this.buttSave);
            this.Controls.Add(this.dataGridView1);
            this.Name = "ToolTable";
            this.Size = new System.Drawing.Size(628, 298);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn toolNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn pocketNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn xOffset;
        private System.Windows.Forms.DataGridViewTextBoxColumn yOffset;
        private System.Windows.Forms.DataGridViewTextBoxColumn zOffset;
        private System.Windows.Forms.DataGridViewTextBoxColumn aOffset;
        private System.Windows.Forms.DataGridViewTextBoxColumn bOffset;
        private System.Windows.Forms.DataGridViewTextBoxColumn diameter;
        private System.Windows.Forms.DataGridViewTextBoxColumn decription;
        private System.Windows.Forms.Button buttSave;
        private System.Windows.Forms.Button buttExport;
        private System.Windows.Forms.Button buttImport;
        private System.Windows.Forms.Button buttRemove;
        private System.Windows.Forms.Button buttAdd;
    }
}