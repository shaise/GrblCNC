using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrblCNC.Glutils;
using OpenTK;

namespace GrblCNC
{
    class MillHead3D : Object3D
    {
        const int resolution = 24;
        const float height = 10;
        const float radiust = 2;
        const float radiusb = 0.1f;

        Vector3 lightColor;
        Vector3 objectColor;
        Vector3 ambientColor;
        Vector3 lightPos = new Vector3(-500, -300, 300);
        

        public MillHead3D()
        {
            GenerateHead();
            SetColors("FFFFFF", "b54fef", "600000");
        }

        public void SetColors(string lampcol, string objcol, string ambcolor)
        {
            if (lampcol != null)
                lightColor = Utils.ColorToVector3(lampcol);
            if (objcol != null)
                objectColor = Utils.ColorToVector3(objcol);
            if (ambcolor != null)
                ambientColor = Utils.ColorToVector3(ambcolor);
        }

        void GenerateHead()
        {
            // the head is a simple cone. but we use n points in the tip, so normal vector will be correct
            float[] verts = new float[(3 * resolution + 1) * 6];
            double angstep = 2 * Math.PI / resolution;
            double ang = 0;
            float raddiff = radiust - radiusb;
            float sidelen = (float)Math.Sqrt(height * height + raddiff * raddiff);
            // side triangles
            int p = 0;
            for (int i = 0; i < resolution; i++)
            {
                float cosAng = (float)Math.Cos(ang);
                float sinAng = (float)Math.Sin(ang);
                // top vertex coord
                verts[p++] = radiust * cosAng;
                verts[p++] = radiust * sinAng;
                verts[p++] = height;
                // top vertex normal
                float lxy = height / sidelen;
                int vPos = p; // save normal location
                verts[p++] = lxy * sinAng;
                verts[p++] = lxy * cosAng;
                verts[p++] = -raddiff / sidelen;

                // bottom vertex
                verts[p++] = radiusb * cosAng;
                verts[p++] = radiusb * sinAng;
                verts[p++] = 0;
                // bottom normal same as top normal
                verts[p++] = verts[vPos++];
                verts[p++] = verts[vPos++];
                verts[p++] = verts[vPos++];

                ang += angstep;
            }
           
            // top triangles. 
            ang = 0;
            for (int i = 0; i < resolution; i++)
            {
                // circ vertex 
                verts[p++] = (float)(radiust * Math.Cos(ang));
                verts[p++] = (float)(radiust * Math.Sin(ang));
                verts[p++] = height;
                // circ vertex normal (simple top)
                verts[p++] = 0;
                verts[p++] = 0;
                verts[p++] = 1;
                ang += angstep;
            }

            // and the top center vetrex
            verts[p++] = 0;
            verts[p++] = 0;
            verts[p++] = height;
            // circ vertex normal (simple top)
            verts[p++] = 0;
            verts[p++] = 0;
            verts[p++] = 1;

            // generate indices, there are resolution * 3 triangles
            uint[] indices = new uint[resolution * 3 * 3];
            // side triangles
            p = 0;
            for (uint i = 0; i < resolution; i++)
            {
                indices[p++] = i * 2;
                indices[p++] = i * 2 + 1;
                indices[p++] = i * 2 + 2;
                indices[p++] = i * 2 + 2;
                indices[p++] = i * 2 + 1;
                indices[p++] = i * 2 + 3;
            }
            // fix the last indices (its back to the first vertex)
            indices[p - 4] = 0;
            indices[p - 3] = 0;
            indices[p - 1] = 1;

            // top triangles
            uint firstind = resolution * 2;
            uint centvert = resolution * 3;
            for (uint i = 0; i < resolution; i++)
            {
                indices[p++] = centvert;
                indices[p++] = i + firstind;
                indices[p++] = i + firstind + 1;
            }
            // fix last index
            indices[p - 1] = firstind;

            Init(Shader.ShadingType.FlatNorm3D, verts, indices);
        }

        public override void Render()
        {
            shader.SetVector3("lightPos", lightPos);
            shader.SetVector3("lightColor", lightColor);
            shader.SetVector3("objectColor", objectColor);
            shader.SetVector3("ambient", ambientColor);
            base.Render();
        }

        public void SetHeadPosition(Vector3 headpos)
        {
            objPos = Matrix4.CreateTranslation(headpos);
        }
        
    }
}
