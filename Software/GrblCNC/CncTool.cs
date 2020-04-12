using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrblCNC.Glutils;


namespace GrblCNC
{
    class CncTool
    {
        public int toolNum;
        public int pocketNum;
        public float [] offsets;
        public float diameter;
        public string description;

        public CncTool()
        {
            toolNum = pocketNum = -1;
            offsets = new float[Global.NUM_AXIS];
            for (int i = 0; i < offsets.Length; i++)
                offsets[i] = 0;
            diameter = 0;
            description = "";
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(toolNum);
            sb.Append("|");
            sb.Append(pocketNum);
            for (int i = 0; i<offsets.Length; i++)
            {
                sb.Append("|");
                sb.Append(Utils.ToInvariantString(offsets[i],"0.000"));
            }
            sb.Append("|");
            sb.Append(Utils.ToInvariantString(diameter, "0.000"));
            sb.Append("|");
            sb.Append(description);
            return sb.ToString();
        }

        public bool [] Parse(bool tryOnly)
        {
            bool[] res = new bool[4 + offsets.Length];
            return res;
        }
    }
}
