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
    public partial class FormConfigGrbl : Form
    {
        public FormConfigGrbl()
        {
            InitializeComponent();
            Global.grblParameterEditor = paramEdit;
        }
    }
}
