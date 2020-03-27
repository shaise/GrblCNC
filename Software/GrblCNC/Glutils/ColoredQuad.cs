using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrblCNC.Glutils
{
    class ColoredQuad : Object3D
    {
        public ColoredQuad(string colTL, string colTR, string colBL, string colBR)
        {
            float[] verts = {-1,  1, 0, 0, 0, 0,
                              1,  1, 0, 0, 0, 0,
                             -1, -1, 0, 0, 0, 0,
                              1, -1, 0, 0, 0, 0};
            uint [] inds = { 0, 1, 2, 2, 1, 3 };
            Glutils.SetColor3(colTL, verts, 3);
            Glutils.SetColor3(colTR, verts, 9);
            Glutils.SetColor3(colBL, verts, 15);
            Glutils.SetColor3(colBR, verts, 21);
            Init(Shader.ShadingType.MultiColor2D, verts, inds);                        
        }
    }
}
