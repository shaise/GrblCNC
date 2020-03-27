using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace GrblCNC
{
    public partial class GcodeViewer : UserControl
    {
        //string[] lines;
        List<string> lines;
        public int startLine = 0;
        public int highlightLine = 10;
        int numVisibleLines = 0;
        float charWidth = 0;
        Bitmap drawBmp = null;
        Brush backBrush, foreBrush;
        public delegate void SelectedLineChangedDelegate(object obj, int selLine);
        public event SelectedLineChangedDelegate SelectedLineChanged;
        int maxNumLines;

        public GcodeViewer()
        {
            InitializeComponent();
            AutoScroll = false;
            VScroll = true;
            HScroll = true;
            maxNumLines = 1000;
        }

        public void SetLines(string [] glines)
        {
            lines = glines == null ? null : glines.ToList();
            maxNumLines = glines == null ? 1000 : glines.Length; 
            UpdateScroll(); 
        }

        public void AddLine(string line)
        {
            if (lines == null)
                lines = new List<string>();
            lines.Add(line);
            while (lines.Count > maxNumLines)
                lines.RemoveAt(0);
            if (lines.Count > numVisibleLines)
                startLine = lines.Count - numVisibleLines;
            UpdateScroll();
            Invalidate();
        }


        void UpdateScroll()
        {
            if (lines == null || lines.Count == 0)
            {
                vScroll.Enabled = false;
                return;
            }
            vScroll.Minimum = 0;
            vScroll.Maximum = lines.Count > 1 ? lines.Count - 1 : 1;
            vScroll.Value = 0;
            vScroll.Enabled = true;
            Invalidate(); 
        }

        protected override void OnLoad(EventArgs e)
        {
            if (Site != null && Site.DesignMode)
                return;

            base.OnLoad(e);
            Font = FontManager.GcodeFont;
            BackColor = Color.Beige;
            ForeColor = Color.Sienna;
            backBrush = new SolidBrush(BackColor);
            foreBrush = new SolidBrush(ForeColor);
            InitDimensions();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // do nothing
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Site != null && Site.DesignMode)
            {
                e.Graphics.Clear(Color.Beige);
                return;
            }
            Graphics g = e.Graphics;
            UpdateBitmap();
            g.DrawImage(drawBmp, 0, 0);
            base.OnPaint(e);
        }

        void UpdateBitmap()
        {
            if (drawBmp == null)
                return;
            using (Graphics g = Graphics.FromImage(drawBmp))
            {
                g.Clear(BackColor);
                if (lines == null || lines.Count == 0)
                    return;
                if (startLine >= lines.Count)
                    startLine = lines.Count - 1;
                for (int i = 0; i < numVisibleLines; i++)
                {
                    int drawLineIx = i + startLine;
                    if (drawLineIx >= lines.Count)
                        break;
                    Brush b;
                    if (drawLineIx == highlightLine)
                    {
                        b = backBrush;
                        g.FillRectangle(foreBrush, 0, i * Font.Height, Width, Font.Height);
                    }
                    else
                        b = new SolidBrush(ForeColor);
                    g.DrawString(lines[drawLineIx], Font, b, 0, i * Font.Height);
                }
            }
        }

        void InitDimensions()
        {
            numVisibleLines = Height / Font.Height;
            drawBmp = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(drawBmp))
            {
                charWidth = g.MeasureString(" ", Font).Width;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            InitDimensions();
            base.OnSizeChanged(e);
        }

        private void vScroll_Scroll(object sender, ScrollEventArgs e)
        {
            startLine = vScroll.Value;
            Invalidate();
        }

        public void SetSelectedLine(int lineno, bool centerLine = false)
        {
            if (lineno == highlightLine || lineno >= lines.Count)
                return;
            highlightLine = lineno;
            if (SelectedLineChanged != null)
                SelectedLineChanged(this, highlightLine);
            if (centerLine)
            {
                startLine = highlightLine - numVisibleLines / 2;
                if (startLine < 0)
                    startLine = 0;
            }
            Invalidate();
        }

        private void GcodeViewer_MouseClick(object sender, MouseEventArgs e)
        {
            int clickLine = e.Y / Font.Height;
            if (clickLine >= numVisibleLines)
                return;
            clickLine += startLine;
            SetSelectedLine(clickLine);
        }
    }
}
