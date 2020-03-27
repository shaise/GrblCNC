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
    public partial class DualProgressTool : ToolStripItem
    {
        int value1 = 0;
        int maximum1 = 100;
        int value2 = 0;
        int maximum2 = 100;
        Color color1 = Color.Blue;
        Color color2 = Color.Blue;
        public DualProgressTool()
        {
        }

        public int Maximum1
        {
            get { return maximum1; }
            set
            {
                maximum1 = value;
                if (maximum1 < 1)
                    maximum1 = 1;
                if (maximum1 < value1)
                    value1 = maximum1;
                Invalidate();
            }
        }

        public int Value1
        {
            get { return value1; }
            set
            {
                value1 = value;
                if (value1 < 0)
                    value1 = 0;
                if (value1 > maximum1)
                    maximum1 = value1;
                Invalidate();
            }
        }

        public Color Color1
        {
            get { return color1; }
            set
            {
                color1 = value;
                Invalidate();
            }
        }
        
        public int Maximum2
        {
            get { return maximum2; }
            set
            {
                maximum2 = value;
                if (maximum2 < 1)
                    maximum2 = 1;
                if (maximum2 < value2)
                    value2 = maximum2;
                Invalidate();
            }
        }

 
        public int Value2
        {
            get { return value2; }
            set
            {
                value2 = value;
                if (value2 < 0)
                    value2 = 0;
                if (value2 > maximum2)
                    maximum2 = value2;
                Invalidate();
            }
        }

        public Color Color2
        {
            get { return color2; }
            set
            {
                color2 = value;
                Invalidate();
            }
        }

        void DrawBar(Graphics g, Color c, Rectangle rect, int val, int max)
        {
            Brush b = new SolidBrush(Color.FromArgb(64, c));
            g.FillRectangle(b, rect);
            rect.Width = (val * rect.Width) / max;
            b = new SolidBrush(c);
            g.FillRectangle(b, rect);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            Rectangle rectangle = e.ClipRectangle;
            rectangle.Height -= 2;
            ProgressBarRenderer.DrawHorizontalBar(e.Graphics, rectangle);
            rectangle.Width = rectangle.Width - 4;
            rectangle.Height = (Height - 4) / 2 - 2;
            rectangle.X = 2;
            rectangle.Y = 2;
            DrawBar(e.Graphics, color1, rectangle, value1, maximum1);
            rectangle.Y += rectangle.Height + 2;
            DrawBar(e.Graphics, color2, rectangle, value2, maximum2);
        }    
    }
}
