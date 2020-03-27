namespace GrblCNC.Controls
{
    partial class MdiControl
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
            this.gcodeViewMDI = new GrblCNC.GcodeViewer();
            this.textGcodeLine = new System.Windows.Forms.TextBox();
            this.buttSendGcodeLine = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // gcodeViewMDI
            // 
            this.gcodeViewMDI.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gcodeViewMDI.Location = new System.Drawing.Point(6, 6);
            this.gcodeViewMDI.Name = "gcodeViewMDI";
            this.gcodeViewMDI.Size = new System.Drawing.Size(234, 307);
            this.gcodeViewMDI.TabIndex = 1;
            // 
            // textGcodeLine
            // 
            this.textGcodeLine.Location = new System.Drawing.Point(6, 319);
            this.textGcodeLine.Name = "textGcodeLine";
            this.textGcodeLine.Size = new System.Drawing.Size(175, 20);
            this.textGcodeLine.TabIndex = 3;
            this.textGcodeLine.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textGcodeLine_KeyDown);
            this.textGcodeLine.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textGcodeLine_KeyPress);
            // 
            // buttSendGcodeLine
            // 
            this.buttSendGcodeLine.Location = new System.Drawing.Point(187, 319);
            this.buttSendGcodeLine.Name = "buttSendGcodeLine";
            this.buttSendGcodeLine.Size = new System.Drawing.Size(55, 23);
            this.buttSendGcodeLine.TabIndex = 4;
            this.buttSendGcodeLine.Text = "Send";
            this.buttSendGcodeLine.UseVisualStyleBackColor = true;
            this.buttSendGcodeLine.Click += new System.EventHandler(this.buttSendGcodeLine_Click);
            // 
            // MdiControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttSendGcodeLine);
            this.Controls.Add(this.textGcodeLine);
            this.Controls.Add(this.gcodeViewMDI);
            this.Name = "MdiControl";
            this.Size = new System.Drawing.Size(267, 370);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GcodeViewer gcodeViewMDI;
        private System.Windows.Forms.TextBox textGcodeLine;
        private System.Windows.Forms.Button buttSendGcodeLine;
    }
}
