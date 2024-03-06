using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrblCNC.Properties;
using GrblCNC.Glutils;
using static System.Windows.Forms.LinkLabel;

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
                HomingOption = 22,
                MaxSpindleSpeedCode = 30,
                MinSpindleSpeedCode = 31,
                MaxXaxisRate = 110,
            }

            public const int HomingOptionEnable = 1;
            public const int HomingOptionSingleAxis = 2;

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
            public string units;
            public ParamType type;
            public string [] groups;
            public string[] options;
            public decimal min;
            public decimal max;
            public bool minValid;
            public bool maxValid;
            public int decimalPaces;
            public bool rebootRequired;
            public bool allowNull;
        }

        public class ParamGroupDesc
        {
            public int code;
            public int parentCode;
            public string name;
        }

        Dictionary<int, ParamDescription> parDesc;
        List<GrblParam> parameters;
        Dictionary<int, ParamGroupDesc> groupNames;
        public bool ResetParams = true;
        public bool NewConfiguration = false;

        public GrblConfig()
        {
            InitParamDictionary();
            ReadParamDescription();
        }

        void InitParamDictionary()
        {
            parDesc = new Dictionary<int, ParamDescription>();
            parameters = new List<GrblParam>();
            groupNames = new Dictionary<int, ParamGroupDesc>();
            NewConfiguration = true;
        }

        void UpdateMinMax(ParamDescription pd)
        {
            pd.minValid = pd.maxValid = false;
            pd.decimalPaces = 3;
            if (pd.options == null)
                return;
            if (pd.type != ParamType.Int && pd.type != ParamType.Float)
                return;
            try
            {
                if (pd.options.Length > 0)
                {
                    pd.min = decimal.Parse(pd.options[0]);
                    pd.minValid = true;
                }
                if (pd.options.Length > 1)
                {
                    pd.max = decimal.Parse(pd.options[1]);
                    pd.maxValid = true;
                }
                if (pd.options.Length > 2)
                    pd.decimalPaces = int.Parse(pd.options[2]);
            }
            catch { }
        }

        void ReadParamDescription()
        { 
            foreach (string line in Resources.GrblParamDescription.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] vars = line.Split('|');
                if (line[0] == '#' || vars.Length < 4)
                    continue;
                try {
                    int code = int.Parse(vars[0]);
                    ParamDescription pd = new ParamDescription();
                    pd.code = code;
                    pd.description = string.Format("{0} [{1}]", vars[1], code);
                    pd.units = vars[2];
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
                    UpdateMinMax(pd);
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
                return parDesc[code].description + ", " + parDesc[code].units;
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

        public void ParseGroup(string stgroup)
        {
            if (ResetParams)
            {
                InitParamDictionary();
                ResetParams = false;
            }
            // group format: <id>|<parent id>|<name>
            try
            {
                string[] vars = stgroup.Split('|');
                ParamGroupDesc paramGroupDesc = new ParamGroupDesc();
                paramGroupDesc.code = int.Parse(vars[0]);
                paramGroupDesc.parentCode = int.Parse(vars[1]);
                paramGroupDesc.name = vars[2];
                groupNames[paramGroupDesc.code] = paramGroupDesc;
            }
            catch { }
        }

        ParamType ParseParamType(string type)
        {
            switch (type)
            {
                case "0": return ParamType.Bool;
                case "1":
                case "2":
                case "4": return ParamType.Mask;
                case "3": return ParamType.Selection;
                case "5": return ParamType.Int;
                case "6": return ParamType.Float;
                case "7":
                case "8":
                case "9": return ParamType.String;   
            }
            return ParamType.String;
        }

        string GetGroupName(string stgroupid)
        {
            try
            {
                int grpid = int.Parse(stgroupid);
                if (groupNames.ContainsKey(grpid))
                    return groupNames[grpid].name;
            }
            catch { }
            return "General";

        }

        public void ParrseParamDescription(string line)
        {
            // param description format: <id>|<group id>|<name>|{<unit>}|<data type>|{<format}|{<min>}|{<max>}|{reboot required}|{allow null}
            string[] vars = line.Split('|');
            if (line[0] == '#' || vars.Length < 4)
                return;
            try
            {
                int code = int.Parse(vars[0]);
                ParamDescription pd = new ParamDescription();
                pd.code = code;
                pd.description = string.Format("{0} [{1}]", vars[2], code);
                pd.units = vars[3];
                pd.type = ParseParamType(vars[4]);
                pd.groups = new string[1];
                pd.groups[0] = GetGroupName(vars[1]);
                if (vars[4] == "4") // axis bit mask
                    pd.options = Global.GetAxesString().Split('|');
                else
                    pd.options = vars[5].Replace("N/A", "").Split(',');
                pd.minValid = pd.maxValid = false;
                if (vars.Length > 7)
                {
                    if (vars[6] != "")
                    {
                        pd.min = decimal.Parse(vars[6]);
                        pd.minValid = true;
                    }
                    if (vars[7] != "")
                    {
                        pd.max = decimal.Parse(vars[7]);
                        pd.maxValid = true;
                    }
                }
                if (vars.Length > 9)
                {
                    pd.rebootRequired = vars[8] == "1";
                    pd.allowNull = vars[9] == "1";
                }
                parDesc[code] = pd;
            }
            catch { }

        }

        public void ParseParam(string stparam)
        {
            string[] vars = stparam.Split(new char[] { '$', '=' }, StringSplitOptions.RemoveEmptyEntries);
            if (vars.Length == 1 && stparam.EndsWith("="))
            {
                string p1 = vars[0];
                vars = new string[2];
                vars[0] = p1;
                vars[1] = "";
            }
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
                    case ParamType.Selection:
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
