using Nuclear.src;
using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace Nuclear
{
    public class PlayerUser : LivingObjects
    {
        private int ID = 999;// индефикатор объекта
        //private string nickname = null;
        private int level;
        private string stateMap;
        private string stateRoom;

        //оружие в руках
        public int d_initial { get; set; } = 3;
        public int d_limit { get; set; } = 1;
        public int type_of_weapon { get; set; } = 1;

        //броня надетая
        public int d_resistance { get; set; } = 1;
        public byte TypeOfArmor { get; set; } = 9;

        //характеристика
        private byte strength = 5;
        private byte perception = 5;
        private byte endurance = 5;
        private byte charisma = 5;
        private byte intelligence = 5;
        private byte agility = 5;
        private byte luck = 5;

        //умения
        public ushort big_guns { get; set; } = 10;
        public ushort small_guns { get; set; } = 35;
        public ushort energy_weapons { get; set; } = 10;
        public ushort throwing { get; set; } = 40;
        public ushort traps { get; set; } = 20;
        public ushort melee_weapons { get; set; } = 55;
        public ushort unarmed { get; set; } = 65;
        public ushort doctor { get; set; } = 15;
        public ushort first_aid { get; set; } = 30;
        public ushort lockpick { get; set; } = 20;
        public ushort repair { get; set; } = 20;
        public ushort science { get; set; } = 25;
        public ushort sneak { get; set; } = 20;
        public ushort steal { get; set; } = 25;
        public ushort barter { get; set; } = 20;
        public ushort gambling { get; set; } = 20;
        public ushort outdoorsman { get; set; } = 5;
        public ushort speech { get; set; } = 25;

        public PlayerUser(int X, int Y, byte Health, byte MovePoints, byte AreaVisibility, Canvas GROD, Game game) : base(X, Y, Health, MovePoints, AreaVisibility)
        {
            x = X;
            y = Y;
            health = Health;
            movePoints = MovePoints;
            areaVisibility = AreaVisibility;
            animationCharacter = new AnimationCharacter(GROD, game, nickname, x, y);
        }

        public PlayerUser(int X, int Y, string Nickname, Canvas GROD, Game game) : base(X, Y)
        {
            nickname = Nickname;
            x = X;
            y = Y;
            animationCharacter = new AnimationCharacter(GROD, game, nickname, x, y);
        }

        public PlayerUser(string Nickname)
        {
            nickname = Nickname;
        }

        public PlayerUser()
        {
        }

        //сеттеры
        public void SetLevel(int a)
        {
            level = a;
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
            type_of_weapon = 143;
            return Convert.ToInt32(Math.Round(type_of_weapon + (perception + C) * A - distance * B - arm_class + bonus));
        }
    }
}