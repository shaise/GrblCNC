using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrblCNC.Glutils;
using System.IO;
using OpenTK;
using GrblCNC.Properties;

namespace GrblCNC
{
    public class GToken
    {
        public char code;
        public float value;
        public GToken(char code, float value)
        {
            this.code = code;
            this.value = value;
        }
    }

    public class GcodeInterp
    {
        enum MotionType
        {
            None = 0,
            Rapid,
            Line,
            ArcCW,
            ArcCCW
        }

        Wire3D wirepath;
        public string[] lines;
        const int NUM_COORDS = 10;
        const int NUM_POS_COORDS = 6;
        const double ARC_RESOLUTION = 1.0; //mm

        public static int pX = 0;
        public static int pY = 1;
        public static int pZ = 2;
        public static int pA = 3;
        public static int pB = 4;
        public static int pC = 5;
        public static int pI = 6;
        public static int pJ = 7;
        public static int pK = 8;
        public static int pR = 9;

        public float[] curCoords = new float[NUM_COORDS];
        public float[] lastCoords = new float[NUM_COORDS];
        public float[] minCoords = new float[NUM_POS_COORDS];
        public float[] maxCoords = new float[NUM_POS_COORDS];

        bool [] haveCoord = new bool[NUM_COORDS];
        MotionType motionType;

        bool relativeMotion = false;
        bool relativeArcCent = true;
        bool useInches = false;

        List<Wire3D.WireVertex> verts;
        int planeAxis1, planeAxis2, perpAxis;
        int curLine;
        int curInternLine;
        int sendLineIx = -1;

        int simLocation;
        float simLocPart;
        float simTargetDist;
        float simSpeed = 0.2f;
        int simGcodeLine;
        Vector3 simDir, simStartSeg, simTargetPos;
        //float fragDist;

        char VerifyCode(char code)
        {
            if (code >= 'a' && code <= 'z')
                return Char.ToUpper(code);
            if (code >= 'A' && code <= 'Z')
                return code;
            return '-'; // error

        }

        bool ValidValueChar(char ch)
        {
            if (ch >= '0' && ch <= '9')
                return true;
            if (ch == '.' || ch == '-')
                return true;
            return false;
        }

        public int CurrentLineNumber
        {
            get { return sendLineIx; }
        }

        List<GToken> TokenizeLine(string line)
        {
            List<GToken> tokens = new List<GToken>();
            int pos = 0;
            while (pos < line.Length)
            {
                while (line[pos] == ' ')
                    pos++;
                if (pos >= line.Length)
                    break;
                char code = VerifyCode(line[pos]);
                if (code == '-')
                    return tokens;
                pos++;
                int datapos = pos;
                while (pos < line.Length && ValidValueChar(line[pos]))
                    pos++;
                int datalen = pos - datapos;
                if (datalen == 0)
                    return tokens;
                float val;
                try
                {
                    val = Utils.ParseFloatInvariant(line.Substring(datapos, datalen));
                }
                catch
                {
                    return tokens;
                }
                tokens.Add(new GToken(code, val));
            }

            return tokens;
        }

        public GcodeInterp()
        {
            wirepath = new Wire3D();
        }

        void SetCoords(float[] coords, float val, int ncoord = NUM_COORDS)
        {
            for (int i = 0; i < ncoord; i++) coords[i] = val;
        }

        void CopyCoords(float[] destCoords, float[] srcCoords, int ncoord = NUM_COORDS)
        {
            for (int i = 0; i < ncoord; i++) destCoords[i] = srcCoords[i];
        }

        public string LoadGcode(string fileName)
        {
            SetCoords(curCoords, 0);
            if (fileName == null)
            {
                // load internal ngc file
                lines = Resources.GrblLogo_ngc.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                try
                {
                    lines = File.ReadAllLines(fileName, Encoding.UTF8);
                }
                catch
                {
                    return "Unable to read file / file not found";
                }
            }
            GeneratePath();
            return "OK";
        }

        void ParseTokens(List<GToken> tokens)
        {
            for (int i = 0; i < NUM_COORDS; i++) haveCoord[i] = false;
            for (int i = 0; i < NUM_COORDS; i++) curCoords[i] = 0;
            
            foreach (GToken token in tokens)
            {
                if (token.code == 'G')
                {
                    switch ((int)(token.value * 10 + 0.5))  // mul by 10 to catch gcoses like g91.1
                    {
                        case 0: motionType = MotionType.Rapid; break;
                        case 10: motionType = MotionType.Line; break;
                        case 20: motionType = MotionType.ArcCW; break;
                        case 30: motionType = MotionType.ArcCCW; break;

                        case 170: planeAxis1 = pX; planeAxis2 = pY; perpAxis = pZ; break;
                        case 180: planeAxis1 = pZ; planeAxis2 = pX; perpAxis = pY; break;
                        case 190: planeAxis1 = pY; planeAxis2 = pZ; perpAxis = pX; break;

                        case 200: useInches = true; break;
                        case 201: useInches = false; break;

                        case 700: useInches = true; break;
                        case 701: useInches = false; break;

                        case 900: relativeMotion = false; break;
                        case 901: relativeArcCent = false; break;
                        case 910: relativeMotion = true; break;
                        case 911: relativeArcCent = true; break;
                    }
                }
                else if (token.code == 'M')
                {

                }
                else
                {
                    switch (token.code)
                    {
                        case 'X': curCoords[pX] = token.value; haveCoord[pX] = true; break;
                        case 'Y': curCoords[pY] = token.value; haveCoord[pY] = true; break;
                        case 'Z': curCoords[pZ] = token.value; haveCoord[pZ] = true; break;
                        case 'A': curCoords[pA] = token.value; haveCoord[pA] = true; break;
                        case 'B': curCoords[pB] = token.value; haveCoord[pB] = true; break;
                        case 'C': curCoords[pC] = token.value; haveCoord[pC] = true; break;
                        case 'I': curCoords[pI] = token.value; haveCoord[pI] = true; break;
                        case 'J': curCoords[pJ] = token.value; haveCoord[pJ] = true; break;
                        case 'K': curCoords[pK] = token.value; haveCoord[pK] = true; break;
                        case 'R': curCoords[pR] = token.value; haveCoord[pR] = true; break;
                        case 'N': curLine = (int)(token.value + 0.5); break;
                    }
                }
            }
        }

        bool isCoordChange()
        {
            foreach (bool b in haveCoord)
                if (b) return true;
            return false;
        }

        void UpdateCoordinates()
        {
            for (int i = 0; i < NUM_POS_COORDS; i++)
            {
                if (relativeMotion)
                    curCoords[i] += lastCoords[i];
                else
                    if (!haveCoord[i])
                        curCoords[i] = lastCoords[i];
            }
        }

        void GenerateLine(int color, int lineno)
        {
            verts.Add(new Wire3D.WireVertex(curCoords[pX], curCoords[pY], curCoords[pZ], lineno, color));
        }

        void GenerateArc(bool isCW)
        {
            float [] cent = new float[3];
            // find arc center
            for (int i = 0; i < 3; i++)
            {
                cent[i] = curCoords[pI + i];
                if (relativeArcCent)
                    cent[i] += lastCoords[pX + i];
            }
            float[] delta1 = new float[3];
            float[] delta2 = new float[3];
            for (int i = 0; i < 3; i++)
            {
                delta1[i] = lastCoords[pX + i] - cent[i]; // start vector
                delta2[i] = curCoords[pX + i] - cent[i];  // end vector
            }
            double radius = Math.Sqrt(delta1[planeAxis1] * delta1[planeAxis1] + delta1[planeAxis2] * delta1[planeAxis2]);
            if (radius < 0.0001)
            {
                // Fixme: Report error
                GenerateLine(0x404040, curInternLine);
                return;
            }
            double resDegree = Math.Atan(ARC_RESOLUTION / radius);
            if (resDegree > Math.PI / 4)
                resDegree = Math.PI / 4;
            double ang1 = Math.Atan2(delta1[planeAxis2], delta1[planeAxis1]);
            double ang2 = Math.Atan2(delta2[planeAxis2], delta2[planeAxis1]);
            double arcAng = 0;
            if (isCW)
                arcAng = ang1 - ang2;
            else
                arcAng = ang2 - ang1;
            if (arcAng < 0.0001)
                arcAng += 2.0 * Math.PI;

            int nsegments = (int)(arcAng / (resDegree - 0.001));
            double segAng = arcAng / nsegments;
            if (isCW)
                segAng = -segAng;
            double perpSeg = (curCoords[perpAxis] - lastCoords[perpAxis]) / nsegments;
            //float[] arcCoords = new float[3];
            curCoords[perpAxis] = lastCoords[perpAxis];
            for (int i = 0 ; i < nsegments; i++)
            {
                double curAng = ang1 + segAng * (i + 1);
                if (curAng < 0) curAng += 2.0 * Math.PI;
                curCoords[planeAxis1] = (float)(cent[planeAxis1] + Math.Cos(curAng) * radius);
                curCoords[planeAxis2] = (float)(cent[planeAxis2] + Math.Sin(curAng) * radius);
                curCoords[perpAxis] += (float)perpSeg;
                GenerateLine(0xa04010, curInternLine);
            }
        }

        Wire3D.WireVertex [] getReversedArray() // transparency looks better when reversed
        {
            Wire3D.WireVertex[] res = new Wire3D.WireVertex[verts.Count];
            for (int i = 0; i < res.Length; i++)
                res[res.Length - i - 1] = verts[i];
            return res;
        }

        void GeneratePath()
        {
            verts = new List<Wire3D.WireVertex>();
            SetCoords(minCoords, 999999, NUM_POS_COORDS);
            SetCoords(maxCoords, -999999, NUM_POS_COORDS);
            planeAxis1 = pX;
            planeAxis2 = pY;
            for (int i = 0; i < lines.Length; i++)
            {
                curInternLine = i;
                string line = lines[i];
                if (line[0] == '(')
                    continue; // remark
                // backup coords
                CopyCoords(lastCoords, curCoords);
                List<GToken> tokens = TokenizeLine(line);
                ParseTokens(tokens);
                for (int j = 0; j < NUM_POS_COORDS; j++) if (maxCoords[j] < curCoords[j]) maxCoords[j] = curCoords[j];
                for (int j = 0; j < NUM_POS_COORDS; j++) if (minCoords[j] > curCoords[j]) minCoords[j] = curCoords[j];
                UpdateCoordinates();
                if (isCoordChange())
                {
                    switch (motionType)
                    {
                        case MotionType.Line: GenerateLine(0xa04040, curInternLine); break;
                        case MotionType.Rapid: GenerateLine(0x40a040, curInternLine); break;
                        case MotionType.ArcCW: GenerateArc(true); break;
                        case MotionType.ArcCCW: GenerateArc(false); break;
                    }
                }
            }
            wirepath.Init(getReversedArray());
        }

        public void ResetGcodeLine()
        {
            sendLineIx = -1;
        }

        bool IsSupportedGcode(string line)
        {
            if (line.Length < 1)
                return false;
            if (line.Contains('T'))
                return false;
            if (line.Contains("G43"))
                return false;
            if (line[0] == '%' || line[0] == '(')
                return false;
            return true;
        }

        // compiled line is a line where the gcode N var is the line in the file.
        // if there is an existing N var, it will be replaced
        public string GetNextCompiledLine()
        {
            if (lines == null) 
                return null;
            string gcommand = "";
            while (sendLineIx < lines.Length)
            {
                sendLineIx++;
                if (sendLineIx >= lines.Length)
                    return null;
                gcommand = lines[sendLineIx];
                if (IsSupportedGcode(gcommand))
                    break;
            }

            bool nstate = false;
            bool commentstate = false;
            StringBuilder sb = new StringBuilder();
            sb.Append("N");
            sb.Append(sendLineIx + 1);
            sb.Append(" ");
            foreach (char ch in gcommand)
            {
                char uch = ch.ToString().ToUpper()[0];
                if (!commentstate)
                {
                    if (nstate == false)
                        nstate = (uch == 'N');
                    else
                    {
                        if (!(uch == '.' || (uch >= '0' && uch <= '9') || uch == ' '))
                            nstate = false;
                    }
                    if (!nstate)
                        sb.Append(uch);
                }
            }
            return sb.ToString();
        }

        public void Render()
        {
            wirepath.Render();
        }

        public void SetHighlightedGCode(int gline, bool hidePrevLines = false)
        {
            if (wirepath == null)
                return;
            wirepath.SetHighlightIndex(gline);
            if (hidePrevLines)
                wirepath.SetChangeIndex(gline);
        }

        // simulation
        #region Simulation


        public void InitSimulation()
        {
            simLocation = 0;
            simLocPart = 0;
            simLocPart = 0;
            simTargetDist = 0;
            simGcodeLine = 0;
            simTargetPos = new Vector3(0, 0, 0);
        }

        public bool StepSimulation(out Vector3 headLocation, out int gcodeLine)
        {
            simLocPart += simSpeed;
            while (simLocPart > simTargetDist)
            {
                if (simLocation >= verts.Count)
                {
                    headLocation = simTargetPos;
                    gcodeLine = simGcodeLine;
                    return true; // simulation ended
                }
                simLocPart -= simTargetDist;
                simStartSeg = simTargetPos;
                Wire3D.WireVertex vert = verts[simLocation];
                simTargetPos = new Vector3(vert.x, vert.y, vert.z);
                simGcodeLine = (int)(vert.index + 0.2);
                simDir = simTargetPos - simStartSeg;
                simTargetDist = simDir.Length;
                simLocation++;
            }
            headLocation = simStartSeg + (simLocPart / simTargetDist) * simDir;
            gcodeLine = simGcodeLine;
            return false;
        }


        #endregion
    }
}
