using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using GrblCNC.Properties;

namespace GrblCNC.Glutils
{
    class Line3D
    {
        protected Shader shader;

        int vertexBufferObject = -1;
        int vertexArrayObject = -1;
        int nVerts = 0;
        public Matrix4 objPos;
        public float lineThickness;
        List<LineVerts> lines;

        const int sizeofLine = 8 * sizeof(float);
        const int sizeofVertex = 4 * sizeof(float);
        public struct LineVerts
        {
            public float x1, y1, z1;
            public float col1; // Fixme: using floats as I cant find a way to pass ints
            public float x2, y2, z2;
            public float col2;
            public LineVerts(float _x1, float _y1, float _z1, float _x2, float _y2, float _z2, int col)
            {
                x1 = _x1;
                y1 = _y1;
                z1 = _z1;
                x2 = _x2;
                y2 = _y2;
                z2 = _z2;
                col1 = col2 = col;
            }
        }

        public Line3D()
        {
            lines = new List<LineVerts>();
        }

        public void Clear()
        {
            lines.Clear();
        }

        public void AddLine(float x1, float y1, float z1, float x2, float y2, float z2, int col)
        {
            lines.Add(new LineVerts(x1, y1, z1, x2, y2, z2, col));
        }

        public void Init()
        {
            CleanObject();
            LineVerts[] lv = lines.ToArray();
            shader = Shader.GetShader(Shader.ShadingType.Line3D);
            objPos = Matrix4.Identity;
            lineThickness = 2;
            nVerts = lv.Length * 2;
            vertexBufferObject = GL.GenBuffer();

            // VAO
            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            // VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, lv.Length * sizeofLine, lv, BufferUsageHint.StaticDraw);
            //GL.BufferData(BufferTarget.ArrayBuffer, fverts.Length * sizeof(float), fverts, BufferUsageHint.StaticDraw);
            // set data strides
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeofVertex, 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 1, VertexAttribPointerType.Float, false, sizeofVertex, 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
        }

        public void Render()
        {
            if (lines.Count == 0)
                return;
            shader.Use();
            shader.SetMatrix4("model", objPos);
            GL.LineWidth(lineThickness);
            GL.BindVertexArray(vertexArrayObject);
            GL.DrawArrays(PrimitiveType.Lines, 0, nVerts);
        }

        void CleanObject()
        {
            if (vertexBufferObject >= 0)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.DeleteBuffer(vertexBufferObject);
                vertexBufferObject = -1;
            }
            if (vertexArrayObject >= 0)
            {
                GL.BindVertexArray(0);
                GL.DeleteVertexArray(vertexArrayObject);
                vertexArrayObject = -1;
            }
        }

        public void Dispose()
        {
            CleanObject();
        }
    }
}
