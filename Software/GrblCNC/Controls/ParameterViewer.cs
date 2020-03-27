using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GrblCNC
{
    public partial class ParameterViewer : UserControl
    {
        DataGridViewCellStyle stdGridStyle;
        DataGridViewCellStyle changedGridStyle;
        DataGridViewCellStyle errorGridStyle;
        bool fillParametersInProgress = false;
        GrblConfig grblConf;
        public ParameterViewer()
        {
            InitializeComponent();
            Global.GrblConnectionChanged += Global_GrblConnectionChanged;
            changedGridStyle = new DataGridViewCellStyle();
            changedGridStyle.BackColor = Color.Yellow;
            changedGridStyle.ForeColor = Color.Black;
            stdGridStyle = dataGridGrblConf.DefaultCellStyle;
            errorGridStyle = new DataGridViewCellStyle();
            errorGridStyle.BackColor = Color.LightPink;
        }

        void Global_GrblConnectionChanged(bool isConnected)
        {
            Enabled = isConnected;
        }

        public void FillParameters(GrblConfig conf)
        {
            fillParametersInProgress = true;
            dataGridGrblConf.Rows.Clear();
            foreach (GrblConfig.GrblParam par in conf.GetParams())
            {
                dataGridGrblConf.Rows.Add(par.code, conf.GetDescription(par.code), par.strVal);
            }
            grblConf = conf;
            fillParametersInProgress = false;
        }

        void UpdateColtrolsLocation()
        {
            buttConfLoad.Location = new Point(buttConfLoad.Location.X, Height - 28);
            buttConfSave.Location = new Point(buttConfSave.Location.X, Height - 28);
            buttConfProg.Location = new Point(Width - 92, Height - 28);
            dataGridGrblConf.Width = Width - 8;
            dataGridGrblConf.Height = Height - 38;
        }

        protected override void OnLoad(EventArgs e)
        {
            UpdateColtrolsLocation();
            Enabled = Global.GrblConnected;
            base.OnLoad(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            UpdateColtrolsLocation();
            base.OnSizeChanged(e);
        }

        private void dataGridGrblConf_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (grblConf == null || fillParametersInProgress || e.RowIndex < 0)
                return;
            DataGridViewRow row = dataGridGrblConf.Rows[e.RowIndex];
            string newval = row.Cells[e.ColumnIndex].Value.ToString();
            GrblConfig.GrblParam par = grblConf.GetParams()[e.RowIndex];
            string validPar = par.ValidateValue(newval);
            if (validPar == null)
            {
                row.DefaultCellStyle = errorGridStyle;
                return;
            }
            fillParametersInProgress = true; // eliminate reentry to this function
            row.Cells[e.ColumnIndex].Value = validPar;
            fillParametersInProgress = false;
            if (par.strVal == validPar)
                row.DefaultCellStyle = stdGridStyle;
            else
                row.DefaultCellStyle = changedGridStyle;
        }

        private void buttConfProg_Click(object sender, EventArgs e)
        {
            if (Global.grblComm == null)
                return;
            bool valChanged = false;
            foreach (DataGridViewRow row in dataGridGrblConf.Rows)
            {
                if (row.DefaultCellStyle == changedGridStyle)
                {
                    // a param was changed, send to grbl
                    int code = int.Parse(row.Cells[0].Value.ToString());
                    string val = row.Cells[2].Value.ToString();
                    Global.grblComm.SetGrblParameter(code, val);
                    valChanged = true;
                }
            }
            if (valChanged)
                Global.grblComm.GetAllGrblParameters(); // refresh view
        }

        private void buttConfSave_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamWriter sw = new StreamWriter(saveFileDialog.FileName);
                    foreach (DataGridViewRow row in dataGridGrblConf.Rows)
                    {
                        sw.WriteLine("${0}={1} ({2})", row.Cells[0].Value, row.Cells[2].Value, row.Cells[1].Value);
                    }
                    sw.Close();
                }
                catch
                {
                    MessageBox.Show("Unable to write to selected file.", "Write Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttConfLoad_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string[] lines = File.ReadAllLines(openFileDialog.FileName);
                    foreach (string line in lines)
                    {
                        string[] vars = line.Split(new char[] { '$', '=', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (vars.Length < 2)
                            continue;
                        // search data grid for the parameter. only load existing parameters
                        foreach (DataGridViewRow row in dataGridGrblConf.Rows)
                        {
                            if (row.Cells[0].Value.ToString() == vars[0])
                            {
                                row.Cells[2].Value = vars[1];
                                break;
                            }
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Unable to read selected file.", "Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
