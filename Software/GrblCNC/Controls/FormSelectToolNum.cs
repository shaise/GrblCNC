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
    public partial class FormSelectToolNum : Form
    {

        public FormSelectToolNum()
        {
            InitializeComponent();
        }

        private void buttOK_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void buttCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        protected override void OnShown(EventArgs e)
        {
            numericTool.Focus();
            numericTool.Select(0, numericTool.Text.Length);
            base.OnShown(e);
        }

        public int Value
        {
            get { return (int)numericTool.Value; }
            set { numericTool.Value = value; }
        }
    }
}
