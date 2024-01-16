using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace GrblCNC
{
    public class GrblCommSerial : GrblCommDevice
    {
        SerialPort port;
        public bool portOpened = false;
        StringBuilder readLine;
        string portName;
        bool portClosing = false;

        public GrblCommSerial()
        {
            port = new SerialPort();
            port.DataReceived += Port_DataReceived;
            readLine = new StringBuilder();
        }

        public override string CommType { get { return "Serial"; } }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            while (!portClosing && port.BytesToRead != 0)
            {
                char ch = (char)port.ReadChar();
                if (ch == '\r')
                    continue;
                if (ch == '\n')
                {
                    onLineReceived(readLine.ToString());
                    readLine.Clear();
                    continue;
                }
                readLine.Append(ch);
            }
        }

        public override string CommName
        {
            get { return portName;  }
        }

        public override bool Open(string portname)
        {
            if (portOpened)
                Close();
            portName = portname;
            try
            {
                port.PortName = portname;
                //serPort.BaudRate = 57600;
                port.BaudRate = 115200;
                port.Parity = Parity.None;
                port.DataBits = 8;
                port.StopBits = StopBits.One;
                port.Handshake = Handshake.None;
                port.WriteTimeout = 500;
                port.Open();
                portClosing = false;
                portOpened = true;
            }
            catch
            {
                return false;
            }
            return true;
        }

        public override void Close()
        {
            if (!portOpened)
                return;
            try {
                portClosing = true;
                System.Threading.Thread.Sleep(100);
                port.Close(); 
            }
            catch { } // sometimes when the usb disconnects, closing the port causes error
            portOpened = false;
        }

        public override string[] GetPortNames()
        {
            return SerialPort.GetPortNames();
        }

        public override void WriteLine(string line)
        {
            line += "\n";
            if (!portClosing)
                port.Write(line);
        }

        public override void WriteByte(byte b)
        {
            byte[] msg = new byte[1];
            msg[0] = b;
            if (!portClosing)
                port.Write(msg, 0, 1);
        }
    }
}
