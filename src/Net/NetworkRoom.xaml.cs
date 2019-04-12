using System;
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
        private PlayerUser User = null;
        private const int port = 2888;
        private const string address = "84.201.150.2";
        private TcpClient client = null;
        private NetworkStream stream;
        private string message = null;

        public NetworkRoom(PlayerUser connectUser)
        {
            InitializeComponent();
            User = connectUser;
            NicknamePlayer.Text = connectUser.GetNickname();
            LevelPlayer.Text = connectUser.GetLevel().ToString();
            UpdateAllRoom();
        }

        public NetworkRoom(PlayerUser connectUser, int exit)
        {
            InitializeComponent();
            User = connectUser;
            NicknamePlayer.Text = connectUser.GetNickname();
            LevelPlayer.Text = connectUser.GetLevel().ToString();
            Exits(exit.ToString());
            User.SetStateRoom(null);
            User.SetStateMap(null);
            UpdateAllRoom();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Exits("1");
            UpdateAllRoom();
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
                (sender as Border).Background = Brushes.DarkGreen;
        }

        private void MoveLeaveRoom_but(object sender, MouseEventArgs e)
        {
            if((sender as Border).Background == Brushes.DarkGreen)
                (sender as Border).Background = Brushes.Black;
        }

        private void MoveClickRoom_but(object sender, MouseEventArgs e)
        {
            foreach (Border br in StackRoom.Children)
                if (br.Background == Brushes.Green)
                    br.Background = Brushes.Black;
            foreach (StackPanel stack in StackPlayer.Children)
                stack.Visibility = Visibility.Collapsed;

            (sender as Border).Background = Brushes.Green;
            if(User.GetStateRoom() == null)
            {
                ConnectRoom.IsEnabled = true;
                ConnectRoom.MouseLeave += MoveLeave_but;
                ConnectRoom.MouseMove += MoveMouseMenuOnline_but;
                ConnectRoom.Opacity = 0.6;
            }
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
                if (Convert.ToInt32(ValuePlayers.Text) <= 1)
                    throw new Exception("Вместительность не должна быть меньше 2!\r\n");
                if (Convert.ToInt32(RangeDown.Text) > Convert.ToInt32(RangeUp.Text))
                    throw new Exception("Нижний порог входа выше чем верхний!\r\n");
                if (Convert.ToInt32(RangeUp.Text) < Convert.ToInt32(RangeDown.Text))
                    throw new Exception("Верхний порог входа ниже чем нижний!\r\n");
                foreach (StackPanel stack in StackPlayer.Children)
                    if (stack.Name == NameRoom.Text)
                        throw new Exception("Комната с таким именем уже существует\r\n");
                if (CreateRoom.Visibility == Visibility.Collapsed)
                {
                    CreateWindow.IsEnabled = false;
                    StatsPlayer.Margin = new Thickness(0, 0, 165, 0);
                    ValuePlayers.Text = "0";
                    RangeUp.Text = "0";
                    RangeDown.Text = "0";
                    CreateRoom.Visibility = Visibility.Visible;
                }
                else if (MapSelection.SelectedItem == null || ValuePlayers.Text == "" || NameRoom.Text == "" || RangeUp.Text == "" || RangeDown.Text == "")
                    throw new Exception("Не все поля заполнены\r\n");
                else
                {
                    client = new TcpClient(address, port);
                    stream = client.GetStream();
                    User.SetStateRoom(NameRoom.Text);
                    User.SetStateMap(MapSelection.Text);
                    message = "2 " + NameRoom.Text + " " + ValuePlayers.Text + " " + RangeUp.Text + " " + RangeDown.Text + " " + MapSelection.Text + " " + User.GetNickname() + " " + User.GetLevel().ToString();
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                    data = new byte[64];
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    if (builder.ToString() == "Комната успешно создана")
                    {
                        CreateWindow.IsEnabled = false;
                        CreateWindow.MouseMove -= MoveMouseMenuOnline_but;
                        CreateWindow.MouseLeave -= MoveLeave_but;
                        CreateWindow.Background = null;
                        CreateWindow.Opacity = 0.3;

                        ChatTextBlock.Text += builder.ToString() + "\r\n";
                        client.Close();

                        StatsPlayer.Margin = new Thickness(0, 86, 5, 0);
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
                        namemap.Width = 220;
                        namemap.TextAlignment = TextAlignment.Center;
                        namemap.Foreground = Brushes.Lime;
                        namemap.FontSize = 12;
                        namemap.FontFamily = new FontFamily(new Uri("pack://application:,,,/Nuclear"), "/data/fonts/#Fallout Display");
                        namemap.Text = NameRoom.Text;

                        TextBlock map = new TextBlock();
                        map.Width = 170;
                        map.TextAlignment = TextAlignment.Center;
                        map.Foreground = Brushes.Lime;
                        map.FontSize = 12;
                        map.FontFamily = new FontFamily(new Uri("pack://application:,,,/Nuclear"), "/data/fonts/#Fallout Display");
                        map.Text = MapSelection.Text;

                        TextBlock valPlayers = new TextBlock();
                        valPlayers.Width = 90;
                        valPlayers.TextAlignment = TextAlignment.Center;
                        valPlayers.Foreground = Brushes.Lime;
                        valPlayers.FontSize = 12;
                        valPlayers.Name = NameRoom.Text + "Count";
                        valPlayers.FontFamily = new FontFamily(new Uri("pack://application:,,,/Nuclear"), "/data/fonts/#Fallout Display");
                        valPlayers.Text = "0" + "/" + ValuePlayers.Text;

                        TextBlock range = new TextBlock();
                        range.Width = 75;
                        range.TextAlignment = TextAlignment.Center;
                        range.Foreground = Brushes.Lime;
                        range.FontSize = 12;
                        range.FontFamily = new FontFamily(new Uri("pack://application:,,,/Nuclear"), "/data/fonts/#Fallout Display");
                        range.Text = RangeDown.Text + " - " + RangeUp.Text;

                        TextBlock active = new TextBlock();
                        active.Width = 115;
                        active.TextAlignment = TextAlignment.Center;
                        active.Foreground = Brushes.Lime;
                        active.FontSize = 12;
                        active.FontFamily = new FontFamily(new Uri("pack://application:,,,/Nuclear"), "/data/fonts/#Fallout Display");
                        active.Text = "ОЖИДАНИЕ";

                        wrap.Children.Add(namemap);
                        wrap.Children.Add(map);
                        wrap.Children.Add(valPlayers);
                        wrap.Children.Add(range);
                        wrap.Children.Add(active);
                        room.Child = wrap;
                        StackRoom.Children.Add(room);

                        StackPanel listPlayer = new StackPanel();
                        listPlayer.Name = NameRoom.Text;
                        listPlayer.Visibility = Visibility.Collapsed;
                        StackPlayer.Children.Add(listPlayer);
                        NameRoom.Text = "";

                        CreateRoom.Visibility = Visibility.Collapsed;
                        UpdateAllRoom();
                        User.SetStateRoom(namemap.Text);
                        this.NavigationService.Navigate(new Game(User));
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception error)
            {
                ChatTextBlock.Text += error.Message + "\r\n";
            }
            finally
            {
                client.Close();
            }
        }

        private void EntranceRoom_Click(object sender, MouseButtonEventArgs e)
        {
            foreach (Border room in StackRoom.Children)
                if (room.Background == Brushes.Green)
                {
                    WrapPanel wrap = (WrapPanel)room.Child;
                    foreach (TextBlock map in wrap.Children)
                    {
                        User.SetStateRoom(map.Text);
                        break;
                    }
                    break;
                }
            client = new TcpClient(address, port);
            stream = client.GetStream();
            message = "3 " + User.GetStateRoom() + " " + User.GetNickname() + " " + User.GetLevel();
            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);
            data = new byte[64];
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (stream.DataAvailable);

            if (builder.ToString() != "Недоступен по уровню" && builder.ToString() != "Комната заполнена" && builder.ToString() != "Комната заполнена")
            {
                this.NavigationService.Navigate(new Game(User));
                client.Close();
                stream.Close();
            }
            else
            {
                ChatTextBlock.Text += builder.ToString() + "\r\n ";
                StreamResourceInfo str = Application.GetResourceStream(
                   new Uri("data/image/mainui/cursor/ACTARROW.cur", UriKind.Relative));
                Cursor customCursorr = new Cursor(str.Stream);
                Mouse.OverrideCursor = customCursorr;
                client.Close();
                stream.Close();
                UpdateAllRoom();
            }
        }

        private void CloseCreteRoom_Click(object sender, MouseButtonEventArgs e)
        {
            CreateWindow.IsEnabled = true;
            StatsPlayer.Margin = new Thickness(0, 86, 5, 0);
            CreateRoom.Visibility = Visibility.Collapsed;
        }

        private void Number_CheckWrite(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789".IndexOf(e.Text) < 0;
        }
        

        /*
        private void ReadinessRoom_Click(object sender, MouseButtonEventArgs e) // засунуть в подключение
        {
            client = new TcpClient(address, port);
            NetworkStream stream = client.GetStream();
            message = "4 " + User.GetStateRoom() + " " + User.GetNickname() + " " + Readiness.Text;
            byte[] data = Encoding.Unicode.GetBytes(message);
            // отправка сообщения
            stream.Write(data, 0, data.Length);
            // получаем ответ
            data = new byte[64]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (stream.DataAvailable);
            client.Close();
            stream.Close();  
        }
        */

        private void ExitRoom_Click(object sender, MouseButtonEventArgs e) // засунуть в гейм (выход из игры)
        {
            Exits("0");
            User.SetStateRoom(null);
            User.SetStateMap(null);
            UpdateAllRoom();
        }

        private void Exits(string exit)
        {
            client = new TcpClient(address, port);
            NetworkStream stream = client.GetStream();
            message = "7 " + User.GetStateRoom() + " " + User.GetNickname() + " " + exit;
            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);
            data = new byte[64];
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (stream.DataAvailable);
            ChatTextBlock.Text += builder.ToString() + "\r\n";
            client.Close();
            stream.Close();
        }

        private void UpdateAllRoom_Click(object sender, MouseButtonEventArgs e)
        {
            UpdateAllRoom();
        }

        private void UpdateAllRoom()
        {
            StackRoom.Children.Clear();
            StackPlayer.Children.Clear();
            client = new TcpClient(address, port);
            NetworkStream stream = client.GetStream();
            message = "5";
            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);
            data = new byte[64];
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (stream.DataAvailable);
            if (builder.ToString() != "Комнат нет")
            {
                client.Close();
                stream.Close();
                string[] rows = builder.ToString().Split(';');
                string[] elements;
                for (int i = 0; i < rows.Length - 1; i++)
                {
                    elements = rows[i].Split(' ');
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
                    namemap.Width = 220;
                    namemap.TextAlignment = TextAlignment.Center;
                    namemap.Foreground = Brushes.Lime;
                    namemap.FontSize = 12;
                    namemap.FontFamily = new FontFamily(new Uri("pack://application:,,,/Nuclear"), "/data/fonts/#Fallout Display");
                    namemap.Text = elements[0];

                    TextBlock map = new TextBlock();
                    map.Width = 170;
                    map.TextAlignment = TextAlignment.Center;
                    map.Foreground = Brushes.Lime;
                    map.FontSize = 12;
                    map.FontFamily = new FontFamily(new Uri("pack://application:,,,/Nuclear"), "/data/fonts/#Fallout Display");
                    map.Text = elements[1];

                    TextBlock valPlayers = new TextBlock();
                    valPlayers.Width = 90;
                    valPlayers.TextAlignment = TextAlignment.Center;
                    valPlayers.Foreground = Brushes.Lime;
                    valPlayers.FontSize = 12;
                    valPlayers.Name = NameRoom.Text + "Count";
                    valPlayers.FontFamily = new FontFamily(new Uri("pack://application:,,,/Nuclear"), "/data/fonts/#Fallout Display");
                    valPlayers.Text = elements[3] + "/" + elements[2];

                    TextBlock range = new TextBlock();
                    range.Width = 75;
                    range.TextAlignment = TextAlignment.Center;
                    range.Foreground = Brushes.Lime;
                    range.FontSize = 12;
                    range.FontFamily = new FontFamily(new Uri("pack://application:,,,/Nuclear"), "/data/fonts/#Fallout Display");
                    range.Text = elements[5] + " - " + elements[4];

                    TextBlock active = new TextBlock();
                    active.Width = 115;
                    active.TextAlignment = TextAlignment.Center;
                    active.Foreground = Brushes.Lime;
                    active.FontSize = 12;
                    active.FontFamily = new FontFamily(new Uri("pack://application:,,,/Nuclear"), "/data/fonts/#Fallout Display");
                    if (elements[6] == "0")
                        active.Text = "ОЖИДАНИЕ";
                    else
                        active.Text = "В ИГРЕ";

                    wrap.Children.Add(namemap);
                    wrap.Children.Add(map);
                    wrap.Children.Add(valPlayers);
                    wrap.Children.Add(range);
                    wrap.Children.Add(active);
                    room.Child = wrap;
                    StackRoom.Children.Add(room);

                    StackPanel listPlayer = new StackPanel();
                    listPlayer.Name = elements[0];
                    listPlayer.Visibility = Visibility.Collapsed;

                    client = new TcpClient(address, port);
                    stream = client.GetStream();
                    message = "6 " + elements[0];
                    data = Encoding.Unicode.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                    data = new byte[64];
                    builder = new StringBuilder();
                    bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);
                    if (builder.ToString() != "")
                    {
                        client.Close();
                        stream.Close();
                        string[] rowsUser = builder.ToString().Split(';');
                        for (int j = 0; j < rowsUser.Length - 1; j++)
                        {
                            elements = rowsUser[j].Split(' ');
                            room = new Border();
                            room.Height = 20;
                            room.Background = Brushes.Black;
                            room.BorderBrush = Brushes.Green;
                            room.BorderThickness = new Thickness(2);
                            room.Margin = new Thickness(0, 5, 0, 0);

                            wrap = new WrapPanel();

                            TextBlock nickname = new TextBlock();
                            nickname.Width = 150;
                            nickname.Margin = new Thickness(0, 0, 0, 0);
                            nickname.TextAlignment = TextAlignment.Center;
                            nickname.Foreground = Brushes.Lime;
                            nickname.FontSize = 12;
                            nickname.FontFamily = new FontFamily(new Uri("pack://application:,,,/Nuclear"), "/data/fonts/#Fallout Display");
                            nickname.Text = elements[0];

                            TextBlock levelUser = new TextBlock();
                            levelUser.Width = 100;
                            levelUser.TextAlignment = TextAlignment.Center;
                            levelUser.Foreground = Brushes.Lime;
                            levelUser.Margin = new Thickness(10, 0, 5, 0);
                            levelUser.FontSize = 12;
                            levelUser.FontFamily = new FontFamily(new Uri("pack://application:,,,/Nuclear"), "/data/fonts/#Fallout Display");
                            levelUser.Text = elements[1];
                            wrap.Children.Add(nickname);
                            wrap.Children.Add(levelUser);
                            room.Child = wrap;
                            listPlayer.Children.Add(room);
                        }
                    }
                    client.Close();
                    stream.Close();
                    StackPlayer.Children.Add(listPlayer);
                }
            }
            else
            {
                ChatTextBlock.Text += builder.ToString() + "\r\n";
                client.Close();
                stream.Close();
            }

            client = new TcpClient(address, port);
            stream = client.GetStream();
            message = "8";
            data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);
            data = new byte[64];
            builder = new StringBuilder();
            bytes = 0;
            do
            {
                bytes = stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (stream.DataAvailable);
            CountOnlineUsers.Text = builder.ToString();
            client.Close();
            stream.Close();
        }
    }
}