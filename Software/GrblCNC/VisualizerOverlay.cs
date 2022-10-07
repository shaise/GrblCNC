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
        VisualizerWin viswin;
        int cury, curx;
        string lastCoordCode = "";
        int lastTool = -2;
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
        
        void PrintLine(Graphics g, string str)
        {
            g.DrawString(str, font, Brushes.Black, curx, cury);
            cury += 15;
        }

        public void Update(GrblStatus status)
        {
            if (status.gState == null)
                return;
            if (CopyCoords(status.workingCoords) || 
                lastCoordCode != status.CurrentCoordSystem || lastTool != Global.ginterp.currentTool)
            {
                lastCoordCode = status.CurrentCoordSystem;
                lastTool = Global.ginterp.currentTool;
                using (Graphics g = viswin.GetOverlayGraphics())
                {
                    g.Clear(Color.Transparent);
                    g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                    cury = curx = 10;
                    PrintLine(g, "WCO active: " + lastCoordCode + ", T" + lastTool.ToString());
                    for (int i = 0; i < lastCoords.Length; i++)
                        PrintLine(g, "WCO " + Utils.GetAxisLetter(i) + ": " + lastCoords[i].ToString("0.000"));
                }
                viswin.UpdateOverlay();
            }
        }
    }
}
