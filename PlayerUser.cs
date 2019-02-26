using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nuclear
{
    class PlayerUser : LivingObjects
    {
        private static int ID = 999;// индефикатор объекта 

        public PlayerUser(int X, int Y, byte Health, byte MovePoints, byte AreaVisibility) : base(X, Y, Health, MovePoints, AreaVisibility)
        {
            x = X;
            y = Y;
            health = Health;
            movePoints = MovePoints;
            areaVisibility = AreaVisibility;
        }
        public PlayerUser()
        {
        }

        public void SetImage()
        {
        }

        public void DeleteImage()
        {
        }

        //геттеры
        public int GetID()
        {
            return ID;
        }
    }
}