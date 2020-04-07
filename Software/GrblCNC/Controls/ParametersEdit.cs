using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrblCNC.Controls
{
    public partial class ParametersEdit : UserControl
    {
        Dictionary<string, List<GrblConfig.ParamDescription>> categoryDict;

        public ParametersEdit()
        {
            InitializeComponent();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            horizTabs.Height = Height - 6;
            base.OnSizeChanged(e);
        }

        public void SetPatrameterTemplate(Dictionary<int, GrblConfig.ParamDescription> parDescs)
        {
            List<string> categories = new List<string>();
            categoryDict = new Dictionary<string, List<GrblConfig.ParamDescription>>();
            foreach (var keyval in parDescs)
            {
                GrblConfig.ParamDescription parDesc = keyval.Value;
                foreach (string category in parDesc.groups)
                {
                    if (!categoryDict.ContainsKey(category))
                    {
                        categoryDict[category] = new List<GrblConfig.ParamDescription>();
                        if (category.StartsWith("General"))
                            categories.Insert(0, category);
                        else
                            categories.Add(category);
                    }
                    categoryDict[category].Add(parDesc);
                }
            }
            horizTabs.TabTexts = categories.ToArray();
        }
    }
}
