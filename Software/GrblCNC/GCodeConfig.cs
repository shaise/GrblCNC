using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrblCNC.Properties;
using GrblCNC.Glutils;

namespace GrblCNC
{
    public class GCodeConfig
    {
        public class GCodeParam
        {
            public string code;
            //public float [] floatVal;
            public string [] strVal;

            public GCodeParam(string parcode, string [] parvals)
            {
                code = parcode;
                strVal = new string[Global.NUM_AXIS];
                for (int i = 0; i < Global.NUM_AXIS; i++)
                    strVal[i] = "0.000";
                SetVals(parvals);
            }

            public void SetVals(string [] vals)
            {
                for (int i = 0; i < Global.NUM_AXIS && i < vals.Length; i++)
                    strVal[i] = vals[i];
            }

            // validate a parameter value, return it formated if valid, or null if not
            public string ValidateValue(string val)
            {
                string result = null;
                try
                {
                    float tfloat = Utils.ParseFloatInvariant(val);
                    result = Utils.ToInvariantString(tfloat, "0.000");
                }
                catch { }
                return result;
            }
        }

        List<GCodeParam> parameters;

        public GCodeConfig()
        {
            parameters = new List<GCodeParam>();
        }

        public List<GCodeParam> GetParams()
        {
            return parameters;
        }

        GCodeParam GetParam(string parcode)
        {
            foreach (GCodeParam par in parameters)
            {
                if (par.code == parcode)
                    return par;
            }
            return null;
        }

        public void ParseParam(string stparam)
        {
            if (!stparam.StartsWith("[G"))
                return;
            string[] vars = stparam.Split(new char[] { '[', ':', ']' }, StringSplitOptions.RemoveEmptyEntries);
            if (vars.Length != 2)
                return;
            string parcode = vars[0];

            // split all axis vals
            vars = vars[1].Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            // see if we already have this param
            GCodeParam par = GetParam(parcode);
            if (par == null)
            {
                par = new GCodeParam(parcode, vars);
                parameters.Add(par);
            }
            else
                par.SetVals(vars);
        }
        
    }
}
