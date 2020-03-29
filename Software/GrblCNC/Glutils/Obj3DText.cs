using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;

namespace GrblCNC.Glutils
{
    class Obj3DText : Object3D
    {
        public enum TextPlane
        {
            XY,
            XZ,
            ZY
        }

        Vector2 ApplyMatrix(Matrix2 rotMat, Vector2 vec)
        {
            return new OpenTK.Vector2(vec.X * rotMat.M11 + vec.Y * rotMat.M12, vec.X * rotMat.M21 + vec.Y * rotMat.M22);
        }

        Vector3 GetTranslatedPoint(float posx, float posy, float posz, float scale, int cornerx, int cornery, Matrix2 rotMat, TextPlane plane)
        {
            Vector3 vres = new OpenTK.Vector3();
            Vector2 vcorner = new OpenTK.Vector2(cornerx * scale, cornery * scale * 2);
            Vector2 tcorner = ApplyMatrix(rotMat, vcorner);
            switch (plane)
            {
                case TextPlane.XY: vres.X = posx + tcorner.X; vres.Y = posy + tcorner.Y; vres.Z = posz; break;
                case TextPlane.XZ: vres.X = posx + tcorner.X; vres.Y = posy; vres.Z = posz + tcorner.Y; break;
                case TextPlane.ZY: vres.X = posx; vres.Y = posy + tcorner.Y; vres.Z = posz + tcorner.X; break;
            }
            return vres;
        }

        Vector2 GetTextureCorner(char ch, int cornerx, int cornery)
        {
            int c = (int)ch - 32;
            if (c < 0 || c >= 96)
                c = 0; // unknown chard display as space
            float x = ((c % 16) + cornerx) * 0.0625f;
            float y = ((c / 16) + cornery) * 0.125f;
            return new Vector2(x, y);
        }

        void SetVertex(float [] verts, int vert, Vector3 v3p, Vector2 vtp)
        {
            int vp = vert * 5;
            verts[vp++] = v3p.X;
            verts[vp++] = v3p.Y;
            verts[vp++] = v3p.Z;
            verts[vp++] = vtp.X;
            verts[vp++] = vtp.Y;
        }

        public Obj3DText(string txt, TextPlane plane, float size, float posX, float posY, float posZ, float dirAngle, string color, Texture fontTex)
        {
            Matrix2 rotMat = Matrix2.CreateRotation((float)(dirAngle * Math.PI / 180));
            float [] verts = new float[txt.Length * 4 * 5];
            uint [] inds = new uint[txt.Length * 6];
            int vp = 0;
            int ip = 0;
            for (int i=0; i<txt.Length;i++)
            {
                char ch = txt[i];
                uint svp = (uint)vp;  // save first index
                // top left
                Vector3 v3p = GetTranslatedPoint(posX, posY, posZ, size, i, 0, rotMat, plane); // 3d vector
                Vector2 vtp = GetTextureCorner(ch, 0, 0); // texture vector
                SetVertex(verts, vp++, v3p, vtp);
                // top right
                v3p = GetTranslatedPoint(posX, posY, posZ, size, i + 1, 0, rotMat, plane); // 3d vector
                vtp = GetTextureCorner(ch, 1, 0); // texture vector
                SetVertex(verts, vp++, v3p, vtp);
                // bootom left
                v3p = GetTranslatedPoint(posX, posY, posZ, size, i, -1, rotMat, plane); // 3d vector
                vtp = GetTextureCorner(ch, 0, 1); // texture vector
                SetVertex(verts, vp++, v3p, vtp);
                // bootom right
                v3p = GetTranslatedPoint(posX, posY, posZ, size, i + 1, -1, rotMat, plane); // 3d vector
                vtp = GetTextureCorner(ch, 1, 1); // texture vector
                SetVertex(verts, vp++, v3p, vtp);
                
                // indexes, 2 triangles
                inds[ip++] = svp;
                inds[ip++] = svp + 2;
                inds[ip++] = svp + 1;
                inds[ip++] = svp + 2;
                inds[ip++] = svp + 3;
                inds[ip++] = svp + 1;
            }
            Init(Shader.ShadingType.Textured3D, verts, inds, fontTex);
            SetColor(color);
        }
    }
}
