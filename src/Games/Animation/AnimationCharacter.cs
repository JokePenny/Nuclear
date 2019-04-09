using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace Nuclear.src
{
    public class AnimationCharacter
    {
        public Image image { get; set; }
        public string PathImage { get; set; }
        public string FullPathImage { get; set; }
        public string Direction { get; set; }
        public int ZIndexImage { get; set; } = 0;
        public byte NumFolderCharacter { get; set; } = 9;
        public byte FirstLetterAnimation { get; set; } = 0;
        public byte SecondLetterAnimation { get; set; } = 0;
        public byte IndexDirection { get; set; } = 0;
        public bool ChangeAnimation { get; set; } = true;

        /* Инструкция к анимации
         * FirstLetterAnimation: анимация     SecondLetterAnimation:
         * A - движения
         * B - смерти
         * С - вставания
         * D - с ножом
         * Е - с палкой
         * F - с кувалдой
         * G - с копьем
         * Н - с пистолетом
         * I - с пистолетом-пулеметом
         * J - с винтовкой
         * K - с тяжелым оружием
         * L - с миниганом
         * M - с гранатометом
         * R - лежачего
         */

        public AnimationCharacter(Canvas GROD, Game game, string nickname, double imageX, double imageY)
        {
            image = new Image();
            image.Source = new BitmapImage(new Uri("/data/image/characters/HMJMPS/HMJMPSAA_e.gif", UriKind.Relative));
            FullPathImage = "pack://application:,,,/data/image/characters/HMJMPS/HMJMPSAA_e.gif";
            image.MouseLeftButtonDown += game.ActionWithPlayer_Click;
            image.MouseMove += game.ActionWithPlayer_Move;
            image.MouseLeave += game.ActionWithPlayer_MouseLeave;
            image.MouseEnter += game.ActionWithPlayer_Focusable;
            image.Name = nickname;
            Canvas.SetLeft(image, imageY);
            Canvas.SetTop(image, imageX);
            GROD.Children.Add(image);
        }

        public string PathForImage()
        {
            switch (NumFolderCharacter)
            {
                case 0:
                    PathImage = "/data/image/characters/HANPWR/HANPWR";
                    break;
                case 1:
                    PathImage = "/data/image/characters/HAPOWR/HAPOWR";
                    break;
                case 2:
                    PathImage = "/data/image/characters/HAROBE/HAROBE";
                    break;
                case 3:
                    PathImage = "/data/image/characters/HFCMBT/HFCMBT";
                    break;
                case 4:
                    PathImage = "/data/image/characters/HFJMPS/HFJMPS";
                    break;
                case 5:
                    PathImage = "/data/image/characters/HFLTHR/HFLTHR";
                    break;
                case 6:
                    PathImage = "/data/image/characters/HFMAXX/HFMAXX";
                    break;
                case 7:
                    PathImage = "/data/image/characters/HFMETL/HFMETL";
                    break;
                case 8:
                    PathImage = "/data/image/characters/HMCMBT/HMCMBT";
                    break;
                case 9:
                    PathImage = "/data/image/characters/HMJMPS/HMJMPS";
                    break;
                case 10:
                    PathImage = "/data/image/characters/HMLTHR/HMLTHR";
                    break;
                case 11:
                    PathImage = "/data/image/characters/HMMAXX/HMMAXX";
                    break;
                case 12:
                    PathImage = "/data/image/characters/HMMETL/HMMETL";
                    break;
            }

            switch (FirstLetterAnimation)
            {
                case 0:
                    PathImage += "A";
                    break;
                case 1:
                    PathImage += "B";
                    break;
                case 2:
                    PathImage += "C";
                    break;
                case 3:
                    PathImage += "D";
                    break;
                case 4:
                    PathImage += "E";
                    break;
                case 5:
                    PathImage += "F";
                    break;
                case 6:
                    PathImage += "G";
                    break;
                case 7:
                    PathImage += "H";
                    break;
                case 8:
                    PathImage += "I";
                    break;
                case 9:
                    PathImage += "J";
                    break;
                case 10:
                    PathImage += "K";
                    break;
                case 11:
                    PathImage += "L";
                    break;
                case 12:
                    PathImage += "M";
                    break;
                case 13:
                    PathImage += "N";
                    break;
                case 14:
                    PathImage += "R";
                    break;
            }

            switch (SecondLetterAnimation)
            {
                case 0:
                    PathImage += "A";
                    break;
                case 1:
                    PathImage += "B";
                    break;
                case 2:
                    PathImage += "C";
                    break;
                case 3:
                    PathImage += "D";
                    image.IsEnabled = false;
                    break;
                case 4:
                    PathImage += "E";
                    break;
                case 5:
                    PathImage += "F";
                    break;
                case 6:
                    PathImage += "G";
                    break;
                case 7:
                    PathImage += "H";
                    break;
                case 8:
                    PathImage += "I";
                    break;
                case 9:
                    PathImage += "J";
                    break;
                case 10:
                    PathImage += "K";
                    break;
                case 11:
                    PathImage += "L";
                    break;
                case 12:
                    PathImage += "M";
                    break;
                case 13:
                    PathImage += "N";
                    break;
                case 14:
                    PathImage += "O";
                    break;
                case 15:
                    PathImage += "P";
                    break;
                case 16:
                    PathImage += "Q";
                    break;
                case 17:
                    PathImage += "R";
                    break;
                case 18:
                    PathImage += "S";
                    break;
                case 19:
                    PathImage += "T";
                    break;
            }
            ChangeAnimation = false;
            return PathImage;
        }

        public void ChangeImage(Canvas GROD, double imageY, double imageX, bool changeDirection, double indexDirection)
        {
            GROD.Children.Remove(image);
            while (true)
            {
                if (!ChangeAnimation)
                {
                    if (changeDirection)
                    {
                        switch (indexDirection)
                        {
                            case 0:
                                Direction = "_w";
                                break;
                            case 1:
                                Direction = "_se";
                                break;
                            case 2:
                                Direction = "_e";
                                break;
                            case 3:
                                Direction = "_sw";
                                break;
                            case 4:
                                Direction = "_ne";
                                break;
                            case 5:
                                Direction = "_nw";
                                break;
                        }
                    }
                    else
                    {
                        switch (indexDirection)
                        {
                            case 0:
                                Direction = "_w";
                                break;
                            case 1:
                                Direction = "_sw";
                                break;
                            case 2:
                                Direction = "_e";
                                break;
                            case 3:
                                Direction = "_nw";
                                break;
                            case 4:
                                Direction = "_se";
                                break;
                            case 5:
                                Direction = "_ne";
                                break;
                        }
                    }

                    var imageAnimation = new BitmapImage();
                    
                    imageAnimation.BeginInit();
                    imageAnimation.UriSource = new Uri("pack://application:,,," + PathImage + Direction + ".gif", UriKind.RelativeOrAbsolute);
                    FullPathImage = "pack://application:,,," + PathImage + Direction + ".gif";
                    imageAnimation.EndInit();
                    ImageBehavior.SetAnimatedSource(image, imageAnimation);
                    var controller = ImageBehavior.GetAnimationController(image);
                    controller.Play();
                    Canvas.SetLeft(image, imageY - 10);
                    Canvas.SetTop(image, imageX);
                    Canvas.SetZIndex(image, ZIndexImage);
                    GROD.Children.Add(image);
                    break;
                }
                else PathImage = PathForImage();
            }
        }

        public string AttackAnimation(Canvas GROD, double imageY, double imageX, bool changeDirection, double indexDirection)
        {
            GROD.Children.Remove(image);
            while (true)
            {
                if (!ChangeAnimation)
                {
                    if (changeDirection)
                    {
                        switch (indexDirection)
                        {
                            case 0:
                                Direction = "_w";
                                break;
                            case 1:
                                Direction = "_se";
                                break;
                            case 2:
                                Direction = "_e";
                                break;
                            case 3:
                                Direction = "_sw";
                                break;
                            case 4:
                                Direction = "_ne";
                                break;
                            case 5:
                                Direction = "_nw";
                                break;
                        }
                    }
                    else
                    {
                        switch (indexDirection)
                        {
                            case 0:
                                Direction = "_w";
                                break;
                            case 1:
                                Direction = "_sw";
                                break;
                            case 2:
                                Direction = "_e";
                                break;
                            case 3:
                                Direction = "_nw";
                                break;
                            case 4:
                                Direction = "_se";
                                break;
                            case 5:
                                Direction = "_ne";
                                break;
                        }
                    }

                    var imageAnimation = new BitmapImage();

                    imageAnimation.BeginInit();
                    imageAnimation.UriSource = new Uri("pack://application:,,," + PathImage + Direction + ".gif", UriKind.RelativeOrAbsolute);
                    FullPathImage = "pack://application:,,," + PathImage + Direction + ".gif";
                    imageAnimation.EndInit();
                    ImageBehavior.SetAnimatedSource(image, imageAnimation);
                    var controller = ImageBehavior.GetAnimationController(image);
                    controller.Play();
                    Canvas.SetLeft(image, imageY - 10);
                    Canvas.SetTop(image, imageX);
                    Canvas.SetZIndex(image, ZIndexImage);
                    GROD.Children.Add(image);
                    break;
                }
                else PathImage = PathForImage();
            }
            return FullPathImage;
        }

        public void SetAnimation(byte numFolderCharacter, byte firstLetterAnimation, byte secondLetterAnimation)
        {
            NumFolderCharacter = numFolderCharacter;
            FirstLetterAnimation = firstLetterAnimation;
            SecondLetterAnimation = secondLetterAnimation;
            ChangeAnimation = true;
        }

        public void SetAnimation(string animation, Canvas GROD, double imageY, double imageX)
        {
            GROD.Children.Remove(image);
            var imageAnimation = new BitmapImage();
            imageAnimation.BeginInit();
            imageAnimation.UriSource = new Uri(animation, UriKind.RelativeOrAbsolute);
            FullPathImage = animation;
            imageAnimation.EndInit();
            ImageBehavior.SetAnimatedSource(image, imageAnimation);
            var controller = ImageBehavior.GetAnimationController(image);
            controller.Play();
            Canvas.SetLeft(image, imageY - 10);
            Canvas.SetTop(image, imageX);
            Canvas.SetZIndex(image, ZIndexImage);
            GROD.Children.Add(image);
            ChangeAnimation = true;
        }
    }
}
