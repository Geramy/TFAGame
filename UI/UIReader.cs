using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;

namespace TFAGame.UI
{
    public class UIReader
    {
        public static Element GetUIElements(XmlReader LayoutReader, ref BitmapHelper bitHelper)
        {
            List<XMLItem> Items = new List<XMLItem>();
            Element parentElem = new Element(null, Element.ElementType.Window);
            Array props = Enum.GetValues(typeof(Element.ElementProperties.PropertyType));
            XmlDocument document = new XmlDocument(LayoutReader.NameTable);
            document.Load(LayoutReader);
            XmlNode RootWindow = document.SelectSingleNode("Window");
            AddChildren(ref RootWindow, ref parentElem, ref bitHelper);
            return parentElem;
        }

        private static void AddChildren(ref XmlNode parent, ref Element rootElement, ref BitmapHelper bitHelper)
        {
            for (int i = 0; i < parent.ChildNodes.Count; i++)
            {
                XmlNode child = parent.ChildNodes[i];
                List<Element.ElementProperties> Properties = new List<Element.ElementProperties>();
                Element.ElementProperties property = new Element.ElementProperties();
                int width = 0;
                int height = 0;
                string ElementText = "";
                if (child.Attributes != null)
                {
                    for (int pi = 0; pi < child.Attributes.Count; pi++)
                    {
                        property = new Element.ElementProperties();
                        property.Value = child.Attributes[pi].Value;
                        property.type = Element.GetPropertyType(child.Attributes[pi].Name);

                        if (property.type == Element.ElementProperties.PropertyType.Content)
                        {
                            ElementText = (string)property.Value;
                        }
                        Properties.Add(property);
                    }
                    Properties.Add(property);
                }
                Element.ElementProperties Width = GetProperty(Properties.ToArray(), Element.ElementProperties.PropertyType.Width);
                Element.ElementProperties Height = GetProperty(Properties.ToArray(), Element.ElementProperties.PropertyType.Height);
                
                Element.ElementType elementType = new Element.ElementType();
                elementType = Element.GetElementType(child.Name.ToLower());
                int[] size = bitHelper.GetTextWidth(ElementText);
                if (Width != null)
                {
                    int iWidth = int.Parse((string)Width.Value);
                    if (iWidth > size[0])
                        size[0] = iWidth;
                }
                if (Height != null)
                {
                    int iHeight = int.Parse((string)Height.Value);
                    size[1] = iHeight;
                }
                Bitmap AfterNine = (System.Drawing.Bitmap)bitHelper.GetImage(size, bitHelper.GetElementImage(elementType));
                System.Drawing.Bitmap finalImage = ResizeImage(AfterNine, new Size(AfterNine.Width, size[1]));
                if (ElementText.Length > 0)
                {
                    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(finalImage))
                    {
                        g.DrawString(ElementText, new Font("Segoe UI", 11, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Black, new RectangleF(0, 0, finalImage.Width, finalImage.Height)); // requires font, brush etc
                    }
                }
                
                Element.ElementData elementData = new Element.ElementData(finalImage, new TextureID(finalImage), Properties.ToArray());
                Element element = new Element(elementData, elementType);
                AddChildren(ref child, ref element, ref bitHelper);
                rootElement.Children.Add(element);
            }
        }
        public static Bitmap ResizeImage(Bitmap imgToResize, Size size)
        {
            try
            {
                Bitmap b = new Bitmap(size.Width, size.Height);
                using (Graphics g = Graphics.FromImage((Image)b))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(imgToResize, 0, 0, size.Width, size.Height);
                }
                return b;
            }
            catch { return null; }
        }
        private static Element.ElementProperties GetProperty(Element.ElementProperties[] props, Element.ElementProperties.PropertyType type)
        {
            Element.ElementProperties property = null;
            for (int i = 0; i < props.Length; i++)
            {
                if (props[i].type == type)
                {
                    property = props[i];
                }
            }
            return property;
        }
    }
    public class XMLItem
    {
        public int Depth = 0;
        public int Row = 0;
        public string Name = "";
        public List<Element.ElementProperties> properties = new List<Element.ElementProperties>();

        public XMLItem(int D, int R)
        {
            Depth = D;
            Row = R;
        }
    }
}
