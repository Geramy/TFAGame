using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace TFAGame
{
    public delegate void ObjectCollision(GameObject obj);
    public class GameObject
    {
        public int TeamNumber = 0;
        public event ObjectCollision OnCollision;

        public event ObjectCollision OnDeath;

        public bool Moveable = false;
        public bool AutoRemove = false;
        public TextureItem item = null;

        public float MaxHealth = 0;

        public float Health = 0;
        public float Damage = 0;
        public int Level = 1;
        public float XP = 0;
        public float RequiredXP = 0;

        public int Population = 0;
        public int Feeds = 0;

        public int _X = 0;
        public int _Y = 0;

        public int[] NewXSmooth = null;
        public int[] NewYSmooth = null;
        public int CurrentSmooth = 0;

        public int X {
            get { return item.PosX; }
            set { item.PosX = value; }
        }
        public int Y {
            get { return item.PosY; }
            set { item.PosY = value; }
        }

        public int SizeX
        {
            get { return item.SizeX; }
        }

        public int SizeY
        {
            get { return item.SizeY; }
        }

        public Type ObjType;

        public GameObject(Type gameType)
        {
            ObjType = gameType;
            item = new TextureItem((System.Drawing.Bitmap)GameCore.CoreFiles.getTexture(ObjType).Clone(), gameType);
            switch (ObjType)
            {
                case Type.Barn:
                    Health = 100;
                    Population = 3;
                    break;
                case Type.House:
                    Health = 200;
                    Population = 5;
                    break;
                case Type.Outhouse:
                    Health = 300;
                    Population = 10;
                    break;
                case Type.BigBarn:
                    Health = 400;
                    Population = 30;
                    break;
                case Type.Shop:
                    Health = 250;
                    Population = 10;
                    break;
                case Type.Hay:
                    Health = 50;
                    Feeds = 10;
                    break;
                case Type.Windmil:
                    Health = 100;
                    Population = 3;
                    break;
                case Type.Footman:
                    Health = 250;
                    Damage = 1;
                    RequiredXP = 100;
                    Population = 1;
                    Moveable = true;
                    break;
                case Type.Grass:
                    break;
            }
            MaxHealth = Health;
            Health -= 20;
            OnCollision += new ObjectCollision(GameObject_OnCollision);
        }

        public void DeleteSelf()
        {
            OnDeath(this);
        }

        void GameObject_OnCollision(GameObject obj)
        {
             
        }

        public GameObject(ref TextureItem ti, Type gameType)
        {
            ObjType = gameType;
            item = ti;
        }

        public void DrawHealthBar(int x, int y)
        {
            float healthFraction = Health / MaxHealth;
            Color healthColor = GetColor(healthFraction);

            GL.Disable(EnableCap.Texture2D);

            GL.Begin(BeginMode.Quads);
            GL.Color4(Color.Black);
            GL.TexCoord2(0, 0); GL.Vertex2(x - 2, y);
            GL.TexCoord2(0, 1); GL.Vertex2(x - 2, y + 7);
            GL.TexCoord2(1, 1); GL.Vertex2(x + SizeX + 2, y + 7);
            GL.TexCoord2(1, 0); GL.Vertex2(x + SizeX + 2, y);
            GL.End();

            float newWidth = (Health / MaxHealth) * SizeX;
            float RedEnd = newWidth - SizeX;

            GL.Begin(BeginMode.Quads);
            GL.Color4((byte)225, (byte)1, (byte)1, (byte)255);
            GL.TexCoord2(0, 0); GL.Vertex2(x, y + 2);
            GL.TexCoord2(0, 1); GL.Vertex2(x, y + 5);
            GL.TexCoord2(1, 1); GL.Vertex2(x + SizeX, y + 5);
            GL.TexCoord2(1, 0); GL.Vertex2(x + SizeX, y + 2);
            GL.End();

            GL.Begin(BeginMode.Quads);
            GL.Color4((byte)1, (byte)225, (byte)1, (byte)255);
            GL.TexCoord2(0, 0); GL.Vertex2(x, y + 2);
            GL.TexCoord2(0, 1); GL.Vertex2(x, y + 5);
            GL.TexCoord2(1, 1); GL.Vertex2(x + newWidth, y + 5);
            GL.TexCoord2(1, 0); GL.Vertex2(x + newWidth, y + 2);
            GL.End();
        }

        public void DrawXPBar(int x, int y)
        {

        }

        public Color GetColor(float healthFraction)
        {
            if (healthFraction >= 0.5f)
            {
                float subprogress = (healthFraction - 0.5f) * 2;
                return Lerp(Color.DarkRed, Color.DarkGreen, subprogress);
            }
            else
            {
                float subprogress = healthFraction * 2;
                return Lerp(Color.Red, Color.DarkRed, subprogress);
            }
        }

        public float Lerp(float start, float end, float amount)
        {
            float difference = end - start;
            float adjusted = difference * amount;
            return start + adjusted;
        }

        public Color Lerp(Color colour, Color to, float amount)
        {
            // start colours as lerp-able floats
            float sr = colour.R, sg = colour.G, sb = colour.B;

            // end colours as lerp-able floats
            float er = to.R, eg = to.G, eb = to.B;

            // lerp the colours to get the difference
            byte r = (byte)Lerp(sr, er, amount),
                 g = (byte)Lerp(sg, eg, amount),
                 b = (byte)Lerp(sb, eb, amount);

            // return the new colour
            return Color.FromArgb(r, g, b);
        }

        public void DrawObject(bool MultipleObj = false)
        {
            if (NewXSmooth != null)
            {
                X = NewXSmooth[CurrentSmooth];
                Y = NewYSmooth[CurrentSmooth];
                if (MultipleObj)
                {
                    _X = NewXSmooth[CurrentSmooth];
                    _Y = NewYSmooth[CurrentSmooth];
                }
                if(CurrentSmooth < NewYSmooth.Length - 1)
                    CurrentSmooth++;
                else
                {
                    NewXSmooth = null;
                    CurrentSmooth = 0;
                }
            }
            item.DrawTexture();
        }

        public void DrawStatusBar()
        {
            if (ObjType != Type.Grass)
                DrawHealthBar(X, Y);
        }

        public void SetOffset(int _x, int _y)
        {
            X += _x;
            Y += _y;
        }

        public bool MouseIsOver(int x, int y)
        {
            Rectangle From = new Rectangle(X, Y, TextureItem.RatioSize, TextureItem.RatioSize);
            Rectangle To = new Rectangle(x, y, 2, 2);
            if (To.IntersectsWith(From))
                return true;
            else
                return false;
        }

        public bool IsBumping(GameObject to)
        {
            if (to == null)
                return false;
            //X obj1 From
            //x Obj2 To
            Rectangle From = new Rectangle(X, Y, TextureItem.RatioSize, TextureItem.RatioSize);
            Rectangle To = new Rectangle(to.X, to.Y, TextureItem.RatioSize, TextureItem.RatioSize);
            if (From.IntersectsWith(To))
                return true;
            else
                return false;
        }

        public enum Type : short
        {
            House = 0,
            Outhouse = 1,
            Barn = 2,
            BigBarn = 3,
            Cabin = 4,
            Shop = 5,
            Windmil = 6,
            Hay = 7,
            Footman = 8,
            Grass = 9,
        }
    }
}
