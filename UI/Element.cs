using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Drawing;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace TFAGame.UI
{
    public delegate void OnElementEvent(ElementEventType type, Element Value);
    public class Element
    {
        private uint CurrentID = 0;
        internal event OnElementEvent OnEvent;
        public ElementData eData = null;
        public ElementData[] EventData = null;
        internal void ElementEvent(ElementEventType type)
        {
            if(OnEvent != null)
                OnEvent(type, this);
        }
        public ElementType eType
        {
            get;
            set;
        }

        public Element Parent
        {
            get;
            set;
        }

        public List<Element> Children
        {
            get;
            set;
        }

        public int X
        {
            get;
            set;
        }
        public int Y
        {
            get;
            set;
        }

        public int Width
        {
            get;
            set;
        }
        public int Height
        {
            get;
            set;
        }

        public Element(ElementData data, ElementType type)
        {
            Parent = null;
            eType = type;
            eData = data;
            if(eData != null)
                CurrentID = eData.ID.textureID;
            Children = new List<Element>();
            if (data == null)
                return;
            Width = eData.ImageData.Size.Width;
            Height = eData.ImageData.Size.Height;
            if (type == ElementType.Button)
            {
                ElementProperties OnClickProperty = GetProperty(data.Properties, ElementProperties.PropertyType.OnClick);
                Bitmap click_img = null;
                TextureID click_id = null;
                if (OnClickProperty != null)
                {
                    EventData = new ElementData[1];
                        EventData[0] = new ElementData(click_img, click_id, data.Properties);
                    EventData[0].ImageEventType = ElementEventType.MouseOver;
                }
                OnEvent += Element_OnEvent;
            }
        }

        void Element_OnEvent(ElementEventType type, Element Value)
        {
            switch (type)
            {
                case ElementEventType.Normal:
                    break;
                case ElementEventType.OnClick:
                    GameCore.OnClickEvent(GetProperty(this.eData.Properties, ElementProperties.PropertyType.OnClick));
                    break;
                case ElementEventType.MouseOver:
                    break;
            }
        }

        public bool HasProperty(Element.ElementProperties.PropertyType type)
        {
            return GetProperty(this.eData.Properties, type) == null ? false : true;
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

        public void DrawTexture()
        {
            GL.PushMatrix();
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, CurrentID);
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0, 0); GL.Vertex2(X, Y);
            GL.TexCoord2(0, 1); GL.Vertex2(X, Y + Height);
            GL.TexCoord2(1, 1); GL.Vertex2(X + Width, Y + Height);
            GL.TexCoord2(1, 0); GL.Vertex2(X + Width, Y);
            GL.End();
            GL.PopMatrix();
        }

        public class ElementData
        {
            public ElementEventType ImageEventType = ElementEventType.Normal;
            public Bitmap ImageData
            {
                get;
                set;
            }
            public TextureID ID
            {
                get;
                set;
            }

            public ElementProperties[] Properties
            {
                get;
                set;
            }

            public ElementData(Bitmap img, TextureID tID, ElementProperties[] properties)
            {
                ImageData = img;
                ID = tID;
                Properties = properties;
            }
        }
        public enum ElementType
        {
            [StringValue("TopBar")]
            TopBar,
            [StringValue("Text")]
            Text,
            [StringValue("Button")]
            Button,
            [StringValue("Label")]
            Label,
            [StringValue("Textbox")]
            Textbox,
            [StringValue("Listbox")]
            Listbox,
            [StringValue("Textarea")]
            Textarea,
            [StringValue("Content")]
            Content,
            [StringValue("Window")]
            Window
        }
        public class StringValue : System.Attribute
        {
            private string _value;
            public StringValue(string value)
            {
                _value = value;
            }
            public string Value
            {
                get { return _value; }
            }
        }
        public class ElementProperties
        {
            public PropertyType type;
            public object Value;

            public enum PropertyType : int
            {
                [StringValue("content")]
                Content,
                [StringValue("align")]
                Align,
                [StringValue("background")]
                Background,
                [StringValue("onclick")]
                OnClick,
                [StringValue("y")]
                PositionY,
                [StringValue("width")]
                Width,
                [StringValue("height")]
                Height,
                [StringValue("")]
                PlainValue,
            }

            public static string GetStringValue(Enum value)
            {
                string output = null;
                Type type = value.GetType();
                FieldInfo fi = type.GetField(value.ToString());
                StringValue[] attrs =
                   fi.GetCustomAttributes(typeof(StringValue),
                                           false) as StringValue[];
                if (attrs.Length > 0)
                {
                    output = attrs[0].Value;
                }

                return output;
            }
        }
        public static ElementType GetElementType(string name)
        {
            switch (name.ToLower())
            {
                case "button":
                    return Element.ElementType.Button;
                case "label":
                    return Element.ElementType.Label;
                case "textbox":
                    return Element.ElementType.Textbox;
                case "listbox":
                    return Element.ElementType.Listbox;
                case "textarea":
                    return Element.ElementType.Textarea;
                case "content":
                    return Element.ElementType.Content;
                case "window":
                    return Element.ElementType.Window;
                case "text":
                    return ElementType.Text;
                case "topbar":
                    return ElementType.TopBar;
                default:
                    return ElementType.Window;
            }
        }

        public static ElementProperties.PropertyType GetPropertyType(string name)
        {
            switch (name.ToLower())
            {
                case "content":
                    return Element.ElementProperties.PropertyType.Content;
                case "align":
                    return Element.ElementProperties.PropertyType.Align;
                case "background":
                    return Element.ElementProperties.PropertyType.Background;
                case "onclick":
                    return Element.ElementProperties.PropertyType.OnClick;
                case "y":
                    return ElementProperties.PropertyType.PositionY;
                case "width":
                    return ElementProperties.PropertyType.Width;
                case "height":
                    return ElementProperties.PropertyType.Height;
                case "":
                    return ElementProperties.PropertyType.PlainValue;
                default:
                    return ElementProperties.PropertyType.PlainValue;
            }
        }
    }
    public enum ElementEventType
    {
        Normal,
        OnClick,
        MouseOver,
    }
}
