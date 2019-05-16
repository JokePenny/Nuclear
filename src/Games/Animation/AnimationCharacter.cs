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
        private Image image { get; set; }
        private Image imageLight { get; set; }
        private string PathImage { get; set; }
        public string FullPathImage { get; set; }
        private string Direction { get; set; }
        private int ZIndexImage { get; set; } = 0;
        private int NumEndAnimaion { get; set; } = 0;
        private byte NumFolderCharacter { get; set; } = 9;
        private byte FirstLetterAnimation { get; set; } = 0;
        private byte SecondLetterAnimation { get; set; } = 0;
        private byte IndexDirection { get; set; } = 0;
        private bool ChangeAnimation { get; set; } = true;
        public bool SpeedMove { get; set; } = false; // false - ходьба, true - бег

        private double imageX { get; set; }
        private double imageY { get; set; }

        private double imageXOld;
        private double imageYOld;
        private bool changeDirection { get; set; } = false;
        private double indexDirection { get; set; } = 0;

        private int[] dx = { 0, 1, 0, 1, -1, -1 };
        private int[] dy = { -1, 0, 1, -1, 0, -1 };
        private int[] dx2 = { 0, 1, 0, -1, 1, -1 };
        private int[] dy2 = { -1, 0, 1, 0, 1, 1 };

        public void SetImageXImageY(int X, int Y)
        {
            imageX = X * 13 - 55;
            if (X % 2 != 0)
                imageY = Y * 96 / 2.74 + 10;
            else
                imageY = Y * 96 / 2.74 - 10;

            double checkImageX;
            double checkImageY;
            for (int d = 0; d < 6; d++)
            {
                if (X % 2 != 0)
                {
                    checkImageX = (imageXOld + dx[d]) * 13 - 55;
                    checkImageY = (imageYOld + dy[d]) * 96 / 2.74 + 10;
                    if (checkImageX == imageX && checkImageY == imageY)
                    {
                        ZIndexImage = X;
                        changeDirection = true;
                        indexDirection = d;
                        break;
                    }
                }
                else
                {
                    checkImageX = (imageXOld + dx2[d]) * 13 - 55;
                    checkImageY = (imageYOld + dy2[d]) * 96 / 2.74 - 10;
                    if (checkImageX == imageX && checkImageY == imageY)
                    {
                        ZIndexImage = X;
                        changeDirection = false;
                        indexDirection = d;
                        break;
                    }
                }
            }
            imageXOld = X;
            imageYOld = Y;
        }



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

        public AnimationCharacter(Canvas GROD, Game game, string nickname, int X, int Y)
        {
            SetImageXImageY(X, Y);
            image = new Image();
            image.IsHitTestVisible = false;
            imageLight = new Image();
            imageLight.Opacity = 0.3;
            imageLight.IsEnabled = false;
            imageLight.IsHitTestVisible = false;
            image.Source = new BitmapImage(new Uri("/data/image/characters/HMJMPS/HMJMPSAA_e.gif", UriKind.Relative));
            imageLight.Source = new BitmapImage(new Uri("/data/image/characters/lightPlayer.png", UriKind.Relative));
            FullPathImage = "pack://application:,,,/data/image/characters/HMJMPS/HMJMPSAA_e.gif";
            image.MouseLeftButtonDown += game.ActionWithPlayer_Click;
            image.MouseMove += game.ActionWithPlayer_Move;
            image.MouseLeave += game.ActionWithPlayer_MouseLeave;
            image.MouseEnter += game.ActionWithPlayer_Focusable;
            image.Name = nickname;
            ZIndexImage = X + 100;
            Canvas.SetLeft(image, imageY);
            Canvas.SetTop(image, imageX);
            Canvas.SetZIndex(image, ZIndexImage);
            Canvas.SetLeft(imageLight, imageY - 50);
            Canvas.SetTop(imageLight, imageX + 50);
            Canvas.SetZIndex(imageLight, ZIndexImage);
            GROD.Children.Add(imageLight);
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
                    if (NumEndAnimaion == 1)
                        PathImage += "A";
                    else
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
                    if (NumEndAnimaion == 1)
                        PathImage += "A";
                    else
                        PathImage += "T";
                    break;
            }
            ChangeAnimation = false;
            return PathImage;
        }

        /*   Direction
         * 
         *       N
         *   NW  |  NE
         *     \ | /
         * W ----+---- E
         *     / | \
         *   SW  |  SE
         *       S
         *
         */

        public void ChangeImage(Canvas GROD, int zindex)
        {
            ZIndexImage = zindex + 100;
            GROD.Children.Remove(image);
            GROD.Children.Remove(imageLight);
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
                                if (NumEndAnimaion != 1)
                                    imageX = imageX - 15;
                                break;
                            case 4:
                                Direction = "_ne";
                                if (NumEndAnimaion != 1)
                                    imageX = imageX - 15;
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
                                if (NumEndAnimaion != 1)
                                    imageX = imageX - 15;
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
                                if (NumEndAnimaion != 1)
                                    imageX = imageX - 15;
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
                    Canvas.SetLeft(imageLight, imageY - 10 - 130);
                    Canvas.SetTop(imageLight, imageX - 10);
                    Canvas.SetZIndex(imageLight, ZIndexImage);
                    GROD.Children.Add(imageLight);
                    GROD.Children.Add(image);
                    break;
                }
                else PathImage = PathForImage();
            }
        }

        public string AttackAnimation(Canvas GROD)
        {
            GROD.Children.Remove(image);
            GROD.Children.Remove(imageLight);
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
                    Canvas.SetLeft(imageLight, imageY - 10 - 50);
                    Canvas.SetTop(imageLight, imageX + 50);
                    Canvas.SetZIndex(imageLight, ZIndexImage);
                    GROD.Children.Add(imageLight);
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

        public void SetAnimation(int numEndAnimation, byte numFolderCharacter, byte firstLetterAnimation, byte secondLetterAnimation)
        {
            NumEndAnimaion = numEndAnimation;
            NumFolderCharacter = numFolderCharacter;
            FirstLetterAnimation = firstLetterAnimation;
            SecondLetterAnimation = secondLetterAnimation;
            ChangeAnimation = true;
        }

        public void SetAnimation(string animation, Canvas GROD)
        {
            GROD.Children.Remove(image);
            GROD.Children.Remove(imageLight);
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
            Canvas.SetLeft(imageLight, imageY - 10 - 50);
            Canvas.SetTop(imageLight, imageX + 50);
            Canvas.SetZIndex(imageLight, ZIndexImage);
            GROD.Children.Add(imageLight);
            GROD.Children.Add(image);
            ChangeAnimation = true;
        }

        public void SetHitTestVisible()
        {
            if (image.IsHitTestVisible)
                image.IsHitTestVisible = false;
            else image.IsHitTestVisible = true;
        }

        public void Disconect(Canvas GROD)
        {
            GROD.Children.Remove(image);
            GROD.Children.Remove(imageLight);
        }
    }
}
