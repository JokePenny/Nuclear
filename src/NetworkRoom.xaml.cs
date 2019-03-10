﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;

namespace Nuclear.src
{
    public partial class NetworkRoom : Page
    {
        PlayerUser User = null;
        // адрес и порт сервера, к которому будем подключаться
        static private int port = 8005; // порт сервера
        static private string address = "127.0.0.1"; // адрес сервера
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

        public NetworkRoom(PlayerUser connectUser)
        {
            InitializeComponent();
            User = connectUser;
            NicknamePlayer.Text = connectUser.GetNickname();
            UpdateAllRoom();
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
                else if (MapSelection.SelectedItem == null || ValuePlayers.Text == "" || NameRoom.Text == "" || RangeUp.Text == "" || RangeDown.Text == "")
                    throw new Exception("\r\n Не все поля заполнены");
                else
                {
                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect(ipPoint);
                    string message = "2 " + NameRoom.Text + " " + ValuePlayers.Text + " " + RangeUp.Text + " " + RangeDown.Text + " " + MapSelection.Text;
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
                    if (builder.ToString() == "Комната успешно создана")
                    {
                        ChatTextBlock.Text += "\r\n" + builder.ToString();
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
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
                        namemap.Width = 136;
                        namemap.Margin = new Thickness(0, 0, 90, 0);
                        namemap.TextAlignment = TextAlignment.Center;
                        namemap.Foreground = Brushes.Lime;
                        namemap.FontSize = 12;
                        namemap.FontFamily = new FontFamily(new Uri("pack://application:,,,/Nuclear"), "/data/fonts/#Fallout Display");
                        namemap.Text = NameRoom.Text;

                        TextBlock map = new TextBlock();
                        map.Width = 160;
                        map.Margin = new Thickness(0, 0, 10, 0);
                        map.TextAlignment = TextAlignment.Center;
                        map.Foreground = Brushes.Lime;
                        map.FontSize = 12;
                        map.FontFamily = new FontFamily(new Uri("pack://application:,,,/Nuclear"), "/data/fonts/#Fallout Display");
                        map.Text = MapSelection.Text;

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
                        wrap.Children.Add(map);
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
            string namemap = "";
            string nameroom = "";
            foreach (Border room in StackRoom.Children)
                if (room.Background == Brushes.Green)
                {
                    WrapPanel wrap = (WrapPanel)room.Child;
                    int i = 0;
                    foreach (TextBlock map in wrap.Children)
                    {
                        nameroom = map.Text;
                        break;
                    }
                    break;
                }
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ipPoint);
            string message = "3 " + nameroom + " " + User.GetNickname() + " " + User.GetLevel();
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
            if (builder.ToString() == "Вы зашли в комнату" || builder.ToString() == "Вы зашли в комнату перед этим ее создав")
            {
                StreamResourceInfo sris = Application.GetResourceStream(
                    new Uri("data/image/mainui/cursor/ACTARROW.cur", UriKind.Relative));
                Cursor customCursors = new Cursor(sris.Stream);
                Mouse.OverrideCursor = customCursors;
                ChatTextBlock.Text += "\r\n" + builder.ToString();

                Readiness.IsEnabled = true;
                Readiness.Opacity = 0.6;
                ExitRoom.IsEnabled = true;
                ExitRoom.Opacity = 0.6;

                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                Border room = new Border();
                room.Height = 20;
                room.Background = Brushes.Black;
                room.BorderBrush = Brushes.Green;
                room.BorderThickness = new Thickness(2);
                room.Margin = new Thickness(0, 5, 0, 0);

                WrapPanel wrap = new WrapPanel();

                TextBlock nickname = new TextBlock();
                nickname.Width = 150;
                nickname.Margin = new Thickness(0, 0, 0, 0);
                nickname.TextAlignment = TextAlignment.Center;
                nickname.Foreground = Brushes.Lime;
                nickname.FontSize = 12;
                nickname.FontFamily = new FontFamily(new Uri("pack://application:,,,/Nuclear"), "/data/fonts/#Fallout Display");
                nickname.Text = User.GetNickname();

                TextBlock levelUser = new TextBlock();
                levelUser.Width = 100;
                levelUser.TextAlignment = TextAlignment.Center;
                levelUser.Foreground = Brushes.Lime;
                levelUser.Margin = new Thickness(10, 0, 5, 0);
                levelUser.FontSize = 12;
                levelUser.FontFamily = new FontFamily(new Uri("pack://application:,,,/Nuclear"), "/data/fonts/#Fallout Display");
                levelUser.Text = "4";

                wrap.Children.Add(nickname);
                wrap.Children.Add(levelUser);
                room.Child = wrap;
                foreach (StackPanel stack in StackPlayer.Children)
                    if (stack.Visibility == Visibility.Visible)
                        stack.Children.Add(room);

                ConnectRoom.IsEnabled = false;
                ConnectRoom.MouseLeave -= MoveLeave_but;
                ConnectRoom.MouseMove -= MoveMouseMenuOnline_but;
                ConnectRoom.Background = null;
                ConnectRoom.Opacity = 0.3;
            }
            else
            {
                ChatTextBlock.Text += "\r\n Комната заполнена или не соответсвует вашему уровню";
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
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

        private void ReadinessRoom_Click(object sender, MouseButtonEventArgs e)
        {
            int check = 0;
            foreach (StackPanel stack in StackPlayer.Children)
                if (stack.Visibility == Visibility.Visible)
                {
                    foreach (Border user in stack.Children)
                    {
                        WrapPanel panel = (WrapPanel)user.Child;
                        Ellipse reads = null;
                        foreach (var nickname in panel.Children)
                            if(nickname is Ellipse)
                            {
                                reads = (nickname as Ellipse);
                                check = 1;
                            }
                        if (check != 1)
                        {
                            reads = new Ellipse();
                            reads.Width = 13;
                            reads.Height = 13;
                            reads.Margin = new Thickness(0, 0, 5, 0);
                            reads.Fill = Brushes.Lime;
                            panel.Background = Brushes.Black;
                            panel.Children.Add(reads);
                            user.Child = panel;
                            Readiness.Text = "НЕ ГОТОВ";
                            break;
                        }
                        else
                        {
                            panel.Children.Remove(reads);
                            user.Child = panel;
                            Readiness.Text = "ГОТОВ";
                            break;
                        }
                    }
                }
        }

        private void ExitRoom_Click(object sender, MouseButtonEventArgs e)
        {

        }

        private void UpdateAllRoom_Click(object sender, MouseButtonEventArgs e)
        {
            UpdateAllRoom();
        }

        private void UpdateAllRoom()
        {
            StackRoom.Children.Clear();
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ipPoint);
            string message = "5";
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
            if (builder.ToString() != "Комнат нет")
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
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
                    namemap.Width = 136;
                    namemap.Margin = new Thickness(0, 0, 90, 0);
                    namemap.TextAlignment = TextAlignment.Center;
                    namemap.Foreground = Brushes.Lime;
                    namemap.FontSize = 12;
                    namemap.FontFamily = new FontFamily(new Uri("pack://application:,,,/Nuclear"), "/data/fonts/#Fallout Display");
                    namemap.Text = elements[0];

                    TextBlock map = new TextBlock();
                    map.Width = 160;
                    map.Margin = new Thickness(0, 0, 10, 0);
                    map.TextAlignment = TextAlignment.Center;
                    map.Foreground = Brushes.Lime;
                    map.FontSize = 12;
                    map.FontFamily = new FontFamily(new Uri("pack://application:,,,/Nuclear"), "/data/fonts/#Fallout Display");
                    map.Text = elements[1];

                    TextBlock valPlayers = new TextBlock();
                    valPlayers.Width = 136;
                    valPlayers.TextAlignment = TextAlignment.Center;
                    valPlayers.Foreground = Brushes.Lime;
                    valPlayers.Margin = new Thickness(0, 0, 4, 0);
                    valPlayers.FontSize = 12;
                    valPlayers.FontFamily = new FontFamily(new Uri("pack://application:,,,/Nuclear"), "/data/fonts/#Fallout Display");
                    valPlayers.Text = elements[3] + "/" + elements[2];

                    TextBlock active = new TextBlock();
                    active.Width = 130;
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
                    wrap.Children.Add(active);
                    room.Child = wrap;
                    StackRoom.Children.Add(room);

                    StackPanel listPlayer = new StackPanel();
                    listPlayer.Name = elements[0];
                    listPlayer.Visibility = Visibility.Collapsed;

                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect(ipPoint);
                    message = "6 " + elements[0];
                    data = Encoding.Unicode.GetBytes(message);
                    socket.Send(data);
                    data = new byte[256];
                    builder = new StringBuilder();
                    bytes = 0;
                    do
                    {
                        bytes = socket.Receive(data, data.Length, 0);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (socket.Available > 0);
                    if (builder.ToString() != "")
                    {
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
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
                            if(elements[2] == "1")
                            {
                                Ellipse reads = new Ellipse();
                                reads.Width = 13;
                                reads.Height = 13;
                                reads.Margin = new Thickness(0, 0, 5, 0);
                                reads.Fill = Brushes.Lime;
                                wrap.Children.Add(reads);
                            }
                            room.Child = wrap;
                            listPlayer.Children.Add(room);
                        }
                    }
                    StackPlayer.Children.Add(listPlayer);
                }
            }
            else
            {
                ChatTextBlock.Text += "\r\n" + builder.ToString();
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }
    }
}