namespace GrblCNC.Controls
{
    partial class ParametersEdit
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
            this.horizTabs = new GrblCNC.Controls.HorizTab();
            this.SuspendLayout();
            // 
            // horizTabs
            // 
            this.horizTabs.Location = new System.Drawing.Point(3, 3);
            this.horizTabs.Name = "horizTabs";
            this.horizTabs.Size = new System.Drawing.Size(146, 385);
            this.horizTabs.TabIndex = 0;
            this.horizTabs.TabTexts = new string[] {
        "General",
        "Hardware timings"};
            this.horizTabs.Title = "Categories";
            // 
            // ParametersEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.horizTabs);
            this.Name = "ParametersEdit";
            this.Size = new System.Drawing.Size(461, 391);
            this.ResumeLayout(false);

        }

        #endregion

        private HorizTab horizTabs;
    }
}
