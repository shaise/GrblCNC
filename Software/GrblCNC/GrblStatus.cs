using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// holds and parses incomming Grbl status messages. 

namespace GrblCNC
{
    public class GrblStatus
    {
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
        public const int NUM_AXIS = 6;
        public float [] axisPos = new float[NUM_AXIS];
        public MachineState state;
        public float feedRate;
        public float spindleRpm;
        public bool dataValid;
        public string alarms = "";
        public int homeStatus;
        public int planBuffer;
        public int uartBuffer;
        public int lineNumber;
        // Note: whenever adding a new member, add it to clone function

        public GrblStatus Clone()
        {
            GrblStatus clone = new GrblStatus();
            for (int i = 0; i < axisPos.Length; i++) clone.axisPos[i] = axisPos[i];
            clone.state = state;
            clone.feedRate = feedRate;
            clone.spindleRpm = spindleRpm;
            clone.dataValid = dataValid;
            clone.alarms = alarms;
            clone.homeStatus = homeStatus;
            clone.planBuffer = planBuffer;
            clone.uartBuffer = uartBuffer;
            clone.lineNumber = lineNumber;
            return clone;
        }

        void ParseMachineState(string statestr)
        {
            if (!Enum.TryParse<MachineState>(statestr, out state))
                state = MachineState.Unknown;
        }

        void ParseAxisPosition(string apos)
        {
            string[] posVals = apos.Split(',');
            for (int i = 0; i < posVals.Length && i < NUM_AXIS; i++)
            {
                try { axisPos[i] = float.Parse(posVals[i]); }
                catch { axisPos[i] = 0;  }
            }
        }

        void ParseFeedSpindle(string rates)
        {
            string[] rateVals = rates.Split(',');
            if (rateVals.Length >= 2)
            {
                try
                {
                    feedRate = float.Parse(rateVals[0]);
                    spindleRpm = float.Parse(rateVals[1]);
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

        public void Parse(string statLine)
        {
            // first split to parts
            string[] statParts = statLine.Split(new char[] { '<', '>', '|' }, StringSplitOptions.RemoveEmptyEntries);
            alarms = "";
            homeStatus = 0x1F;
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
                    case "MPos": ParseAxisPosition(nameData[1]); break;
                    case "FS": ParseFeedSpindle(nameData[1]); break;
                    case "Pn": alarms = nameData[1]; break;
                    case "Hs": homeStatus = ParseInt(nameData[1]); break;
                    case "Bf": ParseBuffers(nameData[1]); break;
                    case "Ln": lineNumber = ParseInt(nameData[1]); break;
                }
            }
        }
    }
}
