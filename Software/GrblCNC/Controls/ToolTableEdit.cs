using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GrblCNC.Glutils;

namespace GrblCNC.Controls
{
    public partial class ToolTableEdit : UserControl
    {
        ToolTable localTable;
        FormSelectToolNum frmSelTool;
        DataGridViewCellStyle stdGridStyle;
        DataGridViewCellStyle changedGridStyle;
        DataGridViewCellStyle errorGridStyle;
        bool haveChanges = false;
        bool haveErrors = false;
        Color changeBackColor;
        public ToolTableEdit()
        {
            InitializeComponent();
            MinimumSize = new Size(600, 200);
            Dock = System.Windows.Forms.DockStyle.Fill;
            Name = "ToolTableEdit";
            Size = new System.Drawing.Size(600, 300);
            TabIndex = 0;
            localTable = new ToolTable();
            frmSelTool = new FormSelectToolNum();
            changedGridStyle = new DataGridViewCellStyle();
            changedGridStyle.BackColor = Color.Yellow;
            changedGridStyle.ForeColor = Color.Black;
            stdGridStyle = dataGridTools.DefaultCellStyle;
            errorGridStyle = new DataGridViewCellStyle();
            errorGridStyle.BackColor = Color.LightPink;
            UpdateColors();
         }

        public void UpdateFromGlobal()
        {
            if (Global.toolTable == null)
                return;
            localTable.Clear();
            localTable.UpdateFrom(Global.toolTable);
            UpdateDisplayTable();
        }

        void UpdateDisplayTable()
        {
            dataGridTools.Rows.Clear();
            foreach (CncTool tool in localTable.Tools)
            {
                dataGridTools.Rows.Add(tool.ToString().Split('|'));
                UpdateLine(dataGridTools.Rows.Count - 1);
            }
        }


        #region Handle changes highlighting
        void UpdateLine(int rowno)
        {
            DataGridViewRow row = dataGridTools.Rows[rowno];
            int toolno = int.Parse(row.Cells[0].Value.ToString());
            CncTool tool = localTable.GetTool(toolno);
            string[] vars = new string[dataGridTools.Columns.Count];
            for (int i = 0; i < vars.Length; i++)
            {
                vars[i] = row.Cells[i].Value.ToString();
                row.Cells[i].Style = null;
            }
            List<int> badcells = tool.Parse(vars, false);
            CncTool gtool = Global.toolTable.GetTool(toolno);
            bool lastChanges = haveChanges;
            bool lastErrors = haveErrors;
            if (gtool == null)
            {
                haveChanges = true;
                row.DefaultCellStyle = changedGridStyle;
            }
            else
            {
                row.DefaultCellStyle = null;
                List<int> changedcells = tool.CompareWith(gtool);
                haveChanges = changedcells.Count > 0;
                foreach (int c in changedcells)
                    row.Cells[c].Style = changedGridStyle;
            }
            foreach (int c in badcells)
                row.Cells[c].Style = errorGridStyle;
            haveErrors = badcells.Count > 0;
            if ((!haveErrors && lastErrors) || (!haveChanges && lastChanges))
                UpdateChangeState();
            UpdateGuiByChanges();
        }

        void UpdateChangeState()
        {
            haveErrors = false;
            haveChanges = Global.toolTable.Tools.Count != localTable.Tools.Count;
            foreach (DataGridViewRow row in dataGridTools.Rows)
            {
                if (row.DefaultCellStyle == changedGridStyle)
                    haveChanges = true;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Style == changedGridStyle)
                        haveChanges = true;
                    else if (cell.Style == errorGridStyle)
                        haveErrors = true;
                }
                if (haveErrors && haveChanges)
                    break;
            }
        }

        void ClearChanges()
        {
            foreach (DataGridViewRow row in dataGridTools.Rows)
            {
                row.DefaultCellStyle = null;
                foreach (DataGridViewCell cell in row.Cells)
                    cell.Style = null;
            }
        }

        void UpdateColors()
        {
            Color c1 = Utils.TuneColor(BackColor, 1.1f);
            Color c2 = Utils.TuneColor(BackColor, 0.9f);
            changeBackColor = Color.FromArgb(c1.R, c1.G, c2.B);
        }

        void UpdateGuiByChanges()
        {
            bool ishighlight = haveChanges && !haveErrors;
            buttUpdate.BackColor = ishighlight ? changeBackColor : default(Color);
            buttUpdate.UseVisualStyleBackColor = !ishighlight;
            buttUpdate.Enabled = ishighlight;
            buttExport.Enabled = !haveErrors;
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            UpdateColors();
            base.OnBackColorChanged(e);
        }
        #endregion

        protected override void OnResize(EventArgs e)
        {
            int bh = Height - buttUpdate.Height - 3;
            buttUpdate.Location = new Point(3, bh);
            buttExport.Location = new Point(buttUpdate.Location.X + buttUpdate.Width + 6, bh);
            buttImport.Location = new Point(buttExport.Location.X + buttExport.Width + 6, bh);
            buttRemove.Location = new Point(Width - buttRemove.Width - 3, bh);
            buttAdd.Location = new Point(buttRemove.Location.X - buttAdd.Width - 6, bh);
            dataGridTools.Width = Width - 6;
            dataGridTools.Height = bh - 6;
            base.OnResize(e);
        }


        string [] GetRowValues(int rowix)
        {
            if (rowix < 0 || rowix >= dataGridTools.Rows.Count)
                return null;
            string[] res = new string[dataGridTools.Columns.Count];
            for (int i = 0; i < res.Length; i++)
                res[i] = dataGridTools.Rows[rowix].Cells[i].Value.ToString();
            return res;
        }

        private void buttAdd_Click(object sender, EventArgs e)
        {
            int i;
            for (i = 1; i <= 100; i++)
                if (localTable.GetTool(i) == null)
                    break;
            if (i <= 100)
                frmSelTool.Value = i;

            if (frmSelTool.ShowDialog() == DialogResult.OK)
            {
                int toolno = frmSelTool.Value;
                if (localTable.GetTool(toolno) != null)
                {
                    Utils.ErrorBox("Tool already exists");
                    return;
                }

                CncTool tool = new CncTool(toolno);
                localTable.AddUpdateTool(tool);
                UpdateDisplayTable();
            }
        }

        private void buttRemove_Click(object sender, EventArgs e)
        {
            Dictionary<DataGridViewRow, int> sellines = new Dictionary<DataGridViewRow,int>();
            foreach ( DataGridViewCell cell in dataGridTools.SelectedCells)
                sellines[cell.OwningRow] = 0;
            foreach (DataGridViewRow row in sellines.Keys)
            {
                localTable.RemoveTool(int.Parse(row.Cells[0].Value.ToString()));
                dataGridTools.Rows.Remove(row);
            }
            if (sellines.Keys.Count > 0)
            {
                haveChanges = true;
                UpdateGuiByChanges();
            }
        }

        private void dataGridTools_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            UpdateLine(e.RowIndex);
        }

        private void buttUpdate_Click(object sender, EventArgs e)
        {
            if (haveErrors)
                return;
            Global.toolTable.UpdateFrom(localTable);
            Global.toolTable.Save(Global.ToolTableFile);
            UpdateDisplayTable();
        }

        private void buttExport_Click(object sender, EventArgs e)
        {
            if (haveErrors)
                return;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string res = localTable.Save(saveFileDialog.FileName);
                if (res != "OK")
                    Utils.ErrorBox(res, "Unable to save file");
            }
        }

        private void buttImport_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string res = localTable.Load(openFileDialog.FileName);
                if (res != "OK")
                    Utils.ErrorBox(res, "Unable to open tool file");
                UpdateDisplayTable();
            }
        }
    }
}
