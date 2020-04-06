using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GrblCNC.Properties;

namespace GrblCNC.Controls
{
    public partial class ErrorView : UserControl
    {
        int id;
        const float xSize = 10;
        const float borderWidth = 2;
        float gap1;
        float gap2;
        bool isOverX;

        public enum ErrorType
        {
            Alarm,
            Error
        }

        public ErrorView()
        {
            InitializeComponent();
            gap1 = borderWidth + 2 + xSize;
            gap2 = borderWidth + 2;
        }

        public override string Text
        {
            get
            {
                return labelError.Text;
            }
            set
            {
                labelError.Text = value;
                toolTip1.SetToolTip(labelError, MakeMultiline(value, 30));
            }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public void SetErrorType(ErrorType typ)
        {
            if (typ == ErrorType.Alarm)
                pictureBox1.Image = Resources.AlarmErrorIcon;
            else
                pictureBox1.Image = Resources.ErrorIcon;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen pOutline = new Pen(Color.DarkRed, borderWidth);
            float bw2 = borderWidth / 2;
            g.DrawRectangle(pOutline, bw2, bw2, Width - borderWidth, Height - borderWidth);
            if (isOverX)
                g.FillRectangle(Brushes.LightBlue, Width - gap1, gap2, xSize, xSize);
            Pen pX = new Pen(ForeColor, 2);
            g.DrawLine(pX, Width - gap1, gap2, Width - gap2, gap1);
            g.DrawLine(pX, Width - gap2, gap2, Width - gap1, gap1);
            base.OnPaint(e);
        }

        string MakeMultiline(string line, int maxCharsPerLine)
        {
            string[] words = line.Split(' ');
            StringBuilder sb = new StringBuilder();
            int len = 0;
            foreach (string word in words)
            {
                if (len + word.Length > maxCharsPerLine)
                {
                    sb.Append("\r\n");
                    len = 0;
                }
                sb.Append(word);
                sb.Append(" ");
                len += word.Length + 1;
            }
            return sb.ToString();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            bool oldState = isOverX;
            isOverX = e.X >= (Width - gap1) && e.X <= (Width - gap2) && e.Y >= gap2 && e.Y <= gap1;
            if (oldState != isOverX)
            {
                oldState = isOverX;
                Invalidate();
            }
            base.OnMouseMove(e);
        }

        private void labelError_Click(object sender, EventArgs e)
        {
            InvokeOnClick(this, e);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            InvokeOnClick(this, e);
        }
    }
}
