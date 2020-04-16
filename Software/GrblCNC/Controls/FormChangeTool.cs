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
    public partial class FormChangeTool : Form
    {
        public FormChangeTool()
        {
            InitializeComponent();
        }

        public void SetToolNumber(int toolno)
        {
            labelTool.Text = "Inset Tool #" + toolno.ToString();
        }

        private void buttResume_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void buttPause_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
    }
}
