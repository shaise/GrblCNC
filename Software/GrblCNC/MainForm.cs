using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using OpenTK;
using GrblCNC.Controls;
using GrblCNC.Glutils;


namespace GrblCNC
{
    public partial class MainForm : Form
    {
        Timer simTimer;
        Timer grblScanTimer;
        GcodeInterp ginterp;
        GrblComm grblComm;
        FormOffset frmOffset;
        FormProbe frmProbe;
        FormPopWindow frmPopup;
        FormChangeTool frmChangeTool;
        ParametersEdit grblParamEdit;
        ToolTableEdit toolTableEdit;
        ToolTable toolTable;
        bool keyHandled;
        bool keyboardJogActive = true;
        VisualizerWin visualizerWinMain;
        VisualizerOverlay visualizerOverlay;
        int presscount = 0;
        string lastGcodeFile = null;
        ErrorDisplayHandler errDisplayHandler;

        public MainForm()
        {
            // we want all key events to pass first on main window, so we can jog
            // regardless of selected sub control
            KeyPreview = true;

            InitializeComponent();
            InitializeGlControl();
            Global.SettingsPath = Assembly.GetEntryAssembly().Location;
            Global.ToolTableFile = Utils.SettingPath("ToolTable.gtt");
            
            toolTable = new ToolTable();
            Global.toolTable = toolTable;
            toolTable.Load(Global.ToolTableFile);

            Global.mdiControl = mdiCtrl;
            toolStrip1.Renderer = new ToolButton();
            ginterp = new GcodeInterp();
            Global.ginterp = ginterp;
            visualizerWinMain.ginterp = ginterp;
            visualizerWinMain.NewGcodeLoaded += visualizerWinMain_NewGcodeLoaded;
            gcodeMainViewer.SelectedLineChanged += gcodeMainViewer_SelectedLineChanged;
            simTimer = new Timer();
            simTimer.Interval = 40;
            simTimer.Tick += simTimer_Tick;
            // grbl communication
            grblComm = new GrblComm();
            grblComm.CommStatusChanged += grblComm_StatusChanged;
            grblComm.LineReceived += grblComm_LineReceived;
            grblComm.StatusUpdate += grblComm_StatusUpdate;
            grblComm.ParameterUpdate += grblComm_ParameterUpdate;
            grblComm.MessageReceived += grblComm_MessageReceived;
            grblComm.ChangeToolNotify += grblComm_ChangeToolNotify;
            Global.grblComm = grblComm;
            grblScanTimer = new Timer();
            grblScanTimer.Interval = 100;
            grblScanTimer.Tick += grblScanTimer_Tick;
            grblScanTimer.Start();

            // grbl manual controll
            manualControl.AxisStepJogPressed += manualControl_AxisStepJogPressed;
            manualControl.AxisContinuesJogPressed += manualControl_AxisContinuesJogPressed;
            manualControl.AxisActionPressed += manualControl_AxisActionPressed;
            manualControl.SpindleAction += manualControl_SpindleAction;

            grblParamEdit = new ParametersEdit();
            Global.grblParameterEditor = grblParamEdit;

            toolTableEdit = new ToolTableEdit();
            Global.toolTableEdit = toolTableEdit;


            frmOffset = new FormOffset();
            frmProbe = new FormProbe();
            frmPopup = new FormPopWindow();
            frmChangeTool = new FormChangeTool();
            Global.grblParameterEditor.SetPatrameterTemplate(Global.grblConfig.GetParamDescription());

            errDisplayHandler = new ErrorDisplayHandler(this);
        }

        void grblComm_ChangeToolNotify(object sender, int newTool, bool isRunning)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => { grblComm_ChangeToolNotify(sender, newTool, isRunning); }));
                return;
            }
            frmChangeTool.SetToolNumber(newTool);
            if (frmChangeTool.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (isRunning)
                    grblComm.SendGcode();
            }
        }

        void grblComm_MessageReceived(object sender, string message, GrblComm.MessageType type)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => { grblComm_MessageReceived(sender, message, type); }));
                return;
            }
            toolStripStatus.ForeColor = type == GrblComm.MessageType.Info ? Color.Blue : Color.Red;
            toolStripStatus.Text = message;
            if (type == GrblComm.MessageType.Alarm || type == GrblComm.MessageType.Error)
                errDisplayHandler.AddError(message, type == GrblComm.MessageType.Alarm);
        }

        void InitializeGlControl()
        {
            // 
            // visualizerWinMain 
            // copied from designr so VS will not crash
            this.visualizerWinMain = new GrblCNC.VisualizerWin();
            this.visualizerWinMain.BackColor = System.Drawing.Color.Black;
            this.visualizerWinMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visualizerWinMain.Location = new System.Drawing.Point(0, 0);
            this.visualizerWinMain.Name = "visualizerWinMain";
            this.visualizerWinMain.Size = new System.Drawing.Size(674, 440);
            this.visualizerWinMain.TabIndex = 0;
            this.visualizerWinMain.VSync = false;
            this.splitTopRight.Panel1.Controls.Add(this.visualizerWinMain);
            Global.visualizeWindow = this.visualizerWinMain;
            visualizerOverlay = new VisualizerOverlay(visualizerWinMain);
        }

        void PerformCoordTouchoff(int axis)
        {
            frmOffset.CoordSystem = Global.grblStatus.CurrentCoordystemIndex;
            if (frmOffset.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                float offset = frmOffset.Offset;
                grblComm.CoordTouchAxis(axis, frmOffset.CoordSystem, offset);
            }
        }

        void PerformProbe(int axis)
        {
            frmProbe.Axis = axis;
            if (frmProbe.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                grblComm.ProbeAxis(axis, -1, frmProbe.Offset, frmProbe.Direction);
            }
        }

        #region Manual control events
        void manualControl_SpindleAction(object sender, float speed, GrblComm.SpindleAction action)
        {
            if (grblComm == null)
                return;
            grblComm.SetSpindle(speed, action);
        }

        void manualControl_AxisActionPressed(object sender, int axis, Controls.ManualControl.AxisAction action)
        {
            if (grblComm == null)
                return;
            switch (action)
            {
                case GrblCNC.Controls.ManualControl.AxisAction.Home: grblComm.HomeAxis(axis); break;
                case ManualControl.AxisAction.CoordTouchOff: PerformCoordTouchoff(axis); break;
                case ManualControl.AxisAction.ToolProbe: PerformProbe(axis); break;
            }
            
        }

        void manualControl_AxisContinuesJogPressed(object sender, int axis, int direction)
        {
            if (grblComm != null)
                grblComm.ContinuesJog(axis, direction, 600);
        }

        void manualControl_AxisStepJogPressed(object sender, int axis, float amount)
        {
            if (grblComm != null)
                grblComm.StepJog(axis, amount, 600);
        }

        #endregion //Manual control events

        void grblComm_ParameterUpdate(object sender, GrblConfig grblConf, GCodeConfig gcodeConf)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => { grblComm_ParameterUpdate(sender, grblConf, gcodeConf); }));
                return;
            }
            paramView.FillParameters(grblConf);
            gcodeParamView.FillParameters(gcodeConf);
            manualControl.SetSliderMinMax(ManualControl.Sliders.SpindleSpeed,
                grblConf.GetParam(GrblConfig.GrblParam.Code.MinSpindleSpeedCode).floatVal,
                grblConf.GetParam(GrblConfig.GrblParam.Code.MaxSpindleSpeedCode).floatVal);
            Global.grblParameterEditor.UpdateGuiParams(grblConf.GetParams());
            toolStripProgressBuff.Maximum1 = 10;
            toolStripProgressBuff.Maximum2 = 10;
        }

        void grblComm_StatusUpdate(object sender, GrblStatus status)
        {
            if (InvokeRequired)
            {
                GrblStatus statCopy = status.Clone();
                BeginInvoke(new MethodInvoker(() => { grblComm_StatusUpdate(sender, statCopy); }));
                return;
            }
            Vector3 headpos = new Vector3(status.axisPos[0], status.axisPos[1], status.axisPos[2]);
            visualizerWinMain.SetMillheadPos(headpos);
            if (status.lineNumber > 0)
                gcodeMainViewer.SetSelectedLine(status.lineNumber - 1, true);
            toolStripEstop.Checked = status.state == GrblStatus.MachineState.Alarm;
            statusView.SetAxisValues(status.axisPos);
            statusView.SetAlarms(status.alarms);
            statusView.SetFeedSpindle(status.feedRate, status.spindleRpm);
            statusView.SetHomeState(status.homeStatus);
            if (status.gStateChange)
                mdiCtrl.SetGcodeParserStatus(status.gState);
            toolStripProgressBuff.Value1 = status.uartBuffer;
            toolStripProgressBuff.Value2 = status.planBuffer;
            toolStripStatusMode.Text = status.state.ToString();
            visualizerOverlay.Update(status);
        }

        void grblComm_LineReceived(object sender, string line, bool isStatus)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => { grblComm_LineReceived(sender, line, isStatus); }));
                return;
            }
            if (!isStatus)
                mdiCtrl.AddLine(line);
        }

        void grblScanTimer_Tick(object sender, EventArgs e)
        {
            grblComm.CommPoll();
        }

        void grblComm_StatusChanged(object sender, GrblComm.CommStatus status)
        {
            if (InvokeRequired)
            {
                // different thread so must invoke
                BeginInvoke(new MethodInvoker(() => { grblComm_StatusChanged(sender, status); }));
                return;
            }
            if (status == GrblComm.CommStatus.Connected)
            {
                toolStripGrbl.Enabled = true;
                toolStripGrbl.ToolTipText = "Grbl Connected at port " + grblComm.activePort;
                toolStripConfGrbl.Enabled = true;
                toolStripPower.Enabled = true;
                Global.GrblConnected = true;
            }
            else
            {
                toolStripGrbl.Enabled = false;
                toolStripGrbl.ToolTipText = "GRBL Disconnected";
                toolStripConfGrbl.Enabled = false;
                toolStripPower.Enabled = false;
                Global.GrblConnected = false;
            }
        }

        void gcodeMainViewer_SelectedLineChanged(object obj, int selLine)
        {
            if (ginterp == null)
                return;
            ginterp.SetHighlightedGCode(selLine, simTimer.Enabled || 
                grblComm.machineState == GrblComm.MachineState.Running);
            visualizerWinMain.Invalidate();
        }

        void visualizerWinMain_NewGcodeLoaded(object sender, string[] lines)
        {
            gcodeMainViewer.SetLines(lines);
        }

        public void Error(string errmsg)
        {
            MessageBox.Show("Error", errmsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        void OpenNewGcodeFile()
        {
            if (openGcodeFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string res = visualizerWinMain.LoadGcodeFile(openGcodeFile.FileName);
                if (res != "OK")
                    Error(res);
                else
                    lastGcodeFile = openGcodeFile.FileName;
            }
        }

        #region Menu commands
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenNewGcodeFile();
        }

        #endregion


        void simTimer_Tick(object sender, EventArgs e)
        {
            Vector3 headPos;
            int gcodeLine;
            if (ginterp.StepSimulation(out headPos, out gcodeLine))
                simTimer.Stop();
            visualizerWinMain.SetMillheadPos(headPos);
            gcodeMainViewer.SetSelectedLine(gcodeLine, true);
        }

        #region Tool Buttons
        private void toolStripStart_Click(object sender, EventArgs e)
        {
            //ginterp.InitSimulation();
            //simTimer.Start();
            grblComm.SendGcode();
        }

        private void toolStripStop_Click(object sender, EventArgs e)
        {
            grblComm.StopGcode();
        }

        private void toolStripPause_Click(object sender, EventArgs e)
        {
            grblComm.PauseGcode();
        }

        private void toolStripStep_Click(object sender, EventArgs e)
        {
            grblComm.StepGcode();
            gcodeMainViewer.SetSelectedLine(ginterp.CurrentLineNumber, true);
        }

        private void toolStripPower_Click(object sender, EventArgs e)
        {
        }

        private void toolStripConfGrbl_Click(object sender, EventArgs e)
        {
            frmPopup.ShowControl(grblParamEdit, "Configure Grbl driver");
        }

        private void toolStripEstop_Click(object sender, EventArgs e)
        {
            if (toolStripEstop.Checked)
                grblComm.AlarmRelease();
            else
                grblComm.EmergencyStop();
        }

        private void toolStripOpen_Click(object sender, EventArgs e)
        {
            OpenNewGcodeFile();
        }

        private void toolStripReload_Click(object sender, EventArgs e)
        {

        }

        private void toolStripToolTable_Click(object sender, EventArgs e)
        {
            toolTableEdit.UpdateFromGlobal();
            frmPopup.ShowControl(toolTableEdit, "Edit Tool Table");
        }

        #endregion

        void StepJog(int axis, float dir = 1)
        {
            float dist = manualControl.GetSelectedJogStep();
            if (dist == 0)
                grblComm.ContinuesJog(axis, dir, 2400);
            else
                grblComm.StepJog(axis, dir * dist, 2400);
            keyHandled = true;
        }

        #region Class overrides
        // using this instead of keyDown because it seems sometime keyDown not sent even if KeyPreview is set
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyboardJogActive)
            {
                presscount++;
                keyHandled = false;
                switch (keyData)
                {
                    case Keys.Right: StepJog(GrblComm.X_AXIS); break;
                    case Keys.Left: StepJog(GrblComm.X_AXIS, -1); break;
                    case Keys.Up: StepJog(GrblComm.Y_AXIS); break;
                    case Keys.Down: StepJog(GrblComm.Y_AXIS, -1); break;
                    case Keys.PageUp: StepJog(GrblComm.Z_AXIS); break;
                    case Keys.PageDown: StepJog(GrblComm.Z_AXIS, -1); break;
                }
                if (keyHandled)
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (manualControl.GetSelectedJogStep() == 0 && keyboardJogActive)
            {
                if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Left || e.KeyCode == Keys.Up ||
                    e.KeyCode == Keys.Down || e.KeyCode == Keys.PageUp || e.KeyCode == Keys.PageDown)
                    grblComm.ContinuesJog(0, 0, 0);
            }
            base.OnKeyUp(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            grblComm.Close();
            base.OnClosing(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (errDisplayHandler != null)
                errDisplayHandler.UpdateSize();
            base.OnSizeChanged(e);
        }

        #endregion

        private void tabControlSystem_Selected(object sender, TabControlEventArgs e)
        {
            keyboardJogActive = e.TabPage == tabControlPanel;
//            if (e.TabPage == tabMdi)
        }

        private void tabMdi_Enter(object sender, EventArgs e)
        {
            mdiCtrl.Select(); ;
        }



    }

}
