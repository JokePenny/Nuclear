using Nuclear.data.map_editor;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для MapEditor.xaml
    /// </summary>
    public partial class MapEditor : Page
    {
        private int selectedObjectID = 0;
        private int sizeArrayX = 12;
        private int sizeArrayY = 27;

        private int[,] ImageIDArray = new int[,] {
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

        private int[,] TriggerIDArray = new int[,] {
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


        public MapEditor()
        {
            InitializeComponent();
            DirectoryInfo dir = new DirectoryInfo(@"D:\01Programms\VS\Repository\Nuclear\Nuclear\data\map editor\sprits");
            foreach (var item in dir.GetDirectories())
            {
                TextBlock text = new TextBlock();
                text.Text = item.Name;
                this.WallAndDecorate.Items.Add(text);
            }
            Props.IsEnabled = false;
            Triggers.IsEnabled = false;
            MapImageGrid();
            MapActiveGrid();
        }

        private void WallAndDecorate_Selected(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            TextBlock selectedItem = (TextBlock)comboBox.SelectedItem;
            if (selectedItem.Text == "wall")
            {
                DirectoryInfo dir = new DirectoryInfo(@"D:\01Programms\VS\Repository\Nuclear\Nuclear\data\map editor\sprits\wall");
                foreach (var item in dir.GetDirectories())
                {
                    TextBlock text = new TextBlock();
                    text.Text = item.Name;
                    this.WallAndDecorate.Items.Add(text);
                }
                SelectedSection.Text = "Стены";
            }
            else
            {
                DirectoryInfo dir = new DirectoryInfo(@"D:\01Programms\VS\Repository\Nuclear\Nuclear\data\map editor\sprits\props");
                foreach (var item in dir.GetDirectories())
                {
                    TextBlock text = new TextBlock();
                    text.Text = item.Name;
                    this.FoldersSection.Items.Add(text);
                }
                SelectedSection.Text = "Декорации";
            }
        }

        private void FolderSubsetion_Selected(object sender, SelectionChangedEventArgs e)
        {
            
            ComboBox comboBox = (ComboBox)sender;
            TextBlock selectedItem = (TextBlock)comboBox.SelectedItem;
            if (selectedItem.Text == "wall")
            {
                DirectoryInfo dir = new DirectoryInfo(@"D:\01Programms\VS\Repository\Nuclear\Nuclear\data\map editor\sprits\wall");
                foreach (var item in dir.GetDirectories())
                {
                    //string Image = item.Name + ".jpg";
                    TextBlock text = new TextBlock();
                    text.Text = item.Name;
                    this.WallAndDecorate.Items.Add(text);
                }
            }
            else
            {
                DirectoryInfo dir = new DirectoryInfo(@"D:\01Programms\VS\Repository\Nuclear\Nuclear\data\map editor\sprits\props");
                foreach (var item in dir.GetDirectories())
                {
                    //string Image = item.Name + ".jpg";
                    TextBlock text = new TextBlock();
                    text.Text = item.Name;
                    this.FoldersSection.Items.Add(text);
                }
            }
            selectedItem = (TextBlock)comboBox.SelectedItem;
            if (selectedItem.Text != null)
            {
                string path = @"D:\01Programms\VS\Repository\Nuclear\Nuclear\bin\Debug\data\map editor\sprits\props\" + selectedItem.Text; // сделать относительный путь файла, как ниже
                DirectoryInfo dir = new DirectoryInfo(path);
                int column = 4;
                int rowChange = 0;
                int columnChange = 0;
                foreach (var item in dir.GetFiles())
                {
                    Image ImageContainer = new Image();
                    ImageSource image = new BitmapImage(new Uri(Environment.CurrentDirectory + "/data/map editor/sprits/props/" + selectedItem.Text + "/" + item.Name, UriKind.Absolute));
                    ImageContainer.Source = image;
                    Grid.SetColumn(ImageContainer, columnChange);
                    Grid.SetRow(ImageContainer, rowChange);
                    ContainerForImage.Children.Add(ImageContainer);
                    if (columnChange + 1 < column)
                        columnChange++;
                    else
                    {
                        RowDefinition row = new RowDefinition();
                        row.MinHeight = 50;
                        ContainerForImage.RowDefinitions.Add(new RowDefinition());
                        rowChange++;
                        columnChange = 0;
                    }
                }
            }
        }

        private void Choice_WorkTool(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if(radioButton.Content.ToString() == "Сетка со спрайтами карты")
            {
                Props.IsEnabled = true;
                Triggers.IsEnabled = false;
                foreach(Grid gridimage in Map.Children)
                {
                    if (gridimage.Name == "ImageMap")
                    {
                        Grid.SetZIndex(gridimage, 999);
                        gridimage.Opacity = 1;
                    }
                    if(gridimage.Name == "TriggerMap")
                    {
                        Grid.SetZIndex(gridimage, 1);
                        gridimage.Visibility = Visibility.Hidden;
                    }
                }
            }
            else
            {
                Props.IsEnabled = false;
                Triggers.IsEnabled = true;
                foreach (Grid gridimage in Map.Children)
                {
                    if (gridimage.Name == "ImageMap")
                    {
                        Grid.SetZIndex(gridimage, 1);
                        gridimage.Opacity = 0.5;
                    }
                    if (gridimage.Name == "TriggerMap")
                    {
                        Grid.SetZIndex(gridimage, 999);
                        gridimage.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        public void MapImageGrid()
        {
            int HeightMap = 12;
            int WidthMap = 27;

            Grid grid = new Grid();
            grid.Name = "ImageMap";
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

            for (int i = 0; i < WidthMap; i++)
                for (int j = 0; j < HeightMap; j++)
                {
                    Rectangle but = new Rectangle();
                    but.MouseDown += but_Click;
                    but.Opacity = 0.0;
                    Grid.SetColumn(but, i);
                    Grid.SetRow(but, j);
                    grid.Children.Add(but);
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
            gridActive.Name = "TriggerMap";
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
                    if (ImageIDArray[i, j] == -1)
                    {
                        TextBlock but = new TextBlock();
                        //but.IsEnabled = false;
                        but.Background = Brushes.Black;
                        Grid.SetColumn(but, i);
                        Grid.SetRow(but, j);
                        gridActive.Children.Add(but);
                    }
                    else
                    {
                        TextBlock but = new TextBlock();
                        but.MouseDown += but_Click;
                        but.Opacity = 0.0;
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
            TextBlock btn = sender as TextBlock;
            int row = (int)btn.GetValue(Grid.RowProperty);
            int column = (int)btn.GetValue(Grid.ColumnProperty);
            MessageBox.Show(string.Format("Клетка {0}, {1}", column, row));
            foreach (Grid gridTrigger in Map.Children)
            { 
                if (gridTrigger.Name == "TriggerMap")
                {
                    UIElementCollection children1 = gridTrigger.Children;
                    var children = children1.OfType<UIElement>().ToList();
                    foreach (TextBlock but in children)
                    {
                        gridTrigger.Children.Remove(btn);
                        break;
                    }
                    if (gridTrigger.Opacity == 1 && ImageIDArray[column, row] != -1)
                    {
                        TextBlock but = new TextBlock();
                        but.Text = DesignationTrigger(selectedObjectID);
                        but.MouseDown += but_Click;
                        but.Opacity = 0.5;
                        Grid.SetColumn(but, column);
                        Grid.SetRow(but, row);
                        gridTrigger.Children.Add(but);
                    }
                    else if (gridTrigger.Opacity == 1 && ImageIDArray[column, row] == -1)
                    {
                        TextBlock but = new TextBlock();
                        but.Background = Brushes.Black;
                        Grid.SetColumn(but, column);
                        Grid.SetRow(but, row);
                        gridTrigger.Children.Add(but);
                    }
                }
            }
        }

        private string DesignationTrigger(int selectedObjectID)
        {
            switch (selectedObjectID)
            {
                case 1:
                    return "SpU";
                case 2:
                    return "SpE";
                case 3:
                    return "Ch";
                case 4:
                    return "LP";
                default:
                    return "";
            }
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

        private void New_Workplace(object sender, RoutedEventArgs e)
        {
            New_Workplace passwordWindow = new New_Workplace();
            passwordWindow.ShowDialog();
        }

        private void Exit_MapEditor(object sender, RoutedEventArgs e)
        {
            StartMenu menu = new StartMenu();
            this.NavigationService.Navigate(menu);
        }

        private void selectedTriggerID(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (object)sender as RadioButton;
            switch (radioButton.Name)
            {
                case "SpawnPoint":
                    selectedObjectID = 1;
                    break;
                case "SpawnPointEnemy":
                    selectedObjectID = 2;
                    break;
                case "Chest":
                    selectedObjectID = 3;
                    break;
                case "LevelPortal":
                    selectedObjectID = 4;
                    break;
                default:
                    break;
            }
        }
    }
}