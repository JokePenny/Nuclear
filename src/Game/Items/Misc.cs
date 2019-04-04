using System.Windows.Controls;

namespace Nuclear.src.Game.Items
{
    public class Misc : ItemsInventory
    {
        private static Image image;
        private static string pathImage;
        private ushort craft;

        private byte strength { get; set; }
        private byte perception { get; set; }
        private byte endurance { get; set; }
        private byte charisma { get; set; }
        private byte intelligence { get; set; }
        private byte agility { get; set; }
        private byte luck { get; set; }
    }
}
