using System.Windows;

namespace Nuclear
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StartMenu menu = new StartMenu();
            WindowState = WindowState.Maximized;

            WindowGame.NavigationService.Navigate(menu);
        }
    }
}
