using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;


namespace GrblCNC
{
    public partial class MainForm : Form
    {
        Timer simTimer;
        Timer grblScanTimer;
        GcodeInterp ginterp;
        GrblComm grblComm;
        bool keyHandled;
        bool keyboardJogActive = true;
        private VisualizerWin visualizerWinMain;
        int presscount = 0;

        public MainForm()
        {
            InitializeComponent();
            InitializeGlControl();
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
            grblComm.StatusChanged += grblComm_StatusChanged;
            grblComm.LineReceived += grblComm_LineReceived;
            grblComm.StatusUpdate += grblComm_StatusUpdate;
            grblComm.ParameterUpdate += grblComm_ParameterUpdate;
            grblComm.ErrorDetected += grblComm_ErrorDetected;
            Global.grblComm = grblComm;
            grblScanTimer = new Timer();
            grblScanTimer.Interval = 100;
            grblScanTimer.Tick += grblScanTimer_Tick;
            grblScanTimer.Start();

            // grbl manual controll
            manualControl.AxisStepJogPressed += manualControl_AxisStepJogPressed;
            manualControl.AxisContinuesJogPressed += manualControl_AxisContinuesJogPressed;
            manualControl.AxisHomePressed += manualControl_AxisHomePressed;

            // we want all key events to pass first on main window, so we can jog
            // regardless of selected sub control
            KeyPreview = true;
        }

        void grblComm_ErrorDetected(object sender, string err)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => { grblComm_ErrorDetected(sender, err); }));
                return;
            }
            toolStripStatus.ForeColor = Color.Red;
            toolStripStatus.Text = err;
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
        }

        void manualControl_AxisHomePressed(object sender, int axis)
        {
            if (grblComm != null)
                grblComm.HomeAxis(axis);
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

        void grblComm_ParameterUpdate(object sender, GrblConfig config)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => { grblComm_ParameterUpdate(sender, config); }));
                return;
            }
            paramView.FillParameters(config);
            toolStripProgressBuff.Maximum1 = 10;
            toolStripProgressBuff.Maximum2 = 10;
        }

        void grblComm_StatusUpdate(object sender, GrblStatus status)
        {
            if (InvokeRequired)
            {
                GrblStatus statCopy = status.Clone();
                Invoke(new MethodInvoker(() => { grblComm_StatusUpdate(sender, statCopy); }));
                return;
            }
            Vector3 headpos = new Vector3(status.axisPos[0], status.axisPos[1], status.axisPos[2]);
            visualizerWinMain.SetMillheadPos(headpos);
            if (status.lineNumber > 0)
                gcodeMainViewer.SetSelectedLine(status.lineNumber - 1, true);
            statusView.SetAxisValues(status.axisPos);
            statusView.SetAlarms(status.alarms);
            statusView.SetHomeState(status.homeStatus);
            toolStripProgressBuff.Value1 = status.uartBuffer;
            toolStripProgressBuff.Value2 = status.planBuffer;
        }

        void grblComm_LineReceived(object sender, string line, bool isStatus)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => { grblComm_LineReceived(sender, line, isStatus); }));
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
                Invoke(new MethodInvoker(() => { grblComm_StatusChanged(sender, status); }));
                return;
            }
            if (status == GrblComm.CommStatus.Connected)
            {
                toolStripGrbl.Enabled = true;
                toolStripGrbl.ToolTipText = "Grbl Connected at port " + grblComm.activePort;
                Global.GrblConnected = true;
            }
            else
            {
                toolStripGrbl.Enabled = false;
                toolStripGrbl.ToolTipText = "GRBL Disconnected";
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

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openGcodeFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

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

        #region Run Buttons
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
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (keyboardJogActive)
            {
                presscount++;
                keyHandled = false;
                switch (e.KeyCode)
                {
                    case Keys.Right: StepJog(GrblComm.X_AXIS); break;
                    case Keys.Left: StepJog(GrblComm.X_AXIS, -1); break;
                    case Keys.Up: StepJog(GrblComm.Y_AXIS); break;
                    case Keys.Down: StepJog(GrblComm.Y_AXIS, -1); break;
                    case Keys.PageUp: StepJog(GrblComm.Z_AXIS); break;
                    case Keys.PageDown: StepJog(GrblComm.Z_AXIS, -1); break;
                }
                e.Handled = keyHandled;
            }
            base.OnKeyDown(e);
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
        #endregion

        private void tabControlSystem_Selected(object sender, TabControlEventArgs e)
        {
            keyboardJogActive = e.TabPage == tabControlPanel;
        }


    }

}
