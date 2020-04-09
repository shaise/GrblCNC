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
    public partial class ParametersEdit : UserControl
    {
 
        Dictionary<string, List<ParameterControl>> categoryDict;
        Dictionary<int, ParameterControl> parameterDict;
        string lastCategory = "";

        public ParametersEdit()
        {
            InitializeComponent();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            horizTabs.Height = Height - 6;
            panelParams.Height = horizTabs.Height;
            panelParams.Width = Width - panelParams.Location.X - 3;
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
            panelParams.Visible = true;
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

        public void UpdateGuiParams(List<GrblConfig.GrblParam> gparams)
        {
            foreach (GrblConfig.GrblParam par in gparams)
            {
                if (parameterDict.ContainsKey(par.code))
                {
                    parameterDict[par.code].UpdateFromParameter(par, true);
                }
            }
        }

        private void horizTabs_SelectionChange(object sender, int selection, string selectionName)
        {
            ShowCategory(selectionName);
        }
    }
}
