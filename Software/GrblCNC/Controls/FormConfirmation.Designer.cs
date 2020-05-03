namespace GrblCNC.Controls
{
    partial class FormConfirmation
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
            this.buttConfirm = new System.Windows.Forms.Button();
            this.labelTool = new System.Windows.Forms.Label();
            this.buttCancel = new System.Windows.Forms.Button();
            this.labelMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttConfirm
            // 
            this.buttConfirm.Location = new System.Drawing.Point(17, 153);
            this.buttConfirm.Margin = new System.Windows.Forms.Padding(6);
            this.buttConfirm.Name = "buttConfirm";
            this.buttConfirm.Size = new System.Drawing.Size(106, 44);
            this.buttConfirm.TabIndex = 0;
            this.buttConfirm.Text = "Confirm";
            this.buttConfirm.UseVisualStyleBackColor = true;
            this.buttConfirm.Click += new System.EventHandler(this.buttConfirm_Click);
            // 
            // labelTool
            // 
            this.labelTool.AutoSize = true;
            this.labelTool.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelTool.ForeColor = System.Drawing.Color.Maroon;
            this.labelTool.Location = new System.Drawing.Point(73, 22);
            this.labelTool.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelTool.Name = "labelTool";
            this.labelTool.Size = new System.Drawing.Size(215, 37);
            this.labelTool.TabIndex = 1;
            this.labelTool.Text = "Are you sure?";
            // 
            // buttCancel
            // 
            this.buttCancel.Location = new System.Drawing.Point(269, 153);
            this.buttCancel.Margin = new System.Windows.Forms.Padding(6);
            this.buttCancel.Name = "buttCancel";
            this.buttCancel.Size = new System.Drawing.Size(106, 44);
            this.buttCancel.TabIndex = 2;
            this.buttCancel.Text = "Cancel";
            this.buttCancel.UseVisualStyleBackColor = true;
            this.buttCancel.Click += new System.EventHandler(this.buttCancel_Click);
            // 
            // labelMessage
            // 
            this.labelMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelMessage.Location = new System.Drawing.Point(13, 68);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(362, 68);
            this.labelMessage.TabIndex = 4;
            this.labelMessage.Text = "This will reset the Grbl CNC driver. After that, location will be lost and the CN" +
    "C must be rehomed";
            // 
            // FormConfirmation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 213);
            this.Controls.Add(this.labelMessage);
            this.Controls.Add(this.buttCancel);
            this.Controls.Add(this.labelTool);
            this.Controls.Add(this.buttConfirm);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "FormConfirmation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Please confirm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttConfirm;
        private System.Windows.Forms.Label labelTool;
        private System.Windows.Forms.Button buttCancel;
        private System.Windows.Forms.Label labelMessage;
    }
}