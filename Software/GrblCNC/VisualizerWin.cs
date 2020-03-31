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
        bool mousedown = false;
        int mouseX, mouseY;
        public GcodeInterp ginterp;
        MillHead3D millHead;
        float screenAspect = 1;
        GCodeDimensions gcodeDim;
        Camera camera;

        public delegate void NewGcodeLoadedDelegate(object sender, string[] lines);
        public event NewGcodeLoadedDelegate NewGcodeLoaded;
        

        public VisualizerWin()
            : base(new OpenTK.Graphics.GraphicsMode(32, 24, 0, 8))
        {
            InitializeComponent();
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
            GL.Viewport(0, 0, Width, Height);
            screenAspect = (float)Width / (float)Height;
            camera = new Camera(screenAspect);
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
            camera.UpdateProjection(screenAspect);
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
            float diffx = (float)(mouseX - e.X) / 2;
            float diffy = (float)(mouseY - e.Y) / 2;
            mouseY = e.Y;
            mouseX = e.X;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                camera.Rotate(diffx, -diffy);
            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
                camera.MoveTarget(diffx / 2, -diffy, 0);
            base.OnMouseMove(e);
            Invalidate();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            camera.Zoom(e.Delta > 0 ? 5 : -5);
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
