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
    public partial class FormConfirmation : Form
    {
        public FormConfirmation()
        {
            InitializeComponent();
        }

        public void SetMessage(string Message)
        {
            labelMessage.Text = Message;
        }

        private void buttConfirm_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void buttCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
    }
}
