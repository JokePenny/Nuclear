using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Resources;

namespace Nuclear
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StartMenu menu = new StartMenu();
            WindowState = WindowState.Maximized;
            StreamResourceInfo sri = Application.GetResourceStream(
            new Uri("data/image/mainui/cursor/ACTARROW.cur", UriKind.Relative));
            Cursor customCursor = new Cursor(sri.Stream);
            this.Cursor = customCursor;

            WindowGame.NavigationService.Navigate(menu);
        }
    }
}
