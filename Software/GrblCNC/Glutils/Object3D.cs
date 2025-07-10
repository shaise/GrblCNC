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
    class Object3D
    {

        protected Shader shader;
        Texture texture;
        int vertexBufferObject = -1;
        int vertexArrayObject = -1;
        int elementBufferObject = -1;
        int nTriangles = 0;
        bool usetexture = false;
        public Matrix4 objPos;
        bool is3D = false;
        bool useIndices = false;
        string tintVar = null;
        Vector4 tintColor;


        public void Init(Shader.ShadingType type, float [] verts, uint [] indices, Texture texture)
        {
            CleanObject();
            shader = Shader.GetShader(type);
            objPos = Matrix4.Identity;
            tintColor = Utils.ColorToVector("ffffffff");
            vertexBufferObject = GL.GenBuffer();
            if (indices != null)
            {
                useIndices = true;
                elementBufferObject = GL.GenBuffer();
            }

            // calculate strides strides
            int stride = 3, size1 = 0;
            switch (type)
            {
                case Shader.ShadingType.Flat2D: break;
                case Shader.ShadingType.MultiColor2D: size1 = 3; break;
                case Shader.ShadingType.Textured2D: size1 = 2; break;
                case Shader.ShadingType.Textured3D: size1 = 2; is3D = true; usetexture = true; tintVar = "tintColor"; break;
                case Shader.ShadingType.Flat3D: is3D = true; break;
                case Shader.ShadingType.FlatNorm3D: size1 = 3; is3D = true; break;
            }
            stride += size1;
            nTriangles = useIndices ? indices.Length : verts.Length / stride;

            // VAO
            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            // VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, verts.Length * sizeof(float), verts, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            if (size1 > 0)
            {
                GL.VertexAttribPointer(1, size1, VertexAttribPointerType.Float, false, stride * sizeof(float), 3 * sizeof(float));
                GL.EnableVertexAttribArray(1);
            }

            if (useIndices)
            {
                // EBO
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            }

            if (usetexture)
                this.texture = texture;
        }

        public void Init(Shader.ShadingType type, float[] verts, uint[] indices = null, string textureName = "")
        {
            Texture tex = null;
            // texture
            if (textureName != null && textureName.Length > 0)
                tex = Texture.From(textureName);
            Init(type, verts, indices, tex);
        }

        public void SetColor(string color)
        {
            tintColor = Utils.ColorToVector(color);
        }

        public virtual void Render()
        {
            shader.Use();
            if (tintVar != null)
                shader.SetVector4(tintVar, tintColor);
            if (is3D)
                shader.SetMatrix4("model", objPos);
            if (usetexture)
            {
                shader.SetInt("texture0", 0);
                texture.Use();
            }
            GL.BindVertexArray(vertexArrayObject);
            if (useIndices)
                GL.DrawElements(PrimitiveType.Triangles, nTriangles, DrawElementsType.UnsignedInt, 0);
            else
                GL.DrawArrays(PrimitiveType.Triangles, 0, nTriangles);
        }

        void CleanObject()
        {
            usetexture = false;
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
