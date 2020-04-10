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
            this.components = new System.ComponentModel.Container();
            this.panelParams = new System.Windows.Forms.Panel();
            this.buttSave = new System.Windows.Forms.Button();
            this.buttLoad = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.buttRevert = new System.Windows.Forms.Button();
            this.buttProgram = new System.Windows.Forms.Button();
            this.horizTabs = new GrblCNC.Controls.HorizTab();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
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
            // buttSave
            // 
            this.buttSave.Location = new System.Drawing.Point(3, 391);
            this.buttSave.Name = "buttSave";
            this.buttSave.Size = new System.Drawing.Size(75, 23);
            this.buttSave.TabIndex = 2;
            this.buttSave.Text = "Save";
            this.toolTip1.SetToolTip(this.buttSave, "Save to disk");
            this.buttSave.UseVisualStyleBackColor = true;
            this.buttSave.Click += new System.EventHandler(this.buttSave_Click);
            // 
            // buttLoad
            // 
            this.buttLoad.Location = new System.Drawing.Point(84, 391);
            this.buttLoad.Name = "buttLoad";
            this.buttLoad.Size = new System.Drawing.Size(75, 23);
            this.buttLoad.TabIndex = 3;
            this.buttLoad.Text = "Load";
            this.toolTip1.SetToolTip(this.buttLoad, "Load from disk");
            this.buttLoad.UseVisualStyleBackColor = true;
            this.buttLoad.Click += new System.EventHandler(this.buttLoad_Click);
            // 
            // buttRevert
            // 
            this.buttRevert.Location = new System.Drawing.Point(398, 391);
            this.buttRevert.Name = "buttRevert";
            this.buttRevert.Size = new System.Drawing.Size(60, 23);
            this.buttRevert.TabIndex = 4;
            this.buttRevert.Text = "Revert";
            this.toolTip1.SetToolTip(this.buttRevert, "Cancel all changes");
            this.buttRevert.UseVisualStyleBackColor = true;
            this.buttRevert.Click += new System.EventHandler(this.buttRevert_Click);
            // 
            // buttProgram
            // 
            this.buttProgram.Location = new System.Drawing.Point(276, 391);
            this.buttProgram.Name = "buttProgram";
            this.buttProgram.Size = new System.Drawing.Size(116, 23);
            this.buttProgram.TabIndex = 5;
            this.buttProgram.Text = "Program!";
            this.toolTip1.SetToolTip(this.buttProgram, "Program settings into Grbl driver");
            this.buttProgram.UseVisualStyleBackColor = true;
            this.buttProgram.Click += new System.EventHandler(this.buttProgram_Click);
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
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Grbl Configuration file|*.txt";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Grbl Configuration file|*.txt";
            // 
            // ParametersEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttProgram);
            this.Controls.Add(this.buttRevert);
            this.Controls.Add(this.buttLoad);
            this.Controls.Add(this.buttSave);
            this.Controls.Add(this.panelParams);
            this.Controls.Add(this.horizTabs);
            this.Name = "ParametersEdit";
            this.Size = new System.Drawing.Size(461, 420);
            this.ResumeLayout(false);

        }

        #endregion

        private HorizTab horizTabs;
        private System.Windows.Forms.Panel panelParams;
        private System.Windows.Forms.Button buttSave;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button buttLoad;
        private System.Windows.Forms.Button buttRevert;
        private System.Windows.Forms.Button buttProgram;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}
