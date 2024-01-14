using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace GrblCNC.Controls
{
    public partial class ValueSlider : UserControl
    {
        string dataFormat = "{0:0.0}";
        //string textFormat = "0.0";
        float value = 0;
        float minValue = 0;
        float maxValue = 100;
        float resetValue;
        int sliderLen = 80;
        Bitmap thumbImage;
        int thumbMinPos = 0, thumbMaxPos, thumbCurPos;
        Color slideBackColor, slideHighlightColor, slideShadowColor;
        bool thumbPressed = false;
        int thumbPressLoc = 0;
        int thumbSavedLoc = 0;
        public delegate void ValueChangeDelegate(object sender, float value);
        public event ValueChangeDelegate ValueChange;
        Timer timer;
        bool reportChange = false;

        public ValueSlider()
        {
            InitializeComponent();
            DoubleBuffered = true;
            // limit reports to 5 times a second
            timer = new Timer();
            timer.Interval = 200; // ms
            timer.Tick += timer_Tick;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (reportChange)
            {
                if (ValueChange != null)
                    ValueChange(this, value);
                reportChange = false;
            }
            else
                timer.Stop();
        }

        void ReportChange()
        {
            reportChange = true;
            if (!timer.Enabled)
                timer.Start();
        }

        public string DataFormat
        {
            get { return dataFormat; }
            set
            {
                dataFormat = value;
                updateValueText();
            }
        }

        public string TitleText
        {
            get { return labelTitle.Text; }
            set { labelTitle.Text = value; }
        }

        public float MinValue
        {
            get { return minValue; }
            set
            {
                minValue = value;
                if (this.value < minValue)
                    this.value = minValue;
                UpdateThumbVars();
                updateValueText();
            }
        }

        public float MaxValue
        {
            get { return maxValue; }
            set
            {
                maxValue = value;
                if (this.value > maxValue)
                    this.value = maxValue;
                UpdateThumbVars();
                updateValueText();
            }
        }

        public float Value
        {
            get { return value; }
            set
            {
                this.value = value;
                UpdateThumbVars();
                updateValueText();
            }
        }

        void UpdateThumbVars()
        {
            if (thumbImage == null)
                return;
            thumbMinPos = Width - sliderLen + 1;
            thumbMaxPos = Width - thumbImage.Width - 1;
            thumbCurPos = (int)((value - minValue) * (thumbMaxPos - thumbMinPos) / (maxValue - minValue) + thumbMinPos);
            Height = thumbImage.Height / 2 + 2;
            UpdateTextLocation();
            Invalidate();
        }

        void UpdateThumbColors()
        {
            if (thumbImage == null)
                return;
            Color c1, c2;
            c1 = slideHighlightColor = thumbImage.GetPixel(0, 0);
            c2 = slideShadowColor = thumbImage.GetPixel(thumbImage.Width - 1, thumbImage.Height / 2 - 1);
            slideBackColor = Color.FromArgb((c1.R + c2.R) / 2, (c1.G + c2.G) / 2, (c1.B + c2.B) / 2);
        }

        public Image ThumbImage
        {
            get { return thumbImage; }
            set { 
                thumbImage = (Bitmap)value;
                UpdateThumbVars();
                UpdateThumbColors();
            }
        }

        void UpdateTextLocation()
        {
            int ypos = (Height - labelTitle.Height) / 2;
            labelData.Location = new Point(Width - sliderLen - labelData.Width, ypos);
            labelTitle.Location = new Point(0, ypos);
        }

        void updateValueText()
        {
            string txt;
            try
            {
                txt = string.Format(CultureInfo.InvariantCulture, dataFormat, value);
            }
            catch
            {
                txt = "Bad Format";
            }
            labelData.Text = txt;
            UpdateTextLocation();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen pHighlight = new Pen(slideHighlightColor);
            Brush bBack = new SolidBrush(slideBackColor);
            Pen pShadow = new Pen(slideShadowColor);
            int sleft = Width - sliderLen;
            g.FillRectangle(bBack, sleft, 0, sliderLen, Height);
            g.DrawLine(pShadow, sleft, 0, Width, 0);
            g.DrawLine(pShadow, sleft, 0, sleft, Height);
            g.DrawLine(pHighlight, sleft, Height - 1, Width, Height - 1);
            g.DrawLine(pHighlight, Width - 1, 1, Width -1, Height - 1);
            if (thumbImage != null)
            {
                int ypos = thumbPressed ? thumbImage.Height / 2 : 0;
                Rectangle src = new Rectangle(0, ypos, thumbImage.Width, thumbImage.Height / 2);
                Rectangle dst = new Rectangle(thumbCurPos, 1, thumbImage.Width, thumbImage.Height / 2);
                g.DrawImage(thumbImage, dst, src, GraphicsUnit.Pixel);
            }
            base.OnPaint(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (thumbImage == null)
                    return;
                if (e.X > thumbCurPos && e.X < thumbCurPos + thumbImage.Width)
                {
                    thumbPressed = true;
                    thumbSavedLoc = thumbCurPos;
                    thumbPressLoc = e.X - thumbCurPos;
                    Invalidate();
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                Value = resetValue;
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (thumbPressed)
            {
                int curloc = thumbCurPos;
                if (e.Y < -30 || e.Y > (Height + 30))
                    thumbCurPos = thumbSavedLoc;
                else
                {
                    thumbCurPos = e.X - thumbPressLoc;
                    if (thumbCurPos < thumbMinPos)
                        thumbCurPos = thumbMinPos;
                    else if (thumbCurPos > thumbMaxPos)
                        thumbCurPos = thumbMaxPos;
                }
                if (curloc != thumbCurPos)
                {
                    value = (float)(thumbCurPos - thumbMinPos) * (maxValue - minValue) / (thumbMaxPos - thumbMinPos) + minValue;
                    updateValueText();
                    ReportChange();
                    Invalidate();
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                thumbPressed = false;
                Invalidate();
            }
            base.OnMouseUp(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            resetValue = value;
        }
    }
}
