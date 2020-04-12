using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrblCNC
{
    public partial class FormPopWindow : Form
    {
        public FormPopWindow()
        {
            InitializeComponent();
        }

        public DialogResult ShowControl(UserControl control, string title)
        {
            Text = title;
            Controls.Clear();
            ClientSize = new System.Drawing.Size(control.Width, control.Height);
            Controls.Add(control);
            return ShowDialog();
        }
    }
}
