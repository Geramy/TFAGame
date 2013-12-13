using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFAGame
{
    public static class GameLogic
    {
        public static void AttackItem(GameObject From, GameObject To)
        {
            if (From.Damage > 0)
            {
                if (To.Health > 0)
                    To.Health -= From.Damage * From.Level;
                else if(To.ObjType != GameObject.Type.Grass)
                    To.DeleteSelf();
            }
        }
    }
}
