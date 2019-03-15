using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HexGridControl;
using WpfAnimatedGif;

namespace Nuclear
{
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

        public List<Point> wave = new List<Point>();
        private List<Point> wavePath = new List<Point>();
        private List<Point> DopWavePath = new List<Point>();
        private PlayerUser User = new PlayerUser(8, 2, 20, 12, 5);
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

        Image img = new Image();
        Image img1 = new Image();
        public object gridHeat { get; private set; }

        public Game()
        {

            InitializeComponent();
            //groupAddress = IPAddress.Parse(HOST); 
            //loginButton_Click();
            MapImageGrid();
            MapActiveGrid();
            MapHeatGrid();
            MapImgPlayerGrid();
            User.SetImage(GROD, img);
            findPath(8, 2, User.GetX(), User.GetY());
            //initUser(8, 2);
        }

        private void initUser(int x, int y)
        {
            var image = new BitmapImage();
            var image1 = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(Environment.CurrentDirectory + "/data/image/characters/NMVALTAA_se.gif");
            image.EndInit();
            ImageBehavior.SetAnimatedSource(img, image);
            image1.BeginInit();
            image1.UriSource = new Uri(Environment.CurrentDirectory + "/data/image/characters/NMVALTAA_se.gif");
            image1.EndInit();
            ImageBehavior.SetAnimatedSource(img1, image1);
            Canvas.SetLeft(img, 16);
            Canvas.SetTop(img, 24);
            Canvas.SetLeft(img1, 34); 
            Canvas.SetTop(img1, 37);
            GROD.Children.Add(img);
            GROD.Children.Add(img1);
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
            int HeightMap = 12;
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
            int HeightMap = 12;
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
            int HeightMap = 12;
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
                    if (PathArray[j, i] == -1)
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
            int HeightMap = 12;
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

            if (PathArray[y, x] == -1 || PathArray[ny, nx] == -1)
            {
                // вывод ошибки выбора - недоступная зона (стена)
                //return;
            }
            while (true)
            {
                //волновой алгоритм поиска пути (заполнение значений достижимости) начиная от конца пути
                DopPathArray = (int[,])PathArray.Clone();
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

                clonePathArray = (int[,])PathArray.Clone();
                List<Point> oldWave = new List<Point>();
                oldWave.Add(new Point(nx, ny));
                nstep = 0;
                PathArray[ny, nx] = nstep;
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
                            if (PathArray[ny, nx] == 0)
                            {
                                wave.Add(new Point(nx, ny));
                                PathArray[ny, nx] = nstep;
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
                while (PathArray[y, x] != 99)
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
                    if (stepX == x && stepY == y)
                        break;
                    if (flag)
                    {
                        // вывод ошибки, пути нет
                        break;
                    }
                }
                wave.Reverse();
                PathArray = clonePathArray;
                DopPathArray = DopClonePathArray;
                /*
                wave.ForEach(delegate (Point i)
                {
                    PathArray[i.y, i.x] = 1;
                });
                */
                //waveOut();
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
                            User.ChangeImage(GROD, img);

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
