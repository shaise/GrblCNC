using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace GrblCNC.Glutils
{
    class Overlay
    {
        Shader shader;
        Texture texture;
        public Bitmap bitmap;
        int vertexBufferObject = -1;
        int vertexArrayObject = -1;
        float [] verts;

        public void Init(int x, int y)
        {
            CleanObject();
            bitmap = new Bitmap(x, y, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.FillRectangle(Brushes.Beige, 20, 20, 20, 20);
            }
            texture = new Texture(bitmap);
            shader = Shader.GetShader(Shader.ShadingType.Textured2D);
            vertexBufferObject = GL.GenBuffer();

            verts = new float[] {
                -1,  1, 0, 0, 0, // top left
                -1, -1, 0, 0, 1, // bottom left
                 1,  1, 0, 1, 0, // top right
                 1,  1, 0, 1, 0, // top right
                -1, -1, 0, 0, 1, // bottom left
                 1, -1, 0, 1, 1, // bottom right
            };

            // VAO
            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            // VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, verts.Length * sizeof(float), verts, BufferUsageHint.DynamicDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
        }


        public void UpdateVerts(int width, int height)
        {
            float maxx = (float)bitmap.Width * 2 / width - 1;
            float maxy = 1 - (float)bitmap.Height * 2 / height;
            verts[6] = verts[21] = verts[26] = maxy;
            verts[10] = verts[15] = verts[25] = maxx;
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)0, verts.Length * sizeof(float), verts);
        }

        public void Render()
        {
            shader.Use();
            //if (tintVar != null)
            shader.SetVector4("tintColor", Utils.ColorToVector("ffffffff"));
            texture.Use();
            GL.BindVertexArray(vertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }

        public Graphics GetOverlayGraphics()
        {
            return Graphics.FromImage(bitmap);
        }

        public void UpdateOverlay()
        {
            texture.UpdateImage(bitmap);
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
