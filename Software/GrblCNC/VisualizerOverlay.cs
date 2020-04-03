using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrblCNC.Glutils;
using System.Drawing;
using System.Drawing.Text;

namespace GrblCNC
{
    class VisualizerOverlay
    {
        string[] lines;
        VisualizerWin viswin;
        string lastCoordCode = "";
        float[] lastCoords = new float[GrblStatus.NUM_AXIS];
        Font font;
        public VisualizerOverlay(VisualizerWin viswin)
        {
            this.viswin = viswin;
            font = new Font("Courier New", 12, FontStyle.Bold);
        }

        bool CopyCoords(float [] newCoords)
        {
            bool isChange = false;
            for (int i=0; i<lastCoords.Length; i++)
            {
                if (lastCoords[i] != newCoords[i])
                {
                    lastCoords[i] = newCoords[i];
                    isChange = true;
                }
            }
            return isChange;
        }
        
        public void Update(GrblStatus status)
        {
            if (status.gState == null)
                return;
            if (CopyCoords(status.workingCoords) || 
                lastCoordCode != status.CurrentCoordSystem)
            {
                lastCoordCode = status.CurrentCoordSystem;
                using (Graphics g = viswin.GetOverlayGraphics())
                {
                    g.Clear(Color.Transparent);
                    g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                    for (int i = 0; i < lastCoords.Length; i++)
                    {
                        string str = lastCoordCode + " " + Utils.GetAxisLetter(i) + ": " + lastCoords[i].ToString("0.000");
                        g.DrawString(str, font, Brushes.Black, 10, i * 15 + 10);
                    }
                }
                viswin.UpdateOverlay();
            }
        }
    }
}
