using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using GrblCNC.Properties;

namespace GrblCNC
{
    class ToolButton : ToolStripProfessionalRenderer
    {
        public Color checkedColor = Color.FromArgb(130,200,250);

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            var btn = e.Item as ToolStripButton;
            if (btn != null && btn.Checked)
            {
                int offsx = (btn.Width - Resources.checkedButt.Width) / 2;
                int offsy = (btn.Height - Resources.checkedButt.Height) / 2;
                Rectangle bounds = new Rectangle(Point.Empty, e.Item.Size);
                e.Graphics.DrawImageUnscaled(Resources.checkedButt, offsx, offsy);
            }
            else base.OnRenderButtonBackground(e);
        }



 /*       protected override void OnPaint(PaintEventArgs e)
        {
            if (Checked)
            {
                Rectangle r = new Rectangle(0, 0, Width, Height);
                if (r.Contains(Control.MousePosition))
                    e.Graphics.DrawRectangle(Pens.DarkBlue, 0, 0, Height - 1, Width - 1);
                int offsx = (Width - Image.Width) / 2;
                int offsy = (Height - Image.Height) / 2;
                //e.Graphics.Clear(checkedColor);
                e.Graphics.DrawImageUnscaled(Resources.checkedButt, offsx, offsy);
                e.Graphics.DrawImageUnscaled(Image, offsx, offsy);
                return;
            }
            base.OnPaint(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            isHovered = true;
            base.OnMouseEnter(e);
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            isHovered = false;
            base.OnMouseLeave(e);
        }*/
    }
}
