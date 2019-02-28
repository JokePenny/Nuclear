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
        public int imageX;
        public int imageY;

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

        public int GetImageX()
        {
            return imageX;
        }

        public int GetImageY()
        {
            return imageY;
        }

        //сеттеры
        public void SetXY(int X, int Y)
        {
            x = X;
            y = Y;
            imageX = (X * 13) - 55;
            if (X % 2 != 0)
                imageY = 34 + (Y * 36);
            else
                imageY = 34 + (Y * 18);
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
