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
        const byte CMD_STOP = 0x19;
        const byte CMD_STATUS_REQUEST = (byte)'?';
        const byte CMD_CYCLE_START = (byte)'~';
        const byte CMD_FEED_HOLD = (byte)'!';
        const byte CMD_SAFETY_DOOR = 0x84;
        const byte CMD_JOG_CANCEL = 0x85;
        const byte CMD_STATUS_REQUEST_FULL = 0x87;
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

        const int MIN_LOCATION = -999999;

        public enum CommStatus
        {
            Disconnected = 0,
            Connected
        }
        public enum MachineState
        {
            Idle = 0,
            Jog,
            StopJog,
            Running,
            Stopping,
            ToolChange,
            CommandBatch,
            Paused,
            ProbeTool,
            ProbeAxis,
        }

        public enum GrblParamTypes
        {
            GrblSysParams = 0,
            ReadGcodeOffsets,
            GcodeStates,

            NumParamTypes // must be last
        }

        public enum MessageType
        {
            Info,
            Error,
            Alarm
        }

        public enum SpindleAction
        {
            Stop = 0,
            StartCW,
            StartCCW,
            Speed,
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
        List<string> insertMsgQueue; // when not empty, will be inserted to current running gcode
        Dictionary<string, string> grblErrorCodes;
        Dictionary<string, string> grblAlarmCodes;
        object lockMsgBufferObj = new object();
        object lockSerialSendObj = new object();
        int jogCount = 0;
        public MachineState machineState;
        public CommStatus ConnectionStatus;
        bool [] isParamUpdated = new bool[(int)GrblParamTypes.NumParamTypes];
        string curJogCommand;
        string lastError;
        int showStatusMsg = 0;
        int gStateCnt = 0;
        List<string> commandBatch;
        int lastTool = -1;
        bool wasRunningWhenToolChanged = false;
        public bool debugSending = true;
        float [] tcpAxisPos;
        float tcpZmax;

        float probeToolOffset;
        int probeToolNum;
        int probeToolAxis;
        float probeDir;
        bool lastG90State;
        bool requestFullStatus = false;
        
        int scanCount;
        StringBuilder readLine;
        int tmpCnt=0; // Removeme
        string tmpLongLine; // Removeme
        int maxlinelen = 0;// Removeme
        // events
        public delegate void CommStatusChangedDelegate(object sender, CommStatus status);
        public event CommStatusChangedDelegate CommStatusChanged;
        public delegate void GrblStatusChangedDelegate(object sender, GrblStatus.MachineState newState, GrblStatus.MachineState oldState);
        public event GrblStatusChangedDelegate GrblStatusChanged;
        public delegate void LineReceivedDelegate(object sender, string line, bool isStatus);
        public event LineReceivedDelegate LineReceived;
        public delegate void StatusUpdateDelegate(object sender, GrblStatus status);
        public event StatusUpdateDelegate StatusUpdate;
        public delegate void ParameterUpdateDelegate(object sender, GrblConfig grblConf, GCodeConfig gcodeConf);
        public event ParameterUpdateDelegate ParameterUpdate;
        public delegate void MessageReceivedDelegate(object sender, string message, MessageType type);
        public event MessageReceivedDelegate MessageReceived;
        public delegate void ChangeToolNotifyDelegate(object sender, int newTool, bool isRunning);
        public event ChangeToolNotifyDelegate ChangeToolNotify;
        

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
            //urgentMsgQueue = new List<string>();
            insertMsgQueue = new List<string>();
            commandBatch = new List<string>();
            ReadErrorCodes();
            scanCount = 0;
            Global.grblStatus = grblStatus;
            Global.grblConfig = grblConfig;
            tcpAxisPos = new float[Global.NUM_AXIS];
        }

        void FillCodes(Dictionary<string, string> dict, string codes)
        {
            foreach (string line in codes.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] codeName = line.Split(':');
                if (codeName.Length != 2)
                    continue;
                dict[codeName[0]] = codeName[1];
            }
        }

        void ReadErrorCodes()
        {
            grblErrorCodes = new Dictionary<string, string>();
            FillCodes(grblErrorCodes, Resources.GrblErrorCodes);
            grblAlarmCodes = new Dictionary<string, string>();
            FillCodes(grblAlarmCodes, Resources.GrblAlarmCodes);
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
                    grblStatus.state = GrblStatus.MachineState.Unknown;
                    if (CommStatusChanged != null)
                        CommStatusChanged(this, ConnectionStatus);
                }
                requestFullStatus = true;
            }
            else if (line.StartsWith("<"))
            {
                isStatusLine = true;
                HandleStatusLine(line);
            }
            else if (line.StartsWith("$"))
                HandleParamLine(line);
            else if (line.StartsWith("ok"))
                HandleOKLine(line);
            else if (line.StartsWith("["))
                HandleMessageLine(line);
            else if (line.StartsWith("error"))
                HandleErrorLine(line, MessageType.Error);
            else if (line.StartsWith("ALARM"))
                HandleErrorLine(line, MessageType.Alarm);
            if (LineReceived != null)
            {
                LineReceived(this, line, isStatusLine && showStatusMsg == 0);
                if (isStatusLine && (showStatusMsg > 0))
                    showStatusMsg--;
            }
        }

        void ReportUpdatedParams()
        {
            if (isParamUpdated[(int)GrblParamTypes.GrblSysParams] || isParamUpdated[(int)GrblParamTypes.ReadGcodeOffsets])
            {
                isParamUpdated[(int)GrblParamTypes.GrblSysParams] = isParamUpdated[(int)GrblParamTypes.ReadGcodeOffsets] = false;
                    if (ParameterUpdate != null)
                        ParameterUpdate(this, grblConfig, gcodeConfig);
            }
            if (isParamUpdated[(int)GrblParamTypes.GcodeStates])
            {
                isParamUpdated[(int)GrblParamTypes.GcodeStates] = false;
                if (StatusUpdate != null)
                    StatusUpdate(this, grblStatus);
            }

        }

        #region Input line handling
        void HandleStatusLine(string line)
        {
            GrblStatus.MachineState oldState = grblStatus.state;
            float[] axis = new float[grblStatus.axisPos.Length];
            for (int i = 0; i < axis.Length; i++)
                axis[i] = grblStatus.axisPos[i];

            grblStatus.Parse(line);
            if (StatusUpdate != null)
                StatusUpdate(this, grblStatus);
            if (oldState != grblStatus.state)
                HandleStateChange(oldState);

            if (wasRunningWhenToolChanged)
            {
                float absZ = grblStatus.axisPos[Z_AXIS] + grblStatus.workingCoords[Z_AXIS];
                if (absZ > tcpZmax)
                    tcpZmax = absZ;
            }

            // handle stopping, if in action. Fixme: need to find a better way to do it.
            if (machineState == MachineState.Stopping && grblStatus.state != GrblStatus.MachineState.Run)
            {
                bool motionStopped = true;
                for (int i = 0; i < axis.Length; i++)
                    if (axis[i] != grblStatus.axisPos[i])
                    {
                        motionStopped = false;
                        break;
                    }
                if (motionStopped)
                    SendStop();
            }
            if (machineState == MachineState.ToolChange && grblStatus.state != GrblStatus.MachineState.Run)
                ChangeTool();
        }

        void HandleStateChange(GrblStatus.MachineState oldState)
        {
            if (grblStatus.state == GrblStatus.MachineState.Hold && machineState != MachineState.Paused && machineState != MachineState.Stopping)
            {
                machineState = MachineState.Paused;
            }
            if (GrblStatusChanged != null)
                GrblStatusChanged(this, grblStatus.state, oldState);
            if (grblStatus.state == GrblStatus.MachineState.Idle)
            {
                if (machineState == MachineState.ProbeTool)
                    ProbeToolEnd();
                if (machineState == MachineState.ProbeAxis && insertMsgQueue.Count == 0)
                    GetGcodeCoordOfsets();
            }
        }

        void HandleErrorLine(string line, MessageType msgType)
        {
            // clear all buffers and stop running
            commandBatch.Clear();
            //urgentMsgQueue.Clear();
            insertMsgQueue.Clear();
            standardMsgQueue.Clear();
            StopGcode();

            lastError = line;
            string errcode = line.Substring(6);
            if (msgType == MessageType.Error)
            {
                PostLine("$"); // clear grbl error state by sending a dummy help request.
                if (grblErrorCodes.ContainsKey(errcode))
                    lastError = grblErrorCodes[errcode];
            }
            else
            {
                if (grblAlarmCodes.ContainsKey(errcode))
                    lastError = grblAlarmCodes[errcode];
            }

            if (MessageReceived != null)
                MessageReceived(this, lastError, msgType);
        }

        void HandleParamLine(string line)
        {
            grblConfig.ParseParam(line);
            isParamUpdated[(int)GrblParamTypes.GrblSysParams] = true;
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
            {
                machineState = MachineState.Idle;
                ReportUpdatedParams();
            }
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
                case MachineState.Idle:
                    if (insertMsgQueue.Count > 0)
                        SendCurrentGcodeLine();
                    else
                        ReportUpdatedParams(); 
                    break;
                case MachineState.ProbeTool:
                    if (insertMsgQueue.Count > 0)
                        SendCurrentGcodeLine();
//                    else
//                       ProbeToolEnd(); 
                    break;

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
            {
                isParamUpdated[(int)GrblParamTypes.ReadGcodeOffsets] = true;
                return;
            }
            switch(vars[0])
            {
                case "GC":
                    grblStatus.ParseGState(vars[1]);
                    isParamUpdated[(int)GrblParamTypes.GcodeStates] = true;
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
        
        void SendLineToPort(string line)
        {
            if (line == null || line.Length < 1 || !portOpened)
                return;
            if (line[0] == '?')
            {   // special case - we want to see the next status request
                showStatusMsg++;
                return;
            }
            if (debugSending)
                Global.mdiControl.AddLine(line);
            line += "\n";
            lock (lockSerialSendObj)
            {
                port.Write(line);
            }
        }


        #region Process Gcode commands
        
        void SendLine(string line)
        {
            line = line.TrimStart(' ');
            char ch = line[0];
            if ((ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z'))
                // preprocess gcode line and send to grbl;
                SendProcessedGcodeLine(Global.ginterp.PreProcessGcodeLine(line));
            else
                SendLineToPort(line);
        }

        void SetTLO(int toolno)
        {
            CncTool tool = Global.toolTable.GetTool(toolno);
            if (tool != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("G43.1");
                for (int i = 0; i < tool.offsets.Length; i++)
                {
                    sb.Append(Utils.GetAxisLetter(i));
                    sb.Append(Utils.F3(tool.offsets[i]));
                }
                PostLine(sb.ToString(), true);
            }
            else
                PostLine("G43.1X0Y0Z0A0B0", true); // assume default tool
        }

        void SendProcessedGcodeLine(string line)
        {
            // check for actions external to grbl (such as tool changes)
            if (Global.ginterp.nonGrblActions.M6 && Global.ginterp.currentTool != lastTool)
            {
                wasRunningWhenToolChanged = machineState == MachineState.Running;
                machineState = MachineState.ToolChange;
            }
            else if (Global.ginterp.nonGrblActions.G43)
            {
                int toolno = Global.ginterp.nonGrblActions.H;
                if (toolno == 0)
                    toolno = Global.ginterp.currentTool;
                SetTLO(toolno);
                if (line.Length > 0)
                    PostLine(line, true);
                SendCurrentGcodeLine();
            }
            else
                SendLineToPort(line);

        }

        void ChangeTool()
        {
            machineState = MachineState.Idle;
            lastTool = Global.ginterp.currentTool;
            if (ChangeToolNotify != null)
            {
                for (int i = 0; i < tcpAxisPos.Length; i++)
                    tcpAxisPos[i] = grblStatus.axisPos[i];
                tcpZmax = MIN_LOCATION;
                ChangeToolNotify(this, lastTool, wasRunningWhenToolChanged);
            }
        }

        #endregion

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
            if (Global.ginterp != null)
                Global.ginterp.ResetGcodeLine();
            machineState = MachineState.Idle;
        }

        void SendStop()
        {
            SendByte(CMD_STOP);
            //SetSpindle(0, SpindleAction.Stop);
            if (Global.ginterp != null)
                Global.ginterp.ResetGcodeLine();
            machineState = MachineState.Idle;
        }

        // add line to send queue. Lines will be send through the poller
        public void PostLine(string line, bool isInsertQ = false)
        {
            lock(lockMsgBufferObj)
            {
                if (isInsertQ)
                    insertMsgQueue.Add(line);
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
            insertMsgQueue.Clear();
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
                port.WriteTimeout = 500;
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

            // pending lines will be sent on idle mode only
            if (machineState != MachineState.Idle && machineState != MachineState.CommandBatch)
                return false; 
            if (grblStatus.state == GrblStatus.MachineState.Run)
                return false;

            lock (lockMsgBufferObj)
            {
                // insert messages have priority over pending messages. do not send if they exist
                if (insertMsgQueue.Count == 0 && standardMsgQueue.Count > 0)
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
                if (requestFullStatus)
                {
                    SendByte(CMD_STATUS_REQUEST_FULL);
                    requestFullStatus = false;
                    showStatusMsg++;
                }
                else
                    SendByte(CMD_STATUS_REQUEST);
            }
            catch 
            {
                ClosePort();
                ConnectionStatus = CommStatus.Disconnected;
                if (CommStatusChanged != null)
                    CommStatusChanged(this, ConnectionStatus);
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
            string[] lines = new string[] { "$$", "$#", "$G" };
            PostLines(lines);
        }

        public void GetGrblParameters()
        {
            PostLine("$$");
        }

        public void GetGcodeCoordOfsets()
        {
            PostLine("$#");
        }

        public void GetGCodeParserState()
        {
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
            int ho = grblConfig.GetParam(GrblConfig.GrblParam.Code.HomingOption).intVal;
            bool homeen = (ho & GrblConfig.GrblParam.HomingOptionEnable) != 0;
            bool singleexis = (ho & GrblConfig.GrblParam.HomingOptionSingleAxis) != 0;
            if (homeen && !singleexis)
            {
                PostLine("$H");
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
            if (coordSystemIx < -1 || coordSystemIx > 8)
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
            coordSystemIx++;
            string axisLetter = Utils.GetAxisLetter(axis);
            if (axisLetter == null)
                return;
            machineState = MachineState.ProbeAxis;
            lastG90State = grblStatus.gState[(int)GrblStatus.GcodeParserStateNames.DistanceMode] == "G90";
            PostLine(string.Format("G91G38.2 {0}{1} F25", axisLetter, Utils.F3(10f * dir)), true);
            PostLine(string.Format("G10 L20 P{0} {1}{2}", coordSystemIx, axisLetter, Utils.F3(offset)), true);
            PostLine(string.Format("G0 {0}{1}", axisLetter, Utils.F3(- 5 * dir)), true);
            if (lastG90State)
                PostLine("G90", true);
            SendCurrentGcodeLine(); //initiate command sending
        }

        public void ToolTouchOff(int axis, int toolno, float offset)
        {
            string axisLetter = Utils.GetAxisLetter(axis);
            if (axisLetter == null)
                return;
            CncTool tool = Global.toolTable.GetTool(toolno);
            if (tool != null)
            {
                tool.offsets[axis] = grblStatus.axisPos[axis] - offset;
                if (Global.ginterp.currentTool == toolno)
                {
                    SetTLO(toolno);
                    SendCurrentGcodeLine();
                }
            }
        }

        public void ProbeTool(int axis, int tool, float offset, float dir)
        {
            string axisLetter = Utils.GetAxisLetter(axis);
            if (axisLetter == null)
                return;
            machineState = MachineState.ProbeTool;
            probeToolOffset = offset;
            probeToolNum = tool;
            probeToolAxis = axis;
            probeDir = dir;
            lastG90State = grblStatus.gState[(int)GrblStatus.GcodeParserStateNames.DistanceMode] == "G90";
            // clear current TLO
            SetTLO(0);
            PostLine(string.Format("G91G38.2{0}{1}F25", axisLetter, Utils.F3(10f * dir)), true);
            //PostLine("G4P0.3");
            SendCurrentGcodeLine();
        }

        void ProbeToolEnd()
        {
            machineState = MachineState.Idle;
            CncTool tool = Global.toolTable.GetTool(probeToolNum);
            if (tool != null)
            {
                tool.offsets[probeToolAxis] = grblStatus.axisPos[probeToolAxis] - probeToolOffset;
                if (Global.ginterp.currentTool != 0)
                    SetTLO(Global.ginterp.currentTool); // restore current TLO
            }
            PostLine(string.Format("G0 {0}{1}", Utils.GetAxisLetter(probeToolAxis), Utils.F3(-5 * probeDir)), true);
            if (lastG90State)
                PostLine("G90", true);
            SendCurrentGcodeLine();
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

        // set spindle speed and direction
        public void SetSpindle(float speed, SpindleAction action)
        {
            switch (action)
            {
                case SpindleAction.Stop: PostLine("M5"); break;
                case SpindleAction.StartCW: PostLine(string.Format("M3 S{0:0.0}", speed)); break;
                case SpindleAction.StartCCW: PostLine(string.Format("M4 S{0:0.0}", speed)); break;
                case SpindleAction.Speed: PostLine(string.Format("S{0:0.0}", speed)); break;
            }
        }

        #endregion

        #region Gcode Sender

        public void SendGcode()
        {
            if (ConnectionStatus != CommStatus.Connected || Global.ginterp == null || Global.ginterp.lines == null)
                return;
            if (machineState == MachineState.Idle)
            {
                if (wasRunningWhenToolChanged)
                {
                    // tool was manually changed, we need first to return to original location
                    if (tcpZmax != MIN_LOCATION)
                        PostLine(string.Format("G53G0Z{0}", Utils.F3(tcpZmax)), true);
                    PostLine(string.Format("G0X{0}Y{1}", Utils.F3(tcpAxisPos[X_AXIS]),
                        Utils.F3(tcpAxisPos[Y_AXIS])), true);
                    wasRunningWhenToolChanged = false;
                }
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
            string curLine = null;
            // send any inseted lines. non need to preprocess, as they are generated
            lock (lockMsgBufferObj)
            {
                if (insertMsgQueue.Count > 0)
                {
                    curLine = insertMsgQueue[0];
                    insertMsgQueue.RemoveAt(0);
                }
            }
            if (curLine != null)
            {
                SendLineToPort(curLine);
                return;
            }
            if (Global.ginterp == null || Global.ginterp.lines == null)
            {
                StopSendingGcode();
                return;
            }
            curLine = Global.ginterp.GetNextProcessedLine();
            if (curLine == null)
            {
                StopSendingGcode();
                return;
            }
            SendProcessedGcodeLine(curLine);
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
                {
                    PauseGcode();
                    machineState = MachineState.Stopping;
                }
                else
                    SendStop();
            }
            if (Global.ginterp != null)
                Global.ginterp.ResetGcodeLine();
        }

        public void StepGcode()
        {
            if (machineState != MachineState.Idle)
                return;
            if (Global.ginterp == null || Global.ginterp.lines == null)
                return;
            string curLine = Global.ginterp.GetNextProcessedLine();
            if (curLine != null)
                PostLine(curLine);
        }
        #endregion
    }
}
