using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace TFAGame
{
    public class Memory
    {
        Dictionary<GameObject.Type, Bitmap> LoadedTextures = new Dictionary<GameObject.Type, Bitmap>();
        public Dictionary<GameObject.Type, TextureID> TextureIDs = new Dictionary<GameObject.Type, TextureID>();
        string[] Files = new string[] { "house", "outhouse", "barn1", "barn", "cabin", "shop", "windmil", "hay", "footman", "grass" };
        //House = 0,
        //Outhouse = 1,
        //Barn = 2,
        //BigBarn = 3,
        //Cabin = 4,
        //Shop = 5,
        //Windmil = 6,
        //Hay = 7,
        //Footman = 8,
        public Memory()
        {
            string MainDirectory = AppDomain.CurrentDomain.BaseDirectory + "Textures\\";
            for (short i = 0; i < Files.Length; i++)
            {
                LoadedTextures.Add((GameObject.Type)i, ScaleImage(Image.FromFile(MainDirectory + Files[i] + ".png"), TextureItem.RatioSize, TextureItem.RatioSize));
                TextureIDs.Add((GameObject.Type)i, new TextureID(LoadedTextures[(GameObject.Type)i]));
            }
        }

        public static Bitmap ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }

        public Bitmap getTexture(GameObject.Type type)
        {
            return LoadedTextures[type];
        }
    }
}
