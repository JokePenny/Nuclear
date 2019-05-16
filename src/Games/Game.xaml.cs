using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;
using HexGridControl;
using Nuclear.data.mapeditor;
using Nuclear.src;
using Nuclear.src.Interface;
using WpfAnimatedGif;

namespace Nuclear
{
    public partial class Game : Page
    {
        //768" d:DesignWidth="1368"
        private bool changeCursor = false;
        private Field field = new Field();
        private List<Point> wave = new List<Point>();
        private List<Point> wavePath = new List<Point>();
        private List<Point> DopWavePath = new List<Point>();
        private List<PlayerUser> Players = new List<PlayerUser>();
        private PlayerUser User = null;
        private Inventory inventory = new Inventory();
        private int locationUserX;
        private int locationUserY;
        private int locationEndX;
        private int locationEndY;

        bool debagdoor = true;

        private Thread receiveThread;
        private Thread sendThread;

        private int run = 0;
        private bool userMoving = false;

        private int port;
        private const string address = "84.201.150.2";
        private static TcpClient client = null;
        private static NetworkStream stream = null;

        private string message;
        private int openSend = 0;

        public object gridHeat { get; private set; }

        public Game()
        {
            InitializeComponent();
            run = 1;
            User = new PlayerUser(23, 17, 20, 200, 15, GROD, this);
            MapImageGrid();
            MapActiveGrid();
            User.SetNickname("lorne");
            HealthPlayer.Text = User.GetHealth().ToString();
            findPath(User.GetX(), User.GetY(), User.GetX() + 1, User.GetY() + 1);
        }

        public Game(PlayerUser connect)
        {
            InitializeComponent();
            port = 2888;
            byte[] data;
            int x, y;  
            User = connect;
            Random point = new Random();

            while (true)
            {
                client = new TcpClient(address, port);
                stream = client.GetStream();
                x = point.Next(1, 14);
                y = point.Next(1, 24);
                message = "10 " + User.GetStateRoom() + " " + x.ToString() + " " + y.ToString() + " " + User.GetNickname();
                data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
                data = new byte[64];
                StringBuilder builders = new StringBuilder();
                int bytess = 0;
                do
                {
                    bytess = stream.Read(data, 0, data.Length);
                    builders.Append(Encoding.Unicode.GetString(data, 0, bytess));
                }
                while (stream.DataAvailable);
                if (builders.ToString() != "0")
                    break;
            }

            User.SetXY(x, y);
            User.animationCharacter.SetImageXImageY(x, y);
            User.SetMovePoints(12);
            User.SetAreaVisibility(16);
            User.SetHealth(20);
            HealthPlayer.Text = User.GetHealth().ToString();
            client = new TcpClient();

            try
            {
                client.Connect(address, port);
                stream = client.GetStream();
                message = "11 " + User.GetStateRoom() + " " + User.GetNickname() + " " + x.ToString() + " " + y.ToString();
                data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
                openSend = 1;
                receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start();
                sendThread = new Thread(new ThreadStart(SendMessage));
                sendThread.Start();
            }
            catch (Exception ex)
            {
                ChatTextBlock.Text += ex.Message + "\r\n";
            }

            MapImageGrid();
            MapActiveGrid();
            User.SetImageScreen(GROD, this);
            findPath(User.GetX(), User.GetY(), User.GetX(), User.GetY());
            timerStart();
        }

        private void SendMessage()
        {
            while (true)
            {
                if(openSend == 1)
                {
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                    openSend = 0;
                }
            }
        }

        // получение сообщений
        private void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[64];
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    message = builder.ToString();
                    CommandDecryption(message.Split(' '));
                }
                catch
                {
                    Dispatcher.Invoke(delegate
                    {
                        ChatTextBlock.Text += "Подключение прервано!\r\n";//вывод сообщения
                    });
                    Disconnect();
                }
            }
        }

        private void Disconnect()
        {
            if (stream != null)
                stream.Close();//отключение потока
            if (client != null)
                client.Close();//отключение клиента
        }

        private void CommandDecryption(string[] command)
        {
            switch (command[0])
            {
                case "0": // ход игроков
                    Dispatcher.Invoke(delegate
                    {
                        foreach(PlayerUser connectedUser in Players)
                        {
                            if (connectedUser.GetNickname() == command[1])
                            {
                                connectedUser.SetXY(Convert.ToInt32(command[2]), Convert.ToInt32(command[3]));
                                connectedUser.animationCharacter.ChangeImage(GROD, Convert.ToInt32(command[2]));
                                break;
                            }
                        }
                    });
                    break;
                case "1": // атакуют
                    Dispatcher.Invoke(delegate
                    {
                        foreach (PlayerUser connectedUser in Players)// анимация атакуюшего
                        {
                            if (connectedUser.GetNickname() == command[2])
                            {
                                connectedUser.animationCharacter.SetAnimation(command[4], GROD);
                                break;
                            }
                        }
                        if(User.GetNickname() == command[1])
                        {
                            User.SetHealth(User.GetHealth() - Convert.ToInt32(command[3]));
                            if (User.GetHealth() <= 0)
                            {
                                User.animationCharacter.SetAnimation(User.TypeOfArmor, 1, 3);
                                User.animationCharacter.ChangeImage(GROD, User.GetX());
                                ChatTextBlock.Text += "Вас убил " + command[2] + "!\r\n";
                                message = "14 " + "d" + " " + User.GetNickname() + " " + User.animationCharacter.FullPathImage;
                                // смерть игрока
                            }
                            else
                            {
                                User.animationCharacter.SetAnimation(User.TypeOfArmor, 0, 14);
                                User.animationCharacter.ChangeImage(GROD, User.GetX());
                                User.animationCharacter.SetAnimation(User.TypeOfArmor, 0, 0);
                                ChatTextBlock.Text += "Вас ранил " + command[2] + "! Потеряно " + command[3] + " здоровья\r\n";
                                message = "14 " + User.GetNickname() + " " + User.animationCharacter.FullPathImage;
                                //ранение
                            }
                            openSend = 1;
                        }
                    });
                    break;
                case "2": // чат
                    Dispatcher.Invoke(delegate
                    {
                        foreach (PlayerUser connectedUser in Players)
                        {
                            if (connectedUser.GetNickname() == command[1])
                            {
                                ChatTextBlock.Text += command[1] + ": " + command[2] + "\r\n";
                                break;
                            }
                        }
                    });
                    break;
                case "11": // подключение
                    Dispatcher.Invoke(delegate
                    {
                        PlayerUser connectedUser = new PlayerUser();
                        connectedUser.SetXY(Convert.ToInt32(command[3]), Convert.ToInt32(command[4]));
                        connectedUser.animationCharacter.SetImageXImageY(Convert.ToInt32(command[3]), Convert.ToInt32(command[4]));
                        connectedUser.SetHealth(20);
                        connectedUser.SetNickname(command[2]);
                        connectedUser.SetImageScreen(GROD, this);
                        Players.Add(connectedUser);
                        ChatTextBlock.Text += "Присоединился " + command[2] + "\r\n";
                        message = "12 " + User.GetX() + " " + User.GetY() + " " + User.GetNickname();
                        openSend = 1;
                    });
                    break;
                case "12": // подключение
                    Dispatcher.Invoke(delegate
                    {
                        PlayerUser connectedUser = new PlayerUser();
                        connectedUser.SetXY(Convert.ToInt32(command[1]), Convert.ToInt32(command[2]));
                        connectedUser.animationCharacter.SetImageXImageY(Convert.ToInt32(command[3]), Convert.ToInt32(command[4]));
                        connectedUser.SetHealth(20);
                        connectedUser.SetNickname(command[3]);
                        connectedUser.SetImageScreen(GROD, this);
                        ChatTextBlock.Text += "Присоединился " + command[3] + "\r\n";
                        Players.Add(connectedUser);
                    });
                    break;
                case "14": // проигрывание анимации
                    Dispatcher.Invoke(delegate
                    {
                        foreach (PlayerUser connectedUser in Players)
                        {
                            if (connectedUser.GetNickname() == command[1])
                            {
                                connectedUser.animationCharacter.SetAnimation(command[2], GROD);
                                break;
                            }
                            else if(command[1] == "d" && connectedUser.GetNickname() == command[2])
                            {
                                connectedUser.animationCharacter.SetAnimation(command[3], GROD);
                                break;
                            }
                        }
                    });
                    break;
                case "t+": // запуск времени
                    if (command[1] == User.GetNickname())
                    {
                        Dispatcher.Invoke(delegate
                        {
                            time.Text = 60.ToString();
                            run = 1;
                            x = 60;
                            User.SetMovePoints(10);
                            Clean_TextBlock();
                            findPath(User.GetX(), User.GetY(), User.GetX(), User.GetY());
                            int i = 0;
                            foreach (Ellipse health in HealthPanel.Children)
                            {
                                if (health.Name == "Ar" + i && i < User.GetMovePoints())
                                {
                                    i++;
                                    health.Opacity = 0;
                                }
                            }
                            ChatTextBlock.Text += "Ваш ход!\r\n";
                        });
                    }
                    break;
                case "200": // отключение игрока
                    Dispatcher.Invoke(delegate
                    {
                        PlayerUser disconetUser = null;
                        foreach (PlayerUser connectedUser in Players)
                        {
                            if (connectedUser.GetNickname() == command[1])
                            {
                                disconetUser = connectedUser;
                                connectedUser.animationCharacter.Disconect(GROD);
                                ChatTextBlock.Text += disconetUser.GetNickname() + " отключился\r\n";
                                break;
                            }
                        }
                        Players.Remove(disconetUser);
                    });
                    break;
                default:
                    Dispatcher.Invoke(delegate
                    {
                        ChatTextBlock.Text += message + "\r\n";//вывод сообщения
                    });
                    break;
            }
        }

        private DispatcherTimer timer = null;
        private int x = 60;

        private void timerStart()
        {
            timer = new DispatcherTimer();  // если надо, то в скобках указываем приоритет, например DispatcherPriority.Render
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Start();
        }

        private void timerTick(object sender, EventArgs e)
        {
            if(run == 1)
            {
                x--;
                if (x <= 0)
                {
                    Dispatcher.Invoke(delegate
                    {
                        message = User.GetNickname() + " " + "t-";
                        run = 0;
                        openSend = 1;
                        ChatTextBlock.Text += "Время на ход закончилось!\r\n";
                    });
                }
                time.Text = x.ToString();
            }
        }

        private void InitInventory()
        {

        }

        private void Opacity_ClickDown(object sender, MouseButtonEventArgs e)
        {
            object tag;
            if (sender is TextBlock)
                tag = ((TextBlock)e.OriginalSource).Tag;
            else
                tag = ((Ellipse)e.OriginalSource).Tag;
            
            switch ((string)tag)
            {
                case "BonusPanel":
                    BonusPanel.Opacity = 1;
                    break;
                case "KarmaPanel":
                    KarmaPanel.Opacity = 1;
                    BonusPanel.Opacity = 0;
                    break;
                case "KilledPanel":
                    KilledPanel.Opacity = 1;
                    KarmaPanel.Opacity = 0;
                    BonusPanel.Opacity = 0;
                    break;
                case "Steal":
                    Steal.Opacity = 1;
                    break;
                case "BreakIn":
                    BreakIn.Opacity = 1;
                    break;
                case "Theft":
                    Theft.Opacity = 1;
                    break;
                case "Traps":
                    Traps.Opacity = 1;
                    break;
                case "Nurse":
                    Nurse.Opacity = 1;
                    break;
                case "Doctor":
                    Doctor.Opacity = 1;
                    break;
                case "Science":
                    Science.Opacity = 1;
                    break;
                case "Repairs":
                    Repairs.Opacity = 1;
                    break;
                case "Craft":
                    Done.Opacity = 1;
                    break;
                case "ArrowUp":
                    ArrowUp.Opacity = 1;
                    break;
                case "ArrowDown":
                    ArrowDown.Opacity = 1;
                    break;
                case "ArrowLeft":
                    ArrowLeft.Opacity = 1;
                    break;
                case "ArrowRight":
                    ArrowRight.Opacity = 1;
                    break;
                case "Fix":
                    Fix.Opacity = 1;
                    break;
                default:
                    if (sender is TextBlock)
                        (sender as TextBlock).Opacity = 1;
                    else
                        (sender as Ellipse).Opacity = 1;
                    break;
            }
            if ((string)tag != "BonusPanel" && (string)tag != "KilledPanel" && (string)tag != "KarmaPanel")
            {
                if (sender is TextBlock)
                    Mouse.Capture(sender as TextBlock);
                else
                    Mouse.Capture(sender as Ellipse);
            }
        }



        public void MapImageGrid()
        {
            int HeightMap = 142;
            int WidthMap = 74;
            HexGrid hexGrid = new HexGrid();
            hexGrid.RowCount = HeightMap;
            hexGrid.ColumnCount = WidthMap;
            hexGrid.Orientation = Orientation.Vertical;
            hexGrid.Name = "ImageMap";
            Grid.SetZIndex(hexGrid, 1);
            this.MapGrid.Children.Add(hexGrid);
            HexGrid mouseHexGrid = new HexGrid();
            mouseHexGrid.RowCount = HeightMap;
            mouseHexGrid.ColumnCount = WidthMap;
            mouseHexGrid.Orientation = Orientation.Vertical;
            mouseHexGrid.Name = "MouseMoveHex";
            Grid.SetZIndex(mouseHexGrid, 3);
            this.MapGrid.Children.Add(mouseHexGrid);
        }

        public void MapActiveGrid()
        {
            int HeightMap = 142;
            int WidthMap = 74;
            double sizeCellWidth = 36;
            double sizeCellHeight = 18;

            HexGrid hexGrid = new HexGrid();
            hexGrid.RowCount = HeightMap;
            hexGrid.ColumnCount = WidthMap;
            hexGrid.Orientation = Orientation.Vertical;
            hexGrid.Name = "PathMap";

            for (int i = 0; i < HeightMap; i++)
            {
                for (int j = 0; j < WidthMap; j++)
                {
                    Style style = new Style();
                    style.Setters.Add(new Setter { Property = Control.HeightProperty, Value = sizeCellHeight });
                    style.Setters.Add(new Setter { Property = Control.WidthProperty, Value = sizeCellWidth });
                    style.Setters.Add(new Setter { Property = Control.MarginProperty, Value = new Thickness(-2.5, -1, 0, 0) });
                    style.Setters.Add(new Setter { Property = Control.BorderThicknessProperty, Value = new Thickness(0, 0, 0, 0) });
                    style.Setters.Add(new Setter { Property = Control.BorderBrushProperty, Value = null });
                    /*
                    if (field.GetPathArray(i, j) == -1)
                    {
                        style.Setters.Add(new Setter { Property = Control.BackgroundProperty, Value = null });
                    }
                    else
                    {
                    */
                        DataTemplate dat = new DataTemplate();
                        dat.DataType = typeof(TextBlock);
                        FrameworkElementFactory factory = new FrameworkElementFactory(typeof(TextBlock));
                        factory.SetValue(TextBlock.WidthProperty, sizeCellWidth);
                        factory.SetValue(TextBlock.HeightProperty, sizeCellHeight);
                        factory.SetValue(TextBlock.OpacityProperty, 0.0);
                        factory.SetValue(TextBlock.TextProperty, (i.ToString() + " " + j.ToString()) as string);
                        factory.AddHandler(TextBlock.MouseLeftButtonDownEvent, new MouseButtonEventHandler(but_Click));
                        factory.AddHandler(TextBlock.MouseEnterEvent, new MouseEventHandler(but_Enter));
                        factory.AddHandler(TextBlock.MouseLeaveEvent, new MouseEventHandler(but_Leave));
                        factory.AddHandler(TextBlock.MouseRightButtonDownEvent, new MouseButtonEventHandler(ChoosAction_RightClick));
                        dat.VisualTree = factory;
                        style.Setters.Add(new Setter { Property = Control.BackgroundProperty, Value = null });
                        style.Setters.Add(new Setter { Property = ContentControl.ContentTemplateProperty, Value = dat });
                   // }
                    HexItem sss = new HexItem();
                    sss.Style = style;
                    Grid.SetColumn(sss, j);
                    Grid.SetRow(sss, i);
                    hexGrid.Children.Add(sss);
                }
            }

            Grid.SetZIndex(hexGrid, 2);
            this.MapGrid.Children.Add(hexGrid);
        }

        private void but_Enter(object sender, MouseEventArgs e)
        {
            if (!changeCursor)
            {
                TextBlock btn = sender as TextBlock;
                string[] buf = btn.Text.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                double sizeCellWidth = 36;
                double sizeCellHeight = 18;
                foreach (HexGrid MouseMoveHex in MapGrid.Children)
                    if (MouseMoveHex.Name == "MouseMoveHex")
                    {
                        HexItem myRect = new HexItem();
                        myRect.Opacity = 0.5;
                        myRect.Height = sizeCellHeight;
                        myRect.Width = sizeCellWidth;
                        myRect.Margin = new Thickness(-2.5, -1, 0, 0);
                        myRect.BorderBrush = null;
                        myRect.BorderThickness = new Thickness(0);
                        if(field.GetPathArray(Convert.ToInt32(buf[0]), Convert.ToInt32(buf[1])) != -1)
                            myRect.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + "/data/image/interface/mouse/msef0032.gif")));
                        else
                            myRect.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + "/data/image/interface/mouse/msef0033.gif")));
                        Grid.SetColumn(myRect, Convert.ToInt32(buf[1]));
                        Grid.SetRow(myRect, Convert.ToInt32(buf[0]));
                        MouseMoveHex.Children.Add(myRect);
                        break;
                    }
            }
        }

        private void but_Leave(object sender, MouseEventArgs e)
        {
            Clean_MoveMouseHex();
        }

        private void but_Click(object sender, MouseButtonEventArgs e)
        {
            if (User.GetMovePoints() > 0 && run == 1 && !changeCursor && !userMoving)
            {
                TextBlock btn = sender as TextBlock;
                string[] buf = btn.Text.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                int row = Convert.ToInt32(buf[0]);
                int column = Convert.ToInt32(buf[1]);
                if(field.GetPathArray(row, column) != -1)
                {
                    userMoving = true;
                    DistView.Text = ViewCalculation(User.GetX(), User.GetY(), row, column).ToString();
                    Clean_TextBlock();
                    locationEndX = row;
                    locationEndY = column;
                    findPath(User.GetX(), User.GetY(), row, column);
                }
            }
        }

        private void Clean_MoveMouseHex()
        {
            foreach (HexGrid grid in MapGrid.Children)
                if (grid.Name == "MouseMoveHex")
                {
                    UIElementCollection children1 = grid.Children;
                    var children = children1.OfType<UIElement>().ToList();
                    foreach (HexItem textblock in children)
                        grid.Children.Remove(textblock);
                    break;
                }
        }

        private void Clean_TextBlock()
        {
            foreach (HexGrid grid in MapGrid.Children)
                if (grid.Name == "ImageMap")
                {
                    UIElementCollection children1 = grid.Children;
                    var children = children1.OfType<UIElement>().ToList();
                    foreach (HexItem textblock in children)
                        grid.Children.Remove(textblock);
                    break;
                }
        }

        public async void findPath(int x, int y, int nx, int ny)
        {
            double sizeCellWidth = 36;
            double sizeCellHeight = 18;
            locationEndX = nx;
            locationEndY = ny;
            int locationEndXDUB = nx;
            int locationEndYDUB = ny;
            locationUserX = x;
            locationUserY = y;
            int DoplocationUserX = x;
            int DoplocationUserY = y;
            int[,] clonePathArray;
            int[,] DopPathArray;
            int[,] DopClonePathArray;

            if (field.GetPathArray(x, y) == -1 || field.GetPathArray(nx, ny) == -1)
            {
                // вывод ошибки выбора - недоступная зона (стена)
                return;
            }
            //волновой алгоритм поиска пути (заполнение значений достижимости) начиная от конца пути
            
            DopPathArray = (int[,])field.GetClonePathArray();
            DopClonePathArray = (int[,])DopPathArray.Clone();
            List<Point> DopOldWave = new List<Point>();
            DopOldWave.Add(new Point(x, y));
            int nstep = 0;
            DopPathArray[DoplocationUserX, DoplocationUserY] = nstep;

            int[] dx = { 0, 1, 0, 1, -1, -1};
            int[] dy = { -1, 0, 1, -1, 0, -1};
            int[] dx2 = { 0, 1, 0, -1, 1, -1};
            int[] dy2 = { -1, 0, 1, 0, 1, 1};
            // окраска пути
            
            while (DopOldWave.Count > 0)
            {
                nstep++;
                DopWavePath.Clear();
                foreach (Point i in DopOldWave)
                {
                    for (int d = 0; d < 6; d++)
                    {
                        if(i.x % 2 == 0)
                        {
                            DoplocationUserX = i.x + dx[d];
                            DoplocationUserY = i.y + dy[d];
                        }
                        else
                        {
                            DoplocationUserX = i.x + dx2[d];
                            DoplocationUserY = i.y + dy2[d];
                        }
                        if (DopPathArray[DoplocationUserX, DoplocationUserY] == 0)
                        {
                            DopWavePath.Add(new Point(DoplocationUserX, DoplocationUserY));
                            DopPathArray[DoplocationUserX, DoplocationUserY] = nstep;
                        }
                    }
                }
                DopOldWave = new List<Point>(DopWavePath);
            }

            DopWavePath.Clear();
            DopOldWave.Clear();
            clonePathArray = (int[,])field.GetClonePathArray();
            List<Point> oldWave = new List<Point>();
            oldWave.Add(new Point(nx, ny));
            nstep = 0;
            clonePathArray[nx, ny] = nstep;

            while (oldWave.Count > 0)
            {
                nstep++;
                wave.Clear();
                foreach (Point i in oldWave)
                {
                    for (int d = 0; d < 6; d++)
                    {
                        if (i.x % 2 == 0)
                        {
                            nx = i.x + dx[d];
                            ny = i.y + dy[d];
                        }
                        else
                        {
                            nx = i.x + dx2[d];
                            ny = i.y + dy2[d];
                        }
                        if (clonePathArray[nx, ny] == 0)
                        {
                            wave.Add(new Point(nx, ny));
                            clonePathArray[nx, ny] = nstep;
                        }
                    }
                }
                oldWave = new List<Point>(wave);
            }

            //волновой алгоритм поиска пути начиная от начала
            bool flag = true;
            wave.Clear();
            wave.Add(new Point(locationEndXDUB, locationEndYDUB));
            int stepX = 0;
            int stepY = 0;

            while (stepX != x && stepY != y)
            {
                flag = true;
                for (int d = 0; d < 6; d++)
                {
                    if (locationEndXDUB % 2 == 0)
                    {
                        stepX = locationEndXDUB + dx[d];
                        stepY = locationEndYDUB + dy[d];
                    }
                    else
                    {
                        stepX = locationEndXDUB + dx2[d];
                        stepY = locationEndYDUB + dy2[d];
                    }
                    if (stepX == x && stepY == y)
                        break;
                    if (DopPathArray[locationEndXDUB, locationEndYDUB] - 1 == DopPathArray[stepX, stepY])
                    {
                        locationEndXDUB = stepX;
                        locationEndYDUB = stepY;
                        wave.Add(new Point(locationEndXDUB, locationEndYDUB));
                        flag = false;
                        break;
                    }
                }
                if (stepX == x && stepY == y)
                    break;
                if (flag)
                {
                    // вывод ошибки, пути нет
                    break;
                }
            }

            wave.Reverse();
            DopPathArray = DopClonePathArray;
            
            while (true)
            {
                if (debagdoor)
                {
                    DopPathArray = (int[,])field.GetClonePathArray();
                    // окраска пути
                    DopWavePath.Clear();
                    DopOldWave.Clear();
                    DopOldWave.Add(new Point(x, y));
                    nstep = 0;
                    while (DopOldWave.Count > 0)
                    {
                        nstep++;
                        DopWavePath.Clear();
                        foreach (Point i in DopOldWave)
                        {
                            for (int d = 0; d < 6; d++)
                            {
                                if (i.x % 2 == 0)
                                {
                                    DoplocationUserX = i.x + dx[d];
                                    DoplocationUserY = i.y + dy[d];
                                }
                                else
                                {
                                    DoplocationUserX = i.x + dx2[d];
                                    DoplocationUserY = i.y + dy2[d];
                                }
                                if (DopPathArray[DoplocationUserX, DoplocationUserY] == 0)
                                {
                                    DopWavePath.Add(new Point(DoplocationUserX, DoplocationUserY));
                                    DopPathArray[DoplocationUserX, DoplocationUserY] = nstep;
                                    foreach (HexGrid gridHeat in MapGrid.Children)
                                        if (gridHeat.Name == "ImageMap")
                                        {
                                            HexItem myRect = new HexItem();
                                            myRect.Opacity = 0.5;
                                            myRect.Height = sizeCellHeight;
                                            myRect.Width = sizeCellWidth;
                                            myRect.Margin = new Thickness(-2.5, -1, 0, 0);
                                            myRect.BorderBrush = null;
                                            myRect.BorderThickness = new Thickness(0);
                                            if (User.GetMovePoints() > nstep && User.GetMovePoints() - 3 > nstep)
                                                myRect.Background = Brushes.Green;
                                            else if (User.GetMovePoints() >= nstep)
                                                myRect.Background = Brushes.Yellow;
                                            else if (User.GetMovePoints() < nstep)
                                                myRect.Background = Brushes.Red;
                                            Grid.SetColumn(myRect, DoplocationUserY);
                                            Grid.SetRow(myRect, DoplocationUserX);
                                            gridHeat.Children.Add(myRect);
                                            break;
                                        }
                                }
                            }
                        }
                        DopOldWave = new List<Point>(DopWavePath);
                        if (nstep == User.GetAreaVisibility()) // поле зрения
                            break;
                    }

                    foreach (Point i in wave)
                    {
                        foreach (HexGrid gridHeat in MapGrid.Children)
                            if (gridHeat.Name == "ImageMap")
                            {
                                HexItem myRect = new HexItem();
                                myRect.Opacity = 0.5;
                                myRect.Background = Brushes.Red;
                                Grid.SetColumn(myRect, i.y);
                                Grid.SetRow(myRect, i.x);
                                gridHeat.Children.Add(myRect);
                                break;
                            }
                    }

                    foreach (HexGrid gridHeat in MapGrid.Children)
                        if (gridHeat.Name == "ImageMap")
                        {
                            HexItem myRect = new HexItem();
                            myRect.Opacity = 0.5;
                            myRect.Height = sizeCellHeight;
                            myRect.Width = sizeCellWidth;
                            myRect.Margin = new Thickness(-2.5, -1, 0, 0);
                            myRect.BorderBrush = null;
                            myRect.BorderThickness = new Thickness(0);
                            myRect.Background = Brushes.Blue;
                            Grid.SetColumn(myRect, User.GetY());
                            Grid.SetRow(myRect, User.GetX());
                            gridHeat.Children.Add(myRect);
                            break;
                        }

                        Dist.Text = wave.Count.ToString();
                }

                if (User.GetMovePoints() > 0 && (User.GetX() != locationEndX || User.GetY() != locationEndY))
                {
                    await Task.Delay(472);
                    Clean_TextBlock();
                    List<Point> step = new List<Point>();
                    step.Add(new Point(User.GetX(), User.GetY()));

                    foreach (Point h in step)
                    {
                        Point c = wave.Find(i => i.x == User.GetX() && i.y == User.GetY());
                        wave.Remove(c);
                        if (wave.Count != 0)
                        {
                            c = wave.First<Point>();
                            User.SetXY(c.x, c.y);
                            User.animationCharacter.SetImageXImageY(c.x, c.y);
                            PositionX.Text = User.GetX().ToString();
                            PositionY.Text = User.GetY().ToString();

                            foreach (Ellipse health in HealthPanel.Children)
                            {
                                if (health.Name == "Ar" + User.GetMovePoints())
                                {
                                    health.Opacity = 0;
                                    break;
                                }
                            }
                            User.SetMovePoints(Convert.ToByte(User.GetMovePoints() - 1));

                            if (User.animationCharacter.SpeedMove)
                                User.animationCharacter.SetAnimation(wave.Count, User.TypeOfArmor, 0, 19);
                            else
                                User.animationCharacter.SetAnimation(wave.Count, User.TypeOfArmor, 0, 1);

                            User.animationCharacter.ChangeImage(GROD, User.GetX());

                            if (Players.Count != 0)
                            {
                                message = "0 " + User.GetNickname() + " " + User.GetX() + " " + User.GetY();
                                openSend = 1;
                            }

                            wavePath = wave;
                        }
                        else
                            break;
                    }
                }
                else
                    break;

                if (wavePath.Count == 0)
                    break;
                else
                {
                    nx = locationEndX;
                    ny = locationEndY;
                    x = User.GetX();
                    y = User.GetY();
                    DoplocationUserX = User.GetX();
                    DoplocationUserY = User.GetY();
                    locationEndXDUB = nx;
                    locationEndYDUB = ny;
                }
            }
            if (User.GetMovePoints() == 0)
            {
                User.SetMovePoints(0);
                MessageBox.Show(string.Format("Очки действия кончились"));
            }
            userMoving = false;
        }

        public struct Point
        {
            public Point(int x, int y)
                : this()
            {
                this.x = x;
                this.y = y;
            }
            public int x;
            public int y;
        }

        public void waveOut() // вывод координат пути
        {
            string str = "";
            wave.ForEach(delegate (Point i)
            {
               str += "x = " + i.x + ", y = " + i.y  + "\n";
            });
            MessageBox.Show(string.Format(str));
        }

        private void OpenGameField_Click(object sender, RoutedEventArgs e)
        {
            Camera.Visibility = Visibility.Visible;
        }
        /* игровое поле */

        /* чат */
        private void SendButton_Key(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                message = User.GetNickname() + ": " + MessageTextBox.Text;
                ChatTextBlock.Text += message + "\r\n";
                MessageTextBox.Text = "";
                openSend = 1;
            }
        }

        private void Collapsed_WindowUI_Click(object sender, MouseButtonEventArgs e)
        {
            object tag;
            if (sender is TextBlock)
                tag = ((TextBlock)e.OriginalSource).Tag;
            else
                tag = ((Ellipse)e.OriginalSource).Tag;
            switch ((string)tag)
            {
                case "MenuOpen":
                    (sender as TextBlock).Opacity = 0;
                    Mouse.Capture(null);
                    MenuGame.Visibility = Visibility.Visible;
                    break;
                case "InventoryOpen":
                    (sender as TextBlock).Opacity = 0;
                    Mouse.Capture(null);
                    Inventory.Visibility = Visibility.Visible;
                    break;
                case "SkillOpen":
                    (sender as Ellipse).Opacity = 0;
                    Mouse.Capture(null);
                    Skill.Visibility = Visibility.Visible;
                    break;
                case "MapWindowOpen":
                    (sender as TextBlock).Opacity = 0;
                    Mouse.Capture(null);
                    MapWindow.Visibility = Visibility.Visible;
                    break;
                case "CharacteristicOpen":
                    (sender as TextBlock).Opacity = 0;
                    Mouse.Capture(null);
                    Characteristic.Visibility = Visibility.Visible;
                    break;
                case "CraftOpen":
                    (sender as TextBlock).Opacity = 0;
                    Mouse.Capture(null);
                    Craft.Visibility = Visibility.Visible;
                    break;
                case "MenuGame":
                    (sender as TextBlock).Opacity = 0;
                    Mouse.Capture(null);
                    MenuGame.Visibility = Visibility.Collapsed;
                    break;
                case "Characteristic":
                    (sender as Ellipse).Opacity = 0;
                    Mouse.Capture(null);
                    Characteristic.Visibility = Visibility.Collapsed;
                    break;
                case "MapWindow":
                    (sender as Ellipse).Opacity = 0;
                    Mouse.Capture(null);
                    MapWindow.Visibility = Visibility.Collapsed;
                    break;
                case "Inventory":
                    (sender as TextBlock).Opacity = 0;
                    Mouse.Capture(null);
                    Inventory.Visibility = Visibility.Collapsed;
                    break;
                case "Craft":
                    Done.Opacity = 0;
                    Mouse.Capture(null);
                    Craft.Visibility = Visibility.Collapsed;
                    break;
                case "Skill":
                    (sender as Ellipse).Opacity = 0;
                    Mouse.Capture(null);
                    Skill.Visibility = Visibility.Collapsed;
                    break;
                case "Exit":
                    if(port == 2888)
                    {
                        Disconnect();
                        sendThread.Abort();
                        sendThread.Join(500);
                        receiveThread.Abort();
                        receiveThread.Join(500);
                        timer.Stop();
                        (sender as TextBlock).Opacity = 0;
                        Mouse.Capture(null);
                        NetworkRoom launcher = new NetworkRoom(User, 0);
                        this.NavigationService.Navigate(launcher);
                    }
                    else
                    {
                        (sender as TextBlock).Opacity = 0;
                        Mouse.Capture(null);
                        StartMenu menu = new StartMenu();
                        this.NavigationService.Navigate(menu);
                    }
                    break;
                case "Settings":
                    (sender as TextBlock).Opacity = 0;
                    break;
                case "Barter":
                    (sender as TextBlock).Opacity = 0;
                    break;
                case "Say":
                    (sender as TextBlock).Opacity = 0;
                    break;
                case "ArrowUp":
                    ArrowUp.Opacity = 0;
                    break;
                case "ArrowDown":
                    ArrowDown.Opacity = 0;
                    break;
                case "ArrowRight":
                    ArrowRight.Opacity = 0;
                    break;
                case "ArrowLeft":
                    ArrowLeft.Opacity = 0;
                    break;
                case "Fix":
                    Fix.Opacity = 0;
                    break;
                case "Steal":
                    Steal.Opacity = 0;
                    break;
                case "BreakIn":
                    BreakIn.Opacity = 0;
                    break;
                case "Theft":
                    Theft.Opacity = 0;
                    break;
                case "Traps":
                    Traps.Opacity = 0;
                    break;
                case "Nurse":
                    Nurse.Opacity = 0;
                    break;
                case "Doctor":
                    Doctor.Opacity = 0;
                    break;
                case "Science":
                    Science.Opacity = 0;
                    break;
                case "Repairs":
                    Repairs.Opacity = 0;
                    break;
                default:
                    if(sender is TextBlock)
                        (sender as TextBlock).Opacity = 0;
                    else
                        (sender as Ellipse).Opacity = 0;
                    Mouse.Capture(null);
                    break;
            }
            Mouse.Capture(null);

        }

        private void FirstWeaponUp_Click(object sender, DragEventArgs e)
        {
            if (Clipboard.GetImage() != null)
            { 
                (sender as Image).Source = inventory.SetItemFirstWeapon().Source;
            }
        }

        private void FirstWeaponDown_Click(object sender, DragEventArgs e)
        {
            if((sender as Image) != null)
            {
                Clipboard.SetImage(new BitmapImage(((sender as Image).Source as BitmapImage).UriSource));
            }
        }

        private void SecondWeaponUp_Click(object sender, DragEventArgs e)
        {
            if ((sender as Image).Source != null)
            {
                //(sender as Image).Source = inventory.SetItemFirstWeapon().Source;
                (sender as Image).Source = new BitmapImage((e.Data.GetData(DataFormats.Bitmap) as BitmapImage).UriSource);
            }
        }

        private void SecondWeaponDown_Click(object sender, DragEventArgs e)
        {
            if ((sender as Image).Source != null)
            {
                Clipboard.SetImage(new BitmapImage(((sender as Image).Source as BitmapImage).UriSource));
                DragDrop.DoDragDrop((sender as Image), (sender as Image).Source, DragDropEffects.Copy);
            }
        }

        public void ChoosAction_RightClick(object sender, MouseButtonEventArgs e)
        {
            if (changeCursor)
            {
                StreamResourceInfo sris = Application.GetResourceStream(
                        new Uri("data/image/mainui/cursor/ACTARROW.cur", UriKind.Relative));
                Cursor customCursors = new Cursor(sris.Stream);
                Mouse.OverrideCursor = customCursors;
                if(client != null)
                    foreach(PlayerUser player in Players)
                    {
                        if(player != User)
                            player.animationCharacter.SetHitTestVisible();
                    }
                changeCursor = false;
            }
            else
            {
                StreamResourceInfo sris = Application.GetResourceStream(
                        new Uri("data/image/mainui/cursor/ACTTAHIT.cur", UriKind.Relative));
                Cursor customCursors = new Cursor(sris.Stream);
                Mouse.OverrideCursor = customCursors;
                if (client != null)
                    foreach (PlayerUser player in Players)
                    {
                        if (player != User)
                            player.animationCharacter.SetHitTestVisible();
                    }
                changeCursor = true;
                Clean_MoveMouseHex();
            }
        }

        public void ActionWithPlayer_Click(object sender, MouseButtonEventArgs e)
        {
            if (changeCursor && run == 1)
            {
                foreach (Image PlayerImage in GROD.Children)
                {
                    if ((sender as Image).Name == PlayerImage.Name && User.GetNickname() != (sender as Image).Name)
                    {
                        foreach (PlayerUser PlayerFinded in Players)
                        {
                            if (PlayerFinded.GetNickname() == (sender as Image).Name)
                            {
                                if (ViewCalculationToPlayer(User.GetX(), User.GetY(), PlayerFinded.GetX(), PlayerFinded.GetY()))
                                {
                                    Random probability = new Random();
                                    int chance_of_hit = User.GetProbabilityHitting(Convert.ToInt32(Dist.Text), 0, 0, 0);
                                    chance_of_hit = chance_of_hit > 95 ? 95 : chance_of_hit;
                                    chance_of_hit = chance_of_hit < 5 ? 5 : chance_of_hit;
                                    if (probability.Next(0, 100) <= chance_of_hit)
                                    {
                                        User.animationCharacter.SetAnimation(9, 0, 16);
                                        message = "1 " + PlayerFinded.GetNickname() + " " + User.GetNickname() + " " + User.GetDamage(1, 1, 1) + " " + User.animationCharacter.AttackAnimation(GROD);
                                        User.animationCharacter.SetAnimation(User.TypeOfArmor, 0, 0);
                                        openSend = 1;
                                    }
                                }
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        }

        public void ActionWithPlayer_Move(object sender, MouseEventArgs e)
        {
            if (changeCursor)
            {
                foreach (Image PlayerImage in GROD.Children)
                    if ((sender as Image).Name == PlayerImage.Name && (sender as Image).Name != User.GetNickname())
                    {
                        PanelAttack.Visibility = Visibility.Visible;
                        DropShadowEffect effect = new DropShadowEffect();
                        effect.ShadowDepth = 0;
                        effect.BlurRadius = 15;
                        effect.Color = Colors.Red;
                        PlayerImage.Effect = effect;
                        PlayerImage.KeyDown += ActionWithPlayer_KeyDown;
                        PlayerImage.KeyUp += ActionWithPlayer_KeyUp;
                        PlayerImage.Focus();
                        break;
                    }
            }
            else
            {
                foreach (Image PlayerImage in GROD.Children)
                    if ((sender as Image).Name == PlayerImage.Name && (sender as Image).Name != User.GetNickname())
                    {
                        PanelAttack.Visibility = Visibility.Hidden;
                        PlayerImage.Effect = null;
                        PlayerImage.KeyDown -= ActionWithPlayer_KeyDown;
                        PlayerImage.KeyUp -= ActionWithPlayer_KeyUp;
                        break;
                    }
            }
        }

        public void ActionWithPlayer_MouseLeave(object sender, MouseEventArgs e)
        {
            if (changeCursor)
                foreach (Image PlayerImage in GROD.Children)
                    if ((sender as Image).Name == PlayerImage.Name && (sender as Image).Name != User.GetNickname())
                    {
                        PanelAttack.Visibility = Visibility.Hidden;
                        PlayerImage.Effect = null;
                        PlayerImage.KeyDown -= ActionWithPlayer_KeyDown;
                        PlayerImage.KeyUp -= ActionWithPlayer_KeyUp;
                        break;
                    }
        }

        private int ViewCalculation(int sx, int sy, int nx, int ny)
        {
            int step = 0;
            int x = sx;
            int y = sy;
            while (true)
            {
                if (sx != nx)
                    sx = (sx > nx) ? --sx : ++sx;
                if (sy != ny)
                    sy = (sy > ny) ? --sy : ++sy;
                step++;
                if (sx == nx && sy == ny)
                    return step;
            }
        }

        private bool ViewCalculationToPlayer(int sx, int sy, int nx, int ny)
        {
            int step = 0;
            int x = sx;
            int y = sy;

            int locationEndXDUB = nx;
            int locationEndYDUB = ny;
            int DoplocationUserX = x;
            int DoplocationUserY = y;
            int[,] clonePathArray;
            int[,] DopPathArray;

            //волновой алгоритм поиска пути (заполнение значений достижимости) начиная от конца пути
            DopPathArray = (int[,])field.GetClonePathArray();
            List<Point> DopOldWave = new List<Point>();
            DopOldWave.Add(new Point(x, y));
            int nstep = 0;
            DopPathArray[DoplocationUserY, DoplocationUserX] = nstep;

            int[] dx = { 0, 1, 0, 1, -1, -1 };
            int[] dy = { -1, 0, 1, -1, 0, -1 };
            int[] dx2 = { 0, 1, 0, -1, 1, -1 };
            int[] dy2 = { -1, 0, 1, 0, 1, 1 };

            while (DopOldWave.Count > 0)
            {
                nstep++;
                DopWavePath.Clear();
                foreach (Point i in DopOldWave)
                {
                    for (int d = 0; d < 6; d++)
                    {
                        if (i.x % 2 == 0)
                        {
                            DoplocationUserX = i.x + dx[d];
                            DoplocationUserY = i.y + dy[d];
                        }
                        else
                        {
                            DoplocationUserX = i.x + dx2[d];
                            DoplocationUserY = i.y + dy2[d];
                        }

                        if (DopPathArray[DoplocationUserY, DoplocationUserX] == 0)
                        {
                            DopWavePath.Add(new Point(DoplocationUserX, DoplocationUserY));
                            DopPathArray[DoplocationUserY, DoplocationUserX] = nstep;
                        }

                        if (DoplocationUserY == ny && DoplocationUserX == nx)
                        {
                            step = nstep;
                            break;
                        }
                    }
                    if (step == nstep) break;
                }
                if (step == nstep) break;
                DopOldWave = new List<Point>(DopWavePath);
            }

            clonePathArray = (int[,])field.GetClonePathArray();
            List<Point> oldWave = new List<Point>();
            oldWave.Add(new Point(nx, ny));
            nstep = 0;
            clonePathArray[ny, nx] = nstep;
            while (oldWave.Count > 0)
            {
                nstep++;
                wave.Clear();
                foreach (Point i in oldWave)
                {
                    for (int d = 0; d < 6; d++)
                    {
                        if (i.x % 2 == 0)
                        {
                            nx = i.x + dx[d];
                            ny = i.y + dy[d];
                        }
                        else
                        {
                            nx = i.x + dx2[d];
                            ny = i.y + dy2[d];
                        }
                        if (clonePathArray[ny, nx] == 0)
                        {
                            wave.Add(new Point(nx, ny));
                            clonePathArray[ny, nx] = nstep;
                        }
                    }
                }
                oldWave = new List<Point>(wave);
            }
            bool flag = true;
            wave.Clear();
            wave.Add(new Point(locationEndXDUB, locationEndYDUB));
            int stepX = 0;
            int stepY = 0;
            while (field.GetPathArray(y, x) != 99)
            {
                flag = true;
                for (int d = 0; d < 6; d++)
                {
                    if (locationEndXDUB % 2 == 0)
                    {
                        stepX = locationEndXDUB + dx[d];
                        stepY = locationEndYDUB + dy[d];
                    }
                    else
                    {
                        stepX = locationEndXDUB + dx2[d];
                        stepY = locationEndYDUB + dy2[d];
                    }
                    if (stepX == x && stepY == y)
                        break;
                    if (DopPathArray[locationEndYDUB, locationEndXDUB] - 1 == DopPathArray[stepY, stepX])
                    {
                        locationEndXDUB = stepX;
                        locationEndYDUB = stepY;
                        wave.Add(new Point(locationEndXDUB, locationEndYDUB));
                        flag = false;
                        break;
                    }
                }
                if (stepX == x && stepY == y)
                    break;
                if (flag)
                {
                    // вывод ошибки, пути нет
                    break;
                }
            }
            Dist.Text = wave.Count.ToString();
            if (wave.Count == step)
                return true;
            return false;
        }

        public void ActionWithPlayer_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.NumPad8:
                    if (changeCursor)
                    {
                        if (User.GetProbabilityHitting(ViewCalculation(User.GetX(), User.GetY(), User.GetX(), User.GetY()), 1, 1, 1) > 95)
                            HeadChance.Text = "95";
                        else HeadChance.Text = User.GetProbabilityHitting(ViewCalculation(User.GetX(), User.GetY(), User.GetX(), User.GetY()), 1, 1, 1).ToString();
                        head.Visibility = Visibility.Visible;
                    }
                    break;
                case Key.NumPad4:
                    if (changeCursor)
                    {
                        if (User.GetProbabilityHitting(ViewCalculation(User.GetX(), User.GetY(), User.GetX(), User.GetY()), 1, 1, 1) > 95)
                            LHandOrBodyChance.Text = "95";
                        else LHandOrBodyChance.Text = User.GetProbabilityHitting(ViewCalculation(User.GetX(), User.GetY(), User.GetX(), User.GetY()), 1, 1, 1).ToString();
                        l_hand.Visibility = Visibility.Visible;
                    }
                    break;
                case Key.NumPad5:
                    if (changeCursor)
                    {
                        if (User.GetProbabilityHitting(ViewCalculation(User.GetX(), User.GetY(), User.GetX(), User.GetY()), 1, 1, 1) > 95)
                            LHandOrBodyChance.Text = "95";
                        else LHandOrBodyChance.Text = User.GetProbabilityHitting(ViewCalculation(User.GetX(), User.GetY(), User.GetX(), User.GetY()), 1, 1, 1).ToString();
                        body.Visibility = Visibility.Visible;
                    }
                    break;
                case Key.NumPad6:
                    if (changeCursor)
                    {
                        if (User.GetProbabilityHitting(ViewCalculation(User.GetX(), User.GetY(), User.GetX(), User.GetY()), 1, 1, 1) > 95)
                            RHandChance.Text = "95";
                        else RHandChance.Text = User.GetProbabilityHitting(ViewCalculation(User.GetX(), User.GetY(), User.GetX(), User.GetY()), 1, 1, 1).ToString();
                        r_hand.Visibility = Visibility.Visible;
                    }
                    break;
                case Key.NumPad1:
                    if (changeCursor)
                    {
                        if (User.GetProbabilityHitting(ViewCalculation(User.GetX(), User.GetY(), User.GetX(), User.GetY()), 1, 1, 1) > 95)
                            LFootChance.Text = "95";
                        else RFootChance.Text = User.GetProbabilityHitting(ViewCalculation(User.GetX(), User.GetY(), User.GetX(), User.GetY()), 1, 1, 1).ToString();
                        l_foot.Visibility = Visibility.Visible;
                    }
                    break;
                case Key.NumPad3:
                    if (changeCursor)
                    {
                        if(User.GetProbabilityHitting(ViewCalculation(User.GetX(), User.GetY(), User.GetX(), User.GetY()), 1, 1, 1) > 95)
                            RFootChance.Text = "95";
                        else RFootChance.Text = User.GetProbabilityHitting(ViewCalculation(User.GetX(), User.GetY(), User.GetX(), User.GetY()), 1, 1, 1).ToString();
                        r_foot.Visibility = Visibility.Visible;
                    }
                    break;
            }
        }

        public void ActionWithPlayer_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.NumPad8:
                    if (changeCursor)
                    {
                        HeadChance.Text = "0";
                        head.Visibility = Visibility.Hidden;
                    }
                    break;
                case Key.NumPad4:
                    if (changeCursor)
                    {
                        LHandOrBodyChance.Text = "0";
                        l_hand.Visibility = Visibility.Hidden;
                    }
                    break;
                case Key.NumPad5:
                    if (changeCursor)
                    {
                        LHandOrBodyChance.Text = "0";
                        body.Visibility = Visibility.Hidden;
                    }
                    break;
                case Key.NumPad6:
                    if (changeCursor)
                    {
                        RHandChance.Text = "0";
                        r_hand.Visibility = Visibility.Hidden;
                    }
                    break;
                case Key.NumPad1:
                    if (changeCursor)
                    {
                        LFootChance.Text = "0";
                        l_foot.Visibility = Visibility.Hidden;
                    }
                    break;
                case Key.NumPad3:
                    if (changeCursor)
                    {
                        RFootChance.Text = "0";
                        r_foot.Visibility = Visibility.Hidden;
                    }
                    break;
            }
        }

        public void Hotkey_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Tab: // list player
                    if (TabPlayer.IsVisible)
                        TabPlayer.Visibility = Visibility.Collapsed;
                    else TabPlayer.Visibility = Visibility.Visible;
                    break;
                case Key.Escape: // menu or close windows
                    if (changeCursor)
                    {
                        LHandOrBodyChance.Text = "0";
                        l_hand.Visibility = Visibility.Hidden;
                    }
                    break;
                case Key.T: // chat
                    if(MessageTextBox.Visibility == Visibility.Visible)
                        MessageTextBox.Focus();
                    break;
                case Key.L: // charactir
                    if (Characteristic.IsVisible)
                        Characteristic.Visibility = Visibility.Collapsed;
                    else Characteristic.Visibility = Visibility.Visible;
                    break;
                case Key.S: // skill
                    if (Skill.IsVisible)
                        Skill.Visibility = Visibility.Collapsed;
                    else Skill.Visibility = Visibility.Visible;
                    break;
                case Key.I: // inventory
                    if (Inventory.IsVisible)
                        Inventory.Visibility = Visibility.Collapsed;
                    else Inventory.Visibility = Visibility.Visible;
                    break;
                case Key.C: // craft
                    if (Craft.IsVisible)
                        Craft.Visibility = Visibility.Collapsed;
                    else Craft.Visibility = Visibility.Visible;
                    break;
                case Key.M: // map
                    if (MapWindow.IsVisible)
                        MapWindow.Visibility = Visibility.Collapsed;
                    else MapWindow.Visibility = Visibility.Visible;
                    break;
                case Key.P: // pip-boy
                    if (DebugPanel.IsVisible)
                        DebugPanel.Visibility = Visibility.Collapsed;
                    else DebugPanel.Visibility = Visibility.Visible;
                    break;
                case Key.D: // debug pnale
                    if (DebugPanel.IsVisible)
                    {
                        DebugPanel.Visibility = Visibility.Collapsed;
                        debagdoor = false;
                    }
                    else
                    {
                        DebugPanel.Visibility = Visibility.Visible;
                        debagdoor = true;
                    }
                    break;
                case Key.R: // смена бега/ходьбы
                    if (User.animationCharacter.SpeedMove)
                        User.animationCharacter.SpeedMove = false;
                    else User.animationCharacter.SpeedMove = true;
                    break;
            }
        }

        public void ActionWithPlayer_Focusable(object sender, MouseEventArgs e)
        {
            (sender as Image).Focusable = true;
            (sender as Image).Focus();
        }


        private void MoveMouse_Scroll(object sender, MouseEventArgs e)
        {
            object tag = ((Border)e.OriginalSource).Tag;
            switch (tag.ToString())
            {
                case "NORTH":
                    Camera.LineUp();
                    break;
                case "SOUTH":
                    Camera.LineDown();
                    break;
                case "EAST":
                    Camera.LineRight();
                    break;
                case "WEST":
                    Camera.LineLeft();
                    break;
            }
            StreamResourceInfo sri = Application.GetResourceStream(
                           new Uri("data/image/interface/mouse/SCR" + tag.ToString() + ".cur", UriKind.Relative));
            Cursor customCursor = new Cursor(sri.Stream);
            Mouse.OverrideCursor = customCursor;
        }

        public void LeaveMouse_Scroll(object sender, MouseEventArgs e)
        {
            StreamResourceInfo sri = Application.GetResourceStream(
                           new Uri("data/image/mainui/cursor/ACTARROW.cur", UriKind.Relative));
            Cursor customCursor = new Cursor(sri.Stream);
            Mouse.OverrideCursor = customCursor;
        }
    }
}