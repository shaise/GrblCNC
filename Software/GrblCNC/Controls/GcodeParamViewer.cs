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
    public partial class GcodeParamViewer : UserControl
    {
        DataGridViewCellStyle stdGridStyle;
        DataGridViewCellStyle changedGridStyle;
        DataGridViewCellStyle errorGridStyle;
        bool fillParametersInProgress = false;
        GCodeConfig gcodeConf;
        static string[] G10Codes = new string[] { "G54", "G55", "G56", "G57", "G58", "G59", "G59.1", "G59.2", "G59.3" };
        public GcodeParamViewer()
        {
            InitializeComponent();
            Global.GrblConnectionChanged += Global_GrblConnectionChanged;
            changedGridStyle = new DataGridViewCellStyle();
            changedGridStyle.BackColor = Color.Yellow;
            changedGridStyle.ForeColor = Color.Black;
            stdGridStyle = dataGridGCodeConf.DefaultCellStyle;
            errorGridStyle = new DataGridViewCellStyle();
            errorGridStyle.BackColor = Color.LightPink;
        }

        void Global_GrblConnectionChanged(bool isConnected)
        {
            Enabled = isConnected;
        }

        public void FillParameters(GCodeConfig conf)
        {
            fillParametersInProgress = true;
            dataGridGCodeConf.Rows.Clear();
            foreach (GCodeConfig.GCodeParam par in conf.GetParams())
            {
                if (Global.NumAxes == 5)
                    dataGridGCodeConf.Rows.Add(par.code, par.strVal[0], par.strVal[1], par.strVal[2], par.strVal[3], par.strVal[4]);
                else if (Global.NumAxes == 4)
                    dataGridGCodeConf.Rows.Add(par.code, par.strVal[0], par.strVal[1], par.strVal[2], par.strVal[3]);
                else
                    dataGridGCodeConf.Rows.Add(par.code, par.strVal[0], par.strVal[1], par.strVal[2]);
            }
            gcodeConf = conf;
            fillParametersInProgress = false;
        }

        void UpdateColtrolsLocation()
        {
            buttConfLoad.Location = new Point(buttConfLoad.Location.X, Height - 28);
            buttConfSave.Location = new Point(buttConfSave.Location.X, Height - 28);
            buttConfProg.Location = new Point(Width - 92, Height - 28);
            dataGridGCodeConf.Width = Width - 8;
            dataGridGCodeConf.Height = Height - 38;
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

        private void dataGridGCodeConf_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (gcodeConf == null || fillParametersInProgress || e.RowIndex < 0)
                return;
            DataGridViewRow row = dataGridGCodeConf.Rows[e.RowIndex];
            string newval = row.Cells[e.ColumnIndex].Value.ToString();
            GCodeConfig.GCodeParam par = gcodeConf.GetParams()[e.RowIndex];
            string validPar = par.ValidateValue(newval);
            if (validPar == null)
            {
                row.Cells[e.ColumnIndex].Style = errorGridStyle;
                return;
            }
            fillParametersInProgress = true; // eliminate reentry to this function
            row.Cells[e.ColumnIndex].Value = validPar;
            fillParametersInProgress = false;
            int axis = e.ColumnIndex - 1;
            if (par.strVal[axis] == validPar)
                row.Cells[e.ColumnIndex].Style = stdGridStyle;
            else
                row.Cells[e.ColumnIndex].Style = changedGridStyle;
        }

        int GetPcode(string gcode)
        {
            for (int i = 0; i < G10Codes.Length; i++)
                if (G10Codes[i] == gcode)
                    return i + 1;
            return -1;
        }

        private void buttConfProg_Click(object sender, EventArgs e)
        {
            if (Global.grblComm == null)
                return;
            bool valChanged = false;
            foreach (DataGridViewRow row in dataGridGCodeConf.Rows)
            {
                StringBuilder sb = new StringBuilder();
                int pcode = GetPcode(row.Cells[0].Value.ToString());
                if (pcode > 0)
                {
                    for (int i = 1; i < row.Cells.Count; i++)
                    {
                        if (row.Cells[i].Style == changedGridStyle)
                        {
                            sb.Append(" ");
                            sb.Append(dataGridGCodeConf.Columns[i].HeaderText);
                            sb.Append(row.Cells[i].Value);
                        }
                    }
                    if (sb.Length > 0)
                    {
                        string cmd = "G10 L2 P" + pcode.ToString() + sb.ToString();
                        Global.grblComm.PostLine(cmd);
                        valChanged = true;
                    }
                }
            }
            if (valChanged)
                Global.grblComm.GetGcodeCoordOfsets(); // refresh view
        }

        private void buttConfSave_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamWriter sw = new StreamWriter(saveFileDialog.FileName);
                    foreach (DataGridViewRow row in dataGridGCodeConf.Rows)
                    {
                        sw.Write("[{0}:", row.Cells[0].Value);
                        for (int i = 1; i < row.Cells.Count; i++)
                        {
                            if (i == row.Cells.Count - 1)
                                sw.WriteLine("{0}]", row.Cells[i].Value);
                            else
                                sw.Write("{0},", row.Cells[i].Value);
                        }
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
                        string[] vars = line.Split(new char[] { '[', ':', ']' }, StringSplitOptions.RemoveEmptyEntries);
                        if (vars.Length < 2)
                            continue;
                        string code = vars[0];
                        vars = vars[1].Split(new char[] { ',', ' '}, StringSplitOptions.RemoveEmptyEntries);
                        if (vars.Length < 3)
                            continue;
                        // search data grid for the parameter. only load existing parameters
                        foreach (DataGridViewRow row in dataGridGCodeConf.Rows)
                        {
                            if (row.Cells[0].Value.ToString() == code)
                            {
                                for (int i = 0; i < vars.Length && i < (row.Cells.Count - 1); i++)
                                    row.Cells[i + 1].Value = vars[i];
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
