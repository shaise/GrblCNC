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
    public partial class GStateViewer : UserControl
    {
        string[] gstates;
        Font selFont;
        int nCellsX = 6, nCellsY = 2;
        public GStateViewer()
        {
            DoubleBuffered = true;
            InitializeComponent();
            //gstates = new string[] { "G54", "T1" };
        }

        void UpdateFont()
        {
            if (Height < 1)
                return;
            selFont = new Font(Font.FontFamily, (float)Height * 0.25f);
        }

        public string[] GStates
        {
            get { return gstates; }
            set
            {
                gstates = value;
                Invalidate();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            UpdateFont();
            base.OnLoad(e);
        }

        protected override void OnResize(EventArgs e)
        {
            UpdateFont();
            Invalidate();
            base.OnResize(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (gstates == null || gstates.Length == 0)
                return;
            Graphics g = e.Graphics;
            int cellh = Height / nCellsY;
            Brush bf = new SolidBrush(ForeColor);
            Brush bb = new SolidBrush(Color.FromArgb(255,BackColor));
            int px = 0;
            int py = 0;
            for (int i = 0; i < gstates.Length; i++ )
            {
                SizeF sz = g.MeasureString(gstates[i], selFont);
                int w = (int)sz.Width + 6;
                if ((px + w) > Width)
                {
                    px = 0;
                    py += cellh;
                }
                g.FillRectangle(bf, px + 1, py + 1, w - 2, cellh - 2);
                g.DrawString(gstates[i], selFont, bb, px + (w - sz.Width) / 2, py + (cellh - sz.Height) / 2);
                px += w;
            }
            base.OnPaint(e);
        }
    }
}
