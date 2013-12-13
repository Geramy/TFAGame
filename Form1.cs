using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using TFAGame.UI;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace TFAGame
{
    public partial class Form1 : Form
    {
        public Graphics graphics = null;
        private GameGenerator gGen = null;
        private Timer drawScreen = new Timer();
        private bool bOpenGLReady = false;
        
        public Form1()
        {
            InitializeComponent();
            Config.InitializeSettings();
            //Put graphics here
            DoubleBuffered = true;
            Text = "The Footman Adventure";
            Size = new Size(800, 650);
            gGen = new GameGenerator();
            GameCore.Size = Size;
            GameCore.gGen = gGen;
            drawScreen.Tick += new EventHandler(drawScreen_Tick);
            drawScreen.Interval = 30;
            drawScreen.Start();
            GameCore.SetupGCMode();
            //GameCore = new GameCore();
        }

        void drawScreen_Tick(object sender, EventArgs e)
        {
            glControl.Invalidate();
        }

        private void start_game_Click(object sender, EventArgs e)
        {
            
        }

        private void end_game_Click(object sender, EventArgs e)
        {

        }

        private void host_game_Click(object sender, EventArgs e)
        {

        }

        private void GLScreen_Load(object sender, EventArgs e)
        {
            bOpenGLReady = true;
            SetupViewport();
            
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Disable(EnableCap.DepthTest);
            GameCore.CoreFiles = new Memory();

            GameObject Grass = new GameObject(GameObject.Type.Grass);
            int MaxX = Size.Width / TextureItem.RatioSize;
            int MaxY = Size.Height / TextureItem.RatioSize;
            GameCore.GrassPatches = new GameObject[MaxX * MaxY];
            int i = 0;
            for (int x = 0; x < MaxX; x++)
            {
                for (int y = 0; y < MaxY; y++)
                {
                    GameCore.GrassPatches[i] = new GameObject(GameObject.Type.Grass);
                    GameCore.GrassPatches[i].X = x * TextureItem.RatioSize;
                    GameCore.GrassPatches[i].Y = y * TextureItem.RatioSize;
                    i++;
                }
            }
            GameCore.SetupModules();
        }

        private void GLScreen_Paint(object sender, PaintEventArgs e)
        {
            if (!bOpenGLReady)
                return;
            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Color4(Color.White);

            GL.LoadIdentity();
            GL.Translate(0, 0, 0);
            
            for (int i = 0; i < GameCore.GrassPatches.Length; i++)
                GameCore.GrassPatches[i].DrawObject(true);
            GL.PushMatrix();
            ShouldTranslate();
            
            GameCore.DrawPlayers();
            GameCore.DrawStatusBars();
            GL.PopMatrix();
            GameCore.DrawUI(800, 600);
            //GL.Flush();
            glControl.SwapBuffers();
        }
        bool FirstLoad = true;
        public void ShouldTranslate()
        {
            GL.Translate(3, 3, 0);
            //if (FirstLoad)
            //{
            //    GL.Translate(3, 3, 0);
            //    FirstLoad = false;
            //}
        }

        private void LoadGrassTexture()
        {

        }

        private void GLScreen_Resize(object sender, EventArgs e)
        {
            if (!bOpenGLReady)
                return;

            SetupViewport();
        }

        private void SetupViewport()
        {
            int w = glControl.Width;
            int h = glControl.Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, w, h, 0, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
            GL.Viewport(0, 0, w, h); // Use all of the glControl painting area
            GL.MatrixMode(MatrixMode.Modelview);
        }

        private void glControl_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void glControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModuleHandler._CoreModules.Keys.Contains("UITextures"))
            {
                CoreModule coreModule = null;
                ModuleHandler._CoreModules.TryGetValue("UITextures", out coreModule);
                UICore coreMod = (UICore)coreModule;
                coreMod.OnClick(e.X, e.Y);
            }
            GameCore.MouseDown(e.X, e.Y);
        }

        private void glControl_KeyUp(object sender, KeyEventArgs e)
        {
            GameCore.KeyUp(e);
        }
    }
}
