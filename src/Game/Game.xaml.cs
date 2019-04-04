using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using Nuclear.src.Interface;
using WpfAnimatedGif;

namespace Nuclear
{
    public partial class Game : Page
    {
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

        private const int port = 8888;
        private const string address = "127.0.0.1";
        private static TcpClient client = null;
        private static NetworkStream stream = null;

        private string message;
        private int openSend = 0;

        public object gridHeat { get; private set; }

        public Game()
        {
            InitializeComponent();
            //InitInventory();
            User = new PlayerUser(8, 2, 20, 12, 5);
            MapImageGrid();
            MapActiveGrid();
            MapHeatGrid();
            MapImgPlayerGrid();
            User.SetNickname("lorne");
            User.SetImageScreen(GROD, this);
            findPath(User.GetX(), User.GetY(), User.GetX(), User.GetY());
        }

        public Game(PlayerUser connect)
        {
            InitializeComponent();
            byte[] data;
            int x;
            int y;  
            User = connect;
            Random point = new Random();
            //stream = client.GetStream();
            while (true)
            {
                client = new TcpClient(address, port);
                stream = client.GetStream();
                x = point.Next(1, 14);
                y = point.Next(1, 24);
                message = "10 " + User.GetStateRoom() + " " + x.ToString() + " " + y.ToString() + " " + User.GetNickname();
                data = Encoding.Unicode.GetBytes(message);
                // отправка сообщения
                stream.Write(data, 0, data.Length);
                // получаем ответ
                data = new byte[64]; // буфер для получаемых данных
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
            User.SetMovePoints(12);
            User.SetAreaVisibility(5);
            User.SetHealth(20);

            client = new TcpClient();
            try
            {
                client.Connect(address, port); //подключение клиента
                stream = client.GetStream();
                message = "11 " + User.GetStateRoom() + " " + User.GetNickname() + " " + x.ToString() + " " + y.ToString();
                data = Encoding.Unicode.GetBytes(message);
                // отправка сообщения
                stream.Write(data, 0, data.Length);
                openSend = 1;
                // получаем ответ
                // запускаем новый поток для получения данных
                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start();
                Thread sendThread = new Thread(new ThreadStart(SendMessage));
                sendThread.Start();
            }
            catch (Exception ex)
            {
                ChatTextBlock.Text += ex.Message + "\r\n";
            }
            //InitInventory();
            MapImageGrid();
            MapActiveGrid();
            MapHeatGrid();
            MapImgPlayerGrid();

            User.SetImageScreen(GROD, this);
            findPath(User.GetX(), User.GetY(), User.GetX(), User.GetY());
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
                    byte[] data = new byte[64]; // буфер для получаемых данных
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
                        ChatTextBlock.Text += "Подключение прервано!";//вывод сообщения
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
            Environment.Exit(0); //завершение процесса
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
                                connectedUser.ChangeImage(GROD);
                                break;
                            }
                        }
                    });
                    break;
                case "1": // атакуют
                    break;
                case "2": // дисконектятся
                    break;
                case "11": // подключение
                    Dispatcher.Invoke(delegate
                    {
                        PlayerUser connectedUser = new PlayerUser();
                        connectedUser.SetXY(Convert.ToInt32(command[3]), Convert.ToInt32(command[4]));
                        connectedUser.SetMovePoints(12);
                        connectedUser.SetAreaVisibility(5);
                        connectedUser.SetHealth(20);
                        connectedUser.SetNickname(command[2]);
                        connectedUser.SetImageScreen(GROD, this);
                        Players.Add(connectedUser);
                        message = "12 " + User.GetX() + " " + User.GetY() + " " + User.GetNickname();
                        openSend = 1;
                    });
                    break;
                case "12": // подключение
                    Dispatcher.Invoke(delegate
                    {
                        PlayerUser connectedUser = new PlayerUser();
                        connectedUser.SetXY(Convert.ToInt32(command[1]), Convert.ToInt32(command[2]));
                        connectedUser.SetMovePoints(12);
                        connectedUser.SetAreaVisibility(5);
                        connectedUser.SetHealth(20);
                        connectedUser.SetNickname(command[3]);
                        connectedUser.SetImageScreen(GROD, this);
                        Players.Add(connectedUser);
                    });
                    break;
                default:
                    Dispatcher.Invoke(delegate
                    {
                        ChatTextBlock.Text += "\r\n" + message;//вывод сообщения
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
            x--;
            if (x < 0)
                x = 60;
            time.Text = x.ToString();
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

        public void MapImgPlayerGrid()
        {
            int HeightMap = 17;
            int WidthMap = 27;
            double sizeCellWidth = 36;
            double sizeCellHeight = 18;

            HexGrid hexGrid = new HexGrid();
            hexGrid.RowCount = HeightMap;
            hexGrid.ColumnCount = WidthMap;
            hexGrid.Orientation = Orientation.Vertical;
            hexGrid.Name = "ImgPlayerGrid";

            for (int i = 0; i < HeightMap; i++)
            {
                for (int j = 0; j < WidthMap; j++)
                {
                    HexItem sss = new HexItem();
                    sss.Background = null;
                    sss.Margin = new Thickness(-3, -1, 0, 0);
                    sss.BorderBrush = null;
                    sss.BorderThickness = new Thickness(0);
                    sss.Width = sizeCellWidth;
                    sss.Height = sizeCellHeight;
                    Grid.SetColumn(sss, j);
                    Grid.SetRow(sss, i);
                    hexGrid.Children.Add(sss);
                }
            }
            Grid.SetZIndex(hexGrid, 100);
            this.MapGrid.Children.Add(hexGrid);
        }


        public void MapImageGrid()
        {
            int HeightMap = 17;
            int WidthMap = 27;
            double sizeCellWidth = 36;
            double sizeCellHeight = 18;

            HexGrid hexGrid = new HexGrid();
            hexGrid.RowCount = HeightMap;
            hexGrid.ColumnCount = WidthMap;
            hexGrid.Orientation = Orientation.Vertical;
            hexGrid.Name = "ImageMap";

            for (int i = 0; i < HeightMap; i++)
            {
                for (int j = 0; j < WidthMap; j++)
                {
                    HexItem sss = new HexItem();
                    sss.Background = null;
                    sss.Margin = new Thickness(-3, -1, 0, 0);
                    sss.BorderBrush = null;
                    sss.BorderThickness = new Thickness(0);
                    sss.Width = sizeCellWidth;
                    sss.Height = sizeCellHeight;
                    Grid.SetColumn(sss, j);
                    Grid.SetRow(sss, i);
                    hexGrid.Children.Add(sss);
                }
            }
            Grid.SetZIndex(hexGrid, 1);
            this.MapGrid.Children.Add(hexGrid);
        }

        public void MapActiveGrid()
        {
            int HeightMap = 17;
            int WidthMap = 27;
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
                    if (field.GetPathArray(j, i) == -1) //PathArray[j, i] == -1
                    {
                        style.Setters.Add(new Setter { Property = Control.BackgroundProperty, Value = null });
                    }
                    else
                    {
                        DataTemplate dat = new DataTemplate();
                        dat.DataType = typeof(TextBlock);
                        FrameworkElementFactory factory = new FrameworkElementFactory(typeof(TextBlock));
                        factory.SetValue(TextBlock.WidthProperty, sizeCellWidth);
                        factory.SetValue(TextBlock.HeightProperty, sizeCellHeight);
                        factory.SetValue(TextBlock.OpacityProperty, 0.0);
                        factory.SetValue(TextBlock.TextProperty, (i.ToString() + " " + j.ToString()) as string);
                        factory.AddHandler(TextBlock.MouseDownEvent, new MouseButtonEventHandler(but_Click));
                        dat.VisualTree = factory;
                        style.Setters.Add(new Setter { Property = Control.BackgroundProperty, Value = null });
                        style.Setters.Add(new Setter { Property = ContentControl.ContentTemplateProperty, Value = dat });
                    }
                    HexItem sss = new HexItem();
                    sss.Style = style;
                    Grid.SetColumn(sss, j);
                    Grid.SetRow(sss, i);
                    hexGrid.Children.Add(sss);
                }
            }

            Grid.SetZIndex(hexGrid, 50);
            this.MapGrid.Children.Add(hexGrid);
        }

        public void MapHeatGrid()
        {
            int HeightMap = 17;
            int WidthMap = 27;
            double sizeCellWidth = 36;
            double sizeCellHeight = 18;

            HexGrid hexGrid = new HexGrid();
            hexGrid.RowCount = HeightMap;
            hexGrid.ColumnCount = WidthMap;
            hexGrid.Orientation = Orientation.Vertical;
            hexGrid.Name = "HeatMap";

            for (int i = 0; i < HeightMap; i++)
            {
                for (int j = 0; j < WidthMap; j++)
                {
                    HexItem sss = new HexItem();
                    sss.Background = null;
                    sss.Margin = new Thickness(-2.5, -1, 0, 0);
                    sss.BorderBrush = null;
                    sss.BorderThickness = new Thickness(0);
                    sss.Width = sizeCellWidth;
                    sss.Height = sizeCellHeight;
                    Grid.SetColumn(sss, j);
                    Grid.SetRow(sss, i);
                    hexGrid.Children.Add(sss);
                }
            }
            Grid.SetZIndex(hexGrid, 5);
            this.MapGrid.Children.Add(hexGrid);
        }

        private void but_Click(object sender, RoutedEventArgs e)
        {
            if (User.GetMovePoints() > 0)
            {
                TextBlock btn = sender as TextBlock;
                string[] buf = btn.Text.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                int row = Convert.ToInt32(buf[0]);
                int column = Convert.ToInt32(buf[1]);
                DistView.Text = ViewCalculation(User.GetX(), User.GetY(), Convert.ToInt32(buf[0]), Convert.ToInt32(buf[1])).ToString();
                MessageBox.Show(string.Format("Клетка {0}, {1}", column, row));
                Clean_TextBlock();
                Clean_HeatMap();
                locationEndX = row;
                locationEndY = column;
                findPath(User.GetX(), User.GetY(), row, column);
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
                }
        }

        private void Clean_HeatMap()
        {
            foreach (HexGrid gridHeat in MapGrid.Children)
                if (gridHeat.Name == "HeatMap")
                {
                    UIElementCollection children1 = gridHeat.Children;
                    var children = children1.OfType<UIElement>().ToList();
                    foreach (HexItem rec in children)
                        gridHeat.Children.Remove(rec);
                }
        }

        public async void findPath(int x, int y, int nx, int ny)
        {
            double sizeCellWidth = 36;
            double sizeCellHeight = 18;

            bool debagdoor = true;

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

            if (field.GetPathArray(y, x) == -1 || field.GetPathArray(ny, nx) == -1)
            {
                // вывод ошибки выбора - недоступная зона (стена)
                return;
            }
            while (true)
            {
                //волновой алгоритм поиска пути (заполнение значений достижимости) начиная от конца пути
                DopPathArray = (int[,])field.GetClonePathArray();
                DopClonePathArray = (int[,])DopPathArray.Clone();
                List<Point> DopOldWave = new List<Point>();
                DopOldWave.Add(new Point(x, y));
                int nstep = 0;
                DopPathArray[DoplocationUserY, DoplocationUserX] = nstep;

                int[] dx = { 0, 1, 0, 1, -1, -1 };
                int[] dy = { -1, 0, 1, -1, 0, -1 };
                int[] dx2 = { 0, 1, 0, -1, 1, -1 };
                int[] dy2 = { -1, 0, 1, 0, 1, 1 };

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


                            if (DopPathArray[DoplocationUserY, DoplocationUserX] == 0)
                            {
                                DopWavePath.Add(new Point(DoplocationUserX, DoplocationUserY));
                                DopPathArray[DoplocationUserY, DoplocationUserX] = nstep;

                                /* использовалось для квадратной сетки
                                  foreach (HexGrid gridImage in MapGrid.Children)
                                      if (gridImage.Name == "ImageMap")
                                      {
                                          HexItem text = new HexItem();
                                          text.Height = sizeCellHeight;
                                          text.Width = sizeCellWidth;
                                          text.Margin = new Thickness(-2.5, -1, 0, 0);
                                          text.BorderBrush = null;
                                          text.Background = null;
                                          text.BorderThickness = new Thickness(0);
                                          text.Content = nstep.ToString();
                                          Grid.SetColumn(text, DoplocationUserY);
                                          Grid.SetRow(text, DoplocationUserX);
                                          gridImage.Children.Add(text);
                                      }
                                      */
                                  foreach (HexGrid gridHeat in MapGrid.Children)
                                      if (gridHeat.Name == "HeatMap")
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
                                      }
                              }
                          }
                      }
                      DopOldWave = new List<Point>(DopWavePath);
                      if (nstep == User.GetAreaVisibility()) // поле зрения
                          break;
                  }
                  //DopWavePath.Clear();
                  //DopOldWave.Clear();

                  clonePathArray = (int[,])field.GetClonePathArray();
                  List<Point> oldWave = new List<Point>();
                  oldWave.Add(new Point(nx, ny));
                  nstep = 0;
                  field.SetPathArray(ny, nx, nstep);
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
                              if (field.GetPathArray(ny, nx) == 0)
                              {
                                  wave.Add(new Point(nx, ny));
                                  field.SetPathArray(ny, nx, nstep);
                              }
                          }
                      }
                      oldWave = new List<Point>(wave);
                  }

                  foreach (HexGrid gridHeat in MapGrid.Children)
                      if (gridHeat.Name == "HeatMap")
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
                      }

                  //волновой алгоритм поиска пути начиная от начала
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
                            /* использовалось для квадратной сетки
                             foreach (HexGrid gridImage in MapGrid.Children)
                                 if (gridImage.Name == "ImageMap")
                                 {
                                     HexItem text = new HexItem();
                                     text.Height = sizeCellHeight;
                                     text.Width = sizeCellWidth;
                                     text.Margin = new Thickness(-2.5, -1, 0, 0);
                                     text.BorderBrush = null;
                                     text.Background = Brushes.Yellow;
                                     text.BorderThickness = new Thickness(0);
                                     text.Content = nstep.ToString();
                                     Grid.SetColumn(text, locationEndYDUB);
                                     Grid.SetRow(text, locationEndXDUB);
                                     gridImage.Children.Add(text);
                                 }
                              */ // показывает путь
                            foreach (HexGrid gridHeat in MapGrid.Children)
                                if (gridHeat.Name == "HeatMap")
                                {
                                    HexItem myRect = new HexItem();
                                    myRect.Opacity = 0.5;
                                    if (User.GetMovePoints() > nstep && User.GetMovePoints() - 3 > nstep)
                                        myRect.Background = Brushes.Green;
                                    else if (User.GetMovePoints() >= nstep)
                                        myRect.Background = Brushes.Yellow;
                                    else if (User.GetMovePoints() < nstep)
                                        myRect.Background = Brushes.Red;
                                    Grid.SetColumn(myRect, locationEndYDUB);
                                    Grid.SetRow(myRect, locationEndXDUB);
                                    gridHeat.Children.Add(myRect);
                                }
                               // */ // показывает путь
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
                field.SetClonePathArray(clonePathArray);
                DopPathArray = DopClonePathArray;
                /*
                wave.ForEach(delegate (Point i)
                {
                    PathArray[i.y, i.x] = 1;
                });
                */
                waveOut();
                if (debagdoor)
                {
                    Dispatcher.Invoke(delegate
                    {
                        Dist.Text = wave.Count.ToString();
                    });
                    debagdoor = false;
                }

                if (User.GetMovePoints() > 0 && (User.GetX() != locationEndX || User.GetY() != locationEndY))
                {
                    await Task.Delay(500);
                    Clean_TextBlock();
                    Clean_HeatMap();
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

                            Dispatcher.Invoke(delegate
                            {
                                PositionX.Text = User.GetX().ToString();
                                PositionY.Text = User.GetY().ToString();
                            });

                            User.ChangeImage(GROD);
                            if(Players.Count != 0)
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
                {
                    break;
                }

                if (wavePath.Count == 0) {
                    break;
                }
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

            debagdoor = true;
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
        // обработчик нажатия кнопки loginButton
        private void loginButton_Click()
        {
            
        }
        // метод приема сообщений
        private void ReceiveMessages()
        {
           
        }
        // обработчик нажатия кнопки sendButton
        private void SendButton_Click(object sender, EventArgs e)
        {
           
        }
        // обработчик нажатия кнопки logoutButton
        private void logoutButton_Click(object sender, EventArgs e)
        {
           
        }
        // выход из чата
        private void ExitChat()
        {
            
        }

        private void SendButton_Key(object sender, KeyEventArgs e)
        {
            
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
                    (sender as TextBlock).Opacity = 0;
                    Mouse.Capture(null);
                    StartMenu menu = new StartMenu();
                    this.NavigationService.Navigate(menu);
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

        private void ChoosAction_RightClick(object sender, MouseButtonEventArgs e)
        {
            if (changeCursor)
            {
                StreamResourceInfo sris = Application.GetResourceStream(
                        new Uri("data/image/mainui/cursor/ACTARROW.cur", UriKind.Relative));
                Cursor customCursors = new Cursor(sris.Stream);
                Mouse.OverrideCursor = customCursors;
                changeCursor = false;
            }
            else
            {
                StreamResourceInfo sris = Application.GetResourceStream(
                        new Uri("data/image/mainui/cursor/ACTTAHIT.cur", UriKind.Relative));
                Cursor customCursors = new Cursor(sris.Stream);
                Mouse.OverrideCursor = customCursors;
                changeCursor = true;
            }
        }

        public void ActionWithPlayer_Click(object sender, MouseButtonEventArgs e)
        {
            if (changeCursor)
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
                                        PlayerFinded.SetHealth(PlayerFinded.GetHealth() - User.GetDamage(1, 1, 1));
                                        if(PlayerFinded.GetHealth() <= 0)
                                        {
                                            // смерть игрока
                                        }
                                        else
                                        {
                                            //ранение
                                        }
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
                    if ((sender as Image).Name == PlayerImage.Name)
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
                    if ((sender as Image).Name == PlayerImage.Name)
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
                    if ((sender as Image).Name == PlayerImage.Name)
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
            while (true)
            {
                if (sx != nx)
                    sx = (sx > nx) ? --sx : ++sx;
                if (sy != ny)
                    sy = (sy > ny) ? --sy : ++sy;
                step++;
                if (sx == nx && sy == ny)
                    break;
            }

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
                    }
                }
                DopOldWave = new List<Point>(DopWavePath);
                //if (nstep == User.GetAreaVisibility()) // поле зрения
                    //  break;
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
                            RHandChance.Text = "95";
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

        public void ActionWithPlayer_Focusable(object sender, MouseEventArgs e)
        {
            (sender as Image).Focusable = true;
            (sender as Image).Focus();
        }
    }
}