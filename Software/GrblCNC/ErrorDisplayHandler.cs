using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using GrblCNC.Controls;

namespace GrblCNC
{
    class ErrorDisplayHandler
    {
        Form mForm;
        List<ErrorView> displayedViews;
        List<ErrorView> hiddenViews;
        const int MAX_ERR_VIEWS = 12;
        int posX, posY, offset;
        int evWidth, evHeight;
        int maxVisibleErrors;
        public ErrorDisplayHandler(Form frm)
        {
            mForm = frm;
            displayedViews = new List<ErrorView>();
            hiddenViews = new List<ErrorView>();
            for (int i = 0; i<MAX_ERR_VIEWS; i++)
            {
                ErrorView v = new ErrorView();
                v.Id = i;
                v.Visible = false;
                v.Click += v_Click;
                mForm.Controls.Add(v);
                hiddenViews.Add(v);
            }
            evWidth = hiddenViews[0].Width;
            evHeight = hiddenViews[0].Height;
            UpdateSize();
        }


        void CloseError(ErrorView v, bool updateLoc = true)
        {
            for (int i = 0; i< displayedViews.Count; i++)
            {
                if (displayedViews[i] == v)
                {
                    displayedViews.RemoveAt(i);
                    v.Visible = false;
                    hiddenViews.Add(v);
                    if (updateLoc)
                        UpdateLocations(i);
                    break;
                }
            }

        }

        void v_Click(object sender, EventArgs e)
        {
            CloseError((ErrorView)sender);
        }

        public void UpdateSize()
        {
            posX = mForm.ClientSize.Width - evWidth - 6;
            posY = mForm.ClientSize.Height - evHeight - 6;
            offset = evHeight + 6;
            maxVisibleErrors = mForm.ClientSize.Height / offset;
            if (maxVisibleErrors > MAX_ERR_VIEWS)
                maxVisibleErrors = MAX_ERR_VIEWS;
            // remove out of frame messages
            while (displayedViews.Count > maxVisibleErrors)
                CloseError(displayedViews[0], false);
            UpdateLocations();
        }

        void UpdateLocations(int from = 0)
        {
            // fix other's location
            for (int i = from; i < displayedViews.Count; i++)
                displayedViews[i].Location = new Point(posX, posY - i * offset);
        }

        public void AddError(string errMessage, bool isAlarm = false)
        {
            if (displayedViews.Count >= maxVisibleErrors)
                CloseError(displayedViews[0]);
            ErrorView v = hiddenViews[0];
            hiddenViews.RemoveAt(0);
            v.Location = new Point(posX, posY - displayedViews.Count * offset);
            displayedViews.Add(v);
            v.Text = errMessage;
            v.SetErrorType(isAlarm ? ErrorView.ErrorType.Alarm : ErrorView.ErrorType.Error);
            v.BringToFront();
            v.Visible = true;
        }

    }
}
