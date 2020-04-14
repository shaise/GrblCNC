using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrblCNC.Glutils;


namespace GrblCNC
{
    public class CncTool : IComparable
    {
        public int toolNum;
        public int pocketNum;
        public float [] offsets;
        public float diameter;
        public string description;

        public const int ToolNumID = 0;
        public const int PocketNumID = 1;
        public const int NumAxesID = 2;
        public const int OffsetsID = 3;
        public const int DiameterID = 3 + Global.NUM_AXIS;
        public const int DescriptionID = 4 + Global.NUM_AXIS; 

        public CncTool(int toolnum)
        {
            toolNum = pocketNum = toolnum;
            offsets = new float[Global.NUM_AXIS];
            for (int i = 0; i < offsets.Length; i++)
                offsets[i] = 0;
            diameter = 0;
            description = "";
        }

        int IComparable.CompareTo(object other)
        {
            return toolNum.CompareTo(((CncTool)other).toolNum);
        }

        public void CopyFrom(CncTool other)
        {
            toolNum = other.toolNum;
            pocketNum = other.pocketNum;
            int naxis = other.offsets.Length > offsets.Length ? offsets.Length : other.offsets.Length;
            for (int i = 0; i < naxis; i++)
                offsets[i] = other.offsets[i];
            diameter = other.diameter;
            description = other.description;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(toolNum);
            sb.Append("|");
            sb.Append(pocketNum);
            sb.Append("|");
            sb.Append(offsets.Length);
            for (int i = 0; i < offsets.Length; i++)
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

        bool ParseInt(string strval, ref int intval, bool tryOnly)
        {
            try
            {
                int tval = int.Parse(strval);
                if (tval < 0)
                    return false;
                if (!tryOnly)
                    intval = tval;
            }
            catch
            {
                return false;
            }
            return true;
        }

        bool ParseFloat(string strval, ref float floatval, bool tryOnly)
        {
            try
            {
                float tval = Utils.ParseFloatInvariant(strval);
                if (!tryOnly)
                    floatval = tval;
            }
            catch
            {
                return false;
            }
            return true;
        }

        public List<int> Parse(string[] vars, bool tryOnly)
        {
            List<int> res = new List<int>();
            int numAxes = 0;
            if (vars.Length <= NumAxesID)
            {
                // major error, too few parameters
                res.Add(NumAxesID); 
                return res;
            }
            bool validnumaxes = ParseInt(vars[NumAxesID], ref numAxes, false);
            {
                if (!validnumaxes || numAxes < 3 || numAxes > 6 || vars.Length < (numAxes + 5))
                {
                    res.Add(NumAxesID);
                    return res;
                }
            }
            if (!ParseInt(vars[ToolNumID], ref toolNum, tryOnly))
                res.Add(ToolNumID);
            if (!ParseInt(vars[PocketNumID], ref pocketNum, tryOnly))
                res.Add(PocketNumID);
            if (numAxes > offsets.Length)
                numAxes = offsets.Length;
            for (int i = 0; i < numAxes; i++)
                if (!ParseFloat(vars[OffsetsID + i], ref offsets[i], tryOnly))
                    res.Add(OffsetsID + i);
            if (!ParseFloat(vars[DiameterID], ref diameter, tryOnly))
                res.Add(DiameterID);
            return res;
        }

        public List<int> Parse(string line, bool tryOnly = false)
        {
            return Parse(line.Split('|'), tryOnly);
        }

        public List<int> TryParse(string line)
        {
            return Parse(line, true);
        }
    }
}
