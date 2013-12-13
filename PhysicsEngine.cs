using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFAGame
{
    public class PhysicsEngine
    {
        public const int MovementsPerSecond = 1500;
        
        List<GameObject> FromMovement = new List<GameObject>();
        List<GameObject> ToMovement = new List<GameObject>();
        List<int> MovementTween = new List<int>();

        List<GameObject> Collideables = new List<GameObject>();

        System.Timers.Timer PhysicsKeepAlive = new System.Timers.Timer(1000 / GameCore.PhysxFPS);
        public PhysicsEngine()
        {
            PhysicsKeepAlive.Elapsed += new System.Timers.ElapsedEventHandler(PhysicsKeepAlive_Elapsed);
            PhysicsKeepAlive.Start();
        }

        void PhysicsKeepAlive_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int remove_i = -1;
            lock (FromMovement)
            {
                lock (ToMovement)
                {
                    for (int i = 0; i < FromMovement.Count; i++)
                    {
                        if (ToMovement[i] == null)
                            break;
                        if (FromMovement[i].IsBumping(ToMovement[i]) && ToMovement[i].AutoRemove && ToMovement[i].Health <= 0)
                        {
                            remove_i = i;
                        }

                        if (MovementTween[i] < MovementsPerSecond && ToMovement[i].Health > 0 && ToMovement[i].ObjType != GameObject.Type.Grass)
                        {
                            float NewToX = (float)ToMovement[i].X + (((float)TextureItem.RatioSize / 6) / (float)MovementsPerSecond) * (float)MovementTween[i];
                            float NewToY = (float)ToMovement[i].Y + (((float)TextureItem.RatioSize / 6) / (float)MovementsPerSecond) * (float)MovementTween[i];

                            FromMovement[i].X = (int)(((float)(NewToX - (float)FromMovement[i].X) / (float)MovementsPerSecond) * (float)MovementTween[i]) + FromMovement[i].X;//(TextureItem.RatioSize / 8);
                            FromMovement[i].Y = (int)(((float)(NewToY - (float)FromMovement[i].Y) / (float)MovementsPerSecond) * (float)MovementTween[i]) + FromMovement[i].Y;
                            MovementTween[i]++;
                        }
                        else if(MovementTween[i] < MovementsPerSecond)
                        {
                            float NewToX = (float)ToMovement[i].X + (((float)TextureItem.RatioSize / 6) / (float)MovementsPerSecond) * (float)MovementTween[i];
                            float NewToY = (float)ToMovement[i].Y + (((float)TextureItem.RatioSize / 6) / (float)MovementsPerSecond) * (float)MovementTween[i];

                            FromMovement[i].X = (int)(((float)(NewToX - (float)FromMovement[i].X) / (float)MovementsPerSecond) * (float)MovementTween[i]) + FromMovement[i].X;//(TextureItem.RatioSize / 8);
                            FromMovement[i].Y = (int)(((float)(NewToY - (float)FromMovement[i].Y) / (float)MovementsPerSecond) * (float)MovementTween[i]) + FromMovement[i].Y;
                            MovementTween[i]++;
                        }

                        if (FromMovement[i].IsBumping(ToMovement[i]) && ToMovement[i].ObjType == GameObject.Type.Grass)
                        {
                            remove_i = i;
                        }

                        if (FromMovement[i].TeamNumber == ToMovement[i].TeamNumber)
                        {
                            continue;
                        }
                        else if(FromMovement[i].IsBumping(ToMovement[i]))
                        {
                            GameLogic.AttackItem(FromMovement[i], ToMovement[i]);
                        }
                    }
                }
            }
            if (remove_i != -1)
            {
                FromMovement[remove_i].AutoRemove = false;
                FromMovement.RemoveAt(remove_i);
                ToMovement.RemoveAt(remove_i);
                MovementTween.RemoveAt(remove_i);
            }
        }

        //public int[] GetDifference(int x1, int y1, int x2, int y2)
        //{

        //}

        public bool IsAttacking(ref GameObject FromObj)
        {
            if (FromMovement.Contains(FromObj))
                return true;
            else
                return false;
        }

        public void StopAttacking(ref GameObject FromObj)
        {
            int pos = -1;
            int i = 0;
            lock (FromMovement)
            {
                foreach (GameObject Obj in FromMovement)
                {
                    if (Obj == FromObj)
                    {
                        pos = i;
                        break;
                    }
                    i++;
                }
            }
            if (pos != -1)
            {
                FromMovement[pos].AutoRemove = false;
                FromMovement.RemoveAt(pos);
                ToMovement.RemoveAt(pos);
                MovementTween.RemoveAt(pos);
            }
        }

        public void MoveTo(ref GameObject From, ref GameObject To, bool AutoRemove)
        {
            From.AutoRemove = AutoRemove;
            FromMovement.Add(From);
            ToMovement.Add(To);
            MovementTween.Add(0);
        }
    }
}
