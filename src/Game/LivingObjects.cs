using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nuclear
{
    public abstract class LivingObjects
    {
        protected int x;
        protected int y;
        protected byte health;
        protected byte movePoints;
        protected byte areaVisibility;
        protected double imageX;
        protected double imageY;

        private double imageXOld;
        private double imageYOld;
        protected double changeImage = 2;
        protected double IndexImage = 0;

        protected Image image;

        private int[] dx = { 0, 1, 0, 1, -1, -1 };
        private int[] dy = { -1, 0, 1, -1, 0, -1 };
        private int[] dx2 = { 0, 1, 0, -1, 1, -1 };
        private int[] dy2 = { -1, 0, 1, 0, 1, 1 };

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

        public double GetImageX()
        {
            return imageX;
        }

        public double GetImageY()
        {
            return imageY;
        }

        public double GetChangeImage()
        {
            return changeImage;
        }

        public Image GetImage()
        {
            return image;
        }

        //сеттеры
        public void SetXY(int X, int Y)
        {
            x = X;
            y = Y;
            imageX = X * 13 - 55;
            if (X % 2 != 0)
                imageY = Y * 96/ 2.74 + 10;
            else
                imageY = Y  * 96/2.74 - 10;

            double checkImageX;
            double checkImageY;
            for (int d = 0; d < 6; d++)
            {
                if (X % 2 != 0)
                {
                    checkImageX = (imageXOld + dx[d]) * 13 - 55;
                    checkImageY = (imageYOld + dy[d]) * 96/2.74 + 10;
                    if (checkImageX == imageX && checkImageY == imageY)
                    {
                        changeImage = 1;
                        IndexImage = d;
                        break;
                    }
                }
                else
                {
                    checkImageX = (imageXOld + dx2[d]) * 13 - 55;
                    checkImageY = (imageYOld + dy2[d]) *96/2.74 - 10;
                    if (checkImageX == imageX && checkImageY == imageY)
                    {
                        changeImage = 2;
                        IndexImage = d;
                        break;
                    }
                }
            }
            imageXOld = X;
            imageYOld = Y;
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
        public void SetImage(Image A)
        {
            image = A;
        }
    }
}
