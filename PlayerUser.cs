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

        public PlayerUser(int X, int Y, byte Health, byte MovePoints) : base(X, Y, Health, MovePoints)
        {
            x = X;
            y = Y;
            health = Health;
            movePoints = MovePoints;
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