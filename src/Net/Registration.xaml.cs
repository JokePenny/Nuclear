using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Nuclear.src
{
    public partial class Registration : Page
    {
        PlayerUser user = null;
        static int port = 2888;
        static string address = "84.201.150.2";
        TcpClient client = null;

        public Registration(PlayerUser connectUser)
        {
            InitializeComponent();
            user = connectUser;
            if(user.GetNickname() != null)
            {
                RegistrUser.Opacity = 0.3;
                RegistrUser.IsEnabled = false;
                Login.Text = user.GetNickname();
                Login.Opacity = 0.3;
                Login.IsEnabled = false;
                Password.Opacity = 0.3;
                Password.IsEnabled = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            client = new TcpClient(address, port);
            NetworkStream stream = client.GetStream();
            string message ="0 " + Login.Text + " " + Password.Text;
            byte[] data = Encoding.Unicode.GetBytes(message);
            data = new byte[256];
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (stream.DataAvailable);
            ChatTextBlock.Text += builder.ToString() + "\r\n";
            if(builder.ToString() == "Регистрация прошла успешно")
            {
                user.SetNickname(Login.Text);
                RegistrUser.Opacity = 0.3;
                RegistrUser.IsEnabled = false;
                Login.Opacity = 0.3;
                Login.IsEnabled = false;
                Password.Opacity = 0.3;
                Password.IsEnabled = false;
            }
            client.Close();
            stream.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new StartMenu(user));
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
    }
}
