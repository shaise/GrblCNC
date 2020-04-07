namespace GrblCNC
{
    partial class FormConfigGrbl
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
            this.paramEdit = new GrblCNC.Controls.ParametersEdit();
            this.SuspendLayout();
            // 
            // paramEdit
            // 
            this.paramEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paramEdit.Location = new System.Drawing.Point(0, 0);
            this.paramEdit.Name = "paramEdit";
            this.paramEdit.Size = new System.Drawing.Size(403, 415);
            this.paramEdit.TabIndex = 0;
            // 
            // FormConfigGrbl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 415);
            this.Controls.Add(this.paramEdit);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormConfigGrbl";
            this.Text = "Configure Grbl driver";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ParametersEdit paramEdit;
    }
}