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

        //оружие в руках
        public int d_initial { get; set; } = 1;
        public int d_limit { get; set; } = 1;
        public int type_of_weapon { get; set; } = 1;

        //броня надетая
        public int d_resistance { get; set; } = 1;

        //характеристика
        private int strength = 5;
        private int perception = 5;
        private int endurance = 5;
        private int charisma = 5;
        private int intelligence = 5;
        private int agility = 5;
        private int luck = 5;

        //умения
        public int big_guns { get; set; } = 10;
        public int small_guns { get; set; } = 35;
        public int energy_weapons { get; set; } = 10;
        public int throwing { get; set; } = 40;
        public int traps { get; set; } = 20;
        public int melee_weapons { get; set; } = 55;
        public int unarmed { get; set; } = 65;
        public int doctor { get; set; } = 15;
        public int first_aid { get; set; } = 30;
        public int lockpick { get; set; } = 20;
        public int repair { get; set; } = 20;
        public int science { get; set; } = 25;
        public int sneak { get; set; } = 20;
        public int steal { get; set; } = 25;
        public int barter { get; set; } = 20;
        public int gambling { get; set; } = 20;
        public int outdoorsman { get; set; } = 5;
        public int speech { get; set; } = 25;

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

        public PlayerUser(int X, int Y, string Nickname) : base(X, Y)
        {
            nickname = Nickname;
            x = X;
            y = Y;
            imageX = (X * 13) - 55;
            if (X % 2 != 0)
                imageY = 34 + (Y * 36);
            else
                imageY = 34 + (Y * 18);
        }

        public PlayerUser(string Nickname)
        {
            nickname = Nickname;
        }

        public PlayerUser()
        {
        }

        public void SetImageScreen(Canvas GROD)
        {
            GetImage().Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "/data/image/characters/NMVALTA/NMVALTAA/NMVALTAA_e.gif"));
            Canvas.SetLeft(GetImage(), imageY);
            Canvas.SetTop(GetImage(), imageX);
            GROD.Children.Add(GetImage());
        }

        public void ChangeImage(Canvas GROD)
        {
            GROD.Children.Remove(GetImage());
            var image = new BitmapImage();
            image.BeginInit();
            if(changeImage == 1)
            {
                switch (IndexImage)
                {
                    case 0:
                        image.UriSource = new Uri(Environment.CurrentDirectory + "/data/image/characters/NMVALTA/NMVALTAA/NMVALTAA_w.gif");
                        Canvas.SetLeft(GetImage(), imageY - 10);
                        break;
                    case 1:
                        image.UriSource = new Uri(Environment.CurrentDirectory + "/data/image/characters/NMVALTA/NMVALTAA/NMVALTAA_se.gif");
                        Canvas.SetLeft(GetImage(), imageY - 10);
                        break;
                    case 2:
                        image.UriSource = new Uri(Environment.CurrentDirectory + "/data/image/characters/NMVALTA/NMVALTAA/NMVALTAA_e.gif");
                        Canvas.SetLeft(GetImage(), imageY);
                        break;
                    case 3:
                        image.UriSource = new Uri(Environment.CurrentDirectory + "/data/image/characters/NMVALTA/NMVALTAA/NMVALTAA_sw.gif");
                        Canvas.SetLeft(GetImage(), imageY - 10);
                        break;
                    case 4:
                        image.UriSource = new Uri(Environment.CurrentDirectory + "/data/image/characters/NMVALTA/NMVALTAA/NMVALTAA_ne.gif");
                        Canvas.SetLeft(GetImage(), imageY - 10);
                        break;
                    case 5:
                        image.UriSource = new Uri(Environment.CurrentDirectory + "/data/image/characters/NMVALTA/NMVALTAA/NMVALTAA_nw.gif");
                        Canvas.SetLeft(GetImage(), imageY - 10);
                        break;

                }
            }
            else if(changeImage == 2)
            {
                switch (IndexImage)
                {
                    case 0:
                        image.UriSource = new Uri(Environment.CurrentDirectory + "/data/image/characters/NMVALTA/NMVALTAA/NMVALTAA_w.gif");
                        Canvas.SetLeft(GetImage(), imageY - 10);
                        break;
                    case 1:
                        image.UriSource = new Uri(Environment.CurrentDirectory + "/data/image/characters/NMVALTA/NMVALTAA/NMVALTAA_sw.gif");
                        Canvas.SetLeft(GetImage(), imageY - 10);
                        break;
                    case 2:
                        image.UriSource = new Uri(Environment.CurrentDirectory + "/data/image/characters/NMVALTA/NMVALTAA/NMVALTAA_e.gif");
                        Canvas.SetLeft(GetImage(), imageY);
                        break;
                    case 3:
                        image.UriSource = new Uri(Environment.CurrentDirectory + "/data/image/characters/NMVALTA/NMVALTAA/NMVALTAA_nw.gif");
                        Canvas.SetLeft(GetImage(), imageY - 10);
                        break;
                    case 4:
                        image.UriSource = new Uri(Environment.CurrentDirectory + "/data/image/characters/NMVALTA/NMVALTAA/NMVALTAA_se.gif");
                        Canvas.SetLeft(GetImage(), imageY - 10);
                        break;
                    case 5:
                        image.UriSource = new Uri(Environment.CurrentDirectory + "/data/image/characters/NMVALTA/NMVALTAA/NMVALTAA_ne.gif");
                        Canvas.SetLeft(GetImage(), imageY - 10);
                        break;
                }
            }
            image.EndInit();
            ImageBehavior.SetAnimatedSource(GetImage(), image);
            ImageBehavior.GetAnimationController(GetImage()).Pause();
            Canvas.SetTop(GetImage(), imageX); 
            GROD.Children.Add(GetImage());
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

        public int GetDamage(int d_resistance_p, int d_resistance_modifier, int d_modifier)
        {
            return (d_initial * d_modifier - d_limit) * (100 - (d_resistance_p - d_resistance_modifier)) / 100;
        }
        public int GetProbabilityHitting(int distance, int arm_class, int perk, int bonus)
        {
            double A = 5, B = 3.5, C = -2 + perk;
            return Convert.ToInt32(Math.Round(type_of_weapon + (perception + C) * A - distance * B - arm_class + bonus));
        }
    }
}