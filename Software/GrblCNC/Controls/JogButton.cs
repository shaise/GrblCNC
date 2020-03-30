using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GrblCNC.Glutils;

namespace GrblCNC.Controls
{
    public partial class JogButton : UserControl
    {
        public enum ArrowDir
        {
            Up = 0,
            Down,
            Left,
            Right,
            Center,
            StrechX
        }
        RectangleF[] imgRects;
        RectangleF screenRect;
        Bitmap bmpNormal, bmpPress, bmpHover;
        Bitmap bmpSelected;  // one of the above, selected by press state
        Bitmap image, disabledImage;
        ArrowDir buttonType;
        string text = "";
        //float textCellOffs = 5;
        float textCellWidth;
        float textHeight;
        bool mouseDown;
        bool mouseIn;
        float fontScale = 0.6f;
        Point textOffset = new Point(0, 0);
        Font activeFont;
        int id = 0;
        public JogButton()
        {
            InitializeComponent();
            DoubleBuffered = true;
            // Disable double click detection, otherwise rapid clicks will not work
            SetStyle(ControlStyles.StandardDoubleClick, false); 
        }

        void UpdateRects()
        {
            if (bmpNormal == null)
                return;
            float h = bmpNormal.Width / 3;
            float w = bmpNormal.Height - h;
            textCellWidth = h;
            imgRects = new RectangleF[5];
            imgRects[0] = new RectangleF(0, h, h, w);
            imgRects[1] = new RectangleF(h, h, h, w);
            imgRects[2] = new RectangleF(0, 0, w, h);
            imgRects[3] = new RectangleF(w, 0, w, h);
            imgRects[4] = new RectangleF(2 * h, h, h, h);
            Invalidate();
        }

        void UpdateSize()
        {
            if (imgRects == null)
                return;
            if (buttonType == ArrowDir.StrechX)
            {
                Height = (int)(imgRects[(int)ArrowDir.Center].Height + 0.3);
                if (Width < textCellWidth)
                    Width = (int)textCellWidth;
            }
            else
            {
                Width = (int)(imgRects[(int)buttonType].Width + 0.3);
                Height = (int)(imgRects[(int)buttonType].Height + 0.3);
            }
            screenRect = new RectangleF(0, 0, Width, Height);
            float minlen = Width < Height ? Width : Height;
            FontFamily ff = buttonType == ArrowDir.StrechX ? Font.FontFamily : FontManager.FamilyFixed;
            activeFont = new Font(ff, minlen * fontScale, FontStyle.Regular, GraphicsUnit.Pixel);
            textHeight = Utils.GetFontAccent(activeFont);
            Invalidate();
        }

        void GenerateDisabledImage()
        {
            if (image == null)
                return;
            disabledImage = Utils.ColorizeBitmap(image, Color.FromArgb(64,255,255,255));
        }  

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public Image ImageNormal
        {
            get { return bmpNormal; }
            set
            {
                bmpNormal = (Bitmap)value;
                bmpSelected = bmpNormal;
                UpdateRects();
            }
        }

        public Image ImagePress
        {
            get { return bmpPress; }
            set { bmpPress = (Bitmap)value; }
        }

        public Image ImageHover
        {
            get { return bmpHover; }
            set { bmpHover = (Bitmap)value; }
        }

        public ArrowDir ButtonType
        {
            get { return (ArrowDir)buttonType; }
            set { 
                buttonType = value;
                UpdateSize();
            }
        }

        public string Caption // not using the standard 'Text' since VS resets it everytime I edit the control
        {
            get { return text; }
            set
            {
                text = value;
                Invalidate();
            }
        }

        public Image Image
        {
            get { return image; }
            set
            {
                image = (Bitmap)value;
                GenerateDisabledImage();
                Invalidate();
            }
        }

        public float FontScale
        {
            get { return fontScale; }
            set
            {
                fontScale = value;
                UpdateSize();
            }
        }

        public Point TextOffset
        {
            get { return textOffset; }
            set
            {
                textOffset = value;
                Invalidate();
            }
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            Invalidate();
            base.OnForeColorChanged(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            UpdateRects();
            UpdateSize();
            base.OnLoad(e);
        }

        void DrawHorizontalStrechButt(Graphics g)
        {
            int len3 = Height / 3;
            RectangleF centRect = imgRects[(int)ArrowDir.Center];
            // left side
            RectangleF patch = new RectangleF(centRect.X, centRect.Y, len3, centRect.Height);
            RectangleF dest = new RectangleF(0, 0, len3, Height);
            g.DrawImage(bmpSelected, dest, patch, GraphicsUnit.Pixel);

            // center
            patch.X += len3;
            dest.X = len3;
            dest.Width = Width - 2 * len3;
            g.DrawImage(bmpSelected, dest, patch, GraphicsUnit.Pixel);

            // right side
            patch.X = centRect.X + centRect.Width - len3;
            dest.X = Width - len3;
            dest.Width = len3;
            g.DrawImage(bmpSelected, dest, patch, GraphicsUnit.Pixel);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (bmpSelected != null && screenRect != null)
            {
                if (buttonType == ArrowDir.StrechX)
                    DrawHorizontalStrechButt(g);
                else
                    g.DrawImage(bmpSelected, screenRect, imgRects[(int)buttonType], GraphicsUnit.Pixel);
            }
            float xpos = 0;
            float ypos = 0;
            if (buttonType == ArrowDir.Left)
                xpos += Width - Height;
            if (buttonType == ArrowDir.Up)
                ypos += Height - Width;
            if (mouseDown && mouseIn)
            {
                xpos++;
                ypos++;
            }
            Bitmap img = Enabled ? image : disabledImage;
            if (img != null)
                g.DrawImage(img, xpos, ypos);
            if (text != null)
            {
                float tCellW = buttonType == ArrowDir.StrechX ? Width : textCellWidth;
                ypos += (textCellWidth - textHeight) / 2 + textOffset.Y;
                SizeF tsize = g.MeasureString(text, activeFont);
                xpos += (tCellW - tsize.Width) / 2 + textOffset.X;
                Brush b;
                if (Enabled)
                    b = new SolidBrush(ForeColor);
                else
                    b = new SolidBrush(Color.FromArgb(64, ForeColor));

                g.DrawString(text, activeFont, b, xpos, ypos);
            }
            base.OnPaint(e);
        }

        void UpdateMouseState()
        {
            if (!mouseIn)
                bmpSelected = bmpNormal;
            else
            {
                if (mouseDown)
                    bmpSelected = bmpPress;
                else
                    bmpSelected = bmpHover;
            }
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (mouseDown)
            {
                bool lastMouseIn = mouseIn;
                if (e.X < 0 || e.X > Width || e.Y < 0 || e.Y > Height)
                    mouseIn = false;
                else
                    mouseIn = true;
                if (lastMouseIn != mouseIn)
                    UpdateMouseState();
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            mouseIn = true;
            UpdateMouseState();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            mouseIn = false;
            UpdateMouseState();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                mouseDown = true;
            UpdateMouseState();
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                mouseDown = false;
            UpdateMouseState();
            base.OnMouseUp(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            Invalidate();
            base.OnEnabledChanged(e);
        }
    }
}
