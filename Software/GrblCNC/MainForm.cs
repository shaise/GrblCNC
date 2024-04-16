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
using System.Threading;
using System.IO;

namespace GrblCNC
{
    public partial class MainForm : Form
    {
        System.Windows.Forms.Timer simTimer;
        System.Windows.Forms.Timer grblScanTimer;
        GcodeInterp ginterp;
        GrblComm grblComm;
        GrblProbber grblProbber;
        FormOffset frmOffset;
        FormProbe frmAxisProbe;
        FormProbe frmToolProbe;
        FormGoto frmGoto;
        FormPopWindow frmPopup;
        FormChangeTool frmChangeTool;
        FormConfirmation frmConfirmation;
        FormAbout frmAbout;
        FormAddMacro frmAddMacro; 
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
            Global.SettingsPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            Global.ToolTableFile = Utils.SettingPath("ToolTable.gtt");
            
            toolTable = new ToolTable();
            Global.toolTable = toolTable;
            toolTable.Load(Global.ToolTableFile);

            Global.mdiControl = mdiCtrl;
            toolStripMain.Renderer = new ToolButton();
            ginterp = new GcodeInterp();
            Global.ginterp = ginterp;
            visualizerWinMain.ginterp = ginterp;
            visualizerWinMain.NewGcodeLoaded += visualizerWinMain_NewGcodeLoaded;
            gcodeMainViewer.SelectedLineChanged += gcodeMainViewer_SelectedLineChanged;
            simTimer = new System.Windows.Forms.Timer();
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
            grblComm.GrblStatusChanged += grblComm_GrblStatusChanged;
            Global.grblComm = grblComm;
            grblScanTimer = new System.Windows.Forms.Timer();
            grblScanTimer.Interval = 100;
            grblScanTimer.Tick += grblScanTimer_Tick;
            grblScanTimer.Start();

            // probe handler
            grblProbber = new GrblProbber(grblComm);

            // grbl manual controll
            manualControl.AxisStepJogPressed += manualControl_AxisStepJogPressed;
            manualControl.AxisContinuesJogPressed += manualControl_AxisContinuesJogPressed;
            manualControl.AxisActionPressed += manualControl_AxisActionPressed;
            manualControl.SpindleAction += manualControl_SpindleAction;
            manualControl.FeedOverride += ManualControl_FeedOverride;

            grblParamEdit = new ParametersEdit();
            Global.grblParameterEditor = grblParamEdit;

            toolTableEdit = new ToolTableEdit();
            Global.toolTableEdit = toolTableEdit;


            frmOffset = new FormOffset();
            frmAxisProbe = new FormProbe();
            frmAxisProbe.SetMode(FormProbe.Mode.Axis);
            frmToolProbe = new FormProbe();
            frmToolProbe.SetMode(FormProbe.Mode.Tool);
            frmGoto = new FormGoto();
            frmGoto.GotoActionPressed += FrmGoto_GotoActionPressed;
            frmPopup = new FormPopWindow();
            frmConfirmation = new FormConfirmation();
            frmChangeTool = new FormChangeTool();
            frmAbout = new FormAbout();
            frmAddMacro = new FormAddMacro();
            frmAddMacro.SetToolStrip(toolStripMain);
            //Global.grblParameterEditor.SetPatrameterTemplate(Global.grblConfig.GetParamDescription());

            errDisplayHandler = new ErrorDisplayHandler(this);
            Global.errHandler = errDisplayHandler;
        }

        private void ManualControl_FeedOverride(object sender, int feedPercent)
        {
            grblComm.SetFeedOverride(feedPercent);
        }

        void grblComm_GrblStatusChanged(object sender, GrblStatus.MachineState newState, GrblStatus.MachineState oldState)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => { grblComm_GrblStatusChanged(sender, newState, oldState); }));
                return;
            }
            toolStripStep.Enabled = newState == GrblStatus.MachineState.Idle;
            toolStripStart.Enabled = newState == GrblStatus.MachineState.Idle || newState == GrblStatus.MachineState.Hold;
            toolStripStart.Checked = newState == GrblStatus.MachineState.Run;
            toolStripPause.Enabled = newState == GrblStatus.MachineState.Run;
            toolStripPause.Checked = newState == GrblStatus.MachineState.Hold;
            frmAddMacro.EnableFuncs(newState == GrblStatus.MachineState.Idle);
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

        void PerformTouchOff(int axis)
        {
            frmAxisProbe.Axis = axis;
            frmAxisProbe.CoordSystem = Global.grblStatus.CurrentCoordystemIndex;
            if (frmAxisProbe.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (frmAxisProbe.IsProbe)
                    grblProbber.ProbeSingle(frmAxisProbe.Axis, frmOffset.CoordSystem, frmAxisProbe.Offset, frmAxisProbe.Direction);
                else
                    grblComm.CoordTouchAxis(frmAxisProbe.Axis, frmOffset.CoordSystem, frmAxisProbe.Offset);
            }
        }

        void PerformToolTouchOff(int axis)
        {
            frmToolProbe.Axis = GrblComm.Z_AXIS; // we always default axis to Z as it is most common.
            frmToolProbe.UpdateTools();
            frmToolProbe.Tool = Global.ginterp.currentTool;
            if (frmToolProbe.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (frmToolProbe.IsProbe)
                    grblComm.ProbeTool(frmToolProbe.Axis, frmToolProbe.Tool, frmToolProbe.Offset, frmToolProbe.Direction);
                else
                    grblComm.ToolTouchOff(frmToolProbe.Axis, frmToolProbe.Tool, frmToolProbe.Offset);
            }
        }

        void PerformGoto(int axis)
        {
            frmGoto.Axis = 1 << axis;
            frmGoto.ShowDialog();
        }

        private void FrmGoto_GotoActionPressed(object sender, GoToAction action)
        {
            grblComm.GoTo(action.axisMask, action.coordSystem, action.pos);
        }

        void PerformHoming(int axis)
        {
            bool needconfirm = false;
            if (axis < 0)
            { 
                // home all
                if (Global.grblStatus.homedMask != 0)
                {
                    frmConfirmation.SetMessage("Some or all axis are already homed. Do you want to home again?");
                    needconfirm = true;
                }
            }
            else
            {
                if ((Global.grblStatus.homedMask & (1 << axis)) != 0)
                {
                    frmConfirmation.SetMessage(GrblUtils.GetAxisLetter(axis) + 
                        " axis is already homed. Do you want to home it again?");
                    needconfirm = true;
                }
            }
            if (needconfirm && frmConfirmation.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            grblComm.HomeAxis(axis);
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
                case ManualControl.AxisAction.Home:          PerformHoming(axis); break;
                case ManualControl.AxisAction.CoordTouchOff: PerformTouchOff(axis); break;
                case ManualControl.AxisAction.ToolTouchOff:  PerformToolTouchOff(axis); break;
                case ManualControl.AxisAction.GoTo:          PerformGoto(axis); break;
                case ManualControl.AxisAction.ProbeHole:     grblProbber.ProbeHoleCenter(); break;
            }

        }

        void manualControl_AxisContinuesJogPressed(object sender, int axis, int direction, float speed)
        {
            if (grblComm != null)
                grblComm.ContinuesJog(axis, direction, speed);
        }

        void manualControl_AxisStepJogPressed(object sender, int axis, float amount, float speed)
        {
            if (grblComm != null)
                grblComm.StepJog(axis, amount, speed);
        }

        #endregion //Manual control events

        void grblComm_ParameterUpdate(object sender, GrblConfig grblConf, GCodeConfig gcodeConf)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => { grblComm_ParameterUpdate(sender, grblConf, gcodeConf); }));
                return;
            }
            //paramView.FillParameters(grblConf);
            if (Global.grblConfig.NewConfiguration)
            {
                Global.grblParameterEditor.SetPatrameterTemplate(Global.grblConfig.GetParamDescription());
                Global.grblConfig.NewConfiguration = false;
            }
            gcodeParamView.FillParameters(gcodeConf);
            manualControl.SetSliderMinMax(ManualControl.Sliders.SpindleSpeed,
                grblConf.GetParam(GrblConfig.GrblParam.Code.MinSpindleSpeedCode).floatVal,
                grblConf.GetParam(GrblConfig.GrblParam.Code.MaxSpindleSpeedCode).floatVal);
            manualControl.SetSliderMinMax(ManualControl.Sliders.JogSpeed, 0,
                grblConf.GetParam(GrblConfig.GrblParam.Code.MaxXaxisRate).floatVal);
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
            if (status.lineNumber > 0 && status.state == GrblStatus.MachineState.Run)
                gcodeMainViewer.SetSelectedLine(status.lineNumber - 1, true);
            toolStripEstop.Checked = status.state == GrblStatus.MachineState.Alarm;
            statusView.SetAxisValues(status.axisPos);
            statusView.SetAlarms(status.alarms);
            statusView.SetFeedSpindle(status.feedRate, status.spindleRpm);
            statusView.SetHomeState(status.homedMask);
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
                toolStripGrbl.ToolTipText = "Grbl Connected at port " + grblComm.activePort.CommName + ", HW ver:" + grblComm.grblVersion;
                toolStripConfGrbl.Enabled = true;
                toolStripPower.Enabled = true;
                toolStripPower.Image = grblComm.CommType == "Telnet" ? Properties.Resources.WifiButt : Properties.Resources.powerButt;
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




        void LoadGcodeFile(string fileName)
        {
            if (fileName == null)
                return;
            string res = visualizerWinMain.LoadGcodeFile(fileName);
            if (res != "OK")
                Error(res);
            else
                lastGcodeFile = openGcodeFile.FileName;
        }

        void OpenNewGcodeFile()
        {
            if (openGcodeFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LoadGcodeFile(openGcodeFile.FileName); 
            }
        }

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
            frmConfirmation.SetMessage("This will reset the Grbl CNC driver. After that, location will be lost and the CNC must be rehomed");
            if (frmConfirmation.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (grblComm != null)
                    grblComm.SendSoftReset();
            }
        }

        private void toolStripConfGrbl_Click(object sender, EventArgs e)
        {
            //if (Global.grblConfig.NewConfiguration)
            //{
            //    Global.grblParameterEditor.SetPatrameterTemplate(Global.grblConfig.GetParamDescription());
            //    Global.grblConfig.NewConfiguration = false;
            //}

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
            LoadGcodeFile(lastGcodeFile);
        }

        private void toolStripToolTable_Click(object sender, EventArgs e)
        {
            toolTableEdit.UpdateFromGlobal();
            frmPopup.ShowControl(toolTableEdit, "Edit Tool Table");
        }

        private void toolStripAbout_Click(object sender, EventArgs e)
        {
            frmAbout.ShowDialog();
        }
        #endregion

        private void toolStripAddMacro_Click(object sender, EventArgs e)
        {
            frmAddMacro.ShowDialog();
        }

        void StepJog(int axis, float dir = 1)
        {
            float dist = manualControl.GetSelectedJogStep();
            if (dist == 0)
                grblComm.ContinuesJog(axis, dir, manualControl.GetJogSpeed());
            else
                grblComm.StepJog(axis, dir * dist, manualControl.GetJogSpeed());
            manualControl.SetCurrentAxis(axis);
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
            Global.AppClosing = true;
            grblComm.Close();
            Thread.Sleep(200);
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
