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
    class Wire3D
    {

        protected Shader shader;

        int vertexBufferObject = -1;
        int vertexArrayObject = -1;
        int elementBufferObject = -1;
        int nLines = 0;
        bool useIndices = false;
        public Matrix4 objPos;
        public float lineThickness;

        const int sizeofVertex = 5 * sizeof(float);
        public struct WireVertex
        {
            public float x, y, z;
            public float index, color; // Fixme: using floats as I cant find a way to pass ints
            public WireVertex(float _x, float _y, float _z, int ind, int col)
            {
                x = _x;
                y = _y;
                z = _z;
                index = ind;
                color = col;
            }
        }


        public void Init(WireVertex [] verts, uint[] indices = null)
        {
            CleanObject();
            shader = Shader.GetShader(Shader.ShadingType.Wire3D);
            //shader.SetUint("changeIndex", 0);
            SetChangeIndex(0);
            objPos = Matrix4.Identity;
            lineThickness = 2;
            nLines = useIndices ? indices.Length : verts.Length;
            vertexBufferObject = GL.GenBuffer();

            // VAO
            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);
            
            // VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, verts.Length * sizeofVertex, verts, BufferUsageHint.StaticDraw);
            //GL.BufferData(BufferTarget.ArrayBuffer, fverts.Length * sizeof(float), fverts, BufferUsageHint.StaticDraw);
            // set data strides
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeofVertex, 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, sizeofVertex, 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // EBO
            if (indices != null)
            {
                useIndices = true;
                elementBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            }
        }

        public void SetChangeIndex(int changeIndex)
        {
            changeIndex &= 0xFFFFFF;
            shader.SetFloat("changeIndex", (float)changeIndex);
        }

        public void SetHighlightIndex(int idx)
        {
            idx &= 0xFFFFFF;
            shader.SetFloat("hltIndex", (float)idx);
        }

        public void Render()
        {
            shader.Use();
            shader.SetMatrix4("model", objPos);
            GL.LineWidth(lineThickness);
            GL.BindVertexArray(vertexArrayObject);
            if (useIndices)
                GL.DrawElements(PrimitiveType.Lines, nLines, DrawElementsType.UnsignedInt, 0);
            else
                GL.DrawArrays(PrimitiveType.LineStrip, 0, nLines);
        }

        void CleanObject()
        {
            useIndices = false;
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
            if (elementBufferObject >= 0)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                GL.DeleteBuffer(elementBufferObject);
                elementBufferObject = -1;
            }
        }

        public void Dispose()
        {
            CleanObject();
        }
    }
}
