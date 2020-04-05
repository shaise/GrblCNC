namespace GrblCNC.Controls
{
    partial class FormProbe
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
            this.numericOffset = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.buttOK = new System.Windows.Forms.Button();
            this.buttCancel = new System.Windows.Forms.Button();
            this.multiSelDir = new GrblCNC.Controls.MultiSelect();
            this.label2 = new System.Windows.Forms.Label();
            this.labelAxis = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericOffset)).BeginInit();
            this.SuspendLayout();
            // 
            // numericOffset
            // 
            this.numericOffset.DecimalPlaces = 3;
            this.numericOffset.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericOffset.Location = new System.Drawing.Point(185, 71);
            this.numericOffset.Margin = new System.Windows.Forms.Padding(6);
            this.numericOffset.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericOffset.Name = "numericOffset";
            this.numericOffset.Size = new System.Drawing.Size(140, 31);
            this.numericOffset.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 73);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Offset (mm):";
            // 
            // buttOK
            // 
            this.buttOK.Location = new System.Drawing.Point(34, 164);
            this.buttOK.Margin = new System.Windows.Forms.Padding(6);
            this.buttOK.Name = "buttOK";
            this.buttOK.Size = new System.Drawing.Size(118, 44);
            this.buttOK.TabIndex = 2;
            this.buttOK.Text = "OK";
            this.buttOK.UseVisualStyleBackColor = true;
            this.buttOK.Click += new System.EventHandler(this.buttOK_Click);
            // 
            // buttCancel
            // 
            this.buttCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttCancel.Location = new System.Drawing.Point(207, 164);
            this.buttCancel.Margin = new System.Windows.Forms.Padding(6);
            this.buttCancel.Name = "buttCancel";
            this.buttCancel.Size = new System.Drawing.Size(118, 44);
            this.buttCancel.TabIndex = 3;
            this.buttCancel.Text = "Cancel";
            this.buttCancel.UseVisualStyleBackColor = true;
            // 
            // multiSelDir
            // 
            this.multiSelDir.Location = new System.Drawing.Point(204, 115);
            this.multiSelDir.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.multiSelDir.Name = "multiSelDir";
            this.multiSelDir.SelectedItem = 0;
            this.multiSelDir.SelectionTexts = "-|+";
            this.multiSelDir.Size = new System.Drawing.Size(121, 37);
            this.multiSelDir.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 121);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(163, 25);
            this.label2.TabIndex = 5;
            this.label2.Text = "Probe direction:";
            // 
            // labelAxis
            // 
            this.labelAxis.AutoSize = true;
            this.labelAxis.Location = new System.Drawing.Point(29, 28);
            this.labelAxis.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelAxis.Name = "labelAxis";
            this.labelAxis.Size = new System.Drawing.Size(59, 25);
            this.labelAxis.TabIndex = 6;
            this.labelAxis.Text = "Axis:";
            // 
            // FormProbe
            // 
            this.AcceptButton = this.buttOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttCancel;
            this.ClientSize = new System.Drawing.Size(351, 229);
            this.ControlBox = false;
            this.Controls.Add(this.labelAxis);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.multiSelDir);
            this.Controls.Add(this.buttCancel);
            this.Controls.Add(this.buttOK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericOffset);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "FormProbe";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Probing axis";
            ((System.ComponentModel.ISupportInitialize)(this.numericOffset)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numericOffset;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttOK;
        private System.Windows.Forms.Button buttCancel;
        private MultiSelect multiSelDir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelAxis;
    }
}