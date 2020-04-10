using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrblCNC.Properties;
using GrblCNC.Glutils;

namespace GrblCNC
{
    public class GrblConfig
    {
        public enum ParamType
        {
            Float = 0,
            Int,
            Bool,
            Mask,
            Selection,
            String // or unknown
        }

        public class GrblParam : IComparable
        {
            public int code;
            public string strVal;
            public int intVal;
            public float floatVal;
            public ParamType type;

            // some params codes used:
            public enum Code
            { 
                MaxSpindleSpeedCode = 30,
                MinSpindleSpeedCode = 31,
            }

            int IComparable.CompareTo(object other)
            {
                return code.CompareTo(((GrblParam)other).code);
            }
            public GrblParam (int parcode, string parstr)
            {
                code = parcode;
                strVal = parstr;
            }

            // validate a parameter value, return it formated if valid, or null if not
            public string ValidateValue(string val)
            {
                float tfloat;
                int tint;
                string result = null;
                try
                {
                    switch (type)
                    {
                        case ParamType.Bool:
                            result = val == "1" || val == "0" ? val : null;
                            break;
                        case ParamType.Float:
                            tfloat = Utils.ParseFloatInvariant(val);
                            result = tfloat >= 0 ? Utils.ToInvariantString(tfloat, "0.000") : null;
                            break;
                        case ParamType.Int:
                        case ParamType.Mask:
                        case ParamType.Selection:
                            tint = int.Parse(val);
                            result = tint >= 0 ? tint.ToString() : null;
                            break;
                        case ParamType.String:
                            result = val;
                            break;
                    }
                }
                catch { }
                return result;
            }
        }

        public class ParamDescription
        {
            public int code;
            public string description;
            public string uints;
            public ParamType type;
            public string [] groups;
            public string[] options;
        }

        Dictionary<int, ParamDescription> parDesc;
        List<GrblParam> parameters;

        public GrblConfig()
        {
            InitParamDictionary();
            parameters = new List<GrblParam>();
        }
        
        void InitParamDictionary()
        {
            parDesc = new Dictionary<int, ParamDescription>();
            foreach (string line in Resources.GrblParamDescription.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] vars = line.Split('|');
                if (line[0] == '#' || vars.Length < 4)
                    continue;
                try {
                    int code = int.Parse(vars[0]);
                    ParamDescription pd = new ParamDescription();
                    pd.code = code;
                    pd.description = vars[1];
                    pd.uints = vars[2];
                    if (!Enum.TryParse<ParamType>(vars[3], out pd.type))
                        pd.type = ParamType.String;
                    if (vars.Length > 4)
                        pd.groups = vars[4].Split(',');
                    else
                        pd.groups = new string [] {"General"};
                    if (vars.Length > 5)
                        pd.options = vars[5].Split(',');
                    else
                        pd.options = null;

                    parDesc[code] = pd; 
                }
                catch { }
            }
        }

        public List<GrblParam> GetParams()
        {
            return parameters;
        }

        public Dictionary<int, ParamDescription> GetParamDescription()
        {
            return parDesc;
        }

        public string GetDescription(int code)
        {
            if (parDesc.ContainsKey(code))
                return parDesc[code].description + ", " + parDesc[code].uints;
            return "";
        }

        GrblParam GetParam(int parcode)
        {
            foreach (GrblParam par in parameters)
            {
                if (par.code == parcode)
                    return par;
                if (par.code > parcode)
                    break;
            }
            return null;
        }

        public GrblParam GetParam(GrblParam.Code parcode)
        {
            return GetParam((int)parcode);
        }

        public void ParseParam(string stparam)
        {
            string[] vars = stparam.Split(new char[] { '$', '=' }, StringSplitOptions.RemoveEmptyEntries);
            if (vars.Length != 2)
                return;
            int parcode;
            try
            {
                parcode = int.Parse(vars[0]);
            }
            catch
            {
                return;
            }
            // see if we already have this param
            GrblParam par = GetParam(parcode);
            if (par == null)
            {
                par = new GrblParam(parcode, vars[1]);
                parameters.Add(par);
                parameters.Sort();
            }
            else
                par.strVal = vars[1];

            // see if we know this val
            if (parDesc.ContainsKey(parcode))
            {
                ParamDescription pd = parDesc[parcode];
                par.type = pd.type;
                switch (pd.type)
                {
                    case ParamType.Float:
                        try { par.floatVal = Utils.ParseFloatInvariant(par.strVal); }
                        catch { }
                        break;
                    case ParamType.Int:
                    case ParamType.Mask:
                    case ParamType.Bool:
                        try { par.intVal = int.Parse(par.strVal); }
                        catch { }
                        break;
                }
                /*switch (pd.uints)
                {
                    case "mm":
                    case "mm/min":
                    case "RPM":
                    case "steps/mm":
                    case "steps/deg":
                    case "mm/deg":
                    case "mm/sec^2":
                    case "deg/sec^2":
                    case "percent":
                        par.type = ParamType.Float;
                        try { par.floatVal = Utils.ParseFloatInvariant(par.strVal); }
                        catch { }
                        break;
                    case "us":
                    case "ms":
                    case "int":
                        par.type = ParamType.Int;
                        try { par.intVal = int.Parse(par.strVal); }
                        catch { }
                        break;
                    case "mask":
                        par.type = ParamType.Mask;
                        try { par.intVal = int.Parse(par.strVal); }
                        catch { }
                        break;
                    case "bool":
                        par.type = ParamType.Boolean;
                        try { par.intVal = int.Parse(par.strVal); }
                        catch { }
                        break;
                }
                 */
            }
        }
        
    }
}
