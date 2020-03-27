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
        List<UserControl> numViewers = null;
        int homestate = -1;

        public StatusViewer()
        {
            InitializeComponent();
            // Fixme: hard coded now, but which data to show should be configurable
            numViewers = new List<UserControl>();
            numViewers.Add(bigNumX);
            numViewers.Add(bigNumY);
            numViewers.Add(bigNumZ);
            numViewers.Add(bigNumA);
            numViewers.Add(bigNumB);
            numViewers.Add(bigNumF);
            numViewers.Add(alarmView);
            ApplyColors();
        }

        void ApplyColors()
        {
            foreach (UserControl ctrl in numViewers)
            {
                // Fixme: maybe AlarmViewer and BigNumViewer inherit from same control?
                if (ctrl is BigNumViewer)
                {
                    BigNumViewer bignum = (BigNumViewer)ctrl;
                    bignum.ForeColor = Color.Indigo;
                    bignum.TitleForeColor = Color.Indigo;
                    bignum.BackColor = Color.Thistle;
                    bignum.TitleBackColor = Color.MediumPurple;
                }
                else if (ctrl is AlarmViewer)
                {
                    AlarmViewer alarmView = (AlarmViewer)ctrl;
                    alarmView.ForeColor = Color.Indigo;
                    alarmView.TitleForeColor = Color.Indigo;
                    alarmView.BackColor = Color.Thistle;
                    alarmView.TitleBackColor = Color.MediumPurple;
                }
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (numViewers == null)
                return;
            int w3 = Width / 3;
            int h2 = Height / 3;
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
                ((BigNumViewer)numViewers[i]).Value = axVals[i];
        }

        public void SetAlarms(string alarms)
        {
            alarmView.Alarms = alarms;
        }

        public void SetHomeState(int homeState)
        {
            if (homestate == homeState)
                return;
            homestate = homeState;
            for (int i = 0; i < 5; i++)
                ((BigNumViewer)numViewers[i]).Homed = (homeState & (1 << i)) != 0;
        }
    }
}
