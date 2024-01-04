using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.IO;
using GrblCNC.Properties;
using OpenTK;
using System.Drawing.Imaging;
using System.Globalization; 
namespace GrblCNC.Glutils
{
    class Utils
    {
        public static void SetColor(string webColor, float [] buff, int offs = 0, bool isAlpha = true)
        {
            if (webColor == null || webColor.Length < 2)
                return;
            float [] c = new float [4];
            int i;
            for (i = 0; i < 4; i ++)
            {
                if (webColor.Length < (i + 1) * 2)
                    break;
                buff[offs + i] = (float)(int.Parse(webColor.Substring(i * 2, 2), System.Globalization.NumberStyles.AllowHexSpecifier)) / 255f;
            }
            if (i < 3)
            {
                // mono color
                if (isAlpha)
                    buff[offs + 3] = i > 1 ? buff[offs + 1] : 1f;
                buff[offs + 1] = buff[offs + 2] = buff[offs];
            }
            else if (i < 4 && isAlpha)
                buff[offs + 3] = 1f;
        }

        public static void SetColor3(string webColor, float [] buff, int offs = 0)
        {
            SetColor(webColor, buff, offs, false);
        }

        public static Vector4 ColorToVector(string webColor)
        {
            float[] fcol = new float[4];
            Utils.SetColor(webColor, fcol);
            return new Vector4(fcol[0], fcol[1], fcol[2], fcol[3]);
        }

        public static Vector3 ColorToVector3(string webColor)
        {
            float[] fcol = new float[4];
            Utils.SetColor(webColor, fcol);
            return new Vector3(fcol[0], fcol[1], fcol[2]);
        }

        public static float GetFontAccent(Font font)
        {
            float ascent = font.FontFamily.GetCellAscent(FontStyle.Regular);
            float ascentPixel = font.Height * ascent / font.FontFamily.GetEmHeight(FontStyle.Regular);
            return ascentPixel;
        }

        public static Bitmap ColorizeBitmap(Bitmap srcbmp, Color col)
        {
            if (srcbmp == null)
                return null;
            Bitmap cbmp = new Bitmap(srcbmp.Width, srcbmp.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(cbmp))
            {
                ColorMatrix matrix = new ColorMatrix();
                matrix.Matrix00 = (float)col.R / 255;
                matrix.Matrix11 = (float)col.G / 255;
                matrix.Matrix22 = (float)col.B / 255;
                matrix.Matrix33 = (float)col.A / 255;
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                g.DrawImage(srcbmp, new Rectangle(0, 0, cbmp.Width, cbmp.Height),
                    0, 0, srcbmp.Width, srcbmp.Height, GraphicsUnit.Pixel, attributes);
            }
            return cbmp;
        }

        public static string ToInvariantString(double value, string format)
        {
            return value.ToString(format, CultureInfo.InvariantCulture);
        }

        // convert float to string with 3 decimal digits
        public static string F3(double value)
        {
            return ToInvariantString(value, "0.000");
        }

        public static float ParseFloatInvariant(string floatNum)
        {
            return float.Parse(floatNum, CultureInfo.InvariantCulture);
        }

        static int TuneColorElement(int cElement, float tune)
        {
            int res = (int)((float)cElement * tune);
            if (res < 0)
                res = 0;
            if (res > 255)
                res = 255;
            return res;
        }

        public static Color TuneColor(Color baseColor, float tune)
        {
            int r = TuneColorElement(baseColor.R, tune);
            int g = TuneColorElement(baseColor.G, tune);
            int b = TuneColorElement(baseColor.B, tune);
            return Color.FromArgb(baseColor.A, r, g, b);
        }

        public static void ErrorBox(string message, string title = "Error")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static string SettingPath(string fileName)
        {
            return Global.SettingsPath + Path.PathSeparator + fileName;
        }
    }
}
