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
        float floatValue, lastFloatValue;
        int intValue, lastIntValue;
        string strValue, lastStrValue; 
        public bool isSimpleMask;
        bool isChanged = false;
        Color stdBackColor, changeBackColor;
        bool updatingBgnd = false;
        bool updatingGui = false;
        public int minimumWidth;
        public delegate void ParameterChangedDelegate(object sender, bool isChanged);
        public event ParameterChangedDelegate ParameterChanged;

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
                        cb.CheckedChanged += ValueChanged;
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
                        l.Text = pardesc.description + " (" + pardesc.uints + "):";
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
                        nud.ValueChanged += ValueChanged;
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
                            ms.SelectionChanged += ms_SelectionChanged;
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
                                    cb.CheckedChanged += ValueChanged;
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
                        this.Controls.Add(l);
                        ComboBox cb = new ComboBox();
                        cb.DropDownStyle = ComboBoxStyle.DropDownList;
                        cb.Name = "Selection_" + pardesc.code.ToString();
                        cb.SelectedIndexChanged += ValueChanged;
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
                        this.Controls.Add(cb);
                    }
                    break;

                case GrblConfig.ParamType.String:
                    {
                        Label l = new Label();
                        l.AutoSize = true;
                        l.Text = pardesc.description + ":";
                        l.Name = "Label_" + pardesc.code.ToString();
                        this.Controls.Add(l);
                        TextBox tb = new TextBox();
                        tb.Name = "String_" + pardesc.code.ToString();
                        tb.TextChanged += ValueChanged;
                        l.Location = new Point(0, (tb.Height - l.Height) / 2);
                        tb.Location = new Point(l.Width, 0);
                        activeControls.Add(tb);
                        this.Width = tb.Location.X + tb.Width;
                        this.Height = tb.Height;
                        this.Controls.Add(tb);
                    }
                    break;
            }
            minimumWidth = Width;
            UpdateColors();
        }

        public bool IsChanged
        {
            get { return isChanged; }
        }

        void ValueChanged(object sender, EventArgs e)
        {
            bool oldIsChanged = isChanged;
            if (!updatingGui)
                UpdateFromGui();
            if (isChanged != oldIsChanged && ParameterChanged != null)
                ParameterChanged(this, isChanged);
        }

        void ms_SelectionChanged(object obj, int newSelection)
        {
            ValueChanged(this, null);
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

        void UpdateColors()
        {
            if (Parent != null)
                stdBackColor = Parent.BackColor;
            else
                stdBackColor = BackColor;
            // change color is more yellow-ish
            Color c1 = Utils.TuneColor(BackColor, 1.1f);
            Color c2 = Utils.TuneColor(BackColor, 0.9f);
            changeBackColor = Color.FromArgb(c1.R, c1.G, c2.B);
            UpdateBackground();
        }

        protected override void OnLoad(EventArgs e)
        {
            UpdateColors();
            base.OnLoad(e);
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            if (updatingBgnd)
                return;
            UpdateColors();
            base.OnBackColorChanged(e);
        }

        void UpdateBackground()
        {
            updatingBgnd = true;
            BackColor = isChanged ? changeBackColor : stdBackColor;
            Invalidate();
            foreach (Control ctl in Controls)
                ctl.Invalidate();
            updatingBgnd = false;
        }

        void UpdateFromGui()
        {
            switch (paramDesc.type)
            {
                case GrblConfig.ParamType.Bool:
                    {
                        CheckBox cb = (CheckBox)activeControls[0];
                        intValue = cb.Checked ? 1 : 0;
                        isChanged = intValue != lastIntValue;
                    }
                    break;

                case GrblConfig.ParamType.Float:
                    {
                        NumericUpDown nud = (NumericUpDown)activeControls[0];
                        floatValue = (float)nud.Value;
                        isChanged = floatValue != lastFloatValue;
                    }
                    break;

                case GrblConfig.ParamType.Int:
                    {
                        NumericUpDown nud = (NumericUpDown)activeControls[0];
                        intValue = (int)nud.Value;
                        isChanged = intValue != lastIntValue;
                    }
                    break;

                case GrblConfig.ParamType.Mask:
                    {
                        if (isSimpleMask)
                        {
                            MultiSelect ms = (MultiSelect)activeControls[0];
                            intValue = ms.SelectedValue;
                        }
                        else
                        {
                            intValue = 0;
                            for (int i = 0; i < activeControls.Count; i++)
                            {
                                CheckBox cb = (CheckBox)activeControls[i];
                                if (cb.Checked)
                                    intValue |= 1 << i;
                            }
                        }
                        isChanged = intValue != lastIntValue;
                    }
                    break;

                case GrblConfig.ParamType.Selection:
                    {
                        ComboBox cb = (ComboBox)activeControls[0];
                        intValue = cb.SelectedIndex;
                        isChanged = intValue != lastIntValue;
                    }
                    break;

                case GrblConfig.ParamType.String:
                    {
                        TextBox tb = (TextBox)activeControls[0];
                        strValue = tb.Text;
                        isChanged = strValue != lastStrValue;
                    }
                    break;
            }
            UpdateBackground();
        }

        public void UpdateFromParameter(GrblConfig.GrblParam par, bool overrideChanges)
        {
            updatingGui = true;
            switch (paramDesc.type)
            {
                case GrblConfig.ParamType.Bool:
                    {
                        CheckBox cb = (CheckBox)activeControls[0];
                        cb.Checked = par.intVal != 0;
                        if (overrideChanges)
                            lastIntValue = par.intVal;
                    }
                    break;

                case GrblConfig.ParamType.Float:
                    {
                        NumericUpDown nud = (NumericUpDown)activeControls[0];
                        nud.Value = (decimal)par.floatVal;
                        if (overrideChanges)
                            lastFloatValue = par.floatVal;
                    }
                    break;

                case GrblConfig.ParamType.Int:
                    {
                        NumericUpDown nud = (NumericUpDown)activeControls[0];
                        nud.Value = (decimal)par.intVal;
                        if (overrideChanges)
                            lastIntValue = par.intVal;
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
                        if (overrideChanges)
                            lastIntValue = par.intVal;
                    }
                    break;

                case GrblConfig.ParamType.Selection:
                    {
                        ComboBox cb = (ComboBox)activeControls[0];
                        cb.SelectedIndex = par.intVal;
                        if (overrideChanges)
                            lastIntValue = par.intVal;
                    }
                    break;

                case GrblConfig.ParamType.String:
                    {
                        TextBox tb = (TextBox)activeControls[0];
                        tb.Text = par.strVal;
                        if (overrideChanges)
                            lastStrValue = par.strVal;
                    }
                    break;
            }
            updatingGui = false;
            if (overrideChanges)
            {
                isChanged = false; 
                UpdateBackground();
            }
            Visible = true;
            UpdateFromGui();
        }

        public string GetParamString()
        {
            string res = "";
            switch (paramDesc.type)
            {
                case GrblConfig.ParamType.Bool:
                case GrblConfig.ParamType.Int:
                case GrblConfig.ParamType.Mask:
                case GrblConfig.ParamType.Selection:
                    res = intValue.ToString();
                    break;

                case GrblConfig.ParamType.Float:
                    res = Utils.ToInvariantString(floatValue, "0.000");
                    break;

                case GrblConfig.ParamType.String:
                    res = strValue;
                    break;
            }
            return res;
        }


        public void FromString(string str)
        {
            GrblConfig.GrblParam par = new GrblConfig.GrblParam(0, str);
            // fixme: lots of duplications of code in parameters. need a good fix
            switch (paramDesc.type)
            {
                case GrblConfig.ParamType.Bool:
                case GrblConfig.ParamType.Int:
                case GrblConfig.ParamType.Mask:
                case GrblConfig.ParamType.Selection:
                    try { par.intVal = int.Parse(str); }
                    catch { }
                    break;

                case GrblConfig.ParamType.Float:
                    par.floatVal = Utils.ParseFloatInvariant(str);
                    break;
            }
            UpdateFromParameter(par, false);
        }



        public override string ToString()
        {
            return string.Format("${0}={1}     ({2}, {3})", paramDesc.code, GetParamString(), paramDesc.description, paramDesc.uints);
        }

    }
}
