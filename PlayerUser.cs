using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace Nuclear
{
    public class PlayerUser : LivingObjects
    {
        private static int ID = 999;// индефикатор объекта
        private string nickname = null;
        private int level;
        private string stateMap;
        private string stateRoom;

        //private int imageX;
        //private int imageY;

        public PlayerUser(int X, int Y, byte Health, byte MovePoints, byte AreaVisibility) : base(X, Y, Health, MovePoints, AreaVisibility)
        {
            x = X;
            y = Y;
            imageX = (X * 13) - 55;
            if (X % 2 != 0)
                imageY = 34 + (Y * 36);
            else
                imageY = 34 + (Y * 18);
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
            image.UriSource = new Uri(Environment.CurrentDirectory + "/data/image/characters/NMVALTA/NMVALTAA/NMVALTAA_e.gif");
            image.EndInit();
            ImageBehavior.SetAnimatedSource(img, image);
            Canvas.SetLeft(img, imageY);
            Canvas.SetTop(img, imageX);
            GROD.Children.Add(img);
        }

        public void ChangeImage(Canvas GROD, Image img)
        {
            Image imgs = img;
            GROD.Children.Remove(img);
            var image = new BitmapImage();
            image.BeginInit();
            if(changeImage == 1)
            {
                switch (IndexImage)
                {
                    case 0:
                        image.UriSource = new Uri(Environment.CurrentDirectory + "/data/image/characters/NMVALTA/NMVALTAA/NMVALTAA_w.gif");
                        Canvas.SetLeft(imgs, imageY - 10);
                        break;
                    case 1:
                        image.UriSource = new Uri(Environment.CurrentDirectory + "/data/image/characters/NMVALTA/NMVALTAA/NMVALTAA_se.gif");
                        Canvas.SetLeft(imgs, imageY - 10);
                        break;
                    case 2:
                        image.UriSource = new Uri(Environment.CurrentDirectory + "/data/image/characters/NMVALTA/NMVALTAA/NMVALTAA_e.gif");
                        Canvas.SetLeft(imgs, imageY);
                        break;
                    case 3:
                        image.UriSource = new Uri(Environment.CurrentDirectory + "/data/image/characters/NMVALTA/NMVALTAA/NMVALTAA_sw.gif");
                        Canvas.SetLeft(imgs, imageY - 10);
                        break;
                    case 4:
                        image.UriSource = new Uri(Environment.CurrentDirectory + "/data/image/characters/NMVALTA/NMVALTAA/NMVALTAA_ne.gif");
                        Canvas.SetLeft(imgs, imageY - 10);
                        break;
                    case 5:
                        image.UriSource = new Uri(Environment.CurrentDirectory + "/data/image/characters/NMVALTA/NMVALTAA/NMVALTAA_nw.gif");
                        Canvas.SetLeft(imgs, imageY - 10);
                        break;

                }
            }
            else if(changeImage == 2)
            {
                switch (IndexImage)
                {
                    case 0:
                        image.UriSource = new Uri(Environment.CurrentDirectory + "/data/image/characters/NMVALTA/NMVALTAA/NMVALTAA_w.gif");
                        Canvas.SetLeft(imgs, imageY - 10);
                        break;
                    case 1:
                        image.UriSource = new Uri(Environment.CurrentDirectory + "/data/image/characters/NMVALTA/NMVALTAA/NMVALTAA_sw.gif");
                        Canvas.SetLeft(imgs, imageY - 10);
                        break;
                    case 2:
                        image.UriSource = new Uri(Environment.CurrentDirectory + "/data/image/characters/NMVALTA/NMVALTAA/NMVALTAA_e.gif");
                        Canvas.SetLeft(imgs, imageY);
                        break;
                    case 3:
                        image.UriSource = new Uri(Environment.CurrentDirectory + "/data/image/characters/NMVALTA/NMVALTAA/NMVALTAA_nw.gif");
                        Canvas.SetLeft(imgs, imageY - 10);
                        break;
                    case 4:
                        image.UriSource = new Uri(Environment.CurrentDirectory + "/data/image/characters/NMVALTA/NMVALTAA/NMVALTAA_se.gif");
                        Canvas.SetLeft(imgs, imageY - 10);
                        break;
                    case 5:
                        image.UriSource = new Uri(Environment.CurrentDirectory + "/data/image/characters/NMVALTA/NMVALTAA/NMVALTAA_ne.gif");
                        Canvas.SetLeft(imgs, imageY - 10);
                        break;
                }
            }
            image.EndInit();
            ImageBehavior.SetAnimatedSource(imgs, image);
            ImageBehavior.GetAnimationController(imgs).Pause();
            Canvas.SetTop(imgs, imageX); 
            GROD.Children.Add(imgs);
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
        public void SetStateMap(string a)
        {
            stateMap = a;
        }
        public void SetStateRoom(string a)
        {
            stateRoom = a;
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
        public string GetStateMap()
        {
            return stateMap;
        }
        public string GetStateRoom()
        {
            return stateRoom;
        }
    }
}