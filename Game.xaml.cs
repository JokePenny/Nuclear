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
    /// Логика взаимодействия для Game.xaml
    /// </summary>
    public partial class Game : Page
    {
        private PlayerUser User = new PlayerUser();
        public Game()
        {
            InitializeComponent();
            GameCamera gameCamera = new GameCamera();
            ProcessGame.NavigationService.Navigate(gameCamera);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Backpack backpack = new Backpack();
            ProcessGame.NavigationService.Navigate(backpack);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SkillTree skillTree = new SkillTree();
            ProcessGame.NavigationService.Navigate(skillTree);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu();
            this.NavigationService.Navigate(menu);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            User.SetMovePoints(12);
        }
    }
}
