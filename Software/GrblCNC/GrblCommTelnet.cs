using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using Redgate.Net.mDNS;

namespace GrblCNC
{
    public class GrblCommTelnet : GrblCommDevice
    {
        TcpClient tcpClient;
        public bool portOpened = false;
        public bool stopthread = false;
        Thread recvThread;
        string portName;

        public GrblCommTelnet()
        {
            tcpClient = new TcpClient();
            tcpClient.NoDelay = true;
        }

        public override string CommType { get { return "Telnet"; } }

        public override string CommName
        {
            get { return portName; }
        }

        public override bool Open(string portname)
        {
            if (portOpened)
                Close();
            portName = portname;
            string[] endpoint = portname.Split(':');
            try
            {
                tcpClient.Connect(endpoint[0], int.Parse(endpoint[1]));
                recvThread = new Thread(ReceiverThread);
                stopthread = false;
                recvThread.Start(this);
            }
            catch
            {
                return false;
            }

            return true;
        }

        void DataReceive()
        {
            NetworkStream stream = tcpClient.GetStream();
            stream.ReadTimeout = 1000;
            StringBuilder readLine = new StringBuilder();
            byte [] data = new byte[1];

            try
            {
                while (!stopthread)
                {
                    int nbytes = stream.Read(data, 0, 1);
                    if (nbytes == 0)
                        continue;

                    char ch = (char)data[0]; // System.Text.Encoding.ASCII.GetString(data, 0, 1);
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
            catch
            {
                Close();
            }
        }

        static void ReceiverThread(Object arg)
        {
            Thread.CurrentThread.Name = "TcpReceiverThread";
            GrblCommTelnet commTelnet = (GrblCommTelnet)arg;
            commTelnet.DataReceive();
        }

        public override void Close()
        {
            if (!portOpened)
                return;

            stopthread = true;
            recvThread.Join();
            tcpClient.Close();
            portOpened = false;
        }

        public override string[] GetPortNames()
        {
            MDnsResolver resolver = new MDnsResolver();
            List<string> portNames = new List<string>();
            MDnsQueryResult[] results = resolver.ResolveServiceName("_device-info._tcp.local");
            if (results.Length == 0)
                return portNames.ToArray();
            foreach (MDnsQueryResult res in results)
            {
                bool isGrblSystem = false;
                foreach (string txt in res.txts)
                    if (txt == "model=grblHAL")
                    {
                        isGrblSystem = true;
                        break;
                    }
                if (!isGrblSystem)
                    continue;

                string host = res.hostname;
                if (res.endpoints.Length > 0)
                    host = res.endpoints[0].ToString().Split(':')[0];
                host += ":23";
                portNames.Add(host);
            }
            return portNames.ToArray();
        }

        public override void WriteLine(string line)
        {
            line += "\n";
            NetworkStream stream = tcpClient.GetStream();
            byte [] byteBuffer = Encoding.ASCII.GetBytes(line);
            stream.Write(byteBuffer, 0, byteBuffer.Length);
        }

        public override void WriteByte(byte b)
        {
            NetworkStream stream = tcpClient.GetStream();
            stream.WriteByte(b);
        }
    }
}
