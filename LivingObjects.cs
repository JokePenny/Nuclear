using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nuclear
{
    class LivingObjects
    {
        public int x;
        public int y;
        public byte health;
        public byte movePoints;
        public byte areaVisibility;
        public LivingObjects(int X, int Y, byte Health, byte MovePoints, byte AreaVisibility)
        {
            x = X;
            y = Y;
            health = Health;
            movePoints = MovePoints;
            areaVisibility = AreaVisibility;
        }
        public LivingObjects(int X, int Y)
        {
            x = X;
            y = Y;
        }
        public LivingObjects()
        {
        }

        //геттеры
        public int GetX()
        {
            return x;
        }
        public int GetY()
        {
            return y;
        }
        public byte GetHealth()
        {
            return health;
        }
        public byte GetMovePoints()
        {
            return movePoints;
        }
        public byte GetAreaVisibility()
        {
            return areaVisibility;
        }

        //сеттеры
        public void SetXY(int X, int Y)
        {
            x = X;
            y = Y;
        }
        public void SetHealth(byte A)
        {
            health = A;
        }
        public void SetMovePoints(byte A)
        {
            movePoints = A;
        }
        public void SetAreaVisibility(byte A)
        {
            areaVisibility = A;
        }
    }
}
