using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace Nuclear
{
    public class PlayerUser : LivingObjects
    {
        private static int ID = 999;// индефикатор объекта
        private string nickname = null;
        private int level;

        //private int imageX;
        //private int imageY;

        public PlayerUser(int X, int Y, byte Health, byte MovePoints, byte AreaVisibility) : base(X, Y, Health, MovePoints, AreaVisibility)
        {
            x = X;
            y = Y;
            //imageX = 34 + (X * 36);
            imageX = (X * 13) - 55;
            if (X % 2 != 0)
                imageY = 34 + (Y * 36);
            else
                imageY = 34 + (Y * 18);
            //imageY = 11;
            health = Health;
            movePoints = MovePoints;
            areaVisibility = AreaVisibility;
        }
        public PlayerUser(string Nickname)
        {
            nickname = Nickname;
        }
        public PlayerUser()
        {
        }

        public void SetImage(Canvas GROD, Image img)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(Environment.CurrentDirectory + "/data/image/characters/NMVALTAA_e.gif");
            image.EndInit();
            ImageBehavior.SetAnimatedSource(img, image);
            //ImageBehavior.SetRepeatBehavior(img, new RepeatBehavior(TimeSpan.FromSeconds(1)));
            Canvas.SetLeft(img, imageY); // (34) + 36 на след клетку по горизонтали  или +18 (эротика) если линия свдинулась
            Canvas.SetTop(img, imageX); // (37) + 13 на след клетку
            GROD.Children.Add(img);
        }

        public ImageAnimationController Moove(Canvas GROD, Image img)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(Environment.CurrentDirectory + "/data/image/characters/NMVALTAB_e.gif");
            image.EndInit();
            //int i = wave.Count;
            //foreach (Game.Point h in wave)
           // {
                //imageX = (h.x * 13) - 55;
               // if (h.x % 2 != 0)
                //    imageY = 34 + (h.y * 36);
               // else
              //      imageY = 34 + (h.y * 18);
            ImageBehavior.SetAnimatedSource(img, image);
            var controller = ImageBehavior.GetAnimationController(img);
            //controller.Play();
            return controller;
           // ImageBehavior.SetRepeatBehavior(img, new RepeatBehavior(count));
           //   Canvas.SetLeft(img, imageY); // (82) + 36 на след клетку по горизонтали  или +18 (эротика) если линия свдинулась
           //  Canvas.SetTop(img, imageX); // (97) + 13 на след клетку
        }

        public void DeleteImage()
        {
        }
        //сеттеры
        public void SetLevel(int a)
        {
            level = a;
        }

        public void SetNickname(string a)
        {
            nickname = a;
        }

        //геттеры
        public int GetID()
        {
            return ID;
        }

        public int GetLevel()
        {
            return level;
        }

        public string GetNickname()
        {
            return nickname;
        }
    }
}