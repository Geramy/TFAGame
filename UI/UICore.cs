using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Reflection;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;

namespace TFAGame.UI
{
    [Export(typeof(CoreModule))]
    [ExportMetadata("Name", "UITextures")]
    public class UICore : CoreModule
    {
        public bool UIActive = true;
        private UIReader uiReader = null;
        private Pages CurrentPage = Pages.MainMenu;
        public bool DisplayPages = true;
        public Element[] UIPages = null;
        internal BitmapHelper bitHelper = new BitmapHelper();
        private string TextureDirectory = "";
        private string LayoutDirectory = "";

        public UICore()
            : base()
        {
            TextureDirectory = AppDomain.CurrentDomain.BaseDirectory + "Images";
            LayoutDirectory = AppDomain.CurrentDomain.BaseDirectory + "Layout";
            string[] files = Directory.GetFiles(LayoutDirectory);
            UIPages = new Element[files.Length];
            int i = 0;
            foreach (string file in files)
            {
                if (file.EndsWith(".xml"))
                {
                    UIPages[i] = UIReader.GetUIElements(XmlReader.Create(file), ref bitHelper);
                }
                i++;
            }
        }

        public override void Start()
        {
            base.Start();
        }

        internal void DrawUI(int windowWidth, int windowHeight) {
            if (GameCore.UIPause)
            {
                for (int i = 0; i < UIPages.Length; i++)
                {
                    if (i == (int)CurrentPage)
                    {
                        DrawItem(UIPages[i], windowWidth, 0);
                    }
                }
            }
        }

        public void OnClick(int x, int y)
        {
            if (GameCore.UIPause)
            {
                Element MouseOver = IsMouseOverChildren(x, y, UIPages[(int)CurrentPage]);
                if(MouseOver != null)
                    MouseOver.ElementEvent(ElementEventType.OnClick);
            }
        }

        private Element IsMouseOverChildren(int x, int y, Element child) {
            Element mO = null;
            Element MouseOver = null;
            if (new System.Drawing.Rectangle(x, y, 1, 1).IntersectsWith(new System.Drawing.Rectangle(child.X, child.Y, child.Width, child.Height)))
            {
                if(child.HasProperty(Element.ElementProperties.PropertyType.OnClick))
                    MouseOver = child;
            }
            for (int i = 0; i < child.Children.Count; i++)
                if((mO = IsMouseOverChildren(x, y, child.Children[i])) != null)
                        MouseOver = mO;
            return MouseOver;
        }

        private void DrawItem(Element item, int width, int y)
        {
            if (item.eData != null)
            {
                int ItemWidth = item.Width;

                switch (item.eType)
                {
                    case Element.ElementType.Window:
                        break;
                    case Element.ElementType.Button:
                        break;
                    case Element.ElementType.Content:
                        break;
                    case Element.ElementType.Label:
                        break;
                    case Element.ElementType.Listbox:
                        break;
                    case Element.ElementType.Text:
                        break;
                    case Element.ElementType.Textarea:
                        break;
                    case Element.ElementType.Textbox:
                        break;
                    case Element.ElementType.TopBar:
                        break;
                }
                int center = (width / 2) - (ItemWidth / 2);
                for (int i = 0; i < item.eData.Properties.Length; i++)
                {
                    if (item.eData.Properties[i].type == Element.ElementProperties.PropertyType.PositionY)
                        y += int.Parse((string)item.eData.Properties[i].Value);
                }
                item.X = center;
                item.Y = y;
                item.DrawTexture();
            }
            for (int i = 0; i < item.Children.Count; i++)
            {
                DrawItem(item.Children[i], width, y);
            }
        }

        public override void Stop()
        {
            base.Stop();
        }
    }
    public enum Pages
    {
        MainMenu = 0,
        Settings,
        Pause
    }
}
