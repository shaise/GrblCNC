using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrblCNC.Properties;

namespace GrblCNC
{
    public class GrblConfig
    {
        public enum ParamType
        {
            Float = 0,
            Int,
            Boolean,
            Mask,
            String // or unknown
        }

        public class GrblParam : IComparable
        {
            public int code;
            public string strVal;
            public int intVal;
            public float floatVal;
            public ParamType type;

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
                        case ParamType.Boolean:
                            result = val == "1" || val == "0" ? val : null;
                            break;
                        case ParamType.Float:
                            tfloat = float.Parse(val);
                            result = tfloat >= 0 ? tfloat.ToString("0.000") : null;
                            break;
                        case ParamType.Int:
                        case ParamType.Mask:
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

        Dictionary<int, string> parDesc;
        List<GrblParam> parameters;

        public GrblConfig()
        {
            InitParamDictionary();
            parameters = new List<GrblParam>();
        }
        
        void InitParamDictionary()
        {
            parDesc = new Dictionary<int, string>();
            foreach (string line in Resources.GrblParamDescription.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] codeName = line.Split(':');
                if (codeName.Length != 2)
                    continue;
                try { parDesc[int.Parse(codeName[0])] = codeName[1]; }
                catch { }
            }
        }

        public List<GrblParam> GetParams()
        {
            return parameters;
        }

        public string GetDescription(int code)
        {
            if (parDesc.ContainsKey(code))
                return parDesc[code];
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
                string desc = parDesc[parcode];
                string[] dvars = desc.Split(' ');
                switch (dvars[dvars.Length - 1])
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
                        try { par.floatVal = float.Parse(par.strVal); }
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
            }
        }
        
    }
}
