namespace GrblCNC.Controls
{
    partial class FormGoto
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
            this.buttGo = new System.Windows.Forms.Button();
            this.buttClose = new System.Windows.Forms.Button();
            this.labelCoord = new System.Windows.Forms.Label();
            this.comboCoord = new System.Windows.Forms.ComboBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.numX = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.numY = new System.Windows.Forms.NumericUpDown();
            this.numZ = new System.Windows.Forms.NumericUpDown();
            this.numB = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numA = new System.Windows.Forms.NumericUpDown();
            this.multiSelAxis = new GrblCNC.Controls.MultiSelect();
            this.buttClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numA)).BeginInit();
            this.SuspendLayout();
            // 
            // buttGo
            // 
            this.buttGo.Location = new System.Drawing.Point(15, 210);
            this.buttGo.Margin = new System.Windows.Forms.Padding(6);
            this.buttGo.Name = "buttGo";
            this.buttGo.Size = new System.Drawing.Size(135, 44);
            this.buttGo.TabIndex = 2;
            this.buttGo.Text = "Go";
            this.toolTip1.SetToolTip(this.buttGo, "Manually Touch Off ");
            this.buttGo.UseVisualStyleBackColor = true;
            this.buttGo.Click += new System.EventHandler(this.buttGO_Click);
            // 
            // buttClose
            // 
            this.buttClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttClose.Location = new System.Drawing.Point(415, 210);
            this.buttClose.Margin = new System.Windows.Forms.Padding(6);
            this.buttClose.Name = "buttClose";
            this.buttClose.Size = new System.Drawing.Size(118, 44);
            this.buttClose.TabIndex = 3;
            this.buttClose.Text = "Close";
            this.buttClose.UseVisualStyleBackColor = true;
            // 
            // labelCoord
            // 
            this.labelCoord.AutoSize = true;
            this.labelCoord.Location = new System.Drawing.Point(288, 15);
            this.labelCoord.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelCoord.Name = "labelCoord";
            this.labelCoord.Size = new System.Drawing.Size(96, 31);
            this.labelCoord.TabIndex = 10;
            this.labelCoord.Text = "Coord:";
            this.labelCoord.Click += new System.EventHandler(this.labelCoord_Click);
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
            this.comboCoord.Location = new System.Drawing.Point(393, 12);
            this.comboCoord.Name = "comboCoord";
            this.comboCoord.Size = new System.Drawing.Size(140, 38);
            this.comboCoord.TabIndex = 11;
            // 
            // numX
            // 
            this.numX.DecimalPlaces = 3;
            this.numX.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numX.Location = new System.Drawing.Point(94, 76);
            this.numX.Margin = new System.Windows.Forms.Padding(6);
            this.numX.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numX.Name = "numX";
            this.numX.Size = new System.Drawing.Size(140, 37);
            this.numX.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 78);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 31);
            this.label1.TabIndex = 1;
            this.label1.Text = "XYZ:";
            // 
            // numY
            // 
            this.numY.DecimalPlaces = 3;
            this.numY.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numY.Location = new System.Drawing.Point(244, 76);
            this.numY.Margin = new System.Windows.Forms.Padding(6);
            this.numY.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numY.Name = "numY";
            this.numY.Size = new System.Drawing.Size(140, 37);
            this.numY.TabIndex = 13;
            // 
            // numZ
            // 
            this.numZ.DecimalPlaces = 3;
            this.numZ.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numZ.Location = new System.Drawing.Point(393, 76);
            this.numZ.Margin = new System.Windows.Forms.Padding(6);
            this.numZ.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numZ.Name = "numZ";
            this.numZ.Size = new System.Drawing.Size(140, 37);
            this.numZ.TabIndex = 14;
            // 
            // numB
            // 
            this.numB.DecimalPlaces = 3;
            this.numB.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numB.Location = new System.Drawing.Point(244, 128);
            this.numB.Margin = new System.Windows.Forms.Padding(6);
            this.numB.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numB.Name = "numB";
            this.numB.Size = new System.Drawing.Size(140, 37);
            this.numB.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 130);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 31);
            this.label2.TabIndex = 16;
            this.label2.Text = "AB:";
            // 
            // numA
            // 
            this.numA.DecimalPlaces = 3;
            this.numA.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numA.Location = new System.Drawing.Point(94, 128);
            this.numA.Margin = new System.Windows.Forms.Padding(6);
            this.numA.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numA.Name = "numA";
            this.numA.Size = new System.Drawing.Size(140, 37);
            this.numA.TabIndex = 15;
            // 
            // multiSelAxis
            // 
            this.multiSelAxis.Location = new System.Drawing.Point(20, 12);
            this.multiSelAxis.Margin = new System.Windows.Forms.Padding(22, 22, 22, 22);
            this.multiSelAxis.MultiSelectionMode = false;
            this.multiSelAxis.Name = "multiSelAxis";
            this.multiSelAxis.SelectedValue = 0;
            this.multiSelAxis.SelectionTexts = "X|Y|Z|A|B";
            this.multiSelAxis.Size = new System.Drawing.Size(240, 40);
            this.multiSelAxis.TabIndex = 18;
            // 
            // buttClear
            // 
            this.buttClear.Location = new System.Drawing.Point(208, 210);
            this.buttClear.Margin = new System.Windows.Forms.Padding(6);
            this.buttClear.Name = "buttClear";
            this.buttClear.Size = new System.Drawing.Size(142, 44);
            this.buttClear.TabIndex = 19;
            this.buttClear.Text = "Clear";
            this.toolTip1.SetToolTip(this.buttClear, "Manually Touch Off ");
            this.buttClear.UseVisualStyleBackColor = true;
            // 
            // FormGoto
            // 
            this.AcceptButton = this.buttGo;
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttClose;
            this.ClientSize = new System.Drawing.Size(548, 269);
            this.ControlBox = false;
            this.Controls.Add(this.buttClear);
            this.Controls.Add(this.multiSelAxis);
            this.Controls.Add(this.numB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numA);
            this.Controls.Add(this.numZ);
            this.Controls.Add(this.numY);
            this.Controls.Add(this.comboCoord);
            this.Controls.Add(this.labelCoord);
            this.Controls.Add(this.buttClose);
            this.Controls.Add(this.buttGo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numX);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "FormGoto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Go To";
            ((System.ComponentModel.ISupportInitialize)(this.numX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numA)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttGo;
        private System.Windows.Forms.Button buttClose;
        private System.Windows.Forms.Label labelCoord;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox comboCoord;
        private System.Windows.Forms.NumericUpDown numX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numY;
        private System.Windows.Forms.NumericUpDown numZ;
        private System.Windows.Forms.NumericUpDown numB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numA;
        private MultiSelect multiSelAxis;
        private System.Windows.Forms.Button buttClear;
    }
}