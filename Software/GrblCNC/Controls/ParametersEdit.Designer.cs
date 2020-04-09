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
            this.panelParams = new System.Windows.Forms.Panel();
            this.horizTabs = new GrblCNC.Controls.HorizTab();
            this.SuspendLayout();
            // 
            // panelParams
            // 
            this.panelParams.AutoScroll = true;
            this.panelParams.Location = new System.Drawing.Point(155, 3);
            this.panelParams.Name = "panelParams";
            this.panelParams.Size = new System.Drawing.Size(303, 385);
            this.panelParams.TabIndex = 1;
            // 
            // horizTabs
            // 
            this.horizTabs.Location = new System.Drawing.Point(3, 3);
            this.horizTabs.Name = "horizTabs";
            this.horizTabs.SelectedTab = 0;
            this.horizTabs.Size = new System.Drawing.Size(146, 385);
            this.horizTabs.TabIndex = 0;
            this.horizTabs.TabTexts = new string[] {
        "General",
        "Hardware timings"};
            this.horizTabs.Title = "Categories";
            this.horizTabs.SelectionChange += new GrblCNC.Controls.HorizTab.SelectionChangeDelegate(this.horizTabs_SelectionChange);
            // 
            // ParametersEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelParams);
            this.Controls.Add(this.horizTabs);
            this.Name = "ParametersEdit";
            this.Size = new System.Drawing.Size(461, 391);
            this.ResumeLayout(false);

        }

        #endregion

        private HorizTab horizTabs;
        private System.Windows.Forms.Panel panelParams;
    }
}
