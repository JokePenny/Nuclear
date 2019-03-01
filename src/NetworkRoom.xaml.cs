using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nuclear.src
{
    /// <summary>
    /// Логика взаимодействия для NetworkRoom.xaml
    /// </summary>
    public partial class NetworkRoom : Page
    {
        public NetworkRoom()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new StartMenu());
        }

        private void MoveMouseMenuOnline_but(object sender, MouseEventArgs e)
        {
            (sender as TextBlock).Opacity = 0.8;
            ImageBrush das = new ImageBrush();
            das.ImageSource = new BitmapImage(new Uri(@"data/image/mainui/startClick/backlight_both.png", UriKind.Relative));
            (sender as TextBlock).Background = das;
        }

        private void MoveMouseMenuSort_but(object sender, MouseEventArgs e)
        {
            (sender as TextBlock).Opacity = 0.8;
        }

        private void MoveMouse_but(object sender, MouseEventArgs e)
        {
            (sender as TextBlock).Opacity = 0.8;
            ImageBrush das = new ImageBrush();
            das.ImageSource = new BitmapImage(new Uri(@"data/image/mainui/startClick/backlight.png", UriKind.Relative));
            (sender as TextBlock).Background = das;
        }

        private void MoveLeave_but(object sender, MouseEventArgs e)
        {
            (sender as TextBlock).Opacity = 0.6;
            ImageBrush das = new ImageBrush();
            (sender as TextBlock).Background = das;
        }

        private void MoveMouseRoom_but(object sender, MouseEventArgs e)
        {
            (sender as Border).Background = Brushes.DarkGreen;
        }

        private void MoveLeaveRoom_but(object sender, MouseEventArgs e)
        {
            (sender as Border).Background = Brushes.Black;
        }

        private void CreateRoom_Click(object sender, MouseButtonEventArgs e)
        {
            if(CreateRoom.Visibility == Visibility.Collapsed)
                CreateRoom.Visibility = Visibility.Visible;
            else
            {
                CreateRoom.Visibility = Visibility.Collapsed;
            }
        }
    }
}
