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

namespace GrblCNC.Controls
{
    public partial class FormProbe : Form
    {
        int axis;
        public FormProbe()
        {
            InitializeComponent();
        }

        public float Offset
        {
            get { return (float)numericOffset.Value; }
            set { numericOffset.Value = (decimal)value; }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible)
                numericOffset.Select();
            base.OnVisibleChanged(e);
        }

        public int Axis
        {
            get { return axis; }
            set
            {
                axis = value;
                labelAxis.Text = "Axis: " + Utils.GetAxisLetter(axis);
            }
        }

        public float Direction
        {
            get { return multiSelDir.SelectedItem == 0 ? -1 : 1; }
            set { multiSelDir.SelectedItem = value < 0 ? 0 : 1; }
        }

        private void buttOK_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }
}
