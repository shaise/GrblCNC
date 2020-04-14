// Global static class to be shared by all components
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrblCNC.Controls;

namespace GrblCNC
{
    public static class Global
    {
        static bool grblConnected;
        public delegate void GrblConnectionChangedDelegate(bool isConnected);
        public static event GrblConnectionChangedDelegate GrblConnectionChanged;

        public static GrblComm grblComm;
        public static MdiControl mdiControl;
        public static GcodeInterp ginterp;
        public static VisualizerWin visualizeWindow;
        public static GrblStatus grblStatus;
        public static GrblConfig grblConfig;
        public static ParametersEdit grblParameterEditor;
        public static ToolTableEdit toolTableEdit;
        public static ToolTable toolTable;

        public const int NUM_AXIS = 5;

        public static bool GrblConnected
        {
            get { return grblConnected; }
            set
            {
                bool lastState = grblConnected;
                grblConnected = value;
                if (lastState != grblConnected && GrblConnectionChanged != null)
                    GrblConnectionChanged(grblConnected);
            }
        }
    }
}
