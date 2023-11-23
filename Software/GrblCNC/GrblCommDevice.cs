using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrblCNC
{
    public abstract class GrblCommDevice
    {
        public delegate void LineReceivedDelegate(GrblCommDevice sender, string line);
        public event LineReceivedDelegate LineReceived;

        public abstract bool Open(string portname);
        public abstract void Close();
        public abstract string[] GetPortNames();
        public abstract void WriteLine(string line);
        public abstract void WriteByte(byte b);

        public abstract string CommType { get; }
        public abstract string CommName { get; }

        public void onLineReceived(string line)
        {
            LineReceived?.Invoke(this, line);
        }
    }
}
