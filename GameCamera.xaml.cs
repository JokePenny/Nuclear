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
        //10hx20w size
        private static int[,] PathArray = new int[,] {
            {-1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
            {-1, 1, 1, 1, -1, 1, 1, 1, 1, -1},
            {-1, 1, 1, 1, -1, 1, 1, 1, 1, -1},
            {-1, 1, 1, 1, -1, 1, 1, 1, 1, -1},
            {-1, 1, 1, 1, -1, 1, -1, 1, 1, -1},
            {-1, 1, 1, 1, -1, 1, -1, 1, 1, -1},
            {-1, 1, 1, 1, -1, 1, -1, 1, 1, -1},
            {-1, 1, 1, 1, 1, 1, -1, -1, 1, -1},
            {-1, 1, 1, 1, -1, 1, 1, 1, 1, -1},
            {-1, 1, 1, 1, -1, 1, 1, 1, 1, -1},
            {-1, 1, 1, 1, -1, 1, 1, 1, 1, -1},
            {-1, 1, 1, 1, -1, 1, -1, -1, 1, -1},
            {-1, 1, 1, 1, -1, 1, 1, -1, 1, -1},
            {-1, 1, 1, 1, 1, 1, -1, -1, 1, -1},
            {-1, 1, 1, 1, -1, 1, 1, 1, 1, -1},
            {-1, 1, 1, 1, -1, 1, 1, 1, 1, -1},
            {-1, 1, 1, 1, -1, 1, 1, 1, 1, -1},
            {-1, 1, 1, 1, 1, 1, 1, 1, 1, -1},
            {-1, 1, 1, 1, 1, 1, 1, 1, 1, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
            };

        private static int[,] ImageIDArray = new int[,] {
            {-1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
            {-1, 1, 1, 1, -1, 1, 1, 1, 1, -1},
            {-1, 1, 1, 1, -1, 1, 1, 1, 1, -1},
            {-1, 1, 1, 1, -1, 1, 1, 1, 1, -1},
            {-1, 1, 1, 1, -1, 1, -1, 1, 1, -1},
            {-1, 1, 1, 1, -1, 1, -1, 1, 1, -1},
            {-1, 1, 1, 1, -1, 1, -1, 1, 1, -1},
            {-1, 1, 1, 1, 1, 1, -1, -1, 1, -1},
            {-1, 1, 1, 1, -1, 1, 1, 1, 1, -1},
            {-1, 1, 1, 1, -1, 1, 1, 1, 1, -1},
            {-1, 1, 1, 1, -1, 1, 1, 1, 1, -1},
            {-1, 1, 1, 1, -1, 1, -1, -1, 1, -1},
            {-1, 1, 1, 1, -1, 1, 1, -1, 1, -1},
            {-1, 1, 1, 1, 1, 1, -1, -1, 1, -1},
            {-1, 1, 1, 1, -1, 1, 1, 1, 1, -1},
            {-1, 1, 1, 1, -1, 1, 1, 1, 1, -1},
            {-1, 1, 1, 1, -1, 1, 1, 1, 1, -1},
            {-1, 1, 1, 1, 1, 1, 1, 1, 1, -1},
            {-1, 1, 1, 1, 1, 1, 1, 1, 1, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
            };

        public GameCamera()
        {
            InitializeComponent();
            MapImageGrid();
            MapActiveGrid();
        }

        public void MapImageGrid()
        {
            int HeightMap = 10;
            int WidthMap = 20;

            Grid grid = new Grid();
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

            /*
            Grid.SetRow(testPanel, 0);
            Grid.SetColumn(testPanel, 0);
            Grid.SetColumnSpan(testPanel, 3);
            */
            grid.Background = Brushes.Black;
            Grid.SetZIndex(grid, 1);
            this.Map.Children.Add(grid);
        }

        public void MapActiveGrid()
        {
            int HeightMap = 10;
            int WidthMap = 20;

            Grid gridActive = new Grid();
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
                    if (PathArray[i,j] == -1)
                    {
                        Rectangle myRect = new Rectangle();
                        myRect.Fill = Brushes.Red;
                        Grid.SetColumn(myRect, i);
                        Grid.SetRow(myRect, j);
                        gridActive.Children.Add(myRect);
                    }
            Grid.SetZIndex(gridActive, 999);

            this.Map.Children.Add(gridActive);
        }
    }
}