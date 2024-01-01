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
        int tool;
        public bool IsProbe = false;
        public delegate void AxisActionPressedDelegate(object sender, int axis, AxisAction action);
        public event AxisActionPressedDelegate AxisActionPressed;

        public FormGoto()
        {
            InitializeComponent();
            Global.NumAxesChanged += Global_NumAxesChanged;
        }

        private void Global_NumAxesChanged()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => { Global_NumAxesChanged(); }));
                return;
            }
        }


        public int CoordSystem
        {
            get
            {
                return comboCoord.SelectedIndex;
            }
            set
            {
                if (value >= 0 && value < comboCoord.Items.Count)
                    comboCoord.SelectedIndex = value;
            }
        }

        private void buttGO_Click(object sender, EventArgs e)
        {
            IsProbe = false;
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void labelCoord_Click(object sender, EventArgs e)
        {

        }
    }
    public class GoToInfo
    {
        public float pos;
        public int coordSystem;
    }
}
