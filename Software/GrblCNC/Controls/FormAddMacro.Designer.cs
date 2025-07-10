namespace GrblCNC.Controls
{
    partial class FormAddMacro
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
            this.comboFuncKey = new System.Windows.Forms.ComboBox();
            this.textDescription = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textGCode = new System.Windows.Forms.TextBox();
            this.buttUpdate = new System.Windows.Forms.Button();
            this.buttCancel = new System.Windows.Forms.Button();
            this.buttClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Macro function key:";
            // 
            // comboFuncKey
            // 
            this.comboFuncKey.FormattingEnabled = true;
            this.comboFuncKey.Items.AddRange(new object[] {
            "F1",
            "F2",
            "F3",
            "F4",
            "F5",
            "F6",
            "F7",
            "F8",
            "F9"});
            this.comboFuncKey.Location = new System.Drawing.Point(160, 12);
            this.comboFuncKey.Name = "comboFuncKey";
            this.comboFuncKey.Size = new System.Drawing.Size(121, 21);
            this.comboFuncKey.TabIndex = 1;
            this.comboFuncKey.SelectedIndexChanged += new System.EventHandler(this.comboFuncKey_SelectedIndexChanged);
            // 
            // textDescription
            // 
            this.textDescription.Location = new System.Drawing.Point(91, 39);
            this.textDescription.Name = "textDescription";
            this.textDescription.Size = new System.Drawing.Size(190, 20);
            this.textDescription.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Description:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "GCode lines to send:";
            // 
            // textGCode
            // 
            this.textGCode.Location = new System.Drawing.Point(15, 84);
            this.textGCode.Multiline = true;
            this.textGCode.Name = "textGCode";
            this.textGCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textGCode.Size = new System.Drawing.Size(266, 168);
            this.textGCode.TabIndex = 5;
            // 
            // buttUpdate
            // 
            this.buttUpdate.Location = new System.Drawing.Point(15, 260);
            this.buttUpdate.Name = "buttUpdate";
            this.buttUpdate.Size = new System.Drawing.Size(75, 23);
            this.buttUpdate.TabIndex = 6;
            this.buttUpdate.Text = "Update";
            this.buttUpdate.UseVisualStyleBackColor = true;
            this.buttUpdate.Click += new System.EventHandler(this.buttUpdate_Click);
            // 
            // buttCancel
            // 
            this.buttCancel.Location = new System.Drawing.Point(206, 260);
            this.buttCancel.Name = "buttCancel";
            this.buttCancel.Size = new System.Drawing.Size(75, 23);
            this.buttCancel.TabIndex = 7;
            this.buttCancel.Text = "Cancel";
            this.buttCancel.UseVisualStyleBackColor = true;
            this.buttCancel.Click += new System.EventHandler(this.buttCancel_Click);
            // 
            // buttClear
            // 
            this.buttClear.Location = new System.Drawing.Point(125, 260);
            this.buttClear.Name = "buttClear";
            this.buttClear.Size = new System.Drawing.Size(75, 23);
            this.buttClear.TabIndex = 8;
            this.buttClear.Text = "Clear";
            this.buttClear.UseVisualStyleBackColor = true;
            this.buttClear.Click += new System.EventHandler(this.buttClear_Click);
            // 
            // FormAddMacro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(293, 295);
            this.Controls.Add(this.buttClear);
            this.Controls.Add(this.buttCancel);
            this.Controls.Add(this.buttUpdate);
            this.Controls.Add(this.textGCode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textDescription);
            this.Controls.Add(this.comboFuncKey);
            this.Controls.Add(this.label1);
            this.Name = "FormAddMacro";
            this.Text = "Add / Edit GCode Macro";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboFuncKey;
        private System.Windows.Forms.TextBox textDescription;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textGCode;
        private System.Windows.Forms.Button buttUpdate;
        private System.Windows.Forms.Button buttCancel;
        private System.Windows.Forms.Button buttClear;
    }
}