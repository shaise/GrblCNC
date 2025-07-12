using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrblCNC
{
    public class GrblUtils
    {
        static string[] axisLetter = new string[] { "X", "Y", "Z", "A", "B", "C" };
        static string[] coordSystems = new string[] { "G54", "G55", "G56", "G57", "G58", "G59", "G59.1", "G59.2", "G59.3" };
        public static int numCoordSystems = coordSystems.Length;
        
        private static int lastNumCoordSystems = -1;
        private static string[] range;

        public static string GetAxisLetter(int axis)
        {
            if (axis < 0 || axis >= axisLetter.Length)
                return null;
            return axisLetter[axis];
        }

        public static string GetCoordSystem(int coordSystemIx)
        {
            if (coordSystemIx < 0 ||  coordSystemIx >= coordSystems.Length)
                return "";
            return coordSystems[coordSystemIx];
        }

        public static string[] GetCoordSystemRange()
        {
            if (numCoordSystems == lastNumCoordSystems)
                return range;

            lastNumCoordSystems = numCoordSystems;
            range = new string[numCoordSystems];
            for (int i = 0; i < numCoordSystems; i++)
            {
                range[i] = string.Format("P{0} ({1})", i + 1, coordSystems[i]);
            }
            return range;
        }

        public static string GetGcodeLocation(int axisMask, double[] pos)
        {
            StringBuilder sb = new StringBuilder();
            int axis = 1;
            for (int i = 0; i < Global.NumAxes; i++)
            {
                if ((axisMask & axis) != 0)
                    sb.Append(string.Format("{0}{1:0.000}", axisLetter[i], pos[i]));
                axis <<= 1;
            }
            return sb.ToString();
        }
    }
}
