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

        public static FontFamily GetResourcefont(byte[] fontfile)
        {
            // load from resource
            /*File.WriteAllBytes("tfont.ttf", Resources.ResourceManager.GetObject(fontName);
            Stream fontStream = typeof(Glutils).Assembly.GetManifestResourceStream(fontName);

              byte[] fontdata = new byte[fontStream.Length];
              fontStream.Read(fontdata,0,(int)fontStream.Length);
              fontStream.Close();*/
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

        //pfc.AddFontFile("C:\\Path To\\YourFont.ttf");
        //label1.Font = new System.Drawing.Font(pfc.Families[0], 16, FontStyle.Regular);


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

    }
}
