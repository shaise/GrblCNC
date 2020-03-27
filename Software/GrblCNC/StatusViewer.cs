using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrblCNC
{
    public partial class StatusViewer : UserControl
    {
        int spacingX = 6;
        int spacingY = 4;
        float desiredRatio = 160f / 36;
        List<BigNumViewer> numViewers = null;

        public StatusViewer()
        {
            InitializeComponent();
            // Fixme: hard coded now, but which data to show should be configurable
            numViewers = new List<BigNumViewer>();
            numViewers.Add(bigNumX);
            numViewers.Add(bigNumY);
            numViewers.Add(bigNumZ);
            numViewers.Add(bigNumA);
            numViewers.Add(bigNumB);
            numViewers.Add(bigNumF);
            ApplyColors();
        }

        void ApplyColors()
        {
            foreach (BigNumViewer bignum in numViewers)
            {
                bignum.ForeColor = Color.Indigo;
                bignum.TitleForeColor = Color.Indigo;
                bignum.BackColor = Color.Thistle;
                bignum.TitleBackColor = Color.MediumPurple;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (numViewers == null)
                return;
            int w3 = Width / 3;
            int h2 = Height / 2;
            float ratio = (float)w3 / h2;
            if (ratio > desiredRatio)
                w3 = (int)(desiredRatio * h2);
            else
                h2 = (int)((float)w3 / desiredRatio);
            int sx = w3 - spacingX * 2;
            int sy = h2 - spacingY;
            for (int i = 0; i < numViewers.Count; i++)
            {
                numViewers[i].Location = new Point((i % 3) * w3 + spacingX, (i / 3) * h2 + spacingY);
                numViewers[i].Width = sx;
                numViewers[i].Height = sy;
            }
            base.OnSizeChanged(e);
        }

        public void SetAxisValues(float [] axVals)
        {
            int naxis = axVals.Length < 5 ? axVals.Length : 5;
            for (int i = 0; i < naxis; i++)
                numViewers[i].Value = axVals[i];
        }
    }
}
