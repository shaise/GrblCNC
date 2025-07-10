using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using GrblCNC.Glutils;

namespace GrblCNC.Controls
{
    public partial class FormAddMacro : Form
    {
        cMacroKey[] macros = new cMacroKey[10];
        cMacroKey[] macrosBkp = new cMacroKey[10];
        ToolStripButton[] buttons = new ToolStripButton[10];
        int currentKey = 0;
        ToolStrip toolstrip;
        public FormAddMacro()
        {
            InitializeComponent();
            
            for (int i = 0; i < macros.Length; i++)
            {
                macrosBkp[i] = new cMacroKey(i);
                macros[i] = new cMacroKey(i);
                buttons[i] = GenerateButton(i);
            }
            comboFuncKey.SelectedIndex = 0;
            LoadFromFile();
        }

        void LoadFromFile()
        {
            try
            {
                StreamReader sr = new StreamReader(Utils.SettingPath("MacroDefs.txt"));
                cMacroKey mac = null;
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().Trim();
                    if (line.StartsWith("#"))
                        continue;
                    if (line.StartsWith(":F"))
                    {
                        string [] vars = line.Split(new char[] {':'}, StringSplitOptions.RemoveEmptyEntries);
                        int id = int.Parse(vars[0][1].ToString()) - 1;
                        if (id >= 0 && id <= 8)
                        {
                            mac = macrosBkp[id];
                            mac.description = vars[1];
                            mac.macroText = "";
                        }
                        else
                            mac = null;
                    }
                    else
                    {
                        if ((mac != null) && (line != ""))
                        {
                            mac.macroText += line + "\r\n";
                        }
                    }
                }
                sr.Close();
                RestoreFunctions();
                UpdateGui(currentKey);
            }
            catch
            {
            }
        }
        void SaveToFile()
        {
            try
            {
                StreamWriter sw = new StreamWriter(Utils.SettingPath("MacroDefs.txt"));
                sw.WriteLine("# GrblCNC macro definitions");
                for (int i = 0; i < macros.Length; i++)
                {
                    if (macrosBkp[i].description != "")
                    {
                        sw.WriteLine(string.Format(":F{0}:{1}", i + 1, macrosBkp[i].description));
                        sw.WriteLine(macrosBkp[i].macroText);
                    }
                }
                sw.Close();
            } catch 
            {
                Global.errHandler.AddError("Unable to save macro definitions");
            } 
        }

        public void SetToolStrip(ToolStrip toolStrip)
        {
            toolstrip = toolStrip;
            UpdateToolStrip();
        }

        public void EnableFuncs(bool isenable)
        {
            for (int i = 0;i < buttons.Length;i++)
            {
                buttons[i].Enabled = isenable;
            }
        }

        void BackupFunctions()
        {
            for (int i = 0; i < macros.Length; i++)
            {
                macrosBkp[i].copyFrom(macros[i]);
            }
            SaveToFile();
        }

        void RestoreFunctions()
        {
            for (int i = 0; i < macros.Length; i++)
            {
                macros[i].copyFrom(macrosBkp[i]);
            }
            UpdateGui(currentKey);
        }

        void UpdateGui(int funcId)
        {
            textDescription.Text = macros[funcId].description;
            textGCode.Text = macros[funcId].macroText;
        }

        void ReadGui(int funcId)
        {
            macros[funcId].description = textDescription.Text;
            macros[funcId].macroText = textGCode.Text;
        }

        public void ActivateFunction(int funcId)
        {
            if (macrosBkp[funcId].description == "")
                return;
            string[] lines = macrosBkp[funcId].macroText.Split(new char [] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
                Global.grblComm.PostLine(line);
        }

        private void buttClear_Click(object sender, EventArgs e)
        {
            macros[currentKey].Clear();
            UpdateGui(currentKey);
        }

        private void comboFuncKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReadGui(currentKey);
            currentKey = comboFuncKey.SelectedIndex;
            UpdateGui(currentKey);
        }

        private void buttCancel_Click(object sender, EventArgs e)
        {
            RestoreFunctions();
            Hide();
        }

        void ClearToolStrip()
        {
            for (int i = 0; i < macros.Length; i++)
            {
                if (macrosBkp[i].description != "")
                    toolstrip.Items.Remove(buttons[i]);
            }
        }

        void UpdateToolStrip()
        {
            for (int i = 0; i < macros.Length; i++)
            {
                if (macrosBkp[i].description != "")
                {
                    buttons[i].ToolTipText = macrosBkp[i].description;
                    toolstrip.Items.Add(buttons[i]);
                }
            }
        }

        private void buttUpdate_Click(object sender, EventArgs e)
        {
            ClearToolStrip();
            ReadGui(currentKey);
            BackupFunctions();
            UpdateToolStrip();
            Hide();
        }

        ToolStripButton GenerateButton(int num)
        {
            ToolStripButton butt = new ToolStripButton();
            butt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            butt.Image = (Image)global::GrblCNC.Properties.Resources.ResourceManager.GetObject(string.Format("MacroF{0}Buttt", num + 1));
            butt.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            butt.ImageTransparentColor = System.Drawing.Color.Magenta;
            butt.Name = "toolStripMacroF" + (num+1).ToString();
            butt.Size = new System.Drawing.Size(44, 44);
            butt.Text = num.ToString();
            butt.ToolTipText = "";
            butt.Click += macro_click;
            return butt;
        }

        private void macro_click(object sender, EventArgs e)
        {
            ToolStripButton butt = (ToolStripButton)sender;
            ActivateFunction(int.Parse(butt.Text));

        }
    }

    public class cMacroKey
    {
        public cMacroKey(int id)
        {
            funcId = id;
            description = "";
            macroText = "";
        }

        public void copyFrom(cMacroKey other)
        {
            funcId = other.funcId;
            description = other.description;
            macroText = other.macroText;
        }

        public void Clear()
        {
            description = "";
            macroText = "";
        }

        public int funcId;
        public string description;
        public string macroText;
    }
}
