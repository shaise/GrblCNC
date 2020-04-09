using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using GrblCNC.Glutils;

namespace GrblCNC.Controls
{
    public class ParameterControl : UserControl
    {
        //public List<GrblConfig.ParamDescription> paramList;
        public List<Control> activeControls;
        public GrblConfig.ParamDescription paramDesc;
        public float floatValue;
        public int intValue;
        public bool isSimpleMask;

        public ParameterControl(GrblConfig.ParamDescription pardesc)
        {
            paramDesc = pardesc;
            activeControls = new List<Control>();
            switch (pardesc.type)
            {
                case GrblConfig.ParamType.Bool:
                    {
                        CheckBox cb = new CheckBox();
                        this.Controls.Add(cb);
                        cb.AutoSize = true;
                        cb.Text = pardesc.description;
                        cb.Name = "Bool_" + pardesc.code.ToString();
                        activeControls.Add(cb);
                        cb.Location = new Point(0, 0);
                        this.Width = cb.Width;
                        this.Height = cb.Height;
                    }
                    break;

                case GrblConfig.ParamType.Float:
                case GrblConfig.ParamType.Int:
                    {
                        Label l = new Label();
                        l.AutoSize = true;
                        l.Text = pardesc.description + ":";
                        l.Name = "Label_" + pardesc.code.ToString();
                        this.Controls.Add(l);
                        NumericUpDown nud = new NumericUpDown();
                        nud.Name = "Numeric_" + pardesc.code.ToString();
                        if (pardesc.type == GrblConfig.ParamType.Float)
                            UpdateNumeric(nud, 3, -999999, 999999, pardesc.options);
                        else
                            UpdateNumeric(nud, 0, 0, 999999, pardesc.options);
                        l.Location = new Point(0, (nud.Height - l.Height) / 2);
                        nud.Location = new Point(l.Width, 0);
                        activeControls.Add(nud);
                        this.Width = nud.Location.X + nud.Width;
                        this.Height = nud.Height;
                        this.Controls.Add(nud);
                    }
                    break;

                case GrblConfig.ParamType.Mask:
                    {
                        Label l = new Label();
                        l.AutoSize = true;
                        l.Text = pardesc.description + ":";
                        l.Name = "Label_" + pardesc.code.ToString();
                        this.Controls.Add(l);
                        isSimpleMask = IsSimpleMask(paramDesc.options);
                        if (isSimpleMask)
                        {
                            MultiSelect ms = new MultiSelect();
                            ms.SetSelectionTexts(paramDesc.options);
                            ms.Height = 20;
                            ms.Width = 22 * paramDesc.options.Length;
                            ms.MultiSelectionMode = true;
                            l.Location = new Point(0, (ms.Height - l.Height) / 2);
                            ms.Location = new Point(l.Width, 0);
                            activeControls.Add(ms);
                            this.Controls.Add(ms);
                            this.Width = ms.Location.X + ms.Width;
                            this.Height = ms.Height;
                        }
                        else
                        {
                            l.Location = new Point(0, 0);
                            int maxwidth = l.Width;
                            int h = l.Height + 4;
                            if (pardesc.options != null)
                            {
                                for (int i = 0; i < pardesc.options.Length; i++)
                                {
                                    CheckBox cb = new CheckBox();
                                    cb.AutoSize = true;
                                    cb.Name = "Mask_" + pardesc.code.ToString() + "_" + i.ToString();
                                    if (pardesc.options[i].Length > 0)
                                    {
                                        this.Controls.Add(cb);
                                        cb.Text = pardesc.options[i];
                                        int ident = 10;
                                        cb.Location = new Point(ident, h);
                                        h += cb.Height;
                                        if (ident + cb.Width > maxwidth)
                                            maxwidth = cb.Width + ident;
                                    }
                                    activeControls.Add(cb);
                                }
                            }
                            this.Width = maxwidth;
                            this.Height = h;
                        }
                    }
                    break;

                case GrblConfig.ParamType.Selection:
                    {
                        Label l = new Label();
                        l.AutoSize = true;
                        l.Text = pardesc.description + ":";
                        l.Name = "Label_" + pardesc.code.ToString();
                        ComboBox cb = new ComboBox();
                        cb.DropDownStyle = ComboBoxStyle.DropDownList;
                        cb.Name = "Selection_" + pardesc.code.ToString();
                        l.Location = new Point(0, (cb.Height - l.Height) / 2);
                        cb.Location = new Point(l.Width, 0);
                        if (pardesc.options != null)
                        {
                            for (int i = 0; i < pardesc.options.Length; i++)
                                cb.Items.Add(paramDesc.options[i]);
                        }
                        activeControls.Add(cb);
                        this.Width = cb.Location.X + cb.Width;
                        this.Height = cb.Height;
                        this.Controls.Add(l);
                        this.Controls.Add(cb);
                    }
                    break;
            }
        }

        bool IsSimpleMask(string[] maskStrs)
        {
            if (maskStrs == null || maskStrs.Length == 0)
                return false;
            foreach (string str in maskStrs)
                if (str.Length != 1)
                    return false;
            return true;
        }

        void UpdateNumeric(NumericUpDown nud, int defaultDecimals, int defaultMin, int defaultMax, string[] options)
        {
            nud.DecimalPlaces = defaultDecimals;
            nud.Minimum = defaultMin;
            nud.Maximum = defaultMax;
            if (options != null)
            {
                try
                {
                    if (options.Length > 0)
                        nud.Minimum = (decimal)Utils.ParseFloatInvariant(options[0]);
                    if (options.Length > 1)
                        nud.Maximum = (decimal)Utils.ParseFloatInvariant(options[1]);
                    if (options.Length > 1)
                        nud.DecimalPlaces = int.Parse(options[2]);
                }
                catch { }
            }
        }

        public void UpdateFromParameter(GrblConfig.GrblParam par)
        {
            switch (paramDesc.type)
            {
                case GrblConfig.ParamType.Bool:
                    {
                        CheckBox cb = (CheckBox)activeControls[0];
                        cb.Checked = par.intVal != 0;
                        intValue = par.intVal;
                    }
                    break;

                case GrblConfig.ParamType.Float:
                    {
                        NumericUpDown nud = (NumericUpDown)activeControls[0];
                        nud.Value = (decimal)par.floatVal;
                        floatValue = par.floatVal;
                    }
                    break;

                case GrblConfig.ParamType.Int:
                    {
                        NumericUpDown nud = (NumericUpDown)activeControls[0];
                        nud.Value = (decimal)par.intVal;
                        floatValue = par.intVal;
                    }
                    break;

                case GrblConfig.ParamType.Mask:
                    {
                        if (isSimpleMask)
                        {
                            MultiSelect ms = (MultiSelect)activeControls[0];
                            ms.SelectedValue = par.intVal;
                        }
                        else
                        {
                            for (int i = 0; i < activeControls.Count; i++)
                            {
                                CheckBox cb = (CheckBox)activeControls[i];
                                cb.Checked = (par.intVal & (1 << i)) != 0;
                            }
                        }
                        intValue = par.intVal;
                    }
                    break;

                case GrblConfig.ParamType.Selection:
                    {
                        ComboBox cb = (ComboBox)activeControls[0];
                        cb.SelectedIndex = par.intVal;
                        intValue = par.intVal;
                    }
                    break;
            }
        }

    }
}
