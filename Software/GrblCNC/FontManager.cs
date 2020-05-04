using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Text;
using GrblCNC.Properties;

namespace GrblCNC
{
    class FontManager
    {
        static PrivateFontCollection pfc = new PrivateFontCollection();
        static FontFamily familyFixed = null;
        static Font gcodeFont = null;
        static Bitmap bitmapFont;

        public static FontFamily GetResourcefont(byte[] fontfile)
        {
            int len = pfc.Families.Length;
            unsafe
            {
                fixed (byte* pFontData = fontfile)
                {
                    pfc.AddMemoryFont((System.IntPtr)pFontData, fontfile.Length);
                }
            }
            return pfc.Families[len];
        }

        public static FontFamily FamilyFixed
        {
            get
            {
                if (familyFixed == null)
                    familyFixed = GetResourcefont(Resources.JetBrainsMono_Medium);
                return familyFixed;
            }
        }

        public static Font GcodeFont
        {
            get
            {
                if (gcodeFont == null)
                    gcodeFont = new Font(FamilyFixed, 10);
                return gcodeFont;
            }
        }

        public static Bitmap BitmapFont
        {
            get
            {
                if (bitmapFont != null)
                    return bitmapFont;
                bitmapFont = new Bitmap(512, 512, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                int fontSize = 40;
                int sx = BitmapFont.Width / 16;
                int sy = sx * 2;
                using (Graphics g = Graphics.FromImage(bitmapFont))
                {
                    g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                    g.Clear(Color.Transparent);
                    Font fnt = new Font(FamilyFixed, fontSize);
                    SizeF charSize = g.MeasureString("W", fnt);
                    int offs = (sx - (int)charSize.Width) / 2;
                    for (int i = 0; i < 96; i++)
                    {
                        string stchar = ((char)(i + 32)).ToString();
                        int x = (i % 16) * sx + offs;
                        int y = (i / 16) * sy;
                        g.DrawString(stchar, fnt, Brushes.White, x, y);
                    }
                }
                //bitmapFont.Save("bmpFont.png", System.Drawing.Imaging.ImageFormat.Png);
                return bitmapFont;
            }
        }
    }
}
