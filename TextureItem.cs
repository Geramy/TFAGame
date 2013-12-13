using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace TFAGame
{
    public class TextureItem
    {
        public static int RatioSize = 64;
        private TextureID textureID = null;
        Bitmap img = null;

        public int PosX = 0;
        public int PosY = 0;

        public int SizeX = 0;
        public int SizeY = 0;

        public TextureItem(Bitmap Img, GameObject.Type type)
        {
            img = Img;
            textureID = GameCore.CoreFiles.TextureIDs[type];
            SizeX = img.Size.Width;
            SizeY = img.Size.Height;
            
        }

        public void MultiplySize(int amount)
        {
            SizeX *= amount;
            SizeY *= amount;
        }

        public void DivideSize(int amount)
        {

        }

        public void DrawTexture()
        {
            GL.PushMatrix();
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, textureID.textureID);
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0, 0); GL.Vertex2(PosX, PosY);
            GL.TexCoord2(0, 1); GL.Vertex2(PosX, PosY + SizeY);
            GL.TexCoord2(1, 1); GL.Vertex2(PosX + SizeX, PosY + SizeY);
            GL.TexCoord2(1, 0); GL.Vertex2(PosX + SizeX, PosY);
            GL.End();
            GL.PopMatrix();
        }
    }
}
