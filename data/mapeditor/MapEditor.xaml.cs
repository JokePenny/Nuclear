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
using System.Windows.Media.Animation;
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
        Ellipse user;

        private int CloneSelectedObjectID = 0;
        private int selectedObjectID = 0;
        private string[] foldersName = new string[21];

        public MapEditor()
        {
            InitializeComponent();
            Props.IsEnabled = false;
            Triggers.IsEnabled = false;
        }

        private void WallAndDecorate_Selected(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            TextBlock selectedItem = (TextBlock)comboBox.SelectedItem;
            if (selectedItem.Text == "wall")
            {
                DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory + "/data/mapeditor/sprits/wall");
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
                DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory + "/data/mapeditor/sprits/props");
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
                    string pathDop;
                    DirectoryInfo dir;
                    if (selectedItemRootFolder.Text == "wall")
                    {
                        dir = new DirectoryInfo(Environment.CurrentDirectory + "/data/mapeditor/sprits/wall/" + selectedItem.Text); // сделать относительный путь файла, как ниже
                        pathDop = "/data/mapeditor/sprits/wall/";
                        selectedObjectID = 1000;
                    } 
                    else
                    {
                        dir = new DirectoryInfo(Environment.CurrentDirectory + "/data/mapeditor/sprits/props/" + selectedItem.Text); // сделать относительный путь файла, как ниже
                        pathDop = "/data/mapeditor/sprits/props/";
                        selectedObjectID = 100;
                    }
                    PurposeRankID(selectedItem.Text);
             
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
                        gridimage.IsEnabled = true;
                    }
                    if(gridimage.Name == "TriggerMap")
                    {
                        Grid.SetZIndex(gridimage, 1);
                        gridimage.Visibility = Visibility.Hidden;
                        gridimage.IsEnabled = true;
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



            DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory + "/data/mapeditor/sprits/wall");
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
            }
            dir = new DirectoryInfo(Environment.CurrentDirectory + "/data/mapeditor/sprits/props");
            this.FoldersSection.Items.Clear();
            numberFolder = 10;
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
            }

            for (int i = 0; i < WidthMap; i++)
                for (int j = 0; j < HeightMap; j++)
                {
                    if(field.GetImageIDArray(j, i) > 1000)
                    {
                        grid.Children.Add(ReturnSavedImage(field.GetImageIDArray(j, i), 1000, 100, j, i, "/data/mapeditor/sprits/wall/"));
                    }
                    else if(field.GetImageIDArray(j, i) > 100)
                    {
                        grid.Children.Add(ReturnSavedImage(field.GetImageIDArray(j, i), 100, 100, j, i, "/data/mapeditor/sprits/props/"));
                    }
                    else
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
                }
            grid.Background = Brushes.White;
            grid.IsEnabled = false;
            Grid.SetZIndex(grid, 1);
            this.Map.Children.Add(grid);
        }

        private Image ReturnSavedImage(int cell, int restriction, int folderIndex, int x, int y, string path)
        {
            string folder = "";
            int factor = 0;
            int cycle;
            int k;
            if (restriction == 1000)
                cycle = 0;
            else
                cycle = 10;

            for (k = 1 + cycle; k < 10 + cycle; k++)
                if (cell < restriction + k * folderIndex)
                {
                    folder = foldersName[k - 1];
                    factor = k - 1 - cycle;
                    break;
                }
            DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory + path + folder); // сделать относительный путь файла, как ниже
            int indexOf = 1;
            foreach (var item in dir.GetFiles())
            {
                if (cell == indexOf + (restriction + factor * folderIndex)) // indexOf - порядок пикчи в папке, 1000 - папка wall, factor * 100 - порядок папки в wall, в которой хранится пикча, - 1 потому что надо так
                {
                    Image ImageContainer = new Image();
                    ImageSource image = new BitmapImage(new Uri(Environment.CurrentDirectory + path + folder + "/" + item.Name, UriKind.Absolute));
                    ImageContainer.Source = image;
                    ImageContainer.MouseDown += DrawingImage_Click;
                    ImageContainer.Height = 50;
                    ImageContainer.Width = 50;
                    ImageContainer.Opacity = 1;
                    Grid.SetColumn(ImageContainer, y);
                    Grid.SetRow(ImageContainer, x);
                    return ImageContainer;
                }
                else indexOf++;
            }


            Image UnreachableCode = new Image();
            return UnreachableCode;
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
                    if (field.GetTriggerIDArray(j, i) > 0 && field.GetTriggerIDArray(j, i) <= 6)
                    {
                        TextBlock but = new TextBlock();
                        but.Text = DesignationTrigger(field.GetTriggerIDArray(j, i));
                        but.MouseDown += but_Click;
                        but.Opacity = 0.5;
                        Grid.SetColumn(but, i);
                        Grid.SetRow(but, j);
                        gridActive.Children.Add(but);
                    }
                    else if (field.GetImageIDArray(j, i) == -1)
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
            gridActive.IsEnabled = false;
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
                    if (Map.Children.Count != 0)
                    {
                        PanelOutsideView.IsEnabled = true;
                        foreach (Grid grid in Map.Children)
                            grid.Visibility = Visibility.Visible;
                    }
                    else PanelOutsideView.IsEnabled = false;
                    name = "PanelOutsideView";
                    break;
                case "Панель звуковых эффектов":
                    name = "PanelSoundEffects";
                    break;
                case "Панель системы частиц":
                    foreach (Grid grid in Map.Children)
                        grid.Visibility = Visibility.Collapsed;
                    AddedPrototypePlayer();
                    name = "PanelSystemParticle";
                    break;
                default:
                    name = "";
                    break;
            }
            VisiblePanel(name);
        }

        private int positionX = 100;
        private int positionY = 100;

        private void AddedPrototypePlayer()
        {
            user = new Ellipse();
            user.Width = 50;
            user.Height = 50;
            user.Fill = Brushes.Black;
            Canvas.SetZIndex(user, 999);
            Canvas.SetLeft(user, positionX);
            Canvas.SetTop(user, positionY);
            Map.Children.Add(user);
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
                MapImageGrid();
                MapActiveGrid();
                VisiblePanel("PanelOutsideView");
                DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory + "/data/mapeditor/sprits");
                foreach (var item in dir.GetDirectories())
                {
                    TextBlock text = new TextBlock();
                    text.Text = item.Name;
                    this.WallAndDecorate.Items.Add(text);
                }
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
                Save_Button.IsEnabled = true;
                SaveAs_Button.IsEnabled = true;
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
                    DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory + "/data/mapeditor/sprits");
                    foreach (var item in dir.GetDirectories())
                    {
                        TextBlock text = new TextBlock();
                        text.Text = item.Name;
                        this.WallAndDecorate.Items.Add(text);
                    }
                    MessageBox.Show(string.Format("Файл открыт"));
                }
            }
            else MessageBox.Show(string.Format("Выберите файл проекта"));
        }

        private void Map_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Random rnd = new Random();
            for (int i = 0; i < 5; i++)
                ShotgunBurst(e.GetPosition(Map).X, e.GetPosition(Map).Y, rnd);
            //LongAutomaticBurst(e.GetPosition(Map).X, e.GetPosition(Map).Y);
            //ShortAutomaticBurst(e.GetPosition(Map).X, e.GetPosition(Map).Y);
            //PistolBurst(e.GetPosition(Map).X, e.GetPosition(Map).Y);
            //GrenadeBurst(e.GetPosition(Map).X, e.GetPosition(Map).Y);
            //SniperBurst(e.GetPosition(Map).X, e.GetPosition(Map).Y);
            //Map.Children.Remove(lineFire);
            //MessageBox.Show(string.Format("Клик по канвасу"));
        }

        void ShotgunBurst(double x, double y, Random rnd)
        {
            Line lineFire = new Line();
            lineFire.Name = "Head";
            if(rnd.Next(0,2) == 1)
                lineFire.X1 = x - rnd.Next(0, 40);
            else
                lineFire.X1 = x + rnd.Next(0, 40);
            if (rnd.Next(0, 2) == 1)
                lineFire.Y1 = y - rnd.Next(0, 40);
            else
                lineFire.Y1 = y + rnd.Next(0, 40);
            lineFire.Stroke = Brushes.Blue;
            lineFire.X2 = positionX + 25;
            lineFire.Y2 = positionY + 25;
            Canvas.SetZIndex(lineFire, 999);
            Map.Children.Add(lineFire);

            DoubleAnimation BulletTrack11 = new DoubleAnimation();
            BulletTrack11.From = lineFire.X2;
            BulletTrack11.To = lineFire.X1;
            BulletTrack11.Duration = TimeSpan.FromSeconds(0.1);
            BulletTrack11.Completed += BulletTrack_Completed;

            DoubleAnimation BulletTrack12 = new DoubleAnimation();
            BulletTrack12.From = lineFire.Y2;
            BulletTrack12.To = lineFire.Y1;
            BulletTrack12.Duration = TimeSpan.FromSeconds(0.1);
            BulletTrack12.Completed += BulletTrack_Completed;
            lineFire.BeginAnimation(Line.X2Property, BulletTrack11);
            lineFire.BeginAnimation(Line.Y2Property, BulletTrack12);
        }

        void ShortAutomaticBurst(double x, double y)
        {
            Line lineFire = new Line();
            lineFire.Name = "Head";
            lineFire.X1 = x;
            lineFire.Y1 = y;
            lineFire.Stroke = Brushes.Blue;
            lineFire.X2 = positionX + 25;
            lineFire.Y2 = positionY + 25;
            Canvas.SetZIndex(lineFire, 999);
            Map.Children.Add(lineFire);

            DoubleAnimation BulletTrack11 = new DoubleAnimation();
            BulletTrack11.From = lineFire.X2;
            BulletTrack11.To = lineFire.X1;
            BulletTrack11.Duration = TimeSpan.FromSeconds(0.1);
            BulletTrack11.Completed += BulletTrack_Completed;

            DoubleAnimation BulletTrack12 = new DoubleAnimation();
            BulletTrack12.From = lineFire.Y2;
            BulletTrack12.To = lineFire.Y1;
            BulletTrack12.Duration = TimeSpan.FromSeconds(0.1);
            BulletTrack12.Completed += BulletTrack_Completed;

            lineFire.BeginAnimation(Line.X2Property, BulletTrack11);
            lineFire.BeginAnimation(Line.Y2Property, BulletTrack12);
        }

        void BulletTrack_Completed(object sender, EventArgs e)
        {
            //Map.Children.Remove((sender as Line));
            //MessageBox.Show("Анимация завершена");
        }
    }
}