﻿using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nuclear.src;

namespace Nuclear
{
    public abstract class LivingObjects
    {
        protected int x;
        protected int y;
        protected int health;
        protected byte movePoints;
        protected byte areaVisibility;
        protected string nickname = null;
        public AnimationCharacter animationCharacter { get; set; }
        /*

        public double imageX { get; set; }
        public double imageY { get; set; }

        private double imageXOld;
        private double imageYOld;
        public bool changeImage { get; set; } = false;
        public double IndexImage { get; set; } = 0;

        private int[] dx = { 0, 1, 0, 1, -1, -1 };
        private int[] dy = { -1, 0, 1, -1, 0, -1 };
        private int[] dx2 = { 0, 1, 0, -1, 1, -1 };
        private int[] dy2 = { -1, 0, 1, 0, 1, 1 };
        */

        public LivingObjects(int X, int Y, int Health, byte MovePoints, byte AreaVisibility)
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

        public int GetHealth()
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
        /*
        public double GetImageX()
        {
            return imageX;
        }

        public double GetImageY()
        {
            return imageY;
        }
        */

        public string GetNickname()
        {
            return nickname;
        }

        //сеттеры
        public void SetXY(int X, int Y)
        {
            x = X;
            y = Y;
        }

        public void SetHealth(int A)
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

        public void SetImageScreen(Canvas GROD, Game game)
        {
            animationCharacter = new AnimationCharacter(GROD, game, nickname, x, y);
        }

        public void SetNickname(string a)
        {
            nickname = a;
        }
    }
}
