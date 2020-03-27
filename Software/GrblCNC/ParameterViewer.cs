using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrblCNC
{
    public partial class ParameterViewer : UserControl
    {
        public ParameterViewer()
        {
            InitializeComponent();
        }

        public void FillParameters(GrblConfig conf)
        {
            dataGridGrblConf.Rows.Clear();
            foreach (GrblConfig.GrblParam par in conf.GetParams())
            {
                dataGridGrblConf.Rows.Add(par.code, conf.GetDescription(par.code), par.strVal);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            buttConfLoad.Location = new Point(buttConfLoad.Location.X, Height - 28);
            buttConfSave.Location = new Point(buttConfSave.Location.X, Height - 28);
            buttConfProg.Location = new Point(Width - 92, Height - 28);
            dataGridGrblConf.Width = Width - 8;
            dataGridGrblConf.Height = Height - 38;
            base.OnSizeChanged(e);
        }

    }
}
