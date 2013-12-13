using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFAGame
{
    public class GameGenerator
    {
        public GameObject[] GenerateStartGame(int TeamNumber, int BaseX, int BaseY)
        {
            List<GameObject> objects = new List<GameObject>();
            GameObject House = new GameObject(GameObject.Type.House);
            GameObject Barn = new GameObject(GameObject.Type.Barn);
            GameObject Footman = new GameObject(GameObject.Type.Footman);
            GameObject Footman1 = new GameObject(GameObject.Type.Footman);
            GameObject Footman2 = new GameObject(GameObject.Type.Footman);
            House.X = BaseX + 0;
            House.Y = BaseY + 0;
            Barn.X = BaseX + 0;
            Barn.Y = BaseY + TextureItem.RatioSize * 1;
            Footman.X = BaseX + TextureItem.RatioSize * 1;
            Footman.Y = BaseY + 0;
            Footman1.X = BaseX + TextureItem.RatioSize * 1;
            Footman1.Y = BaseY + TextureItem.RatioSize * 1;
            Footman2.X = BaseX + TextureItem.RatioSize * 2;
            Footman2.Y = BaseY + 0;

            objects.Add(House);
            objects.Add(Barn);
            objects.Add(Footman);
            objects.Add(Footman1);
            objects.Add(Footman2);
            foreach (GameObject gameObj in objects)
                gameObj.TeamNumber = TeamNumber;


            return objects.ToArray();
        }
    }
}
