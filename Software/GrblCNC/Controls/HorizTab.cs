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
    public partial class HorizTab : UserControl
    {
        string title;
        string[] tabTexts;
        int selectedTab = 0;
        int hoverTab = 1;
        Font titleFont;
        Brush bback;
        Brush bfore;
        Brush bbackDark;
        Brush bbackMed;
        Pen pHiglight, pShadow;
        Point[] triPoints;

        public delegate void SelectionChangeDelegate(object sender, int selection);
        public event SelectionChangeDelegate SelectionChange;

        public HorizTab()
        {
            InitializeComponent();
            DoubleBuffered = true;
            UpdateTitleFont();
            UpdateBrushes();
            triPoints = new Point[] { new Point(), new Point(), new Point() };

        }

        void UpdateTitleFont()
        {
            titleFont = new Font(Font.FontFamily, Font.Size * 1.5f);
        }

        void UpdateBrushes()
        {
            bback = new SolidBrush(BackColor);
            bfore = new SolidBrush(ForeColor);
            bbackDark = new SolidBrush(Utils.TuneColor(BackColor, 0.90f));
            bbackMed = new SolidBrush(Utils.TuneColor(BackColor, 0.95f));
            pHiglight = new Pen(Utils.TuneColor(BackColor, 1.2f));
            pShadow = new Pen(Utils.TuneColor(BackColor, 0.7f));
        }

        public string Title
        {
            get { return title; }
            set 
            { 
                title = value;
                Invalidate();
            }
        }

        public string [] TabTexts
        {
            get { return tabTexts; }
            set
            {
                tabTexts = value;
                if (selectedTab > tabTexts.Length)
                    selectedTab = 0;
                Invalidate();
            }
        }

        public int SelectedTab
        {
            get { return selectedTab; }
            set
            {
                if (tabTexts != null && value >= 0 && value < tabTexts.Length && value != selectedTab)
                {
                    selectedTab = value;
                    Invalidate();
                }
            }
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            UpdateBrushes();
            base.OnBackColorChanged(e);
        }

        void AdjustTriangle(int px, int py, int w, int h)
        {
            triPoints[0].X = px;
            triPoints[0].Y = py;
            triPoints[1].X = px;
            triPoints[1].Y = py + h;
            triPoints[2].X = px + w;
            triPoints[2].Y = py + h / 2;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.FillRectangle(bbackDark, 0, 0, Width, Height);
            g.DrawLine(pShadow, 0, 0, 0, Height - 1);
            g.DrawLine(pShadow, 0, 0, Width - 1, 0);
            g.DrawLine(pHiglight, Width - 1, 0, Width - 1, Height - 1);
            g.DrawLine(pHiglight, 0, Height - 1, Width - 1, Height - 1);

            if (title != null)
                g.DrawString(title, titleFont, bfore, 0, 0);

            if (tabTexts == null)
                return;
            int th = Font.Height + 4;
            int px = 10;
            int py = titleFont.Height + 10;
            int w = Width - px;
            for (int i = 0; i < tabTexts.Length; i++ )
            {
                if (i == selectedTab)
                {
                    g.FillRectangle(bback, px, py, w, th);
                    g.DrawLine(pHiglight, px, py, Width, py);
                    g.DrawLine(pHiglight, px, py, px, py + th);
                    g.DrawLine(pShadow, px, py + th, Width, py + th);
                }
                if (i == hoverTab)
                {
                    if (i != selectedTab)
                        g.FillRectangle(bbackMed, px+1, py+1, w-2, th-2);
                    AdjustTriangle(2, py + 2, px - 4, th - 4);
                    g.FillPolygon(bfore, triPoints);
                }
                //SizeF sz = g.MeasureString(tabTexts[i], Font);
                //float tpx = Width - sz.Width - 5;
                //if (tpx < px + 5)
                //    tpx = px + 5;
                g.DrawString(tabTexts[i], Font, bfore, px + 6, py + 2);
                py += th;
            }

            base.OnPaint(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            int th = Font.Height + 4;
            int py = titleFont.Height + 10;
            int selpos = (e.Y - py) / th;
            if (tabTexts == null || selpos < 0 || selpos >= tabTexts.Length)
                selpos = -1;
            if (selpos != hoverTab)
            {
                hoverTab = selpos;
                Invalidate();
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (hoverTab >= 0 && hoverTab != selectedTab)
            {
                selectedTab = hoverTab;
                Invalidate();
                if (SelectionChange != null)
                    SelectionChange(this, selectedTab);
            }
            base.OnMouseClick(e);
        }

    }
}
