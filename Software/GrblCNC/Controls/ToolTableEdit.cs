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
        int ttt;
        string tt;
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
            ttt = 0;
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
                dataGridTools.Rows.Add(tool.ToString().Split('|'));
        }


        protected override void OnResize(EventArgs e)
        {
            int bh = Height - buttSave.Height - 3;
            buttSave.Location = new Point(3, bh);
            buttExport.Location = new Point(buttSave.Location.X + buttSave.Width + 6, bh);
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
        }

        private void dataGridTools_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            ttt++;
        }
    }
}
