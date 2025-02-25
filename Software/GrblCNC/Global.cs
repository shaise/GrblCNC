﻿// Global static class to be shared by all components
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
        public static ErrorDisplayHandler errHandler;

        public static string SettingsPath;
        public static string ToolTableFile;

        public const int MAX_AXES = 5;
        private static int numAxes = MAX_AXES;

        public delegate void NumAxesChangedDelegate();
        public static event NumAxesChangedDelegate NumAxesChanged;

        public static bool AppClosing = false;


        public static int NumAxes
        {
            get { return numAxes; }
            set
            {
                if (numAxes != value)
                {
                    numAxes = value;
                    NumAxesChanged?.Invoke();
                }
            }
        }

        public static string GetAxesString()
        {
            if (numAxes > 4)
                return "X|Y|Z|A|B";
            else if (numAxes > 3)
                return "X|Y|Z|A";
            
            return "X|Y|Z";
        }

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
