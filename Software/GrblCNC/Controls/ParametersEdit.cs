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
using GrblCNC.Glutils;

namespace GrblCNC.Controls
{
    public partial class ParametersEdit : UserControl
    {
 
        Dictionary<string, List<ParameterControl>> categoryDict;
        Dictionary<int, ParameterControl> parameterDict;
        List<GrblConfig.GrblParam> gParams;
        string lastCategory = "";

        public ParametersEdit()
        {
            InitializeComponent();
            MinimumSize = new Size(buttProgram.Width + buttRevert.Width + buttLoad.Width + buttSave.Width + 30, 200);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            int by = Height - buttSave.Height -3;
            buttSave.Location = new Point(buttSave.Location.X, by);
            buttLoad.Location = new Point(buttLoad.Location.X, by);
            buttRevert.Location = new Point(Width - buttRevert.Width - 3, by);
            buttProgram.Location = new Point(buttRevert.Location.X - buttProgram.Width - 6, by);
            horizTabs.Height = by - 6;
            panelParams.Height = horizTabs.Height;
            panelParams.Width = Width - panelParams.Location.X - 3;
            UpdateParametersWidth();
            base.OnSizeChanged(e);
        }

        void ShowCategory(string category)
        {
            if (lastCategory == category || !categoryDict.ContainsKey(category))
                return;
            panelParams.Visible = false;
            panelParams.Controls.Clear();
            int h = 0;
            foreach (ParameterControl par in categoryDict[category])
            {
                par.Location = new Point(0, h);
                h += par.Height + 4;
                panelParams.Controls.Add(par);
            }
            lastCategory = category;
            UpdateParametersWidth();
            panelParams.Visible = true;
            panelParams.Invalidate();
        }

        protected override void OnLoad(EventArgs e)
        {
            UpdateParametersWidth();
            base.OnLoad(e);
        }

        void UpdateParametersWidth()
        {
            foreach (Control con in panelParams.Controls)
            {
                ParameterControl pc = (ParameterControl)con;
                int w = panelParams.ClientSize.Width - 3;
                pc.Width = w > pc.minimumWidth ? w : pc.minimumWidth;
            }
        }

        public void SetPatrameterTemplate(Dictionary<int, GrblConfig.ParamDescription> parDescs)
        {
            List<string> categories = new List<string>();
            categoryDict = new Dictionary<string, List<ParameterControl>>();
            parameterDict = new Dictionary<int, ParameterControl>();
            foreach (var keyval in parDescs)
            {
                GrblConfig.ParamDescription parDesc = keyval.Value;
                ParameterControl parcont = new ParameterControl(parDesc);
                parameterDict[parDesc.code] = parcont;
                foreach (string category in parDesc.groups)
                {
                    if (!categoryDict.ContainsKey(category))
                    {
                        categoryDict[category] = new List<ParameterControl>();
                        if (category.StartsWith("General"))
                            categories.Insert(0, category);
                        else
                            categories.Add(category);
                    }
                    categoryDict[category].Add(parcont);
                }
            }
            horizTabs.TabTexts = categories.ToArray();
            if (categories.Count > 0)
                ShowCategory(categories[0]);
        }

        public void UpdateGuiParams()
        {
            if (gParams == null)
                return;
            foreach (GrblConfig.GrblParam par in gParams)
            {
                if (parameterDict.ContainsKey(par.code))
                {
                    parameterDict[par.code].UpdateFromParameter(par, true);
                }
            }
        }

        public void UpdateGuiParams(List<GrblConfig.GrblParam> gparams)
        {
            gParams = gparams;
            UpdateGuiParams();
        }

        private void horizTabs_SelectionChange(object sender, int selection, string selectionName)
        {
            ShowCategory(selectionName);
        }

        private void buttConfProg_Click(object sender, EventArgs e)
        {
         }

        private void buttSave_Click(object sender, EventArgs e)
        {
            if (gParams == null)
                return;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamWriter sw = new StreamWriter(saveFileDialog.FileName);
                    List<int> codes = parameterDict.Keys.ToList();
                    codes.Sort();
                    foreach (int code in codes)
                        sw.WriteLine(parameterDict[code].ToString());
                    sw.Close();
                }
                catch
                {
                    MessageBox.Show("Unable to write to selected file.", "Write Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttLoad_Click(object sender, EventArgs e)
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
                        if (vars[0][0] == '#')
                            continue;
                        try
                        {
                            int code = int.Parse(vars[0]);
                            ParameterControl pc = parameterDict[code];
                            pc.FromString(vars[1]);
                        }
                        catch { }
                    }
                }
                catch
                {
                    MessageBox.Show("Unable to read selected file.", "Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttProgram_Click(object sender, EventArgs e)
        {
            if (Global.grblComm == null)
                return;
            bool valChanged = false;
            foreach (var keyVal in parameterDict)
            {
                ParameterControl pc = keyVal.Value;
                if (pc.IsChanged)
                {
                    Global.grblComm.SetGrblParameter(pc.paramDesc.code, pc.GetParamString());
                    valChanged = true;
                }
            }
            if (valChanged)
                Global.grblComm.GetAllGrblParameters(); // refresh view
        }

        private void buttRevert_Click(object sender, EventArgs e)
        {
            UpdateGuiParams();
        }
    }
}
