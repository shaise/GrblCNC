using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using GrblCNC.Glutils;
using GrblCNC.Properties;

namespace GrblCNC
{
    public partial class VisualizerWin : GLControl
    {
        bool loaded = false;
        ColoredQuad coloredQuad;
        Obj3DFloor floor;
        //Wire3D gcodepath;
        Matrix4 cam;
        Shader texShade, flatShade, normShade, lineShade;
        bool mousedown = false;
        int mouseX, mouseY;
        float xshift = 0;
        Vector3 camTarget;
        float camDist, camAngleHor, camAngleVert;
        public GcodeInterp ginterp;
        MillHead3D millHead;
        float screenAspect = 1;
        GCodeDimensions gcodeDim;

        public delegate void NewGcodeLoadedDelegate(object sender, string[] lines);
        public event NewGcodeLoadedDelegate NewGcodeLoaded;
        

        public VisualizerWin()
            : base(new OpenTK.Graphics.GraphicsMode(32, 24, 0, 8))
        {
            InitializeComponent();
        }    

        void UpdateCamera()
        {
            if (camAngleVert > 79)
                camAngleVert = 79;
            if (camAngleVert < -79)
                camAngleVert = -79;
            if (camAngleHor >= 360)
                camAngleHor -= 360;
            if (camAngleHor < 0)
                camAngleHor += 360;
            if (camDist < 5)
                camDist = 5;
            double angvertrad = camAngleVert * Math.PI / 180;
            double anghorrad = camAngleHor * Math.PI / 180;
            double distH = camDist * Math.Cos(angvertrad);
            double z = camDist * Math.Sin(angvertrad);
            double x = distH * Math.Sin(anghorrad);
            double y = -distH * Math.Cos(anghorrad);
            Vector3 eye = new Vector3((float)x, (float)y, (float)z);
            eye = eye + camTarget;
            cam = Matrix4.LookAt(eye, camTarget, new Vector3(0.0f, 0.0f, 1.0f));
            Matrix4 trans = Matrix4.CreateTranslation(xshift, 0, 0);
            cam = cam * trans;
            texShade.SetMatrix4("view", cam);
            flatShade.SetMatrix4("view", cam);
            normShade.SetMatrix4("view", cam);
            lineShade.SetMatrix4("view", cam);
            UpdateProjection();
        }
        
        void UpdateProjection()
        {
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), screenAspect, 0.1f, 1000.0f);
            texShade.SetMatrix4("projection", projection);
            flatShade.SetMatrix4("projection", projection);
            normShade.SetMatrix4("projection", projection);
            lineShade.SetMatrix4("projection", projection);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (Site != null && Site.DesignMode) return;
            MakeCurrent();
            Context.LoadAll();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.ProvokingVertex(ProvokingVertexMode.FirstVertexConvention);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, 
                (int)TextureMagFilter.Nearest); 
            coloredQuad = new ColoredQuad("d495f8", "ca7ff5", "bf67f2", "b54fef");
            floor = new Obj3DFloor(20);
            gcodeDim = new GCodeDimensions();
            millHead = new MillHead3D();
            //gcodepath = new Wire3D();
            //gcodepath.Init(new float[] { 0, 0, 0, 0, 0, 10, 10, 10, 10, 10, 10, 20 });
            texShade = Shader.GetShader(Shader.ShadingType.Textured3D);
            flatShade = Shader.GetShader(Shader.ShadingType.Wire3D);
            normShade = Shader.GetShader(Shader.ShadingType.FlatNorm3D);
            lineShade = Shader.GetShader(Shader.ShadingType.Line3D);
            GL.Viewport(0, 0, Width, Height);
            camTarget = new Vector3(0.0f, 0.0f, 10.0f);
            camDist = 50;
            camAngleHor = 0;
            camAngleVert = 20;
            screenAspect = (float)Width / (float)Height;
            UpdateCamera();
            LoadGcodeFile(null);
            base.OnLoad(e);
            loaded = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Site != null && Site.DesignMode)
            {
                e.Graphics.Clear(Color.Aqua);
                return;
            }
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            coloredQuad.Render();
            GL.Clear(ClearBufferMask.DepthBufferBit);    // clearing again after paintin bgnd
            floor.Render();
            //gcodepath.Render();
            millHead.Render();
            ginterp.Render();
            gcodeDim.Render();
            Context.SwapBuffers();
            //e.Graphics.DrawLine(Pens.Black, 0, 0, 100, 100);
            base.OnPaint(e);
        }

        protected override void OnResize(EventArgs e)
        {
            if (Site != null && Site.DesignMode) return;
            if (!loaded) return;

            //MakeCurrent();
            GL.Viewport(0, 0, Width, Height);
            screenAspect = (float)Width / (float)Height;
            UpdateProjection();
            base.OnResize(e);
        }
        

        protected override void OnHandleDestroyed(System.EventArgs e)
        {
            if (Site != null && Site.DesignMode) return;
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            coloredQuad.Dispose();
            base.OnHandleDestroyed(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            /*cam = rotr * cam;
            texShade.SetMatrix4("view", cam);
            base.OnMouseClick(e);
            Invalidate();*/
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            mousedown = true;
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            mousedown = false;
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            int diffx = (mouseX - e.X) / 2;
            int diffy = (mouseY - e.Y) / 2;
            mouseY = e.Y;
            mouseX = e.X;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                camAngleHor += diffx;
                camAngleVert -= diffy;
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
               xshift -= diffx;
            }
            UpdateCamera();
            base.OnMouseMove(e);
            Invalidate();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            camDist += e.Delta > 0 ? 5 : -5;
            UpdateCamera();
            base.OnMouseWheel(e);
            Invalidate();
        }

        public string LoadGcodeFile(string fileName)
        {
            string res = ginterp.LoadGcode(fileName);
            if (res != "OK")
                return res;
            gcodeDim.Init(ginterp.minCoords[GcodeInterp.pX], ginterp.minCoords[GcodeInterp.pY], ginterp.minCoords[GcodeInterp.pZ],
                          ginterp.maxCoords[GcodeInterp.pX], ginterp.maxCoords[GcodeInterp.pY], ginterp.maxCoords[GcodeInterp.pZ]);
            floor.SetFloorZ(ginterp.minCoords[GcodeInterp.pZ] - 0.5f);
            if (NewGcodeLoaded != null)
                NewGcodeLoaded(this, ginterp.lines);
            Invalidate();
            return "OK";
        }

        public void SetMillheadPos(Vector3 pos)
        {
            millHead.SetHeadPosition(pos);
            Invalidate();
        }
    }
}
