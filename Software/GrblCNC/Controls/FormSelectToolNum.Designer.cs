namespace GrblCNC.Controls
{
    partial class FormSelectToolNum
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
            this.label1 = new System.Windows.Forms.Label();
            this.numericTool = new System.Windows.Forms.NumericUpDown();
            this.buttOK = new System.Windows.Forms.Button();
            this.buttCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericTool)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tool #:";
            // 
            // numericTool
            // 
            this.numericTool.Location = new System.Drawing.Point(119, 12);
            this.numericTool.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericTool.Name = "numericTool";
            this.numericTool.Size = new System.Drawing.Size(120, 31);
            this.numericTool.TabIndex = 1;
            this.numericTool.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // buttOK
            // 
            this.buttOK.Location = new System.Drawing.Point(15, 61);
            this.buttOK.Margin = new System.Windows.Forms.Padding(6);
            this.buttOK.Name = "buttOK";
            this.buttOK.Size = new System.Drawing.Size(118, 44);
            this.buttOK.TabIndex = 3;
            this.buttOK.Text = "OK";
            this.buttOK.UseVisualStyleBackColor = true;
            this.buttOK.Click += new System.EventHandler(this.buttOK_Click);
            // 
            // buttCancel
            // 
            this.buttCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttCancel.Location = new System.Drawing.Point(170, 61);
            this.buttCancel.Margin = new System.Windows.Forms.Padding(6);
            this.buttCancel.Name = "buttCancel";
            this.buttCancel.Size = new System.Drawing.Size(118, 44);
            this.buttCancel.TabIndex = 4;
            this.buttCancel.Text = "Cancel";
            this.buttCancel.UseVisualStyleBackColor = true;
            this.buttCancel.Click += new System.EventHandler(this.buttCancel_Click);
            // 
            // FormSelectToolNum
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 126);
            this.Controls.Add(this.buttCancel);
            this.Controls.Add(this.buttOK);
            this.Controls.Add(this.numericTool);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSelectToolNum";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Tool Number";
            ((System.ComponentModel.ISupportInitialize)(this.numericTool)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericTool;
        private System.Windows.Forms.Button buttOK;
        private System.Windows.Forms.Button buttCancel;
    }
}