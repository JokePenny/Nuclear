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
        public LivingObjects(int X, int Y, byte Health)
        {
            x = X;
            y = Y;
            health = Health;
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
    }
}
