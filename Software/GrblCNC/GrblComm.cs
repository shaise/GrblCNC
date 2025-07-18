﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;
using System.Globalization;
using GrblCNC.Properties;
using GrblCNC.Glutils;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
        public const int MAX_NUM_AXIS = 6;

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
            waitStatus,
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
            SpeedOverride,
        }

        string [] portNames;
        public GrblCommDevice activePort;
        public GrblCommDevice commDevice;
        GrblCommTelnet telnetcomm;
        public string grblVersion = "Unknown";
        int scanDeviceIx;
        int scanPortIx;
        //SerialPort port;
        bool portOpened;
        //static int PORT_BUFF_LEN = 256;
        static int SCAN_INTERVAL = 10;  // in 100ms units = 1 second
        GrblStatus grblStatus;
        GrblConfig grblConfig;
        GCodeConfig gcodeConfig;
        List<string> standardMsgQueue;
        List<string> insertMsgQueue; // when not empty, will be inserted to current running gcode
        List<GrblCommDevice> commDevices;
        Dictionary<string, string> grblErrorCodes;
        Dictionary<string, string> grblAlarmCodes;
        object lockMsgBufferObj = new object();
        object lockSerialSendObj = new object();
        int jogCount = 0;
        public MachineState machineState;
        public CommStatus ConnectionStatus;
        public string driverVersion = null;
        bool [] isParamUpdated = new bool[(int)GrblParamTypes.NumParamTypes];
        string curJogCommand;
        string lastError;
        int showStatusMsg = 0;
        List<string> commandBatch;
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

        int curFeedOverride = 100;
        int curSpindleOverride = 100;

        float[] probeVals = new float[MAX_NUM_AXIS];

        int scanCount;
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
        public delegate void ProbeCompletedDelegate(object sender, float[] prbVals);
        public event ProbeCompletedDelegate ProbeCompleted;

        public GrblComm()
        {
            portNames = null;
            scanPortIx = scanDeviceIx = 0;

            activePort = null;
            portOpened = false;
            commDevices = new List<GrblCommDevice>();
            GrblCommSerial sercomm = new GrblCommSerial();
            sercomm.LineReceived += CommDevice_LineReceived;
            telnetcomm = new GrblCommTelnet();
            telnetcomm.LineReceived += CommDevice_LineReceived;
            commDevices.Add(sercomm);
            commDevices.Add(telnetcomm);
            //port = new SerialPort();
            grblStatus = new GrblStatus();
            grblConfig = new GrblConfig();
            gcodeConfig = new GCodeConfig();
            //port.DataReceived += port_DataReceived;
            standardMsgQueue = new List<string>();
            //urgentMsgQueue = new List<string>();
            insertMsgQueue = new List<string>();
            commandBatch = new List<string>();
            ReadErrorCodes();
            scanCount = 0;
            Global.grblStatus = grblStatus;
            Global.grblConfig = grblConfig;
            tcpAxisPos = new float[Global.NumAxes];
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

        public string CommType
        {
            get
            {
                if (commDevice == null) return "None";
                return commDevice.CommType;
            }
        }

         void HandleReceivedLine(string line)
        {
            if (line.Length == 0)
                return;
            bool isStatusLine = false;
            if (line.StartsWith("Grbl"))
                TestConnection();
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
            if (machineState == MachineState.waitStatus)
                machineState = MachineState.Idle;

            GrblStatus.MachineState oldState = grblStatus.state;
            float[] axis = new float[grblStatus.axisPos.Length];
            for (int i = 0; i < axis.Length; i++)
                axis[i] = grblStatus.axisPos[i];

            grblStatus.Parse(line);
            if (StatusUpdate != null)
                StatusUpdate(this, grblStatus);
            grblStatus.gStateChange = false;
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
                {
                    machineState = MachineState.waitStatus;
                    GetGcodeCoordOfsets();
                }
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
                case MachineState.ProbeAxis:
                    if (insertMsgQueue.Count > 0)
                        SendCurrentGcodeLine();
                    //if (insertMsgQueue.Count == 0)
                    //    machineState
//                    else
//                       ProbeToolEnd(); 
                    break;

                case MachineState.Jog: HandleJogInProgress(); break;
                case MachineState.StopJog: HandleStopJog(); break;
                case MachineState.Running: SendCurrentGcodeLine(); break;
                case MachineState.CommandBatch: HandleCommandBatch(); break;
            }
        }


        void HandleVersion(string verstr)
        {
            string[] vervars = verstr.Split('.');
            grblVersion = vervars[vervars.Length - 1];
        }

        void HandleAxis(string axisstr)
        {
            int naxes;
            if (int.TryParse(axisstr, out naxes))
                Global.NumAxes = naxes;
        }

        void HandleReconnection(string version)
        {
            driverVersion = version;
            activePort = commDevice;
            if (machineState == MachineState.Idle)
                GetAllGrblParameters();
            //machineState = MachineState.Idle;
            if (ConnectionStatus != CommStatus.Connected)
            {
                ConnectionStatus = CommStatus.Connected;
                grblStatus.state = GrblStatus.MachineState.Unknown;
                if (CommStatusChanged != null)
                    CommStatusChanged(this, ConnectionStatus);
                if (MessageReceived != null)
                    MessageReceived(this, "Connected.", MessageType.Info);
            }
            requestFullStatus = true;
        }

        void HandleProbeResult(string prbResult)
        {
            try
            {
                string [] strvals = prbResult.Split(new char[] { ',', ' '}, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < strvals.Length; i++)
                {
                    if (i < probeVals.Length)
                        probeVals[i] = float.Parse(strvals[i]) - grblStatus.workingCoords[i];
                }
                if (ProbeCompleted != null)
                    ProbeCompleted(this, probeVals);
            }
            catch { }

        }

        void HandleMessageLine(string line)
        {
            //machineState = MachineState.ParamRead;
            //paramReadStage = ParamReadStage.ReadGcodeOffsets;
            string[] vars = line.Split(new char[] { '[', ']', ':' }, StringSplitOptions.RemoveEmptyEntries);
            if (vars.Length < 2)
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

                case "VER":
                    HandleVersion(vars[1]);
                    break;

                case "AXS":
                    HandleAxis(vars[1]);
                    break;

                case "DRIVER VERSION":
                    HandleReconnection(vars[1]);
                    break;

                case "PRB":
                    HandleProbeResult(vars[1]);
                    break;

                case "SETTINGGROUP":
                    Global.grblConfig.ParseGroup(vars[1]);
                    break;

                case "SETTING":
                    Global.grblConfig.ParrseParamDescription(vars[1]);
                    break;

            }
        }


        #endregion

        void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
        }

        private void CommDevice_LineReceived(GrblCommDevice sender, string line)
        {
            HandleReceivedLine(line);
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
            if (debugSending && ConnectionStatus == CommStatus.Connected)
                Global.mdiControl.AddLine(line);
            lock (lockSerialSendObj)
            {
                commDevice.WriteLine(line);
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
            StringBuilder sb = new StringBuilder();
            sb.Append("G43.1");
            int naxes = Global.NumAxes;
            if (tool != null && tool.offsets.Length < naxes)
                naxes = tool.offsets.Length;
            for (int i = 0; i < naxes; i++)
            {
                if (i >= Global.NumAxes)
                    break;
                sb.Append(GrblUtils.GetAxisLetter(i));
                if (tool != null)
                    sb.Append(Utils.F3(tool.offsets[i]));
                else
                    sb.Append("0");
            }
            PostLine(sb.ToString(), true);
        }

        void SendProcessedGcodeLine(string line)
        {
            // check for actions external to grbl (such as tool changes)
            if (Global.ginterp.nonGrblActions.M6 && Global.ginterp.currentTool !=  Global.ginterp.lastTool)
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
            Global.ginterp.lastTool = Global.ginterp.currentTool;
            if (ChangeToolNotify != null)
            {
                for (int i = 0; i < tcpAxisPos.Length; i++)
                    tcpAxisPos[i] = grblStatus.axisPos[i];
                tcpZmax = MIN_LOCATION;
                ChangeToolNotify(this, Global.ginterp.currentTool, wasRunningWhenToolChanged);
            }
        }

        #endregion

        public void SendByte(byte b)
        {
            if (!portOpened)
                return;
            if (ConnectionStatus == CommStatus.Connected && b != CMD_STATUS_REQUEST && !Global.AppClosing)
                Global.mdiControl.AddLine(string.Format("<0x{0:X}>", (int)b));
            lock (lockSerialSendObj)
            {
                activePort.WriteByte(b);
            }
        }

        void ClearMachineState()
        {
            if (Global.ginterp != null)
                Global.ginterp.ResetGcodeLine();
            machineState = MachineState.Idle;
        }

        public void SendSoftReset()
        {
            SendByte(CMD_SOFT_RESET);
            ClearMachineState();
        }
        void SetOverrideSpeed(int overridePercent, ref int curOverride, byte set100cmd, byte add10cmd, byte dec10cmd, byte add1cmd, byte dec1cmd)
        {
            if (overridePercent == 100)
            {
                SendByte(set100cmd);
                curOverride = 100;
                return;
            }
            if (overridePercent == curOverride)
                return;
            if ((overridePercent > 100 && curOverride <= 100) || (overridePercent < 100 && curOverride >= 100))
            {
                SendByte(set100cmd);
                curOverride = 100;
            }
            if (overridePercent > curOverride)
            {
                while (overridePercent >= (curOverride + 10))
                {
                    SendByte(add10cmd);
                    curOverride += 10;
                }
                while (overridePercent > curOverride)
                {
                    SendByte(add1cmd);
                    curOverride++;
                }
            }
            else
            {
                while (overridePercent <= (curOverride - 10))
                {
                    SendByte(dec10cmd);
                    curOverride -= 10;
                }
                while (overridePercent < curOverride)
                {
                    SendByte(dec1cmd);
                    curOverride--;
                }
            }
        }
        public void SetFeedOverride(int overridePercent)
        {
            SetOverrideSpeed(overridePercent, ref curFeedOverride, CMD_FEED_SET_100, CMD_FEED_ADD_10, CMD_FEED_DEC_10, CMD_FEED_ADD_1, CMD_FEED_DEC_1);
        }

        public void SetSpindleOverride(int overridePercent)
        {
            SetOverrideSpeed(overridePercent, ref curSpindleOverride, CMD_SPINDLE_SET_100, CMD_SPINDLE_ADD_10, CMD_SPINDLE_DEC_10, CMD_SPINDLE_ADD_1, CMD_SPINDLE_DEC_1);
        }

        void TestConnection()
        {
            SendLineToPort("$I");
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
            if (machineState == MachineState.waitStatus)
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
            if (commDevice == null)
                return false;
            if (commDevice.Open(portName))
            {
                portOpened = true;
                //SendSoftReset();
                try
                {
                    TestConnection();
                }
                catch
                {
                    return false;
                }
                ClearMachineState();
                return true;
            }
            return false;
        }

        public void ClosePort()
        {
            if (!portOpened)
                return;
            commDevice.Close();
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
            if (scanPortIx == 0)
            {
                commDevice = commDevices[scanDeviceIx];
                portNames = commDevice.GetPortNames();
                // Fixme: for linux we can filter the names to USB/Serial ports only
                scanPortIx = portNames.Length;
                scanDeviceIx++;
                if (scanDeviceIx >= commDevices.Count)
                    scanDeviceIx = 0;
            }
            if (scanPortIx == 0)
                return; // empty list

            while (scanPortIx > 0)
            {
                scanPortIx--;
                if (MessageReceived != null)
                    MessageReceived(this, string.Format("Trying {0}...",portNames[scanPortIx]), MessageType.Info);

                if (OpenPort(portNames[scanPortIx]))
                    break;
            }

        }

        public float GetAxisPosition(int axis)
        {
            return grblStatus.axisPos[axis];
        }

        public float GetAbsAxisPosition(int axis)
        {
            return grblStatus.workingCoords[axis] + grblStatus.axisPos[axis];
        }

        public void Close()
        {
            new Thread(new ThreadStart(delegate
            {
                if (machineState == MachineState.Running)
                {
                    SendStop();
                    Thread.Sleep(100);
                }
                ClosePort();
                telnetcomm.Shutdown();
            })).Start();
        }

        #region Grbl control commands
        public void GetAllGrblParameters()
        {
            string[] lines = new string[] { "$EG", "$ES", "$$", "$#", "$G" };
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

        public void GoTo(int axisMask, int coordSystemIx, double[] pos, float speed = 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GrblUtils.GetCoordSystem(coordSystemIx));
            if (speed == 0)
                sb.Append("G0");
            else
                sb.Append(string.Format("G1F{0:0.0}", speed));
            sb.Append(GrblUtils.GetGcodeLocation(axisMask, pos));
            PostLine(sb.ToString());
        }

        public string HomeAxis(int axis)
        {
            int ho = grblConfig.GetParam(GrblConfig.GrblParam.Code.HomingOption).intVal;
            bool homeen = (ho & GrblConfig.GrblParam.HomingOptionEnable) != 0;
            if (!homeen)
                return "Driver homing is not enabled. Check homing parameters";
            if (axis < 0)
            {
                PostLine("$H");
                return "OK";
            }
            bool singleexis = (ho & GrblConfig.GrblParam.HomingOptionSingleAxis) != 0;
            if (!singleexis)
                return "Single axis homing is not enabled. Check homing parameters";

            string axisLetter = GrblUtils.GetAxisLetter(axis);
            if (axisLetter == null)
                return "Invalid homing axis selected";
            string cmd = string.Format("$H{0}", axisLetter);
            PostLine(cmd);
            return "OK";
        }


        public void MoveRelative(int axis, float distance)
        {
            string axisLetter = GrblUtils.GetAxisLetter(axis);
            if (axisLetter == null)
                return;
            PostLine(string.Format("G91G0 {0}{1}", axisLetter, Utils.F3(distance)), true);
            PostLine("G90", true);
            SendCurrentGcodeLine();
        }

        public void CoordTouchAxis(int axis, int coordSystemIx, float offset, float retract = 0)
        {
            if (coordSystemIx < -1 || coordSystemIx > 8)
                return;
            string axisLetter = GrblUtils.GetAxisLetter(axis);
            if (axisLetter == null)
                return;
            PostLine(string.Format("G10 L20 P{0} {1}{2}", coordSystemIx + 1, axisLetter, offset), true);
            if (retract != 0)
                MoveRelative(axis, retract);
            else
                SendCurrentGcodeLine(); //initiate command sending
        }

        public void ProbeAxis(int axis, float relativeDist)
        {
            string axisLetter = GrblUtils.GetAxisLetter(axis);
            if (axisLetter == null)
                return;
            machineState = MachineState.ProbeAxis;
            PostLine(string.Format("G91G38.2 {0}{1} F25", axisLetter, Utils.F3(relativeDist)), true);
            PostLine("G90", true);
            SendCurrentGcodeLine(); //initiate command sending
        }

        public void ProbeAxisAll(int axis, int coordSystemIx, float offset, float dir)
        {
            if (coordSystemIx < -1 || coordSystemIx > 8)
                return;
            coordSystemIx++;
            string axisLetter = GrblUtils.GetAxisLetter(axis);
            if (axisLetter == null)
                return;
            machineState = MachineState.ProbeAxis;
            lastG90State = grblStatus.gState[(int)GrblStatus.GcodeParserStateNames.DistanceMode] == "G90";
            PostLine(string.Format("G91G38.2 {0}{1} F25", axisLetter, Utils.F3(10f * dir)), true);
            PostLine(string.Format("G10 L20 P{0} {1}{2}", coordSystemIx, axisLetter, Utils.F3(offset)), true);
            PostLine(string.Format("G0 {0}{1}", axisLetter, Utils.F3(-5 * dir)), true);
            if (lastG90State)
                PostLine("G90", true);
            SendCurrentGcodeLine(); //initiate command sending
        }

        public void ToolTouchOff(int axis, int toolno, float offset)
        {
            string axisLetter = GrblUtils.GetAxisLetter(axis);
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
            string axisLetter = GrblUtils.GetAxisLetter(axis);
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
            PostLine(string.Format("G0 {0}{1}", GrblUtils.GetAxisLetter(probeToolAxis), Utils.F3(-5 * probeDir)), true);
            if (lastG90State)
                PostLine("G90", true);
            SendCurrentGcodeLine();
        }

        public void StepJog(int axis, float dist, float feedrate)
        {
            string axisLetter = GrblUtils.GetAxisLetter(axis);
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

            // start jogging
            string axisLetter = GrblUtils.GetAxisLetter(axis);
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
                case SpindleAction.Stop:          PostLine("M5"); break;
                case SpindleAction.StartCW:       PostLine(string.Format("M3 S{0:0.0}", speed)); break;
                case SpindleAction.StartCCW:      PostLine(string.Format("M4 S{0:0.0}", speed)); break;
                case SpindleAction.Speed:         PostLine(string.Format("S{0:0.0}", speed)); break;
                case SpindleAction.SpeedOverride: SetSpindleOverride((int)(speed + 0.5)); break;
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
            if (Global.ginterp != null)
                Global.ginterp.ResetGcodeLine();
        }

        void SendCurrentGcodeLine()
        {
            string curLine = null;
            // send any inserted lines. no need to preprocess, as they are generated
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

        public void WaitUntillIdle()
        {
            while (machineState != MachineState.Idle)
            {
                Thread.Sleep(100);
            }
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
