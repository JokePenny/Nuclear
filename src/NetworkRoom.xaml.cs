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

namespace Nuclear.src
{
    public partial class NetworkRoom : Page
    {
        PlayerUser User = null;
        // адрес и порт сервера, к которому будем подключаться
        static private int port = 8005; // порт сервера
        static private string address = "127.0.0.1"; // адрес сервера
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public NetworkRoom(string nicknamePlayer, PlayerUser connectUser)
        {
            InitializeComponent();
            NicknamePlayer.Text = nicknamePlayer;
            User = connectUser;
            socket.Connect(ipPoint);
        }

        public NetworkRoom(string nicknamePlayer)
        {
            InitializeComponent();
            NicknamePlayer.Text = nicknamePlayer;
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
            (sender as TextBlock).Background = null;
        }

        private void MoveMouseRoom_but(object sender, MouseEventArgs e)
        {
            if ((sender as Border).Background == Brushes.Black)
            {
                (sender as Border).Background = Brushes.DarkGreen;
                if (Room.Visibility == Visibility.Collapsed)
                {
                    WrapPanel wp = (WrapPanel)(sender as Border).Child;
                    foreach (TextBlock bl in wp.Children)
                    {
                        NameRoomSelection.Text = bl.Text;
                        break;
                    }
                    Room.Visibility = Visibility.Visible;
                }
            }
        }

        private void MoveLeaveRoom_but(object sender, MouseEventArgs e)
        {
            if((sender as Border).Background == Brushes.DarkGreen)
            {
                (sender as Border).Background = Brushes.Black;
                if (Room.Visibility == Visibility.Visible && Room.IsEnabled == false)
                    Room.Visibility = Visibility.Collapsed;
            }
        }

        private void MoveClickRoom_but(object sender, MouseEventArgs e)
        {
            foreach (Border br in StackRoom.Children)
                if (br.Background == Brushes.Green)
                    br.Background = Brushes.Black;
            foreach (StackPanel stack in StackPlayer.Children)
                stack.Visibility = Visibility.Collapsed;

            (sender as Border).Background = Brushes.Green;
            ConnectRoom.IsEnabled = true;
            ConnectRoom.MouseLeave += MoveLeave_but;
            ConnectRoom.MouseMove += MoveMouseMenuOnline_but;
            ConnectRoom.Opacity = 0.6;
            Room.Visibility = Visibility.Visible;
            Room.IsEnabled = true;
            foreach (Border br in StackRoom.Children)
            {
                if (br.Background == Brushes.Green)
                {
                    WrapPanel wp = (WrapPanel)br.Child;
                    foreach (TextBlock bl in wp.Children)
                    { 
                        NameRoomSelection.Text = bl.Text;
                        break;
                    }
                }
            }
            foreach (StackPanel stack in StackPlayer.Children)
            {
                if (stack.Name == NameRoomSelection.Text)
                    stack.Visibility = Visibility.Visible;
                else stack.Visibility = Visibility.Collapsed;
            }
        }

        private void CreateRoom_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(RangeDown.Text) > Convert.ToInt32(RangeUp.Text))
                    throw new Exception("\r\n Нижний порог входа выше чем верхний!");
                if (Convert.ToInt32(RangeUp.Text) < Convert.ToInt32(RangeDown.Text))
                    throw new Exception("\r\n Верхний порог входа ниже чем нижний!");
                foreach (StackPanel stack in StackPlayer.Children)
                    if (stack.Name == NameRoom.Text)
                        throw new Exception("\r\n Комната с таким именем уже существует");
                if (CreateRoom.Visibility == Visibility.Collapsed)
                {
                    CreateWindow.IsEnabled = false;
                    StatsPlayer.Margin = new Thickness(0, 0, 165, 0);
                    ValuePlayers.Text = "0";
                    RangeUp.Text = "0";
                    RangeDown.Text = "0";
                    CreateRoom.Visibility = Visibility.Visible;
                }
                else if (MapSelection.SelectedItem == null || ValuePlayers.Text == "" || NameRoom.Text == "")
                    throw new Exception("\r\n Не все поля заполнены");
                else
                {
                    ChatTextBlock.Text += "\r\n" + MapSelection.Text;
                    string message = "2 " + NameRoom.Text + " " + " " + ValuePlayers.Text + " " + RangeUp.Text + " " + RangeDown.Text + " " + MapSelection.Text;
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
                    //ChatTextBlock.Text += "\r\n" + builder.ToString();
                    if (builder.ToString() == "Комната успешно создана")
                    {
                        ChatTextBlock.Text += "\r\n" + builder.ToString();

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
                        active.Text = "ОЖИДАНИЕ";

                        wrap.Children.Add(namemap);
                        wrap.Children.Add(valPlayers);
                        wrap.Children.Add(active);
                        room.Child = wrap;
                        StackRoom.Children.Add(room);

                        StackPanel listPlayer = new StackPanel();
                        listPlayer.Name = NameRoom.Text;
                        listPlayer.Visibility = Visibility.Collapsed;
                        StackPlayer.Children.Add(listPlayer);
                        NameRoom.Text = "";

                        CreateRoom.Visibility = Visibility.Collapsed;
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception error)
            {
                ChatTextBlock.Text += "\r\n" + error.Message;
            }
        }

        private void EntranceRoom_Click(object sender, MouseButtonEventArgs e)
        {
            string message = "2 " + User.GetNickname();
            message += " " + User.GetLevel();
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
            //ChatTextBlock.Text += "\r\n" + builder.ToString();
            if (builder.ToString() == "Вы зашли в комнату" || builder.ToString() == "Вы зашли в комнату перед этим ее создав")
            {
                StreamResourceInfo sris = Application.GetResourceStream(
                    new Uri("data/image/mainui/cursor/ACTARROW.cur", UriKind.Relative));
                Cursor customCursors = new Cursor(sris.Stream);
                Mouse.OverrideCursor = customCursors;
                ChatTextBlock.Text += "\r\n" + builder.ToString();

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
                namemap.Width = 140;
                namemap.Margin = new Thickness(0, 0, 0, 0);
                namemap.TextAlignment = TextAlignment.Center;
                namemap.Foreground = Brushes.Lime;
                namemap.FontSize = 12;
                namemap.FontFamily = new FontFamily(new Uri("pack://application:,,,/Nuclear"), "/data/fonts/#Fallout Display");
                namemap.Text = "Lorne";

                TextBlock valPlayers = new TextBlock();
                valPlayers.Width = 140;
                valPlayers.TextAlignment = TextAlignment.Center;
                valPlayers.Foreground = Brushes.Lime;
                valPlayers.Margin = new Thickness(0, 0, 5, 0);
                valPlayers.FontSize = 12;
                valPlayers.FontFamily = new FontFamily(new Uri("pack://application:,,,/Nuclear"), "/data/fonts/#Fallout Display");
                valPlayers.Text = "4";

                wrap.Children.Add(namemap);
                wrap.Children.Add(valPlayers);
                room.Child = wrap;
                foreach (StackPanel stack in StackPlayer.Children)
                    if (stack.Visibility == Visibility.Visible)
                        stack.Children.Add(room);

                ConnectRoom.IsEnabled = false;
                ConnectRoom.MouseLeave -= MoveLeave_but;
                ConnectRoom.MouseMove -= MoveMouseMenuOnline_but;
                ConnectRoom.Background = null;
                ConnectRoom.Opacity = 0.3;
                //socket.Shutdown(SocketShutdown.Both);
                //socket.Close();
            }
            else
            {
                ChatTextBlock.Text += "\r\n Комната заполнена или не соответсвует вашему уровню";
                //socket.Shutdown(SocketShutdown.Both);
                //socket.Close();
                StreamResourceInfo str = Application.GetResourceStream(
                   new Uri("data/image/mainui/cursor/ACTARROW.cur", UriKind.Relative));
                Cursor customCursorr = new Cursor(str.Stream);
                Mouse.OverrideCursor = customCursorr;
            }
        }

        private void CloseCreteRoom_Click(object sender, MouseButtonEventArgs e)
        {
            CreateWindow.IsEnabled = true;
            StatsPlayer.Margin = new Thickness(0, 105, 5, 0);
            CreateRoom.Visibility = Visibility.Collapsed;
        }

        private void Number_CheckWrite(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789".IndexOf(e.Text) < 0;
        }
    }
}