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
        public GameCamera()
        {
            InitializeComponent();
            InitOldGrid();
        }

        public void InitOldGrid()
        {
            int HeightMap = 20;
            int WidthMap = 30;


            Grid grid = new Grid();
            grid.ShowGridLines = true;
            for(int i = 0; i < HeightMap; i++)
                grid.RowDefinitions.Add(new RowDefinition());
            for(int i = 0; i < WidthMap; i++)
                grid.ColumnDefinitions.Add(new ColumnDefinition());

            StackPanel testPanel = new StackPanel();
            testPanel.Orientation = Orientation.Horizontal;
            testPanel.HorizontalAlignment = HorizontalAlignment.Center;

            Grid.SetRow(testPanel, 0);
            Grid.SetColumn(testPanel, 0);
            Grid.SetColumnSpan(testPanel, 3);

            grid.Children.Add(testPanel);

            this.Content = grid;
        }
    }
}
