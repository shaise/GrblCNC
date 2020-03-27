namespace GrblCNC
{
    partial class MainForm
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
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripEstop = new System.Windows.Forms.ToolStripButton();
            this.toolStripPower = new System.Windows.Forms.ToolStripButton();
            this.toolStripGrbl = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripReload = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripStart = new System.Windows.Forms.ToolStripButton();
            this.toolStripStep = new System.Windows.Forms.ToolStripButton();
            this.toolStripPause = new System.Windows.Forms.ToolStripButton();
            this.toolStripStop = new System.Windows.Forms.ToolStripButton();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.splitBottom = new System.Windows.Forms.SplitContainer();
            this.openGcodeFile = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.splitTop = new System.Windows.Forms.SplitContainer();
            this.tabControlSystem = new System.Windows.Forms.TabControl();
            this.tabControlPanel = new System.Windows.Forms.TabPage();
            this.manualControl = new GrblCNC.Controls.ManualControl();
            this.tabMdi = new System.Windows.Forms.TabPage();
            this.mdiCtrl = new GrblCNC.Controls.MdiControl();
            this.tabGrbl = new System.Windows.Forms.TabPage();
            this.paramView = new GrblCNC.ParameterViewer();
            this.splitTopRight = new System.Windows.Forms.SplitContainer();
            this.gcodeMainViewer = new GrblCNC.GcodeViewer();
            this.statusView = new GrblCNC.StatusViewer();
            this.toolStripProgressBuff = new GrblCNC.Controls.DualProgressTool();
            this.menuMain.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitBottom)).BeginInit();
            this.splitBottom.Panel1.SuspendLayout();
            this.splitBottom.Panel2.SuspendLayout();
            this.splitBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitTop)).BeginInit();
            this.splitTop.Panel1.SuspendLayout();
            this.splitTop.Panel2.SuspendLayout();
            this.splitTop.SuspendLayout();
            this.tabControlSystem.SuspendLayout();
            this.tabControlPanel.SuspendLayout();
            this.tabMdi.SuspendLayout();
            this.tabGrbl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitTopRight)).BeginInit();
            this.splitTopRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(1264, 24);
            this.menuMain.TabIndex = 1;
            this.menuMain.Text = "menuMain";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBuff,
            this.toolStripStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 659);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1264, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(32, 17);
            this.toolStripStatusLabel1.Text = "Buff:";
            // 
            // toolStripStatus
            // 
            this.toolStripStatus.Name = "toolStripStatus";
            this.toolStripStatus.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatus.Text = "Status";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripEstop,
            this.toolStripPower,
            this.toolStripGrbl,
            this.toolStripSeparator1,
            this.toolStripOpen,
            this.toolStripReload,
            this.toolStripSeparator2,
            this.toolStripStart,
            this.toolStripStep,
            this.toolStripPause,
            this.toolStripStop});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1264, 47);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripEstop
            // 
            this.toolStripEstop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripEstop.Image = global::GrblCNC.Properties.Resources.estop;
            this.toolStripEstop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripEstop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripEstop.Name = "toolStripEstop";
            this.toolStripEstop.Size = new System.Drawing.Size(44, 44);
            this.toolStripEstop.Text = "EStop";
            this.toolStripEstop.ToolTipText = "Emergency Stop";
            this.toolStripEstop.Click += new System.EventHandler(this.toolStripEstop_Click);
            // 
            // toolStripPower
            // 
            this.toolStripPower.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripPower.Image = global::GrblCNC.Properties.Resources.powerButt;
            this.toolStripPower.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripPower.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripPower.Name = "toolStripPower";
            this.toolStripPower.Size = new System.Drawing.Size(44, 44);
            this.toolStripPower.Text = "Power";
            this.toolStripPower.ToolTipText = "Power on CNC driver";
            this.toolStripPower.Click += new System.EventHandler(this.toolStripPower_Click);
            // 
            // toolStripGrbl
            // 
            this.toolStripGrbl.Enabled = false;
            this.toolStripGrbl.Image = global::GrblCNC.Properties.Resources.grblIcon;
            this.toolStripGrbl.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripGrbl.Name = "toolStripGrbl";
            this.toolStripGrbl.Size = new System.Drawing.Size(50, 44);
            this.toolStripGrbl.ToolTipText = "GRBL Disconnected";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 47);
            // 
            // toolStripOpen
            // 
            this.toolStripOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripOpen.Image = global::GrblCNC.Properties.Resources.OpenButt;
            this.toolStripOpen.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripOpen.Name = "toolStripOpen";
            this.toolStripOpen.Size = new System.Drawing.Size(44, 44);
            this.toolStripOpen.Text = "Open";
            this.toolStripOpen.ToolTipText = "Open GCODE file";
            this.toolStripOpen.Click += new System.EventHandler(this.toolStripOpen_Click);
            // 
            // toolStripReload
            // 
            this.toolStripReload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripReload.Image = global::GrblCNC.Properties.Resources.ReloaButt;
            this.toolStripReload.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripReload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripReload.Name = "toolStripReload";
            this.toolStripReload.Size = new System.Drawing.Size(44, 44);
            this.toolStripReload.Text = "Reload";
            this.toolStripReload.ToolTipText = "Reload GCODE file";
            this.toolStripReload.Click += new System.EventHandler(this.toolStripReload_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 47);
            // 
            // toolStripStart
            // 
            this.toolStripStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripStart.Image = global::GrblCNC.Properties.Resources.playButt;
            this.toolStripStart.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripStart.Name = "toolStripStart";
            this.toolStripStart.Size = new System.Drawing.Size(44, 44);
            this.toolStripStart.Text = "Start";
            this.toolStripStart.ToolTipText = "Start CNC program";
            this.toolStripStart.Click += new System.EventHandler(this.toolStripStart_Click);
            // 
            // toolStripStep
            // 
            this.toolStripStep.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripStep.Image = global::GrblCNC.Properties.Resources.StepButt;
            this.toolStripStep.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripStep.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripStep.Name = "toolStripStep";
            this.toolStripStep.Size = new System.Drawing.Size(44, 44);
            this.toolStripStep.Text = "Start";
            this.toolStripStep.ToolTipText = "Start CNC program";
            this.toolStripStep.Click += new System.EventHandler(this.toolStripStep_Click);
            // 
            // toolStripPause
            // 
            this.toolStripPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripPause.Image = global::GrblCNC.Properties.Resources.pauseButt;
            this.toolStripPause.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripPause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripPause.Name = "toolStripPause";
            this.toolStripPause.Size = new System.Drawing.Size(44, 44);
            this.toolStripPause.Text = "Pause";
            this.toolStripPause.ToolTipText = "Pause CNC program";
            this.toolStripPause.Click += new System.EventHandler(this.toolStripPause_Click);
            // 
            // toolStripStop
            // 
            this.toolStripStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripStop.Image = global::GrblCNC.Properties.Resources.stopButt;
            this.toolStripStop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripStop.Name = "toolStripStop";
            this.toolStripStop.Size = new System.Drawing.Size(44, 44);
            this.toolStripStop.Text = "Stop";
            this.toolStripStop.ToolTipText = "Stop CNC Program";
            this.toolStripStop.Click += new System.EventHandler(this.toolStripStop_Click);
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 71);
            this.splitMain.Name = "splitMain";
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.splitTop);
            this.splitMain.Panel1MinSize = 440;
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.splitBottom);
            this.splitMain.Size = new System.Drawing.Size(1264, 588);
            this.splitMain.SplitterDistance = 455;
            this.splitMain.TabIndex = 4;
            // 
            // splitBottom
            // 
            this.splitBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitBottom.Location = new System.Drawing.Point(0, 0);
            this.splitBottom.Name = "splitBottom";
            // 
            // splitBottom.Panel1
            // 
            this.splitBottom.Panel1.Controls.Add(this.gcodeMainViewer);
            // 
            // splitBottom.Panel2
            // 
            this.splitBottom.Panel2.Controls.Add(this.statusView);
            this.splitBottom.Size = new System.Drawing.Size(1264, 129);
            this.splitBottom.SplitterDistance = 642;
            this.splitBottom.TabIndex = 0;
            // 
            // openGcodeFile
            // 
            this.openGcodeFile.Filter = "GCode Files|*.nc;*.ngc|All Files|*.*";
            // 
            // splitTop
            // 
            this.splitTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitTop.Location = new System.Drawing.Point(0, 0);
            this.splitTop.Name = "splitTop";
            // 
            // splitTop.Panel1
            // 
            this.splitTop.Panel1.Controls.Add(this.tabControlSystem);
            this.splitTop.Panel1MinSize = 300;
            // 
            // splitTop.Panel2
            // 
            this.splitTop.Panel2.Controls.Add(this.splitTopRight);
            this.splitTop.Panel2MinSize = 100;
            this.splitTop.Size = new System.Drawing.Size(1264, 455);
            this.splitTop.SplitterDistance = 385;
            this.splitTop.TabIndex = 0;
            // 
            // tabControlSystem
            // 
            this.tabControlSystem.Controls.Add(this.tabControlPanel);
            this.tabControlSystem.Controls.Add(this.tabMdi);
            this.tabControlSystem.Controls.Add(this.tabGrbl);
            this.tabControlSystem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlSystem.Location = new System.Drawing.Point(0, 0);
            this.tabControlSystem.Name = "tabControlSystem";
            this.tabControlSystem.SelectedIndex = 0;
            this.tabControlSystem.Size = new System.Drawing.Size(385, 455);
            this.tabControlSystem.TabIndex = 0;
            this.tabControlSystem.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControlSystem_Selected);
            // 
            // tabControlPanel
            // 
            this.tabControlPanel.Controls.Add(this.manualControl);
            this.tabControlPanel.Location = new System.Drawing.Point(4, 22);
            this.tabControlPanel.Name = "tabControlPanel";
            this.tabControlPanel.Padding = new System.Windows.Forms.Padding(3);
            this.tabControlPanel.Size = new System.Drawing.Size(377, 429);
            this.tabControlPanel.TabIndex = 0;
            this.tabControlPanel.Text = "Manual Control (F3)";
            this.tabControlPanel.UseVisualStyleBackColor = true;
            // 
            // manualControl
            // 
            this.manualControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(220)))), ((int)(((byte)(232)))));
            this.manualControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.manualControl.Enabled = false;
            this.manualControl.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.manualControl.Location = new System.Drawing.Point(3, 3);
            this.manualControl.Name = "manualControl";
            this.manualControl.Size = new System.Drawing.Size(371, 423);
            this.manualControl.TabIndex = 0;
            // 
            // tabMdi
            // 
            this.tabMdi.Controls.Add(this.mdiCtrl);
            this.tabMdi.Location = new System.Drawing.Point(4, 22);
            this.tabMdi.Name = "tabMdi";
            this.tabMdi.Padding = new System.Windows.Forms.Padding(3);
            this.tabMdi.Size = new System.Drawing.Size(377, 429);
            this.tabMdi.TabIndex = 1;
            this.tabMdi.Text = "MDI (F5)";
            this.tabMdi.UseVisualStyleBackColor = true;
            this.tabMdi.Enter += new System.EventHandler(this.tabMdi_Enter);
            // 
            // mdiCtrl
            // 
            this.mdiCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mdiCtrl.Location = new System.Drawing.Point(3, 3);
            this.mdiCtrl.Name = "mdiCtrl";
            this.mdiCtrl.Size = new System.Drawing.Size(371, 423);
            this.mdiCtrl.TabIndex = 0;
            // 
            // tabGrbl
            // 
            this.tabGrbl.Controls.Add(this.paramView);
            this.tabGrbl.Location = new System.Drawing.Point(4, 22);
            this.tabGrbl.Name = "tabGrbl";
            this.tabGrbl.Padding = new System.Windows.Forms.Padding(1);
            this.tabGrbl.Size = new System.Drawing.Size(377, 429);
            this.tabGrbl.TabIndex = 2;
            this.tabGrbl.Text = "Grbl";
            this.tabGrbl.UseVisualStyleBackColor = true;
            // 
            // paramView
            // 
            this.paramView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paramView.Location = new System.Drawing.Point(1, 1);
            this.paramView.Name = "paramView";
            this.paramView.Size = new System.Drawing.Size(375, 427);
            this.paramView.TabIndex = 0;
            // 
            // splitTopRight
            // 
            this.splitTopRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitTopRight.Location = new System.Drawing.Point(0, 0);
            this.splitTopRight.Name = "splitTopRight";
            // 
            // splitTopRight.Panel1
            // 
            this.splitTopRight.Panel1.Padding = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.splitTopRight.Panel2Collapsed = true;
            this.splitTopRight.Size = new System.Drawing.Size(875, 455);
            this.splitTopRight.SplitterDistance = 241;
            this.splitTopRight.TabIndex = 0;
            // 
            // gcodeMainViewer
            // 
            this.gcodeMainViewer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gcodeMainViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcodeMainViewer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gcodeMainViewer.Location = new System.Drawing.Point(0, 0);
            this.gcodeMainViewer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcodeMainViewer.Name = "gcodeMainViewer";
            this.gcodeMainViewer.ShowLineNumbers = true;
            this.gcodeMainViewer.Size = new System.Drawing.Size(642, 129);
            this.gcodeMainViewer.TabIndex = 0;
            // 
            // statusView
            // 
            this.statusView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusView.Location = new System.Drawing.Point(0, 0);
            this.statusView.Name = "statusView";
            this.statusView.Size = new System.Drawing.Size(618, 129);
            this.statusView.TabIndex = 0;
            // 
            // toolStripProgressBuff
            // 
            this.toolStripProgressBuff.AutoSize = false;
            this.toolStripProgressBuff.Color1 = System.Drawing.Color.Blue;
            this.toolStripProgressBuff.Color2 = System.Drawing.Color.Blue;
            this.toolStripProgressBuff.Maximum1 = 100;
            this.toolStripProgressBuff.Maximum2 = 100;
            this.toolStripProgressBuff.Name = "toolStripProgressBuff";
            this.toolStripProgressBuff.Size = new System.Drawing.Size(60, 20);
            this.toolStripProgressBuff.Value1 = 0;
            this.toolStripProgressBuff.Value2 = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuMain);
            this.MainMenuStrip = this.menuMain;
            this.Name = "MainForm";
            this.Text = "GrblCNC";
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.splitBottom.Panel1.ResumeLayout(false);
            this.splitBottom.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitBottom)).EndInit();
            this.splitBottom.ResumeLayout(false);
            this.splitTop.Panel1.ResumeLayout(false);
            this.splitTop.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitTop)).EndInit();
            this.splitTop.ResumeLayout(false);
            this.tabControlSystem.ResumeLayout(false);
            this.tabControlPanel.ResumeLayout(false);
            this.tabMdi.ResumeLayout(false);
            this.tabGrbl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitTopRight)).EndInit();
            this.splitTopRight.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.SplitContainer splitTop;
        private System.Windows.Forms.TabControl tabControlSystem;
        private System.Windows.Forms.TabPage tabControlPanel;
        private System.Windows.Forms.TabPage tabMdi;
        private System.Windows.Forms.SplitContainer splitTopRight;
        private System.Windows.Forms.SplitContainer splitBottom;
        private GcodeViewer gcodeMainViewer;
        private System.Windows.Forms.OpenFileDialog openGcodeFile;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripButton toolStripPower;
        private System.Windows.Forms.ToolStripButton toolStripPause;
        private System.Windows.Forms.ToolStripButton toolStripStop;
        private System.Windows.Forms.ToolStripButton toolStripEstop;
        private System.Windows.Forms.ToolStripButton toolStripStart;
        private System.Windows.Forms.ToolStripLabel toolStripGrbl;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripOpen;
        private System.Windows.Forms.ToolStripButton toolStripReload;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.TabPage tabGrbl;
        private ParameterViewer paramView;
        private StatusViewer statusView;
        private Controls.ManualControl manualControl;
        private Controls.MdiControl mdiCtrl;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private Controls.DualProgressTool toolStripProgressBuff;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatus;
        private System.Windows.Forms.ToolStripButton toolStripStep;
    }
}

