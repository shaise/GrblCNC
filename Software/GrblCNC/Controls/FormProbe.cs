using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GrblCNC.Glutils;

namespace GrblCNC.Controls
{
    public partial class FormProbe : Form
    {
        int tool;
        public bool IsProbe = false;
        public FormProbe()
        {
            InitializeComponent();
            comboTool.Items.Add("No Tool");
            Global.NumAxesChanged += Global_NumAxesChanged;
        }

        private void Global_NumAxesChanged()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => { Global_NumAxesChanged(); }));
                return;
            }
            UpdateNumAxes();
        }

        public void UpdateNumAxes()
        {
            if (Global.numAxes > 4)
                multiSelAxis.SelectionTexts = "X|Y|Z|A|B";
            else if (Global.numAxes > 3)
                multiSelAxis.SelectionTexts = "X|Y|Z|A";
            else
                multiSelAxis.SelectionTexts = "X|Y|Z";
        }

        public float Offset
        {
            get { return (float)numericOffset.Value; }
            set { numericOffset.Value = (decimal)value; }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible)
                numericOffset.Select();
            base.OnVisibleChanged(e);
        }

        public int Axis
        {
            get { return multiSelAxis.SelectedValue; }
            set { multiSelAxis.SelectedValue = value; }
        }

        int FindTool(int toolnum)
        {
            for (int i = 1; i < comboTool.Items.Count; i++)
            {
                string[] vars = comboTool.Items[i].ToString().Split(':');
                if (toolnum == int.Parse(vars[0]))
                    return i;
            }
            return 0;
        }
        public int Tool
        {
            get { return tool; }
            set 
            {
                int toolix = FindTool(value);
                comboTool.SelectedIndex = toolix;
                labelCoord.Text = "Tool:";
                comboCoord.Visible = false;
                comboTool.Visible = true;
                UpdateToolFromSelection();
            }
        }

        public int CoordSystem
        {
            get
            {
                if (comboCoord.Enabled)
                    return comboCoord.SelectedIndex;
                else
                    return -1;
            }
            set
            {
                if (value >= 0 && value < comboCoord.Items.Count)
                {
                    comboCoord.SelectedIndex = value;
                    comboCoord.Enabled = true;
                }
                else
                    comboCoord.Enabled = false;
                labelCoord.Text = "Coordinates:";
                comboTool.Visible = false;
                comboCoord.Visible = true;
            }
        }

        void UpdateToolFromSelection()
        {
            if (comboTool.SelectedIndex == 0)
                tool = 0;
            else
            {
                string[] vars = comboTool.SelectedItem.ToString().Split(':');
                tool = int.Parse(vars[0]);
            }
        }



        public void UpdateTools()
        {
            comboTool.Items.Clear();
            comboTool.Items.Add("No Tool");
            foreach (CncTool tool in Global.toolTable.Tools)
            {
                if (tool.description.Length > 0)
                    comboTool.Items.Add(string.Format("{0}: {1}", tool.toolNum, tool.description));
                else
                    comboTool.Items.Add(tool.toolNum.ToString());
            }
        }

        public float Direction
        {
            get { return multiSelDir.SelectedValue == 0 ? -1 : 1; }
            set { multiSelDir.SelectedValue = value < 0 ? 0 : 1; }
        }

        private void buttOK_Click(object sender, EventArgs e)
        {
            IsProbe = false;
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void comboTool_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateToolFromSelection();
        }

        private void buttProbe_Click(object sender, EventArgs e)
        {
            IsProbe = true;
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }
}
