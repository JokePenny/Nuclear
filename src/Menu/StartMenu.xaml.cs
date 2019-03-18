using Nuclear.src;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace Nuclear
{
    public partial class StartMenu : Page
    {
        PlayerUser user = null;
        public StartMenu()
        {
            InitializeComponent();
            user = new PlayerUser();
            ShowsNavigationUI = false;
        }

        public StartMenu(PlayerUser connectUser)
        {
            InitializeComponent();
            user = connectUser;
            if(user.GetNickname() != null)
                Login.Text = user.GetNickname();
            ShowsNavigationUI = false;
        }

        private void Game_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Game());
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Registration(user));
        }

        private void OnlineGame_Click(object sender, RoutedEventArgs e)
        {
            if(Login.Text != "" && Password.Text != "")
            {
                Mouse.OverrideCursor = Cursors.Wait;
                // адрес и порт сервера, к которому будем подключаться
                int port = 8005; // порт сервера
                string address = "127.0.0.1"; // адрес сервера
                Socket socket = null;
                try
                {
                    IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect(ipPoint);

                    string message = "1 " + Login.Text;
                    message += " " + Password.Text;
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    socket.Send(data);
                    data = new byte[256];
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = socket.Receive(data, data.Length, 0);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (socket.Available > 0);
                    string[] answer = builder.ToString().Split(';');
                    //ChatTextBlock.Text += "\r\n" + builder.ToString();
                    if (answer[0] == "Вход выполнен")
                    {
                        StreamResourceInfo sris = Application.GetResourceStream(
                            new Uri("data/image/mainui/cursor/ACTARROW.cur", UriKind.Relative));
                        Cursor customCursors = new Cursor(sris.Stream);
                        Mouse.OverrideCursor = customCursors;
                        user.SetNickname(Login.Text);
                        user.SetLevel(Convert.ToInt32(answer[1]));
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                        this.NavigationService.Navigate(new NetworkRoom(user));
                    }
                    else
                    {
                        ChatTextBlock.Text += "\r\n" + builder.ToString();
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                        StreamResourceInfo str = Application.GetResourceStream(
                           new Uri("data/image/mainui/cursor/ACTARROW.cur", UriKind.Relative));
                        Cursor customCursorr = new Cursor(str.Stream);
                        Mouse.OverrideCursor = customCursorr;
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                    }
                }
                catch (Exception ex)
                {
                    StreamResourceInfo sria = Application.GetResourceStream(
                           new Uri("data/image/mainui/cursor/ACTARROW.cur", UriKind.Relative));
                    Cursor customCursore = new Cursor(sria.Stream);
                    Mouse.OverrideCursor = customCursore;
                    ChatTextBlock.Text += "\r\n Сервер неактивен!";
                }
            }
            else
                ChatTextBlock.Text += "\r\n Введите Логин и Пароль!";
            StreamResourceInfo sri = Application.GetResourceStream(
                           new Uri("data/image/mainui/cursor/ACTARROW.cur", UriKind.Relative));
            Cursor customCursor = new Cursor(sri.Stream);
            Mouse.OverrideCursor = customCursor;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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
