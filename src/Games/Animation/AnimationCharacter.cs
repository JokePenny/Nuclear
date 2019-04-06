using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Nuclear.src
{
    class AnimationCharacter
    {
        public Image image { get; set; }
        public string pathImage { get; set; } = "/data/image/characters/HMJMPS";
        public string pathanimation { get; set; } = "/AA/";
        public string direction { get; set; } = "_w";
        public byte IDCharacter { get; set; }
        public byte IDAnimation { get; set; }
        public byte indexDirection { get; set; } = 0;
        public bool changeDirection { get; set; } = false;

        AnimationCharacter(byte ID, byte IDAnim)
        {
            IDCharacter = ID;
            IDAnimation = IDAnim;
            switch (IDCharacter)
            {
                case 0:
                    pathImage = "/data/image/characters/HMJMPS";
                    break;
                case 1:
                    pathImage = "";
                    break;
                case 2:
                    pathImage = "";
                    break;
                case 3:
                    pathImage = "";
                    break;
                case 4:
                    pathImage = "";
                    break;
                case 5:
                    pathImage = "";
                    break;
                case 6:
                    pathImage = "";
                    break;
                case 7:
                    pathImage = "";
                    break;
                case 8:
                    pathImage = "";
                    break;
                case 9:
                    pathImage = "";
                    break;
                case 10:
                    pathImage = "";
                    break;
                case 11:
                    pathImage = "";
                    break;
                case 12:
                    pathImage = "";
                    break;
                case 13:
                    pathImage = "";
                    break;
                case 14:
                    pathImage = "";
                    break;
                case 15:
                    pathImage = "";
                    break;
            }
            //for(int i = 1; i < 8; i++)
                //if(IDAnimation < i * 8)
        }


        public void ChangeImage(Canvas GROD, int imageY, int imageX)
        {
            GROD.Children.Remove(image);
            if (changeDirection)
            {
                switch (indexDirection)
                {
                    case 0:
                        direction = "_w";
                        break;
                    case 1:
                        direction = "_se";
                        break;
                    case 2:
                        direction = "_e";
                        break;
                    case 3:
                        direction = "_sw";
                        break;
                    case 4:
                        direction = "_ne";
                        break;
                    case 5:
                        direction = "_nw";
                        break;
                }
            }
            else
            {
                switch (indexDirection)
                {
                    case 0:
                        direction = "_w";
                        break;
                    case 1:
                        direction = "_sw";
                        break;
                    case 2:
                        direction = "_se";
                        break;
                    case 3:
                        direction = "_nw";
                        break;
                    case 4:
                        direction = "_se";
                        break;
                    case 5:
                        direction = "_ne";
                        break;
                }
            }
            image.Source = new BitmapImage(new Uri(pathImage + pathanimation + direction +".gif", UriKind.Relative));
            Canvas.SetLeft(image, imageY - 10);
            Canvas.SetTop(image, imageX);
            GROD.Children.Add(image);
        }
    }
}
