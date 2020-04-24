using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrblCNC.Glutils;
using GrblCNC.Properties;

// holds and parses incomming Grbl status messages. 

namespace GrblCNC
{
    public class GrblStatus
    {

        public class GcodeParserState
        {
            public string name;
            public string [] states;
            public GcodeParserState(string name, string[] states)
            {
                this.name = name;
                this.states = states;
            }
            public bool HaveCode(string code)
            {
                return states.Contains(code);
            }
            public int GetIndex(string code)
            {
                for (int i = 0; i < states.Length; i++)
                    if (code == states[i])
                        return i;
                return -1;
            }
            public string GetCode(int code_ix)
            {
                if (code_ix < 0 || code_ix > states.Length)
                    return null;
                return states[code_ix];
            }
        }

        public enum MachineState
        {
            Idle = 0,
            Run, 
            Hold, 
            Jog, 
            Alarm, 
            Door, 
            Check, 
            Home, 
            Sleep,
            Unknown
        }

        public enum GcodeParserStateNames
        {
            MotionMode,         // G0,G1,G2,G3,G38.2,G80
            FeedMode,           // G93,G94
            UnitsMode,          // G20,G21
            DistanceMode,       // G90,G91
            LatheDiameterMode,  // G7,G8
            PlaneSelect,        // G17,G18,G19
            ToolOfsetMode,      // G43,G43.1,G49
            CoordinateSystem,   // G54,G55,G56,G57,G58,G59,G59.1,G59.2,G59.3
            ProgramState,       // M0,M1,M2,M30
            CoolantMode,        // M7,M8,M9
            SpindleState,       // M3,M4,M5
            GCodeOverrides,     // M48,M49,M50,M51,M53,M56
            SpindleRPMMode,     // G96,G97
            CannedRetractMode,  // G98,G99
            ScalingMode,        // G50,G51
            PathMode,           // G61
            ArcDistanceMode,    // G91.1
            CutterCompensation, // G40
            FeedRate,            // F0
            SpindleRPM           // S0
        }

        public const int NUM_AXIS = 5;
        public float [] axisPos = new float[NUM_AXIS];
        public float[] workingCoords = new float[NUM_AXIS];
        public MachineState state;
        public float feedRate;
        public float spindleRpm;
        public bool dataValid;
        public string alarms = "";
        public int homeStatus;
        public int planBuffer;
        public int uartBuffer;
        public int lineNumber;
        public int alarmCode;
        public string [] gState;
        public bool gStateChange;
        // Note: whenever adding a new public member, add it to clone function

        static GcodeParserState[] gcodeStatesDict = new GcodeParserState[] {
            new GcodeParserState ( "Motion Mode", new string [] { "G0", "G1", "G2", "G3", "G38.2", "G80" }),
            new GcodeParserState ( "Feed Mode", new string [] { "G93", "G94" }),
            new GcodeParserState ( "Units Mode", new string [] { "G20", "G21" }),
            new GcodeParserState ( "Distance Mode", new string [] { "G90", "G91" }),
            new GcodeParserState ( "Lathe Diameter Mode", new string [] { "G7", "G8" }),
            new GcodeParserState ( "Plane Select", new string [] { "G17", "G18", "G19" }),
            new GcodeParserState ( "Tool Ofset Mode", new string [] { "G43", "G43.1", "G49" }),
            new GcodeParserState ( "Coordinate System", new string [] { "G54", "G55", "G56", "G57", "G58", "G59", "G59.1", "G59.2", "G59.3" }),
            new GcodeParserState ( "Program State", new string [] { "M0", "M1", "M2", "M30" }),
            new GcodeParserState ( "Coolant Mode", new string [] { "M7", "M8", "M9" }),
            new GcodeParserState ( "Spindle State", new string [] { "M3", "M4", "M5" }),
            new GcodeParserState ( "GCode Overrides", new string [] { "M48", "M49", "M50", "M51", "M53", "M56" }),
            new GcodeParserState ( "Spindle RPM Mode", new string [] { "G96", "G97" }),
            new GcodeParserState ( "Canned Retract Mode", new string [] { "G98", "G99" }),
            new GcodeParserState ( "Scaling Mode", new string [] { "G50", "G51" }),
            new GcodeParserState ( "Path Mode", new string [] { "G61" }),
            new GcodeParserState ( "Arc Distance Mode", new string [] { "G91.1" }),
            new GcodeParserState ( "Cutter Compensation", new string [] { "G40" }),
            new GcodeParserState ( "Feed Rate", new string [] { "F0" }),
            new GcodeParserState ( "Spindle RPM", new string [] { "S0" })
        };

        public GrblStatus Clone()
        {
            GrblStatus clone = new GrblStatus();
            for (int i = 0; i < axisPos.Length; i++) clone.axisPos[i] = axisPos[i];
            for (int i = 0; i < axisPos.Length; i++) clone.workingCoords[i] = workingCoords[i];
            clone.state = state;
            clone.feedRate = feedRate;
            clone.spindleRpm = spindleRpm;
            clone.dataValid = dataValid;
            clone.alarms = alarms;
            clone.homeStatus = homeStatus;
            clone.planBuffer = planBuffer;
            clone.uartBuffer = uartBuffer;
            clone.lineNumber = lineNumber;
            clone.alarmCode = alarmCode;
            clone.gState = gState;
            clone.gStateChange = gStateChange;
            return clone;
        }

        void ParseMachineState(string statestr)
        {
            string[] vars = statestr.Split(':');
            if (!Enum.TryParse<MachineState>(vars[0], out state))
                state = MachineState.Unknown;
            if (vars.Length > 1)
            {
                try { alarmCode = int.Parse(vars[1]); }
                catch { }
            }
        }

        void ParseAxisPosition(string apos, float [] vals)
        {
            string[] posVals = apos.Split(',');
            for (int i = 0; i < posVals.Length && i < NUM_AXIS; i++)
            {
                try { vals[i] = Utils.ParseFloatInvariant(posVals[i]); }
                catch { vals[i] = 0;  }
            }
        }

        void ParseFeedSpindle(string rates)
        {
            string[] rateVals = rates.Split(',');
            if (rateVals.Length >= 2)
            {
                try
                {
                    feedRate = Utils.ParseFloatInvariant(rateVals[0]);
                    spindleRpm = Utils.ParseFloatInvariant(rateVals[1]);
                }
                catch { }
            }
        }

        void ParseBuffers(string buffs)
        {
            string[] buffVals = buffs.Split(',');
            if (buffVals.Length >= 2)
            {
                try
                {
                    planBuffer = int.Parse(buffVals[0]);
                    uartBuffer = int.Parse(buffVals[1]);
                }
                catch { }
            }
        }

        int ParseInt(string stnum)
        {
            int res = 0;
            try { res = int.Parse(stnum); }
            catch { }
            return res;
        }

        int GetParserStateGroup(string code)
        {
            for (int i = 0; i < gcodeStatesDict.Length; i++)
                if (gcodeStatesDict[i].HaveCode(code))
                    return i;
            return -1;
        }

        public void ParseGState(string line)
        {
            gStateChange = true;

            gState = new string[gcodeStatesDict.Length];
            // fill with defaults
            for (int i = 0; i < gState.Length; i++)
                gState[i] = gcodeStatesDict[i].states[0];

            string [] vars = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string var in vars)
            {
                if (var[0] == 'S')
                    gState[(int)GcodeParserStateNames.SpindleRPM] = var;
                else if (var[0] == 'F')
                    gState[(int)GcodeParserStateNames.FeedRate] = var;
                else
                {
                    int ix = GetParserStateGroup(var);
                    if (ix >= 0)
                        gState[ix] = var;
                }
            }
        }

        public void Parse(string statLine)
        {
            // first split to parts
            string[] statParts = statLine.Split(new char[] { '<', '>', '|' }, StringSplitOptions.RemoveEmptyEntries);
            alarms = "";
            gStateChange = false;
            //homeStatus = 0x1F;
            for (int i = 0; i < statParts.Length; i++)
            {
                string statPart = statParts[i];
                if (i == 0) // first token is machine state
                {
                    ParseMachineState(statPart);
                    continue;
                }
                string[] nameData = statPart.Split(':');
                if (nameData.Length != 2)
                    continue;  // not valid part
                switch (nameData[0])
                {
                    case "WPos":
                    case "MPos": ParseAxisPosition(nameData[1], axisPos); break;
                    case "WCO": ParseAxisPosition(nameData[1], workingCoords); break;
                    case "FS": ParseFeedSpindle(nameData[1]); break;
                    case "Pn": alarms = nameData[1]; break;
                    case "H": homeStatus = ParseInt(nameData[1]); break;
                    case "Bf": ParseBuffers(nameData[1]); break;
                    case "Ln": lineNumber = ParseInt(nameData[1]); break;
                }
            }
        }

        public string CurrentCoordSystem
        {
            get { return gState[(int)GcodeParserStateNames.CoordinateSystem];  }
        }

        public int CurrentCoordystemIndex
        {
            get
            {
                return gcodeStatesDict[(int)GcodeParserStateNames.CoordinateSystem].GetIndex(CurrentCoordSystem);
            }
        }
    }
}
