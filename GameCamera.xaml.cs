using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для GameCamera.xaml
    /// </summary>
    public partial class GameCamera : Page
    {
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
        private PlayerUser User = new PlayerUser(1, 1, 20);

        public GameCamera()
        {
            InitializeComponent();
            MapImageGrid();
            MapActiveGrid();
            findPath(1, 1, User.GetX(), User.GetY());
        }

        public void MapImageGrid()
        {
            int HeightMap = 12;
            int WidthMap = 27;

            Grid grid = new Grid();
            grid.Name = "Image";
            grid.ShowGridLines = true;
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
            gridActive.Name = "Path";
            gridActive.ShowGridLines = true;
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
                        myRect.Fill = Brushes.Red;
                        Grid.SetColumn(myRect, i);
                        Grid.SetRow(myRect, j);
                        gridActive.Children.Add(myRect);
                    }
                    else
                    {
                        Button but = new Button();
                        but.Click += but_Click;
                        but.Opacity = 0.5;
                        Grid.SetColumn(but, i);
                        Grid.SetRow(but, j);
                        gridActive.Children.Add(but);
                    }
                }
            Grid.SetZIndex(gridActive, 999);
            this.Map.Children.Add(gridActive);
        }

        private void but_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int row = (int)btn.GetValue(Grid.RowProperty);
            int column = (int)btn.GetValue(Grid.ColumnProperty);
            User.SetXY(row, column);
            MessageBox.Show(string.Format("Button clicked at column {0}, row {1}", column, row));
            Clean_TextBlock();
            findPath(1, 1, User.GetX(), User.GetY());
        }

        private void Clean_TextBlock()
        {
            foreach (Grid grid in Map.Children)
                if (grid.Name == "Image")
                {
                    UIElementCollection children1 = grid.Children;
                    var children = children1.OfType<UIElement>().ToList();
                    foreach (TextBlock textblock in children)
                    {
                        grid.Children.Remove(textblock);
                    }
                }
        }


        public void findPath(int x, int y, int nx, int ny)
        {
            if (PathArray[y, x] == -1 || PathArray[ny, nx] == -1)
            {
                // вывод ошибки выбора - недоступная зона (стена)
                return;
            }

            //волновой алгоритм поиска пути (заполнение значений достижимости) начиная от конца пути
            int[,] clonePathArray = (int[,])PathArray.Clone();
            List<Point> oldWave = new List<Point>();
            oldWave.Add(new Point(nx, ny));
            int nstep = 0;
            PathArray[ny, nx] = nstep;

            int[] dx = { 0, 1, 0, -1 };
            int[] dy = { -1, 0, 1, 0 };

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
                            TextBlock text = new TextBlock();
                            text.Text = nstep.ToString();
                            Grid.SetColumn(text, ny);
                            Grid.SetRow(text, nx);
                            foreach(Grid gridActive in Map.Children)
                                if(gridActive.Name == "Image")
                                    gridActive.Children.Add(text);
                        }
                    }
                }
                oldWave = new List<Point>(wave);
            }

            //волновой алгоритм поиска пути начиная от начала
            bool flag = true;
            wave.Clear();
            wave.Add(new Point(x, y));
            while (PathArray[y, x] != 0)
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
                        break;
                    }
                }
                if (flag)
                {
                    // вывод ошибки, пути нет
                    break;
                }
            }

            PathArray = clonePathArray;

            wave.ForEach(delegate (Point i)
            {
                PathArray[i.y, i.x] = 0;
            });
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
    }
}