﻿using Nuclear.src;
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
    /// Логика взаимодействия для StartMenu.xaml
    /// </summary>
    public partial class StartMenu : Page
    {
        public StartMenu()
        {
            InitializeComponent();
            ShowsNavigationUI = false;
        }

        private void Game_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Game());
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Registration());
        }

        private void OnlineGame_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new NetworkRoom());
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MoveMouse_but(object sender, MouseEventArgs e)
        {
            (sender as TextBlock).Opacity = 0.8;
            ImageBrush das = new ImageBrush();
            das.ImageSource = new BitmapImage(new Uri(@"data/image/mainui/startClick/backlight.png", UriKind.Relative));
            (sender as TextBlock).Background = das;
        }

        private void MoveLeave_but(object sender, MouseEventArgs e)
        {
            (sender as TextBlock).Opacity = 0.6;
            ImageBrush das = new ImageBrush();
            (sender as TextBlock).Background = das;
        }
    }
}
