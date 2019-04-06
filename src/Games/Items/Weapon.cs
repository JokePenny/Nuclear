using System.Windows.Controls;
using System;

namespace Nuclear.src
{
    public class Weapon : ItemsInventory
    {
        private static Image image;
        private static string pathImage;
        private ushort damage_d_lim;
        private ushort damage_u_lim;
        private ushort[] craft;

        private byte strength { get; set; }
        private byte perception { get; set; }
        private byte endurance { get; set; }
        private byte charisma { get; set; }
        private byte intelligence { get; set; }
        private byte agility { get; set; }
        private byte luck { get; set; }
    }
}
