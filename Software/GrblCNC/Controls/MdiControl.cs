using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq.Expressions;

namespace GrblCNC.Controls
{
    public partial class MdiControl : UserControl
    {
        List<string> mdiLines;
        int curMdiLine;
        public MdiControl()
        {
            InitializeComponent();
            mdiLines = new List<string>();
            curMdiLine = 0;
            Global.GrblConnectionChanged += Global_GrblConnectionChanged;
        }

        void Global_GrblConnectionChanged(bool isConnected)
        {
            buttSendGcodeLine.Enabled = isConnected;
            textGcodeLine.Enabled = isConnected;
        }

        private void buttSendGcodeLine_Click(object sender, EventArgs e)
        {
            if (Global.grblComm == null)
                return;
            string line = textGcodeLine.Text;
            if (line.Length == 0)
                return;
            //gcodeViewMDI.AddLine(line);
            Global.grblComm.PostLine(line);
            mdiLines.Add(line);
            curMdiLine = mdiLines.Count;
            textGcodeLine.Text = "";
        }

        private void textGcodeLine_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (curMdiLine > 0)
                    {
                        curMdiLine--;
                        textGcodeLine.Text = mdiLines[curMdiLine];
                        //textGcodeLine.c
                        e.Handled = true;
                    }
                    break;

                case Keys.Down:
                    if (curMdiLine < (mdiLines.Count - 1))
                    {
                        curMdiLine++;
                        textGcodeLine.Text = mdiLines[curMdiLine];
                        e.Handled = true;
                    }
                    break;
            }
        }

        private void textGcodeLine_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                buttSendGcodeLine.PerformClick();
            }
        }

        void UpdateControlLocations()
        {
            // scale internal controls
            gStateView.Location = new Point(6, Height - 40);
            gStateView.Width = Width - 6;
            textGcodeLine.Location = new Point(6, gStateView.Location.Y - 25);
            textGcodeLine.Width = Width - 73;
            buttSendGcodeLine.Location = new Point(Width - 61, gStateView.Location.Y - 26);
            gcodeViewMDI.Width = Width - 14;
            gcodeViewMDI.Height = gStateView.Location.Y - 38;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            UpdateControlLocations();
            base.OnSizeChanged(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            buttSendGcodeLine.Enabled = Global.GrblConnected;
            UpdateControlLocations();
            base.OnLoad(e);
        }

        public void AddLine(string line)
        {
            if (InvokeRequired)
            {
                try
                {
                    if (!Global.AppClosing)
                        Invoke(new MethodInvoker(() => { AddLine(line); }));
                }
                catch { }
                return;
            }
            gcodeViewMDI.AddLine(line);
        }

        public void SetGcodeParserStatus(string [] stat)
        {
            gStateView.GStates = stat;
        }

        //private void gcodeViewMDI_Enter(object sender, EventArgs e)
        //{
        //    textGcodeLine.Select();
        //}
    }
}
