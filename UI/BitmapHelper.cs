using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Design;
using System.Drawing.Drawing2D;

namespace TFAGame.UI
{
    public class BitmapHelper
    {
        public List<Element.ElementData> textures = new List<Element.ElementData>();
        public Image[] images = new Image[Enum.GetValues(typeof(MenuItems)).Length];

        public BitmapHelper()
        {
            int i = 0;
            foreach (MenuItems menuItem in Enum.GetValues(typeof(MenuItems)))
            {
                images[i++] = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + GetImageLocation(menuItem));
            }
        }
        //MenuBar = 0,
        //ContentBackground = 1,
        //Button = 2,
        //ButtonPressed = 3,
        //SideButton = 4,
        //SideButtonPressed = 5,
        //TopButton = 6,
        //TopButtonPressed = 7,
        //Transparent = 8
        public Image GetElementImage(Element.ElementType type, bool Pressed = false)
        {
            switch (type)
            {
                case Element.ElementType.Button:
                    if (Pressed)
                        return images[3];
                    else
                        return images[2];
                case Element.ElementType.Content:
                    return images[1];
                case Element.ElementType.TopBar:
                    return images[0];
                case Element.ElementType.Textbox:
                    return images[8];
                case Element.ElementType.Window:
                    return images[1];
                default:
                    return images[8];
            }
        }

        private string GetImageLocation(MenuItems mi)
        {
            switch (mi)
            {
                case MenuItems.MenuBar:
                    return @"Images\menu_bar.9.png";
                case MenuItems.ContentBackground:
                    return @"Images\dialogbackground.9.png";
                case MenuItems.Button:
                    return @"Images\button.9.png";
                case MenuItems.ButtonPressed:
                    return @"Images\button_pressed.9.png";
                case MenuItems.SideButton:
                    return @"Images\sidebtnnormal.9.png";
                case MenuItems.SideButtonPressed:
                    return @"Images\sidebtnpressed.9.png";
                case MenuItems.TopButton:
                    return @"Images\top_tab_unpressed.9.png";
                case MenuItems.TopButtonPressed:
                    return @"Images\top_tab_pressed.9.png";
                case MenuItems.Transparent:
                    return @"Images\transparent.9.png";
                default:
                    return "";
            }
        }

        public int[] GetTextWidth(string text)
        {
            SizeF size = new SizeF();
            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(new Bitmap(10, 10)))
            {
                size = graphics.MeasureString(text, new Font("Segoe UI", 11, FontStyle.Regular, GraphicsUnit.Pixel));
            }
            return new int[] { (int)size.Width + 10, (int)size.Height + 10 };
        }

        public Image GetImage(int[] size, Image img)
        {
            int width = size[0];
            int height = size[1];
            if (size[0] < img.Width)
                width = img.Width;
            if (size[1] < height)
                height = img.Height;
            NinePatch image = new NinePatch(img);
            return image.ImageSizeOf(width, height);
        }

        public enum MenuItems
        {
            MenuBar = 0,
            ContentBackground = 1,
            Button = 2,
            ButtonPressed = 3,
            SideButton = 4,
            SideButtonPressed = 5,
            TopButton = 6,
            TopButtonPressed = 7,
            Transparent = 8,
        }
    }
}
