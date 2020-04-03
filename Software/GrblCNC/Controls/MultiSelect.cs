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
    public partial class MultiSelect : UserControl
    {
        string selTextJoined;
        int depth = 2;
        Color highlightColor = Color.White, shadowColor = Color.Gray;
        int selectedItem = 0;
        Font selFont = null;
        int nsels = 0, cellw;
        public delegate void SelectionChangedDelegate(object obj, int newSelection);
        public event SelectionChangedDelegate SelectionChanged;
        public MultiSelect()
        {
            DoubleBuffered = true;
            InitializeComponent();
        }

        public string SelectionTexts
        {
            get { return selTextJoined; }
            set
            {
                selTextJoined = value;
                Invalidate();
            }
        }

        public int SelectedItem
        {
            get { return selectedItem; }
            set 
            { 
                selectedItem = value;
                Invalidate();
            }
        }

        void UpdateFont()
        {
            if (Height < 1)
                return;
            selFont = new Font(Font.FontFamily, (float)Height * 0.5f);
        }

        protected override void OnLoad(EventArgs e)
        {
           // UpdateFont();
            base.OnLoad(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen ps = new Pen(shadowColor);
            Pen ph = new Pen(highlightColor);
            int h = Height - 1;
            int w = Width - 1;
            for (int i = 0; i < depth; i++)
            {
                g.DrawLine(ps, i, i, w - i, i);
                g.DrawLine(ps, i, i, i, h - i);
                g.DrawLine(ph, i, h - i, w - i, h - i);
                g.DrawLine(ph, w - i, i, w - i, h - i);
            }
            if (selTextJoined == null)
                return;
            string[] selTexts = selTextJoined.Split('|');
            nsels = selTexts.Length;
            if (nsels == 0)
                return;
            //h = Height - 2 * depth;
            //w = Width - 2 * depth;
            cellw = (Width - 2 * depth) / nsels;
            Brush bf = Enabled ? new SolidBrush(ForeColor) : new SolidBrush(Color.FromArgb(64, ForeColor));
            Brush bb = new SolidBrush(BackColor);

            for (int i = 0; i < nsels; i++)
            {
                int sp = i * cellw + depth;
                if (i > 0)
                {
                    g.DrawLine(ph, sp - 1, depth, sp - 1, h - depth);
                    g.DrawLine(ps, sp + 1, depth, sp + 1, h - depth);
                }
                SizeF sz = g.MeasureString(selTexts[i], selFont);
                float offsx = (cellw - sz.Width) / 2;
                float offsy = (Height - sz.Height) / 2;
                if (i == selectedItem)
                {
                    g.FillRectangle(bf, sp + 1, depth + 1, cellw - 1, Height - 2 * depth - 2);
                    g.DrawString(selTexts[i], selFont, bb, sp + offsx, offsy);
                }
                else
                    g.DrawString(selTexts[i], selFont, bf, sp + offsx, offsy);

            }
            base.OnPaint(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            UpdateFont();
            base.OnSizeChanged(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (cellw == 0)
                return;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                int newsel = (e.X - depth) / cellw;
                if (newsel != selectedItem)
                {
                    selectedItem = newsel;
                    Invalidate();
                    if (SelectionChanged != null)
                        SelectionChanged(this, selectedItem);
                }
            }
            base.OnMouseClick(e);
        }
    }
}
