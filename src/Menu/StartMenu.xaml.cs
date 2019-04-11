using Nuclear.src;
using System;
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
        const int port = 2888;
        const string address = "84.201.150.2";
        TcpClient client = null;
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
                try
                {
                    client = new TcpClient(address, port);
                    NetworkStream stream = client.GetStream();

                    string message = "1 " + Login.Text;
                    message += " " + Password.Text;
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    // отправка сообщения
                    stream.Write(data, 0, data.Length);
                    // получаем ответ
                    data = new byte[256]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string[] answer = builder.ToString().Split(';');
                    if (answer[0] == "Вход выполнен")
                    {
                        StreamResourceInfo sris = Application.GetResourceStream(
                            new Uri("data/image/mainui/cursor/ACTARROW.cur", UriKind.Relative));
                        Cursor customCursors = new Cursor(sris.Stream);
                        Mouse.OverrideCursor = customCursors;
                        user.SetNickname(Login.Text);
                        user.SetLevel(Convert.ToInt32(answer[1]));
                        client.Close();
                        stream.Close();
                        this.NavigationService.Navigate(new NetworkRoom(user));
                    }
                    else
                    {
                        ChatTextBlock.Text += "\r\n" + builder.ToString();
                        client.Close();
                        stream.Close();
                        StreamResourceInfo str = Application.GetResourceStream(
                           new Uri("data/image/mainui/cursor/ACTARROW.cur", UriKind.Relative));
                        Cursor customCursorr = new Cursor(str.Stream);
                        Mouse.OverrideCursor = customCursorr;
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

        private void Editor_Click(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new MapEditor());
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
