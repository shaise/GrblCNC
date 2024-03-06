using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GrblCNC.Glutils;
using static GrblCNC.Controls.ManualControl;

namespace GrblCNC.Controls
{
    public partial class FormGoto : Form
    {
        public delegate void GotoActionPressedDelegate(object sender, GoToAction action);
        public event GotoActionPressedDelegate GotoActionPressed;

        public FormGoto()
        {
            InitializeComponent();
            comboCoord.Items.Add("Current");
            comboCoord.Items.AddRange(GrblUtils.GetCoordSystemRange());
            comboCoord.SelectedIndex = 0;
            Global.NumAxesChanged += Global_NumAxesChanged;
        }

        private void Global_NumAxesChanged()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => { Global_NumAxesChanged(); }));
                return;
            }
            multiSelAxis.SelectionTexts = Global.GetAxesString();
            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            numA.Visible = Global.NumAxes > 3;
            numB.Visible = Global.NumAxes > 4;
            int selstate = multiSelAxis.SelectedValue;
            numX.Enabled = (selstate & 1) != 0;
            numY.Enabled = (selstate & 2) != 0;
            numZ.Enabled = (selstate & 4) != 0;
            numA.Enabled = (selstate & 8) != 0;
            numB.Enabled = (selstate & 16) != 0;
        }

        public int CoordSystem
        {
            get
            {
                return comboCoord.SelectedIndex - 1;
            }
            set
            {
                if (value >= -1 && value < (comboCoord.Items.Count - 1))
                    comboCoord.SelectedIndex = value + 1;
            }
        }

        public int Axis
        {
            get { return multiSelAxis.SelectedValue; }
            set { 
                multiSelAxis.SelectedValue = value;
                UpdateVisibility();
            }
        }


        private void buttGO_Click(object sender, EventArgs e)
        {
            if (GotoActionPressed == null)
                return;
            GoToAction action = new GoToAction();
            action.pos = new double[Global.MAX_AXES];
            action.pos[0] = (double)numX.Value;
            action.pos[1] = (double)numY.Value;
            action.pos[2] = (double)numZ.Value;
            action.pos[3] = (double)numA.Value;
            action.pos[4] = (double)numB.Value;
            action.coordSystem = CoordSystem;
            action.axisMask = multiSelAxis.SelectedValue;
            GotoActionPressed(this, action);
        }


        private void multiSelAxis_SelectionChanged(object obj, int newSelection)
        {
            UpdateVisibility();
        }

        private void buttClose_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void buttClear_Click(object sender, EventArgs e)
        {
            numX.Value = 0;
            numY.Value = 0;
            numZ.Value = 0;
            numA.Value = 0;
            numB.Value = 0;
            comboCoord.SelectedIndex = 0;
        }

        private void numX_ValueChanged(object sender, EventArgs e)
        {

        }
    }
    public class GoToAction
    {
        public double [] pos;
        public int coordSystem;
        public int axisMask;
    }
}
