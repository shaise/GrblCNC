namespace GrblCNC.Controls
{
    partial class FormChangeTool
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
            this.buttResume = new System.Windows.Forms.Button();
            this.labelTool = new System.Windows.Forms.Label();
            this.buttPause = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttResume
            // 
            this.buttResume.Location = new System.Drawing.Point(17, 178);
            this.buttResume.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.buttResume.Name = "buttResume";
            this.buttResume.Size = new System.Drawing.Size(106, 44);
            this.buttResume.TabIndex = 0;
            this.buttResume.Text = "Resume";
            this.buttResume.UseVisualStyleBackColor = true;
            this.buttResume.Click += new System.EventHandler(this.buttResume_Click);
            // 
            // labelTool
            // 
            this.labelTool.AutoSize = true;
            this.labelTool.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelTool.ForeColor = System.Drawing.Color.Maroon;
            this.labelTool.Location = new System.Drawing.Point(73, 22);
            this.labelTool.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelTool.Name = "labelTool";
            this.labelTool.Size = new System.Drawing.Size(184, 37);
            this.labelTool.TabIndex = 1;
            this.labelTool.Text = "Inset Tool #";
            // 
            // buttPause
            // 
            this.buttPause.Location = new System.Drawing.Point(268, 178);
            this.buttPause.Margin = new System.Windows.Forms.Padding(6);
            this.buttPause.Name = "buttPause";
            this.buttPause.Size = new System.Drawing.Size(106, 44);
            this.buttPause.TabIndex = 2;
            this.buttPause.Text = "Pause";
            this.buttPause.UseVisualStyleBackColor = true;
            this.buttPause.Click += new System.EventHandler(this.buttPause_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(288, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "Press resume when finished.";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label3.Location = new System.Drawing.Point(12, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(362, 68);
            this.label3.TabIndex = 4;
            this.label3.Text = "If you need to adjust head location or probe the new tool, press Pause, make the " +
    "adjustments, then press the Play button to continue.";
            // 
            // FormChangeTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 240);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttPause);
            this.Controls.Add(this.labelTool);
            this.Controls.Add(this.buttResume);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "FormChangeTool";
            this.Text = "Change Tool";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttResume;
        private System.Windows.Forms.Label labelTool;
        private System.Windows.Forms.Button buttPause;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}