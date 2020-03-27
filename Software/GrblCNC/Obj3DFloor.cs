using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrblCNC.Glutils;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace GrblCNC
{
    class Obj3DFloor
    {
        Object3D FCenter;
        Object3D FRuler;
        Object3D FRest;
        float scale = 10;

        public Obj3DFloor(int nUnits)
        {
            // center tile
            FCenter = new Object3D();
            float[] verts = { 
                -1,  1, 0, 0, 0,
                 1,  1, 0, 1, 0,
                -1, -1, 0, 0, 1,
                 1, -1, 0, 1, 1,
            };
            ScaleVerts(verts);

            uint[] inds = { 0, 2, 1, 2, 3, 1 };
            FCenter.Init(Shader.ShadingType.Textured3D, verts, inds, "GridCenter");
 
 
            // ruler tiles
            FRuler = new Object3D();
            float[] vertsr = { 
                 1,  1, 0, 0, 0,                   
                 1 + 2 * nUnits,  1, 0, nUnits, 0, 
                 1, -1, 0, 0, 1,                   
                 1 +  2 * nUnits, -1, 0, nUnits, 1,
                -1,  1 +  2 * nUnits, 0, nUnits, 1,
                 1,  1 +  2 * nUnits, 0, nUnits, 0,
                -1, 1, 0, 0, 1, 
                -1 - 2 * nUnits,  1, 0, nUnits, 1,
                -1 - 2 * nUnits, -1, 0, nUnits, 0,
                -1, -1, 0, 0, 0, 
                -1, -1 - 2 * nUnits, 0, nUnits, 0,
                 1, -1 - 2 * nUnits, 0, nUnits, 1,
            };
            ScaleVerts(vertsr);

            uint[] indsr = { 0, 2, 1, 2, 3, 1, 6, 5, 4, 6, 0, 5, 8, 6, 7, 8, 9, 6, 10, 2, 9, 10, 11, 2 };
            FRuler.Init(Shader.ShadingType.Textured3D, vertsr, indsr, "GridRuler");

            // empty tiles
            FRest = new Object3D();
            float[] vertse = { 
                 1,  1, 0, 0, nUnits,                   
                 1 + 2 * nUnits,  1, 0, nUnits, nUnits, 
                 1, -1, 0, 0, 0,                   
                 1 +  2 * nUnits, -1, 0, nUnits, 0,
                -1,  1 +  2 * nUnits, 0, nUnits, 0,
                 1,  1 +  2 * nUnits, 0, 0, 0,
                -1, 1, 0, nUnits, nUnits, 
                -1 - 2 * nUnits,  1, 0, 0, nUnits,
                -1 - 2 * nUnits, -1, 0, 0, 0,
                -1, -1, 0, nUnits, 0, 
                -1, -1 - 2 * nUnits, 0, nUnits, nUnits,
                 1, -1 - 2 * nUnits, 0, 0, nUnits,
                 1 + 2 * nUnits,  1 + 2 * nUnits, 0, nUnits, 0, 
                -1 - 2 * nUnits,  1 + 2 * nUnits, 0, 0, 0, 
                -1 - 2 * nUnits, -1 - 2 * nUnits, 0, 0, nUnits, 
                 1 + 2 * nUnits, -1 - 2 * nUnits, 0, nUnits, nUnits, 
             };
            ScaleVerts(vertse);
            uint[] indse = { 0, 1, 2, 0, 12, 5, 0, 1, 12, 7, 4, 13, 7, 6, 4, 14, 9, 8, 14, 10, 9, 11, 3, 2, 11, 15, 3 };
            FRest.Init(Shader.ShadingType.Textured3D, vertse, indse, "GridEmpty");
        }

        void ScaleVerts(float [] verts)
        {
            for (int i = 0; i < verts.Length; i += 5)
            {
                verts[i] *= scale;
                verts[i + 1] *= scale;
            }
        }

        public void Render()
        {
            FCenter.Render();
            FRuler.Render();
            FRest.Render();
        }

        public void SetFloorZ(float zHeight)
        {
            Matrix4 flevel = Matrix4.CreateTranslation(0, 0, zHeight);
            FCenter.objPos = flevel;
            FRuler.objPos = flevel;
            FRest.objPos = flevel;
        }
    }
}
