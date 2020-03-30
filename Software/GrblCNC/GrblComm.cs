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
            ParamRead,
            Jog,
            StopJog,
            Running,
            Paused
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
        List<string> standardMsgQueue;
        List<string> urgentMsgQueue;
        Dictionary<string, string> grblErrorCodes;
        object lockMsgBufferObj = new object();
        object lockSerialSendObj = new object();
        int jogCount = 0;
        public MachineState machineState;
        public CommStatus ConnectionStatus;
        string curJogCommand;
        string lastError;
        int showStatusMsg = 0;
        
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
        public delegate void ParameterUpdateDelegate(object sender, GrblConfig config);
        public event ParameterUpdateDelegate ParameterUpdate;
        public delegate void ErrorDetectedDelegate(object sender, string err);
        public event ErrorDetectedDelegate ErrorDetected;
        

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
            port.DataReceived += port_DataReceived;
            standardMsgQueue = new List<string>();
            urgentMsgQueue = new List<string>();
            ReadErrorCodes();
            scanCount = 0;
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
                machineState = MachineState.Idle;
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
            else if (line.StartsWith("error"))
            {
                lastError = line.Substring(6);
                if (grblErrorCodes.ContainsKey(lastError))
                    lastError = grblErrorCodes[lastError];
                if (ErrorDetected != null)
                    ErrorDetected(this, lastError);
           }
            if (LineReceived != null)
            {
                LineReceived(this, line, isStatusLine && showStatusMsg == 0);
                if (showStatusMsg > 0)
                    showStatusMsg--;
            }
        }

        #region Input line handling
        void HandleParamLine(string line)
        {
            machineState = MachineState.ParamRead;
            grblConfig.ParseParam(line);
        }

        void HandleEndReadParam()
        {
            if (ParameterUpdate != null)
                ParameterUpdate(this, grblConfig);
            machineState = MachineState.Idle;
        }

        void HandleJogInProgress()
        {
            SendLine(curJogCommand);
            

            //SendLine("$J=G91 X10 F600.0");
            /*if (((tmpCnt / 5) & 1) == 0)
                SendLine("$J=G91 X0.25 F600");
            else
                SendLine("$J=G91 X0.25 F600");*/
            //tmpCnt++;
        }

        void HandleStopJog()
        {
            SendByte(CMD_JOG_CANCEL);
            machineState = MachineState.Idle;
        }

        void HandleOKLine(string line)
        {
            switch (machineState)
            {
                case MachineState.ParamRead: HandleEndReadParam(); break;
                case MachineState.Jog: HandleJogInProgress(); break;
                case MachineState.StopJog: HandleStopJog(); break;
                case MachineState.Running: SendCurrentGcodeLine(); break;
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
            PostLine("$$");
            machineState = MachineState.ParamRead;
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
