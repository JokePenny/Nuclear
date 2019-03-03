using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Nuclear.src
{
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
            if ((sender as Border).Background == Brushes.Black)
            {
                (sender as Border).Background = Brushes.DarkGreen;
                if (Room.Visibility == Visibility.Collapsed)
                    Room.Visibility = Visibility.Visible;
            }
        }

        private void MoveLeaveRoom_but(object sender, MouseEventArgs e)
        {
            if((sender as Border).Background == Brushes.DarkGreen)
            {
                (sender as Border).Background = Brushes.Black;
                if (Room.Visibility == Visibility.Visible && ExitRoom.IsEnabled == false)
                    Room.Visibility = Visibility.Collapsed;
            }
        }

        private void MoveClickRoom_but(object sender, MouseEventArgs e)
        {
            (sender as Border).Background = Brushes.Green;
            ConnectRoom.IsEnabled = true;
            ConnectRoom.Opacity = 0.6;
            Room.Visibility = Visibility.Visible;
        }

        private void CreateRoom_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (CreateRoom.Visibility == Visibility.Collapsed)
                {
                    CreateWindow.IsEnabled = false;
                    StatsPlayer.Margin = new Thickness(0, 0, 165, 0);
                    CreateRoom.Visibility = Visibility.Visible;
                }
                else if (MapSelection.SelectedItem == null && ValuePlayers.Text == "" && NameRoom.Text == "")
                    ChatTextBlock.Text += "\r\n Не все поля заполнены";
                else
                {
                    StatsPlayer.Margin = new Thickness(0, 105, 5, 0);
                    CreateWindow.IsEnabled = true;
                    Border room = new Border();
                    room.Height = 20;
                    room.Background = Brushes.Black;
                    room.BorderBrush = Brushes.Green;
                    room.BorderThickness = new Thickness(2);
                    room.Margin = new Thickness(0, 5, 0, 0);
                    room.MouseMove += MoveMouseRoom_but;
                    room.MouseLeave += MoveLeaveRoom_but;
                    room.MouseLeftButtonDown += MoveClickRoom_but;

                    WrapPanel wrap = new WrapPanel();

                    TextBlock namemap = new TextBlock();
                    namemap.Width = 200;
                    namemap.Margin = new Thickness(0, 0, 196, 0);
                    namemap.TextAlignment = TextAlignment.Center;
                    namemap.Foreground = Brushes.Lime;
                    namemap.FontSize = 12;
                    namemap.FontFamily = new FontFamily(new Uri("pack://application:,,,/Nuclear"), "/data/fonts/#Fallout Display");
                    namemap.Text = NameRoom.Text;

                    TextBlock valPlayers = new TextBlock();
                    valPlayers.Width = 136;
                    valPlayers.TextAlignment = TextAlignment.Center;
                    valPlayers.Foreground = Brushes.Lime;
                    valPlayers.Margin = new Thickness(0, 0, 4, 0);
                    valPlayers.FontSize = 12;
                    valPlayers.FontFamily = new FontFamily(new Uri("pack://application:,,,/Nuclear"), "/data/fonts/#Fallout Display");
                    valPlayers.Text = "0" + "/" + ValuePlayers.Text;

                    TextBlock active = new TextBlock();
                    active.Width = 130;
                    active.TextAlignment = TextAlignment.Center;
                    active.Foreground = Brushes.Lime;
                    active.FontSize = 12;
                    active.FontFamily = new FontFamily(new Uri("pack://application:,,,/Nuclear"), "/data/fonts/#Fallout Display");
                    active.Text = "АКТИВЕН";

                    wrap.Children.Add(namemap);
                    wrap.Children.Add(valPlayers);
                    wrap.Children.Add(active);
                    room.Child = wrap;
                    StackRoom.Children.Add(room);
                    CreateRoom.Visibility = Visibility.Collapsed;
                }
            }
            catch{
                ChatTextBlock.Text += "\r\n Не все поля заполнены";
            }
        }
    }
}