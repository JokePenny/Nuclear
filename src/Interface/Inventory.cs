using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace Nuclear.src.Interface
{
    class Inventory
    {
        private List<Image> items;
        private Image Armor;
        private Image FirstWeapon;
        private Image SecondWeapon;
        private Image Character;
        private byte angle = 0;
        private string angleCharacter = "";
        private string nameArmor = "";


        public int GetItem(object sender, MouseButtonEventArgs e)
        {
            Image picture = sender as Image;
            ImageSource image = Clipboard.GetImage();
            return items.IndexOf(sender as Image);
        }

        public void DeleteItemOfInventory(int a)
        {
            items.RemoveAt(a);
        }

        public Image SetItemInventory()
        {
            Image picture = null;
            ImageSource image = Clipboard.GetImage();
            picture.Source = image;
            items.Add(picture);
            return picture;
        }

        public Image SetItemFirstWeapon()
        {
            ImageSource image = Clipboard.GetImage();
            SecondWeapon.Source = image;
            return SecondWeapon;
        }

        public Image SetItemSecondWeapon()
        {
            ImageSource image = Clipboard.GetImage();
            FirstWeapon.Source = image;
            return FirstWeapon;
        }

        public Image SetItemArmor()
        {
            ImageSource image = Clipboard.GetImage();
            Armor.Source = image;
            return Armor;
        }

        public void RotationCharacterOfInventory(object sender, MouseButtonEventArgs e)
        {
            angle = angle < 5 ? angle++ : (byte)0;

            var image = new BitmapImage();
            image.BeginInit();
            switch (angle)
            {
                case 0:
                    angleCharacter = "sw";
                    break;
                case 1:
                    angleCharacter = "w";
                    break;
                case 2:
                    angleCharacter = "nw";
                    break;
                case 3:
                    angleCharacter = "ne";
                    break;
                case 4:
                    angleCharacter = "e";
                    break;
                case 5:
                    angleCharacter = "se";
                    break;
            }
            image.UriSource = new Uri(Environment.CurrentDirectory + "/data/image/characters/" + nameArmor + angleCharacter + ".gif");
            image.EndInit();
            ImageBehavior.SetAnimatedSource((sender as Image), image);
        }
    }
}
