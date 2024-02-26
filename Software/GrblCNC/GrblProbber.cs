using GrblCNC.Glutils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrblCNC
{
    internal class GrblProbber
    {
        enum ProbeType
        {
            None,
            SingleAxis,
            Hole
        }

        ProbeType probeType;
        int probeStage;
        GrblComm grblComm;
        int probeAxis;
        float probeAxisPos;
        int probeCoord;
        float probeOffset;
        float probeDir;

        public GrblProbber(GrblComm gComm)
        {
            grblComm = gComm;
            probeType = ProbeType.None;
            grblComm.ProbeCompleted += GrblComm_ProbeCompleted;
        }

        public void ProbeSingle(int axis, int coordSystemIx, float offset, float dir)
        {
            if (coordSystemIx < -1 || coordSystemIx > 8)
                return;
            probeType = ProbeType.SingleAxis;
            probeAxisPos = grblComm.GetAxisPosition(axis);
            probeAxis = axis;
            probeDir = dir;
            probeCoord = coordSystemIx;
            probeOffset = offset;
            grblComm.ProbeAxis(axis, dir * 10);
        }

        void ProbeSingleComplete(float[] prbVals)
        {
            probeType = ProbeType.None;
            float retractDist = probeAxisPos - prbVals[probeAxis];
            if (retractDist < -5)
                retractDist = -5;
            if (retractDist > 5)
                retractDist = 5;
            grblComm.CoordTouchAxis(probeAxis, probeCoord, probeOffset, retractDist);
        }

        public void ProbeHoleCenter()
        {
            probeType = ProbeType.Hole;
            probeAxis = GrblComm.X_AXIS;
            probeDir = -1;
            probeAxisPos = grblComm.GetAxisPosition(probeAxis);
            grblComm.ProbeAxis(probeAxis, probeDir * 50);
        }

        private void GrblComm_ProbeCompleted(object sender, float[] prbVals)
        {
            switch (probeType)
            {
                case ProbeType.None:
                    return;

                case ProbeType.SingleAxis:
                    ProbeSingleComplete(prbVals);
                    break;

                case ProbeType.Hole:
                    break;
            }
        }
    }
}
