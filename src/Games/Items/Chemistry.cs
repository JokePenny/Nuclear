using System.Windows.Controls;
using System;

namespace Nuclear.src
{
    public class Chemistry : ItemsInventory
    {
        private static Image image;
        private static string pathImage;
        private ushort[] effect_skill = new ushort[18];
        private ushort[] craft;
        private byte[] effect_special = new byte[6];
        private byte time_action;

        private byte strength { get; set; }
        private byte perception { get; set; }
        private byte endurance { get; set; }
        private byte charisma { get; set; }
        private byte intelligence { get; set; }
        private byte agility { get; set; }
        private byte luck { get; set; }
    }
}
