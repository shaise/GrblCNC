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
            this.components = new System.ComponentModel.Container();
            this.numericOffset = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.buttOK = new System.Windows.Forms.Button();
            this.buttCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.labelAxis = new System.Windows.Forms.Label();
            this.comboTool = new System.Windows.Forms.ComboBox();
            this.buttProbe = new System.Windows.Forms.Button();
            this.labelCoord = new System.Windows.Forms.Label();
            this.comboCoord = new System.Windows.Forms.ComboBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.multiSelAxis = new GrblCNC.Controls.MultiSelect();
            this.multiSelDir = new GrblCNC.Controls.MultiSelect();
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
            this.numericOffset.Location = new System.Drawing.Point(250, 71);
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
            this.label1.Location = new System.Drawing.Point(15, 73);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Offset (mm):";
            // 
            // buttOK
            // 
            this.buttOK.Location = new System.Drawing.Point(15, 210);
            this.buttOK.Margin = new System.Windows.Forms.Padding(6);
            this.buttOK.Name = "buttOK";
            this.buttOK.Size = new System.Drawing.Size(106, 44);
            this.buttOK.TabIndex = 2;
            this.buttOK.Text = "Set";
            this.toolTip1.SetToolTip(this.buttOK, "Manually Touch Off ");
            this.buttOK.UseVisualStyleBackColor = true;
            this.buttOK.Click += new System.EventHandler(this.buttOK_Click);
            // 
            // buttCancel
            // 
            this.buttCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttCancel.Location = new System.Drawing.Point(272, 210);
            this.buttCancel.Margin = new System.Windows.Forms.Padding(6);
            this.buttCancel.Name = "buttCancel";
            this.buttCancel.Size = new System.Drawing.Size(118, 44);
            this.buttCancel.TabIndex = 3;
            this.buttCancel.Text = "Cancel";
            this.buttCancel.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 165);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(163, 25);
            this.label2.TabIndex = 5;
            this.label2.Text = "Probe direction:";
            // 
            // labelAxis
            // 
            this.labelAxis.AutoSize = true;
            this.labelAxis.Location = new System.Drawing.Point(15, 24);
            this.labelAxis.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelAxis.Name = "labelAxis";
            this.labelAxis.Size = new System.Drawing.Size(59, 25);
            this.labelAxis.TabIndex = 6;
            this.labelAxis.Text = "Axis:";
            // 
            // comboTool
            // 
            this.comboTool.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboTool.FormattingEnabled = true;
            this.comboTool.Location = new System.Drawing.Point(152, 115);
            this.comboTool.Name = "comboTool";
            this.comboTool.Size = new System.Drawing.Size(238, 33);
            this.comboTool.TabIndex = 8;
            this.comboTool.SelectedIndexChanged += new System.EventHandler(this.comboTool_SelectedIndexChanged);
            // 
            // buttProbe
            // 
            this.buttProbe.Location = new System.Drawing.Point(145, 210);
            this.buttProbe.Margin = new System.Windows.Forms.Padding(6);
            this.buttProbe.Name = "buttProbe";
            this.buttProbe.Size = new System.Drawing.Size(106, 44);
            this.buttProbe.TabIndex = 9;
            this.buttProbe.Text = "Probe";
            this.toolTip1.SetToolTip(this.buttProbe, "Automatically Probe and set Touch Off");
            this.buttProbe.UseVisualStyleBackColor = true;
            this.buttProbe.Click += new System.EventHandler(this.buttProbe_Click);
            // 
            // labelCoord
            // 
            this.labelCoord.AutoSize = true;
            this.labelCoord.Location = new System.Drawing.Point(15, 118);
            this.labelCoord.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelCoord.Name = "labelCoord";
            this.labelCoord.Size = new System.Drawing.Size(60, 25);
            this.labelCoord.TabIndex = 10;
            this.labelCoord.Text = "Tool:";
            // 
            // comboCoord
            // 
            this.comboCoord.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboCoord.FormattingEnabled = true;
            this.comboCoord.Items.AddRange(new object[] {
            "P1  (G54)",
            "P2  (G55)",
            "P3  (G56)",
            "P4  (G57)",
            "P5  (G58)",
            "P6  (G59)",
            "P7  (G59.1)",
            "P8  (G59.2)",
            "P9  (G59.3)"});
            this.comboCoord.Location = new System.Drawing.Point(250, 115);
            this.comboCoord.Name = "comboCoord";
            this.comboCoord.Size = new System.Drawing.Size(140, 33);
            this.comboCoord.TabIndex = 11;
            // 
            // multiSelAxis
            // 
            this.multiSelAxis.Location = new System.Drawing.Point(153, 24);
            this.multiSelAxis.Margin = new System.Windows.Forms.Padding(12);
            this.multiSelAxis.MultiSelectionMode = false;
            this.multiSelAxis.Name = "multiSelAxis";
            this.multiSelAxis.SelectedValue = 0;
            this.multiSelAxis.SelectionTexts = "X|Y|Z|A|B";
            this.multiSelAxis.Size = new System.Drawing.Size(237, 34);
            this.multiSelAxis.TabIndex = 7;
            // 
            // multiSelDir
            // 
            this.multiSelDir.Location = new System.Drawing.Point(269, 157);
            this.multiSelDir.Margin = new System.Windows.Forms.Padding(6);
            this.multiSelDir.MultiSelectionMode = false;
            this.multiSelDir.Name = "multiSelDir";
            this.multiSelDir.SelectedValue = 0;
            this.multiSelDir.SelectionTexts = "-|+";
            this.multiSelDir.Size = new System.Drawing.Size(121, 37);
            this.multiSelDir.TabIndex = 4;
            // 
            // FormProbe
            // 
            this.AcceptButton = this.buttOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttCancel;
            this.ClientSize = new System.Drawing.Size(405, 269);
            this.ControlBox = false;
            this.Controls.Add(this.comboCoord);
            this.Controls.Add(this.labelCoord);
            this.Controls.Add(this.buttProbe);
            this.Controls.Add(this.comboTool);
            this.Controls.Add(this.multiSelAxis);
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
            this.Text = "Touch off";
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
        private MultiSelect multiSelAxis;
        private System.Windows.Forms.ComboBox comboTool;
        private System.Windows.Forms.Button buttProbe;
        private System.Windows.Forms.Label labelCoord;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox comboCoord;
    }
}