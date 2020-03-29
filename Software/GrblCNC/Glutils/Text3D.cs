using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;

namespace GrblCNC.Glutils
{
    class Text3D
    {
        List<Obj3DText> texts;
        Texture texture;

        public Text3D(Bitmap bmpFont)
        {
            texture = new Texture(bmpFont);
            texts = new List<Obj3DText>();
        }

        public void AddText(string txt, Obj3DText.TextPlane plane, float size, float posX, float posY, float posZ, float dirAngle, string color)
        {
            texts.Add(new Obj3DText(txt, plane, size, posX, posY, posZ, dirAngle, color, texture));
        }

        public void Clear()
        {
            texts.Clear();
        }

        public void Render()
        {
            foreach (Obj3DText otxt in texts)
                otxt.Render();
        }
    }
}
