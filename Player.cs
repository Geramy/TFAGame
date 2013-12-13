using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFAGame
{
    public class Player
    {
        private List<GameObject> gameObjects = null;
        private System.Drawing.Graphics graphics = null;

        public int OffsetX = 0;
        public int OffsetY = 0;

        public Player(System.Drawing.Graphics graph)
        {
            gameObjects = new List<GameObject>();
            graphics = graph;
        }

        public void DrawObjects()
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].SetOffset(OffsetX, OffsetY);
                gameObjects[i].DrawObject();
                //gameObjects[i].DrawStatusBar();
            }
        }

        public void DrawStatusBars()
        {
            if (drawn)
                return;
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].DrawStatusBar();
            }
            //drawn = true;
        }

        bool drawn = false;

        public void AddObjects(GameObject[] Objects)
        {
            for (int i = 0; i < Objects.Count(); i++)
            {
                Objects[i].OnDeath += new ObjectCollision(Player_OnDeath);
                gameObjects.Add(Objects[i]);
            }
        }

        void Player_OnDeath(GameObject obj)
        {
            gameObjects.Remove(obj);
        }

        public GameObject GetObject(int x, int y, bool GetMoveable = false)
        {
            GameObject obj = null;
            foreach (GameObject gObj in gameObjects)
            {
                if (gObj.MouseIsOver(x, y))
                {
                    if (GetMoveable)
                        if (!gObj.Moveable)
                            continue;
                    obj = gObj;
                    break;
                }
            }
            return obj;
        }
    }
}
