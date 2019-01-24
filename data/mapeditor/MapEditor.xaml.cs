using Nuclear.data.mapeditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
        Field field;

        private int CloneSelectedObjectID = 0;
        private int selectedObjectID = 0;
        private string[] foldersName = new string[21];
        private int sizeArrayX = 12;
        private int sizeArrayY = 27;

        private int[,] ImageIDArray = new int[,] {
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            };

        private int[,] TriggerIDArray = new int[,] {
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            };


        public MapEditor()
        {
            InitializeComponent();
            /*
            DirectoryInfo dir = new DirectoryInfo(@"D:\01Programms\VS\Repository\Nuclear\Nuclear\data\mapeditor\sprits");
            foreach (var item in dir.GetDirectories())
            {
                TextBlock text = new TextBlock();
                text.Text = item.Name;
                this.WallAndDecorate.Items.Add(text);
            }
            */
            Props.IsEnabled = false;
            Triggers.IsEnabled = false;
            //MapImageGrid();
            //MapActiveGrid();
        }

        private void WallAndDecorate_Selected(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            TextBlock selectedItem = (TextBlock)comboBox.SelectedItem;
            if (selectedItem.Text == "wall")
            {
                DirectoryInfo dir = new DirectoryInfo(@"D:\01Programms\VS\Repository\Nuclear\Nuclear\data\mapeditor\sprits\wall");
                this.FoldersSection.Items.Clear();
                int numberFolder = 0;
                foreach (var item in dir.GetDirectories())
                {
                    try
                    {
                        foldersName[numberFolder] = item.Name;
                        numberFolder++;
                    }
                    catch (IndexOutOfRangeException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                        
                    TextBlock text = new TextBlock();
                    text.Text = item.Name;
                    this.FoldersSection.Items.Add(text);
                }
                SelectedSection.Text = "Стены";
            }
            else
            {
                DirectoryInfo dir = new DirectoryInfo(@"D:\01Programms\VS\Repository\Nuclear\Nuclear\data\mapeditor\sprits\props");
                this.FoldersSection.Items.Clear();
                int numberFolder = 10;
                foreach (var item in dir.GetDirectories())
                {
                    try
                    {
                        foldersName[numberFolder] = item.Name;
                        numberFolder++;
                    }
                    catch (IndexOutOfRangeException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

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
            TextBlock selectedItemRootFolder = (TextBlock)WallAndDecorate.SelectedItem;
            try
            {
                if (selectedItem.Text != null)
                {
                    string path, pathDop;
                    if (selectedItemRootFolder.Text == "wall")
                    {
                        path = @"D:\01Programms\VS\Repository\Nuclear\Nuclear\bin\Debug\data\mapeditor\sprits\wall\" + selectedItem.Text; // сделать относительный путь файла, как ниже
                        pathDop = "/data/mapeditor/sprits/wall/";
                        selectedObjectID = 1000;
                    } 
                    else
                    {
                        path = @"D:\01Programms\VS\Repository\Nuclear\Nuclear\bin\Debug\data\mapeditor\sprits\props\" + selectedItem.Text; // сделать относительный путь файла, как ниже
                        pathDop = "/data/mapeditor/sprits/props/";
                        selectedObjectID = 100;
                    }
                    PurposeRankID(selectedItem.Text);

                    DirectoryInfo dir = new DirectoryInfo(path);
                    int column = 4;
                    int rowChange = 0;
                    int columnChange = 0;
                    foreach (var item in dir.GetFiles())
                    {
                        Image ImageContainer = new Image();
                        ImageSource image = new BitmapImage(new Uri(Environment.CurrentDirectory + pathDop + selectedItem.Text + "/" + item.Name, UriKind.Absolute));
                        ImageContainer.Source = image;
                        ImageContainer.MouseDown += SelectedImageForMapImage_Click;
                        Grid.SetColumn(ImageContainer, columnChange);
                        Grid.SetRow(ImageContainer, rowChange);
                        ContainerForImage.Children.Add(ImageContainer);
                        if (columnChange + 1 < column)
                            columnChange++;
                        else
                        {
                            RowDefinition row = new RowDefinition();
                            row.MinHeight = 50;
                            row.MaxHeight = 50;
                            ContainerForImage.RowDefinitions.Add(row);
                            rowChange++;
                            columnChange = 0;
                        }
                    }
                }
            }
            catch(NullReferenceException)
            {
                int x = ContainerForImage.RowDefinitions.Count() - 1;
                for (int i = x; i > 0; i--)
                    ContainerForImage.RowDefinitions.RemoveAt(i);
                ContainerForImage.Children.Clear();
            }
        }

        private void SelectedImageForMapImage_Click(object sender, RoutedEventArgs e)
        {

            Image btn = sender as Image;
            Clipboard.SetImage(new BitmapImage((btn.Source as BitmapImage).UriSource));
            //Clipboard.SetText();
            int row = (int)btn.GetValue(Grid.RowProperty);
            int column = (int)btn.GetValue(Grid.ColumnProperty);
            UIElementCollection children1 = ContainerForImage.Children;
            var children = children1.OfType<UIElement>().ToList();
            foreach (Image but in children)
            {
                int i = ContainerForImage.Children.IndexOf(btn);
                if (selectedObjectID == CloneSelectedObjectID)
                    selectedObjectID += i + 1;
                else
                {
                    selectedObjectID = CloneSelectedObjectID;
                    selectedObjectID += i + 1;
                }
                break;
            }
            //MessageBox.Show(string.Format("Клетка {0}, {1}\n size {2}", column, row, selectedObjectID));
        }

        private void PurposeRankID(string nameFolder)
        {
            if (selectedObjectID == 1000)
                for (int i = 0; i < 10; i++)
                {
                    if (foldersName[i] == nameFolder)
                    {
                        selectedObjectID = selectedObjectID + i * 100;
                        CloneSelectedObjectID = selectedObjectID;
                        return;
                    }

                }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    if (foldersName[10 + i] == nameFolder)
                    {
                        selectedObjectID = selectedObjectID + i * 100;
                        CloneSelectedObjectID = selectedObjectID;
                        return;
                    }

                }
            }
        }

        private void Choice_WorkTool(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;

            if (radioButton.Content.ToString() == "Сетка со спрайтами карты")
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
                        CheckChangeOnMapImage(gridimage);
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
                        CheckChangeOnMapImage(gridimage);
                    }
                }
            }
        }

        public void CheckChangeOnMapImage(Grid gridTrigger)
        {
            int x = 0, y = 0;
            UIElementCollection children1 = gridTrigger.Children;
            var children = children1.OfType<UIElement>().ToList();
            foreach (TextBlock text1 in children)
            {
                TextBlock text = text1;
                if (field.GetImageIDArray(x, y) > 1000)
                {
                    if (text.Background == Brushes.White)
                    {
                        gridTrigger.Children.Remove(text);
                        TextBlock newText = new TextBlock();
                        newText.Background = Brushes.Black;
                        Grid.SetColumn(newText, y);
                        Grid.SetRow(newText, x);
                        gridTrigger.Children.Add(newText);
                    }
                }
                else
                {
                    if (text.Background == Brushes.Black)
                    {
                        gridTrigger.Children.Remove(text);
                        TextBlock newText = new TextBlock();
                        newText.Background = Brushes.White;
                        text.MouseDown += but_Click;
                        text.Opacity = 0.0;
                        Grid.SetColumn(newText, y);
                        Grid.SetRow(newText, x);
                        gridTrigger.Children.Add(newText);
                    }
                }
                if (y < field.GetSizeArrayY() - 1)
                    y++;
                else if (x < field.GetSizeArrayX() - 1)
                {
                    x++;
                    y = 0;
                }
                else break;
            }

        }

        public void MapImageGrid()
        {
            int HeightMap = field.GetSizeArrayX();
            int WidthMap = field.GetSizeArrayY();

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
                    Image but = new Image();
                    ImageSource image = new BitmapImage(new Uri(Environment.CurrentDirectory + "/data/mapeditor/sprits/wall/wallr/hhh.jpg", UriKind.Absolute));
                    but.Source = image;
                    but.MouseDown += DrawingImage_Click;
                    but.Height = 50;
                    but.Width = 50;
                    but.Opacity = 1;
                    Grid.SetColumn(but, i);
                    Grid.SetRow(but, j);
                    grid.Children.Add(but);
                }
            grid.Background = Brushes.White;
            Grid.SetZIndex(grid, 1);
            this.Map.Children.Add(grid);
        }

        private void DrawingImage_Click(object sender, RoutedEventArgs e)
        {
            Image picture = sender as Image;
            ImageSource image = Clipboard.GetImage();
            picture.Source = image;
            int row = (int)picture.GetValue(Grid.RowProperty);
            int column = (int)picture.GetValue(Grid.ColumnProperty);
            field.SetImageIDArray(row, column, selectedObjectID);
            //MessageBox.Show(string.Format("Клетка {0}, {1}", column, row));
        }

        public void MapActiveGrid()
        {
            int HeightMap = field.GetSizeArrayX();
            int WidthMap = field.GetSizeArrayY();

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
                    if (field.GetImageIDArray(j, i) == -1)
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
                        but.Background = Brushes.White;
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
                    if (gridTrigger.Opacity == 1 && field.GetImageIDArray(row, column) < 1000)
                    {
                        TextBlock but = new TextBlock();
                        but.Text = DesignationTrigger(selectedObjectID);
                        field.SetTriggerIDArray(row, column, selectedObjectID);
                        but.MouseDown += but_Click;
                        but.Opacity = 0.5;
                        Grid.SetColumn(but, column);
                        Grid.SetRow(but, row);
                        gridTrigger.Children.Add(but);
                    }
                    else if (gridTrigger.Opacity == 1 && field.GetImageIDArray(row, column) > 1000)
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
            string name;
            switch((sender as MenuItem).Header.ToString())
            {
                case "Новый проект":
                    name = "NewProject";
                    break;
                case "Открыть проект":
                    name = "OpenProject";
                    DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory + "/data/mapcreateds/");
                    this.NameFileMap.Items.Clear();
                    foreach (var item in dir.GetFiles())
                    {
                        TextBlock text = new TextBlock();
                        text.Text = item.Name;
                        this.NameFileMap.Items.Add(text);
                    }
                    break;
                case "Сохранить как":
                    name = "SaveAs";
                    break;
                case "Панель внешнего вида":
                    name = "PanelOutsideView";
                    break;
                case "Панель звуковых эффектов":
                    name = "PanelSoundEffects";
                    break;
                case "Панель системы частиц":
                    name = "PanelSystemParticle";
                    break;
                default:
                    name = "";
                    break;
            }
            VisiblePanel(name);
        }

        private void VisiblePanel(string namePanel)
        {
            foreach (var panel in Editor.Children)
            {
                if (panel is Border)
                {
                    Border panelVisible = panel as Border;
                    if (panelVisible.Name != namePanel && panelVisible.Name != "PanelBottom")
                        panelVisible.Visibility = Visibility.Collapsed;
                    else
                        panelVisible.Visibility = Visibility.Visible;
                }
            }
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
                case "ClearMesh":
                    selectedObjectID = 0;
                    break;
                default:
                    break;
            }
        }

        private void SaveMap_Click(object sender, RoutedEventArgs e)
        {
            SaveMap();
        }

        private void SaveMap()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(Environment.CurrentDirectory + "/data/mapcreateds/" + field.GetName() + ".dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, field);
                MessageBox.Show(string.Format("Объект сериализован"));
            }
        }

        private void CreateMap_Click(object sender, RoutedEventArgs e)
        {
            if(Convert.ToInt32(SizeX.Text) <= 100 && Convert.ToInt32(SizeX.Text) > 0 && Convert.ToInt32(SizeY.Text) <= 100 && Convert.ToInt32(SizeY.Text) > 0 && NameMap.Text.Length < 15 && NameMap.Text.Length > 0)
            {
                Save_Button.IsEnabled = true;
                SaveAs_Button.IsEnabled = true;
                Field createMap = new Field(Convert.ToInt32(SizeX.Text), Convert.ToInt32(SizeY.Text), NameMap.Text);
                field = createMap;
                VisiblePanel("PanelOutsideView");
                DirectoryInfo dir = new DirectoryInfo(@"D:\01Programms\VS\Repository\Nuclear\Nuclear\data\mapeditor\sprits");
                foreach (var item in dir.GetDirectories())
                {
                    TextBlock text = new TextBlock();
                    text.Text = item.Name;
                    this.WallAndDecorate.Items.Add(text);
                }
                MapImageGrid();
                MapActiveGrid();
            }
            else MessageBox.Show(string.Format(" Ошибка!\n Проверьте введенные значения:\n Максимальная высота и ширина - 100 клеток\n Ваша Высота - {0}, Ширина - {1}\n Максимальная длина имени файла - 14 символов\n Длина заданного Вами имени - {2}", Convert.ToInt32(SizeX.Text), Convert.ToInt32(SizeY.Text), NameMap.Text.Length));  
        }

        private void Number_CheckWrite(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789".IndexOf(e.Text) < 0;
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            if (NameMap.Text.Length < 15 && NameMap.Text.Length > 0)
            {
                field.SetName(ChangeNameMap.Text);
                SaveMap();
                VisiblePanel("PanelOutsideView");
            }
            else MessageBox.Show(string.Format(" Ошибка!\n Проверьте введенные значения:\n Максимальная длина имени файла - 14 символов\n Длина заданного Вами имени - {0}", ChangeNameMap.Text.Length));
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            if (NameFileMap.SelectedItem != null)
            {
                VisiblePanel("PanelOutsideView");
                BinaryFormatter formatter = new BinaryFormatter();
                TextBlock selectedFile = (TextBlock)NameFileMap.SelectedItem;
                using (FileStream fs = new FileStream(Environment.CurrentDirectory + "/data/mapcreateds/" + selectedFile.Text, FileMode.OpenOrCreate))
                {
                    field = (Field)formatter.Deserialize(fs);
                    foreach (Grid grid in Map.Children)
                        if (grid.Name == "TriggerMap" || grid.Name == "ImageMap")
                            grid.Children.Clear();
                    MapImageGrid();
                    MapActiveGrid();
                    MessageBox.Show(string.Format("eeeeeee"));
                }
            }
            else MessageBox.Show(string.Format("Выберите файл проекта"));
        }
    }
}