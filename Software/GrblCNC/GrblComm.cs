using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;
using System.Globalization;
using GrblCNC.Properties;
using GrblCNC.Glutils;

namespace GrblCNC
{
    public class GrblComm
    {
        public const int X_AXIS = 0;
        public const int Y_AXIS = 1;
        public const int Z_AXIS = 2;
        public const int A_AXIS = 3;
        public const int B_AXIS = 4;
        public const int C_AXIS = 5;

        const byte CMD_SOFT_RESET = 0x18;
        const byte CMD_STATUS_REQUEST = (byte)'?';
        const byte CMD_CYCLE_START = (byte)'~';
        const byte CMD_FEED_HOLD = (byte)'!';
        const byte CMD_SAFETY_DOOR = 0x84;
        const byte CMD_JOG_CANCEL = 0x85;
        const byte CMD_FEED_SET_100 = 0x90;
        const byte CMD_FEED_ADD_10 = 0x91;
        const byte CMD_FEED_DEC_10 = 0x92;
        const byte CMD_FEED_ADD_1 = 0x93;
        const byte CMD_FEED_DEC_1 = 0x94;
        const byte CMD_RAPID_SET_100 = 0x95;
        const byte CMD_RAPID_SET_50 = 0x96;
        const byte CMD_RAPID_SET_25 = 0x97;
        const byte CMD_SPINDLE_SET_100 = 0x99;
        const byte CMD_SPINDLE_ADD_10 = 0x9A;
        const byte CMD_SPINDLE_DEC_10 = 0x9B;
        const byte CMD_SPINDLE_ADD_1 = 0x9C;
        const byte CMD_SPINDLE_DEC_1 = 0x9D;
        const byte CMD_TOGGLE_SPINDLE_STOP = 0x9E;
        const byte CMD_TOGGLE_FLOOD = 0xA0;
        const byte CMD_TOGGLE_MIST = 0xA1;

        public enum CommStatus
        {
            Disconnected = 0,
            Connected
        }
        public enum MachineState
        {
            Idle = 0,
            ParamReadAll,
            ParamReadSingle,
            Jog,
            StopJog,
            Running,
            CommandBatch,
            Paused
        }

        public enum ParamReadStage
        {
            ReadGrblParam = 0,
            ReadGcodeOffsets,
            ReadGcodeState,
            ReadEnd
        }

        public enum MessageType
        {
            Info,
            Error
        }
        string [] portNames;
        public string activePort;
        public string grblVersion;
        int scanPortIx;
        SerialPort port;
        bool portOpened;
        //static int PORT_BUFF_LEN = 256;
        static int SCAN_INTERVAL = 10;  // in 100ms units = 1 second
        GrblStatus grblStatus;
        GrblConfig grblConfig;
        GCodeConfig gcodeConfig;
        List<string> standardMsgQueue;
        List<string> urgentMsgQueue;
        Dictionary<string, string> grblErrorCodes;
        object lockMsgBufferObj = new object();
        object lockSerialSendObj = new object();
        int jogCount = 0;
        public MachineState machineState;
        public CommStatus ConnectionStatus;
        ParamReadStage paramReadStage;
        string curJogCommand;
        string lastError;
        int showStatusMsg = 0;
        int gStateCnt = 0;
        List<string> commandBatch;
        

        
        int scanCount;
        StringBuilder readLine;
        int tmpCnt=0; // Removeme
        string tmpLongLine; // Removeme
        int maxlinelen = 0;// Removeme
        // events
        public delegate void StatusChangedDelegate(object sender, CommStatus status);
        public event StatusChangedDelegate StatusChanged;
        public delegate void LineReceivedDelegate(object sender, string line, bool isStatus);
        public event LineReceivedDelegate LineReceived;
        public delegate void StatusUpdateDelegate(object sender, GrblStatus status);
        public event StatusUpdateDelegate StatusUpdate;
        public delegate void ParameterUpdateDelegate(object sender, GrblConfig grblConf, GCodeConfig gcodeConf);
        public event ParameterUpdateDelegate ParameterUpdate;
        public delegate void MessageReceivedDelegate(object sender, string message, MessageType type);
        public event MessageReceivedDelegate MessageReceived;
        

        public GrblComm()
        {
            portNames = null;
            scanPortIx = -1;
            activePort = null;
            portOpened = false;
            port = new SerialPort();
            readLine = new StringBuilder();
            grblStatus = new GrblStatus();
            grblConfig = new GrblConfig();
            gcodeConfig = new GCodeConfig();
            port.DataReceived += port_DataReceived;
            standardMsgQueue = new List<string>();
            urgentMsgQueue = new List<string>();
            commandBatch = new List<string>();
            paramReadStage = ParamReadStage.ReadEnd;
            ReadErrorCodes();
            scanCount = 0;
            Global.grblStatus = grblStatus;
        }

        void ReadErrorCodes()
        {
            grblErrorCodes = new Dictionary<string, string>();
            foreach (string line in Resources.GrblErrorCodes.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] codeName = line.Split(':');
                if (codeName.Length != 2)
                    continue;
                grblErrorCodes[codeName[0]] = codeName[1];
            }
        }

        public bool connectionActive
        {
            get { return activePort != null; }
        }


        void HandleReceivedLine(string line)
        {
            if (line.Length == 0)
                return;
            bool isStatusLine = false;
            if (line.Length > maxlinelen)
            {
                maxlinelen = line.Length;
                tmpLongLine = line;
            }
            if (line.StartsWith("Grbl"))
            {
                activePort = port.PortName;
                if (machineState == MachineState.Idle)
                    GetAllGrblParameters();
                //machineState = MachineState.Idle;
                if (ConnectionStatus != CommStatus.Connected)
                {
                    ConnectionStatus = CommStatus.Connected;
                    if (StatusChanged != null)
                        StatusChanged(this, ConnectionStatus);
                }
            }
            else if (line.StartsWith("<"))
            {
                isStatusLine = true;
                grblStatus.Parse(line);
                if (StatusUpdate != null)
                    StatusUpdate(this, grblStatus);
            }
            else if (line.StartsWith("$"))
                HandleParamLine(line);
            else if (line.StartsWith("ok"))
                HandleOKLine(line);
            else if (line.StartsWith("["))
                HandleMessageLine(line);
            else if (line.StartsWith("error"))
                HandleErrorLine(line);
            if (LineReceived != null)
            {
                LineReceived(this, line, isStatusLine && showStatusMsg == 0);
                if (showStatusMsg > 0)
                    showStatusMsg--;
            }
        }

        #region Input line handling
        void HandleErrorLine(string line)
        {
            // clear all buffers and stop running
            commandBatch.Clear();
            urgentMsgQueue.Clear();
            standardMsgQueue.Clear();
            StopGcode();
            // clear grbl error state by sending a dummy help request.
            PostLine("$");
            lastError = line.Substring(6);
            if (grblErrorCodes.ContainsKey(lastError))
                lastError = grblErrorCodes[lastError];
            if (MessageReceived != null)
                MessageReceived(this, lastError, MessageType.Error);
        }

        void HandleParamLine(string line)
        {
            grblConfig.ParseParam(line);
        }

        void HandleReadParam()
        {
            if (paramReadStage >= ParamReadStage.ReadEnd)
                return;
            paramReadStage++;
            switch (paramReadStage)
            {
                case ParamReadStage.ReadGcodeOffsets:
                    GetGcodeCoordOfsets();
                    break;

                case ParamReadStage.ReadGcodeState:
                    GetGCodeParserState();
                    break;

                case ParamReadStage.ReadEnd:
                    if (ParameterUpdate != null)
                        ParameterUpdate(this, grblConfig, gcodeConfig);
                    machineState = MachineState.Idle;
                    break;
            }
        }

        void HandleReadSingleParam()
        {
            if (ParameterUpdate != null)
                ParameterUpdate(this, grblConfig, gcodeConfig);
            machineState = MachineState.Idle;
        }

        void HandleJogInProgress()
        {
            SendLine(curJogCommand);
        }

        void HandleStopJog()
        {
            SendByte(CMD_JOG_CANCEL);
            machineState = MachineState.Idle;
        }

        void HandleCommandBatch()
        {
            if (commandBatch.Count == 0)
                machineState = MachineState.Idle;
            else
            {
                PostLine(commandBatch[0]);
                commandBatch.RemoveAt(0);
            }
        }

        void HandleOKLine(string line)
        {
            switch (machineState)
            {
                case MachineState.ParamReadAll: HandleReadParam(); break;
                case MachineState.ParamReadSingle: HandleReadSingleParam(); break;
                case MachineState.Jog: HandleJogInProgress(); break;
                case MachineState.StopJog: HandleStopJog(); break;
                case MachineState.Running: SendCurrentGcodeLine(); break;
                case MachineState.CommandBatch: HandleCommandBatch(); break;
            }
        }

        void HandleMessageLine(string line)
        {
            //machineState = MachineState.ParamRead;
            //paramReadStage = ParamReadStage.ReadGcodeOffsets;
            string[] vars = line.Split(new char[] { '[', ']', ':' }, StringSplitOptions.RemoveEmptyEntries);
            if (vars.Length != 2)
                return;
            // first check if this is a gcode parameter 
            if (gcodeConfig.ParseParam(vars[0], vars[1]))
                return;
            switch(vars[0])
            {
                case "GC":
                    grblStatus.ParseGState(vars[1]);
                    if (StatusUpdate != null)
                        StatusUpdate(this, grblStatus);
                    break;

                case "MSG":
                    if (MessageReceived != null)
                        MessageReceived(this, vars[1], MessageType.Info);
                    break;

            }
        }


        #endregion

        void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort port = (SerialPort)sender;
            
            while (port.BytesToRead != 0)
            {
                char ch = (char)port.ReadChar();
                if (ch == '\r')
                    continue;
                if (ch == '\n')
                {
                    HandleReceivedLine(readLine.ToString());
                    readLine.Clear();
                    continue;
                }
                readLine.Append(ch);
            }
        }
        
        void SendLine(string line)
        {
            if (!portOpened)
                return;
            if (line[0] == '?')
            {   // special case - we want to see the next status request
                showStatusMsg++;
                return;
            }
            line += "\n";
            Global.mdiControl.AddLine(line);
            lock (lockSerialSendObj)
            {
                port.Write(line);
            }
        }

        void SendByte(byte b)
        {
            if (!portOpened)
                return;
            byte[] msg = new byte[1];
            msg[0] = b;
            if (ConnectionStatus == CommStatus.Connected && b != CMD_STATUS_REQUEST)
                Global.mdiControl.AddLine(string.Format("<0x{0:X}>", (int)b));
            lock (lockSerialSendObj)
            {
                port.Write(msg, 0, 1);
            }
        }
        
        void SendSoftReset()
        {
            SendByte(CMD_SOFT_RESET);
        }

        // add line to send queue. Lines will be send through the poller
        public void PostLine(string line, bool isurgent = false)
        {
            lock(lockMsgBufferObj)
            {
                if (isurgent)
                    urgentMsgQueue.Add(line);
                else
                    standardMsgQueue.Add(line);
            }
        }

        // post a series of lines one after the other, waiting for 'OK' from line to line, stop on error
        public void PostLines(string [] lines)
        {
            if (lines == null || lines.Length == 0)
                return;
            if (machineState != MachineState.Idle && machineState != MachineState.CommandBatch)
                return;
            commandBatch.AddRange(lines);
            if (machineState != MachineState.CommandBatch)
            {
                machineState = MachineState.CommandBatch;
                PostLine(commandBatch[0]);
                commandBatch.RemoveAt(0);
            }
        }

        // purge any queued gcode commands 
        public void PurgeMessages()
        {
            urgentMsgQueue.Clear();
            standardMsgQueue.Clear();
        }

        bool OpenPort(string portName)
        {
            if (portOpened)
                ClosePort();
            try
            {
                port.PortName = portName;
                //serPort.BaudRate = 57600;
                port.BaudRate = 115200;
                port.Parity = Parity.None;
                port.DataBits = 8;
                port.StopBits = StopBits.One;
                port.Handshake = Handshake.None;
                port.Open();
                portOpened = true;
                SendSoftReset();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void ClosePort()
        {
            if (!portOpened)
                return;
            try { port.Close(); }
            catch { } // sometimes when the usb disconnects, closing the port causes error
            portOpened = false;
            activePort = null;
        }

        bool SendPendingLines()
        {
            string line = null;

            lock (lockMsgBufferObj)
            {
                if (urgentMsgQueue.Count > 0)
                {

                    line = urgentMsgQueue[0];
                    urgentMsgQueue.RemoveAt(0);
                }
                else if (standardMsgQueue.Count > 0)
                {
                    line = standardMsgQueue[0];
                    standardMsgQueue.RemoveAt(0);
                }
            }
            if (line == null)
                return false;
            SendLine(line);
            return true;
        }

        void SendStatusRequest()
        {
            if (!connectionActive /*|| machineState == MachineState.Jog*/)
                return;
            try
            {
                SendByte(CMD_STATUS_REQUEST);
            }
            catch 
            {
                ClosePort();
                ConnectionStatus = CommStatus.Disconnected;
                if (StatusChanged != null)
                    StatusChanged(this, ConnectionStatus);
            }

        }

        // Grbl communication main engine. Should be called 10 times a second. 
        public void CommPoll()
        {
            if (connectionActive)
            {
                // send any pending lines
                SendPendingLines();
                SendStatusRequest(); // send periodic status requests
                return;
            }

            scanCount++;
            if (scanCount < SCAN_INTERVAL)
                return;

            scanCount = 0;
            if (scanPortIx == -1)
            {
                portNames = SerialPort.GetPortNames();
                // Fixme: for linux we can filter the names to USB/Serial ports only
                scanPortIx = portNames.Length - 1;
            }
            if (scanPortIx < 0)
                return; // empty list

            while (scanPortIx >= 0)
            {
                if (OpenPort(portNames[scanPortIx]))
                {
                    scanPortIx--;
                    break;
                }
                scanPortIx--;
            }

        }

        public void Close()
        {
            ClosePort();
        }

        #region Grbl control commands
        public void GetAllGrblParameters()
        {
            GetGrblParameters();
            machineState = MachineState.ParamReadAll;
            paramReadStage = ParamReadStage.ReadGrblParam;
        }

        public void GetGrblParameters()
        {
            if (machineState == MachineState.Idle)
                machineState = MachineState.ParamReadSingle;
            PostLine("$$");
        }

        public void GetGcodeCoordOfsets()
        {
            if (machineState == MachineState.Idle)
                machineState = MachineState.ParamReadSingle;
            PostLine("$#");
        }

        public void GetGCodeParserState()
        {
            if (machineState == MachineState.Idle)
                machineState = MachineState.ParamReadSingle;
            PostLine("$G");
        }

        public void SetGrblParameter(int code, string value)
        {
            if (!connectionActive)
                return;
            string cmd = string.Format("${0}={1}", code, value);
            PostLine(cmd);
        }

        public void HomeAxis(int axis)
        {
            if (axis == 10)
            {
                PostLine("$J=G91 X10 F600\n!");
                return;
            }
            string axisLetter = Utils.GetAxisLetter(axis);
            if (axisLetter == null)
                return;
            string cmd = string.Format("$H{0}", axisLetter);
            PostLine(cmd);
        }

        public void CoordTouchAxis(int axis, int coordSystemIx, float offset)
        {
            if (coordSystemIx < 0 || coordSystemIx > 8)
                return;
            string axisLetter = Utils.GetAxisLetter(axis);
            if (axisLetter == null)
                return;
            string cmd = string.Format("G10 L20 P{0} {1}{2}", coordSystemIx + 1, axisLetter, offset);
            PostLine(cmd);
        }

        public void ProbeAxis(int axis, int coordSystemIx, float offset, float dir)
        {
            if (coordSystemIx < -1 || coordSystemIx > 8)
                return;
            string axisLetter = Utils.GetAxisLetter(axis);
            if (axisLetter == null)
                return;
            string[] cmdbatch = new string[4];
            cmdbatch[0] = string.Format("G10 L20 P0 {0}0", axisLetter);
            cmdbatch[1] = string.Format("G38.2 {0}{1} F25", axisLetter, 10f * dir);
            cmdbatch[2] = string.Format("G10 L20 P0 {0}{1}", axisLetter, offset);
            cmdbatch[3] = string.Format("G0 {0}{1}", axisLetter, Math.Floor(offset - 5.5 * dir));
            PostLines(cmdbatch);
        }

        public void StepJog(int axis, float dist, float feedrate)
        {
            string axisLetter = Utils.GetAxisLetter(axis);
            if (axisLetter == null)
                return;
            string cmd = string.Format("$J=G91 {0}{1:0.000} F{2:0.0}", axisLetter, dist, feedrate);
            PostLine(cmd);
            jogCount++;
        }

        public void ContinuesJog(int axis, float dir, float feedrate)
        {
            if (dir == 0)
            {
                // stop jogging
                //machineState = MachineState.StopJog;
                machineState = MachineState.Idle;
                SendByte(CMD_JOG_CANCEL);
                return;
            }

            if (machineState != MachineState.Idle)
                return; // busy or already jogging

            tmpCnt++;
            // start jogging
            string axisLetter = Utils.GetAxisLetter(axis);
            if (axisLetter == null)
                return;
            PurgeMessages();
            float s = dir * 0.025f * feedrate / 60;
            //s = 2 * dir;
            curJogCommand = string.Format(CultureInfo.InvariantCulture, "$J=G91 {0}{1:0.000} F{2:0.0}", axisLetter, s, feedrate);
            SendLine(curJogCommand);
            machineState = MachineState.Jog;
        }

        public void EmergencyStop()
        {
            SendByte(CMD_SAFETY_DOOR);
        }

        public void AlarmRelease()
        {
            PostLine("$X");
        }
        #endregion

        #region Gcode Sender

        public void SendGcode()
        {
            if (ConnectionStatus != CommStatus.Connected || Global.ginterp == null || Global.ginterp.lines == null)
                return;
            if (machineState == GrblComm.MachineState.Idle)
            {
                Global.ginterp.ResetGcodeLine();
                machineState = MachineState.Running;
                SendCurrentGcodeLine();
            }
            else if (machineState == GrblComm.MachineState.Paused)
                ResumeGcode();
        }

        void StopSendingGcode()
        {
            machineState = MachineState.Idle;
        }

        void SendCurrentGcodeLine()
        {
            if (Global.ginterp == null || Global.ginterp.lines == null)
            {
                StopSendingGcode();
                return;
            }
            string curLine = Global.ginterp.GetNextCompiledLine();
            if (curLine == null)
            {
                StopSendingGcode();
                return;
            }
            SendLine(curLine);
        }

        public void PauseGcode()
        {
            if (machineState != MachineState.Running)
                return;
            machineState = MachineState.Paused;
            SendByte(CMD_FEED_HOLD);
        }

        public void ResumeGcode()
        {
            if (machineState != MachineState.Paused)
                return;
            machineState = MachineState.Running;
            SendByte(CMD_CYCLE_START);
        }

        public void StopGcode()
        {
            if (machineState == MachineState.Running || machineState == MachineState.Paused)
            {
                if (machineState == MachineState.Running)
                    PauseGcode();
                SendByte(CMD_SOFT_RESET);
            }
            if (Global.ginterp != null)
                Global.ginterp.ResetGcodeLine();
            machineState = MachineState.Idle;
        }

        public void StepGcode()
        {
            if (machineState != MachineState.Idle)
                return;
            if (Global.ginterp == null || Global.ginterp.lines == null)
                return;
            string curLine = Global.ginterp.GetNextCompiledLine();
            if (curLine != null)
                PostLine(curLine);
        }
        #endregion
    }
}
