using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrblCNC.Controls
{
    public partial class FormOffset : Form
    {
        public FormOffset()
        {
            InitializeComponent();
            comboCoord.Items.AddRange(GrblUtils.GetCoordSystemRange());
            comboCoord.SelectedIndex = 0;
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

        

        public int CoordSystem
        {
            get 
            {
                if (comboCoord.Enabled)
                    return comboCoord.SelectedIndex;
                else
                    return -1;
            }
            set
            {
                if (value >= 0 && value < comboCoord.Items.Count)
                {
                    comboCoord.SelectedIndex = value;
                    comboCoord.Enabled = true;
                }
                else
                    comboCoord.Enabled = false;
            }
        }

        private void buttOK_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }
}
