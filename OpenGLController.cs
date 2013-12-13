using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace TFAGame
{
    public class OpenGLController
    {
        private OpenTK.GLControl controller = null;

        public OpenGLController(GLControl glControl)
        {
            controller = glControl;
        }

        public void DrawObjects(Player player)
        {
            player.DrawObjects();
        }
    }
}
