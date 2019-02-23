using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace Nuclear
{
    /// <summary>
    /// Логика взаимодействия для Game.xaml
    /// </summary>
    public partial class Game : Page
    {
        /* игровое поле */
        //12hx27w size
        private static int[,] PathArray = new int[,] {
            {-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
            {-1, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, -1, 0, -1, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, -1, 0, -1, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, -1, 0, -1, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, 0, 0, -1, -1, 0, 0, 0, -1},
            {-1, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, -1, 0, -1, -1, 0, 0, 0, -1},
            {-1, 0, 0, 0, -1, 0, 0, -1, 0, 0, 0, -1},
            {-1, 0, 0, 0, 0, 0, -1, -1, 0, 0, 0, -1},
            {-1, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
            };

        private static int[,] ImageIDArray = new int[,] {
            {-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
            {-1, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, -1, 0, -1, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, -1, 0, -1, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, -1, 0, -1, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, 0, 0, -1, -1, 0, 0, 0, -1},
            {-1, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, -1, 0, -1, -1, 0, 0, 0, -1},
            {-1, 0, 0, 0, -1, 0, 0, -1, 0, 0, 0, -1},
            {-1, 0, 0, 0, 0, 0, -1, -1, 0, 0, 0, -1},
            {-1, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1},
            {-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
            };

        private List<Point> wave = new List<Point>();
        private List<Point> wavePath = new List<Point>();
        private List<Point> DopWavePath = new List<Point>();
        private PlayerUser User = new PlayerUser(1, 1, 20, 12);
        private int locationUserX;
        private int locationUserY;
        private int locationEndX;
        private int locationEndY;


        string userName = "Lorne"; // имя пользователя в чате


        bool alive = false; // будет ли работать поток для приема
        UdpClient client;
        const int LOCALPORT = 8001; // порт для приема сообщений
        const int REMOTEPORT = 8001; // порт для отправки сообщений
        const int TTL = 20;
        const string HOST = "235.5.5.1"; // хост для групповой рассылки
        IPAddress groupAddress; // адрес для групповой рассылки

        //private PlayerUser User = new PlayerUser();
        public Game()
        {

            InitializeComponent();
            groupAddress = IPAddress.Parse(HOST);
            loginButton_Click();
            MapImageGrid();
            MapActiveGrid();
            MapHeatGrid();
            findPath(1, 1, User.GetX(), User.GetY());

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
            int HeightMap = 12;
            int WidthMap = 27;

            Grid grid = new Grid();
            grid.Name = "ImageMap";
            grid.ShowGridLines = false;
            for (int i = 0; i < HeightMap; i++)
            {
                RowDefinition row = new RowDefinition();
                row.MinHeight = 50;
                grid.RowDefinitions.Add(row);
            }
            for (int i = 0; i < WidthMap; i++)
            {
                ColumnDefinition column = new ColumnDefinition();
                column.MinWidth = 50;
                grid.ColumnDefinitions.Add(column);
            }
            grid.Background = Brushes.White;
            Grid.SetZIndex(grid, 1);
            this.Map.Children.Add(grid);
        }

        public void MapActiveGrid()
        {
            int HeightMap = 12;
            int WidthMap = 27;

            Grid gridActive = new Grid();
            gridActive.Name = "PathMap";
            gridActive.ShowGridLines = false;
            for (int i = 0; i < HeightMap; i++)
            {
                RowDefinition row = new RowDefinition();
                row.MinHeight = 50;
                gridActive.RowDefinitions.Add(row);
            }
            for (int i = 0; i < WidthMap; i++)
            {
                ColumnDefinition column = new ColumnDefinition();
                column.MinWidth = 50;
                gridActive.ColumnDefinitions.Add(column);
            }

            for (int i = 0; i < WidthMap; i++)
                for (int j = 0; j < HeightMap; j++)
                {
                    if (PathArray[i, j] == -1)
                    {
                        Rectangle myRect = new Rectangle();
                        myRect.Fill = Brushes.Black;
                        Grid.SetColumn(myRect, i);
                        Grid.SetRow(myRect, j);
                        gridActive.Children.Add(myRect);
                    }
                    else
                    {
                        Button but = new Button();
                        but.Click += but_Click;
                        but.Opacity = 0.0;
                        Grid.SetColumn(but, i);
                        Grid.SetRow(but, j);
                        gridActive.Children.Add(but);
                    }
                }
            Grid.SetZIndex(gridActive, 999);
            this.Map.Children.Add(gridActive);
        }

        public void MapHeatGrid()
        {
            int HeightMap = 12;
            int WidthMap = 27;

            Grid gridHeat = new Grid();
            gridHeat.Name = "HeatMap";
            gridHeat.ShowGridLines = false;
            for (int i = 0; i < HeightMap; i++)
            {
                RowDefinition row = new RowDefinition();
                row.MinHeight = 50;
                gridHeat.RowDefinitions.Add(row);
            }
            for (int i = 0; i < WidthMap; i++)
            {
                ColumnDefinition column = new ColumnDefinition();
                column.MinWidth = 50;
                gridHeat.ColumnDefinitions.Add(column);
            }
            Grid.SetZIndex(gridHeat, 5);
            this.Map.Children.Add(gridHeat);
        }

        private void but_Click(object sender, RoutedEventArgs e)
        {
            if (User.GetMovePoints() > 0)
            {
                Button btn = sender as Button;
                int row = (int)btn.GetValue(Grid.RowProperty);
                int column = (int)btn.GetValue(Grid.ColumnProperty);
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
            foreach (Grid grid in Map.Children)
                if (grid.Name == "ImageMap")
                {
                    UIElementCollection children1 = grid.Children;
                    var children = children1.OfType<UIElement>().ToList();
                    foreach (TextBlock textblock in children)
                        grid.Children.Remove(textblock);
                }
        }

        private void Clean_HeatMap()
        {
            foreach (Grid gridHeat in Map.Children)
                if (gridHeat.Name == "HeatMap")
                {
                    UIElementCollection children1 = gridHeat.Children;
                    var children = children1.OfType<UIElement>().ToList();
                    foreach (Rectangle rec in children)
                        gridHeat.Children.Remove(rec);
                }
        }

        public async void findPath(int x, int y, int nx, int ny)
        {
            locationEndX = nx;
            locationEndY = ny;
            locationUserX = x;
            locationUserY = y;
            int DoplocationUserX = x;
            int DoplocationUserY = y;
            int[,] clonePathArray;
            int[,] DopPathArray;
            int[,] DopClonePathArray;

            if (PathArray[y, x] == -1 || PathArray[ny, nx] == -1)
            {
                // вывод ошибки выбора - недоступная зона (стена)
                //return;
            }
            while (true)
            {
                //волновой алгоритм поиска пути (заполнение значений достижимости) начиная от конца пути
                clonePathArray = (int[,])PathArray.Clone();
                DopPathArray = (int[,])PathArray.Clone();
                DopClonePathArray = (int[,])DopPathArray.Clone();
                List<Point> oldWave = new List<Point>();
                List<Point> DopOldWave = new List<Point>();
                oldWave.Add(new Point(nx, ny));
                DopOldWave.Add(new Point(DoplocationUserX, DoplocationUserY));
                int nstep = 0;
                PathArray[ny, nx] = nstep;
                DopPathArray[DoplocationUserY, DoplocationUserX] = nstep;

                int[] dx = { 0, 1, 0, -1 };
                int[] dy = { -1, 0, 1, 0 };

                while (DopOldWave.Count > 0)
                {
                    nstep++;
                    DopWavePath.Clear();
                    foreach (Point i in DopOldWave)
                    {
                        for (int d = 0; d < 4; d++)
                        {
                            DoplocationUserX = i.x + dx[d];
                            DoplocationUserY = i.y + dy[d];

                            if (DopPathArray[DoplocationUserY, DoplocationUserX] == 0)
                            {
                                DopWavePath.Add(new Point(DoplocationUserX, DoplocationUserY));
                                DopPathArray[DoplocationUserY, DoplocationUserX] = nstep;

                                foreach (Grid gridImage in Map.Children)
                                    if (gridImage.Name == "ImageMap")
                                    {
                                        TextBlock text = new TextBlock();
                                        text.Text = nstep.ToString();
                                        Grid.SetColumn(text, DoplocationUserY);
                                        Grid.SetRow(text, DoplocationUserX);
                                        gridImage.Children.Add(text);
                                    }
                                foreach (Grid gridHeat in Map.Children)
                                    if (gridHeat.Name == "HeatMap")
                                    {
                                        Rectangle myRect = new Rectangle();
                                        myRect.Opacity = 0.5;
                                        if (User.GetMovePoints() > nstep && User.GetMovePoints() - 3 > nstep)
                                            myRect.Fill = Brushes.Green;
                                        else if (User.GetMovePoints() >= nstep)
                                            myRect.Fill = Brushes.Yellow;
                                        else if (User.GetMovePoints() < nstep)
                                            myRect.Fill = Brushes.Red;
                                        Grid.SetColumn(myRect, DoplocationUserY);
                                        Grid.SetRow(myRect, DoplocationUserX);
                                        gridHeat.Children.Add(myRect);
                                    }

                            }
                        }
                    }
                    DopOldWave = new List<Point>(DopWavePath);
                }

                DopWavePath.Clear();
                DopOldWave.Clear();
                nstep = 0;
                while (oldWave.Count > 0)
                {
                    nstep++;
                    wave.Clear();
                    foreach (Point i in oldWave)
                    {
                        for (int d = 0; d < 4; d++)
                        {
                            nx = i.x + dx[d];
                            ny = i.y + dy[d];
                            if (PathArray[ny, nx] == 0)
                            {
                                wave.Add(new Point(nx, ny));
                                PathArray[ny, nx] = nstep;
                            }
                        }
                    }
                    oldWave = new List<Point>(wave);
                }

                foreach (Grid gridHeat in Map.Children)
                    if (gridHeat.Name == "HeatMap")
                    {
                        Rectangle myRect = new Rectangle();
                        myRect.Opacity = 0.5;
                        myRect.Fill = Brushes.Blue;
                        Grid.SetColumn(myRect, User.GetY());
                        Grid.SetRow(myRect, User.GetX());
                        gridHeat.Children.Add(myRect);
                    }

                //волновой алгоритм поиска пути начиная от начала
                bool flag = true;
                wave.Clear();
                while (PathArray[y, x] != 1)
                {
                    flag = true;
                    for (int d = 0; d < 4; d++)
                    {
                        nx = x + dx[d];
                        ny = y + dy[d];
                        if (PathArray[y, x] - 1 == PathArray[ny, nx])
                        {
                            x = nx;
                            y = ny;
                            wave.Add(new Point(x, y));
                            flag = false;
                            /*
                            foreach (Grid gridImage in Map.Children)
                                if (gridImage.Name == "ImageMap")
                                {
                                    TextBlock text = new TextBlock();
                                    text.Text = nstep.ToString();
                                    Grid.SetColumn(text, y);
                                    Grid.SetRow(text, x);
                                    gridImage.Children.Add(text);
                                }
                            foreach (Grid gridHeat in Map.Children)
                                if (gridHeat.Name == "HeatMap")
                                {
                                    Rectangle myRect = new Rectangle();
                                    myRect.Opacity = 0.5;
                                    if (User.GetMovePoints() > nstep && User.GetMovePoints() - 3 > nstep)
                                        myRect.Fill = Brushes.Green;
                                    else if (User.GetMovePoints() >= nstep)
                                        myRect.Fill = Brushes.Yellow;
                                    else if (User.GetMovePoints() < nstep)
                                        myRect.Fill = Brushes.Red;
                                    Grid.SetColumn(myRect, y);
                                    Grid.SetRow(myRect, x);
                                    gridHeat.Children.Add(myRect);
                                }
                                */ // показывает путь
                            break;
                        }
                    }
                    if (flag)
                    {
                        // вывод ошибки, пути нет
                        break;
                    }
                }
                wave.Add(new Point(locationEndX, locationEndY));
                PathArray = clonePathArray;
                DopPathArray = DopClonePathArray;
                /*
                wave.ForEach(delegate (Point i)
                {
                    PathArray[i.y, i.x] = 1;
                });
                */
                if (User.GetMovePoints() > 0 && (User.GetX() != locationEndX || User.GetY() != locationEndY))
                {
                    User.SetMovePoints(Convert.ToByte(User.GetMovePoints() - 1));
                    await Task.Delay(1000);
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
                            foreach (Grid gridHeat in Map.Children)
                                if (gridHeat.Name == "HeatMap")
                                {
                                    Rectangle myRect = new Rectangle();
                                    myRect.Opacity = 0.5;
                                    myRect.Fill = Brushes.Blue;
                                    Grid.SetColumn(myRect, User.GetY());
                                    Grid.SetRow(myRect, User.GetX());
                                    gridHeat.Children.Add(myRect);
                                }
                            wavePath = wave;
                        }
                        else break;
                    }
                }
                else break;
                if (wavePath.Count == 0) break;
                else
                {
                    nx = locationEndX;
                    ny = locationEndY;
                    x = User.GetX();
                    y = User.GetY();
                    DoplocationUserX = User.GetX();
                    DoplocationUserY = User.GetY();
                }
            }
            if (User.GetMovePoints() == 0)
            {
                User.SetMovePoints(0);
                MessageBox.Show(string.Format("Очки действия кончились"));
            }
            else  MessageBox.Show(string.Format("Ход сделан"));
        }

        struct Point
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
            wave.ForEach(delegate (Point i)
            {
                Console.WriteLine("x = " + i.x + ", y = " + i.y);
            });
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
            try
            {
                client = new UdpClient(LOCALPORT);
                // присоединяемся к групповой рассылке
                client.JoinMulticastGroup(groupAddress, TTL);

                // запускаем задачу на прием сообщений
                Task receiveTask = new Task(ReceiveMessages);
                receiveTask.Start();

                // отправляем первое сообщение о входе нового пользователя
                string message = userName + " вошел в чат";
                byte[] data = Encoding.Unicode.GetBytes(message);
                client.Send(data, data.Length, HOST, REMOTEPORT);

                //SendButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // метод приема сообщений
        private void ReceiveMessages()
        {
            alive = true;
            try
            {
                while (alive)
                {
                    IPEndPoint remoteIp = null;
                    byte[] data = client.Receive(ref remoteIp);
                    string message = Encoding.Unicode.GetString(data);

                    // добавляем полученное сообщение в текстовое поле
                    Dispatcher.Invoke(delegate
                    {
                        string time = DateTime.Now.ToShortTimeString();
                        ChatTextBlock.Text = time + " " + message + "\r\n" + ChatTextBlock.Text;
                    });
                }
            }
            catch (ObjectDisposedException)
            {
                if (!alive)
                    return;
                throw;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // обработчик нажатия кнопки sendButton
        private void SendButton_Click(object sender, EventArgs e)
        {
            try
            {
                string message = String.Format("{0}: {1}", userName, MessageTextBox.Text);
                byte[] data = Encoding.Unicode.GetBytes(message);
                client.Send(data, data.Length, HOST, REMOTEPORT);
                MessageTextBox.Clear();
                MessageTextBox.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // обработчик нажатия кнопки logoutButton
        private void logoutButton_Click(object sender, EventArgs e)
        {
            ExitChat();
        }
        // выход из чата
        private void ExitChat()
        {
            string message = userName + " покидает чат";
            byte[] data = Encoding.Unicode.GetBytes(message);
            client.Send(data, data.Length, HOST, REMOTEPORT);
            client.DropMulticastGroup(groupAddress);

            alive = false;
            client.Close();
        }

        private void SendButton_Key(object sender, KeyEventArgs e)
        {
            string s = e.Key.ToString();
            if (e.Key == Key.Enter)
            {
                try
                {
                    string message = String.Format("{0}: {1}", userName, MessageTextBox.Text);
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    client.Send(data, data.Length, HOST, REMOTEPORT);
                    MessageTextBox.Clear();
                    MessageTextBox.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
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
        // обработчик события закрытия формы
        /*
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (alive)
                ExitChat();
        }
        /* чат */
    }
}
