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
    /// Логика взаимодействия для SkillTree.xaml
    /// </summary>
    public partial class SkillTree : Page
    {
        private static byte[] memorySkills = new byte[14];

        public SkillTree()
        {
            InitializeComponent();
            LazyCheck();

        }

        public void LazyCheck()
        {
            for (byte i = 0; i < 14; i++)
                switch (i)
                {
                    case 0:
                        if (memorySkills[0] == 0) MainSkills.IsEnabled = true;
                        else MainSkills.IsEnabled = false;
                        break;
                    case 1:
                        if (memorySkills[0] == 1 && memorySkills[1] == 0) but1.IsEnabled = true;
                        else but1.IsEnabled = false;
                        break;
                    case 2:
                        if (memorySkills[0] == 1 && memorySkills[2] == 0) but2.IsEnabled = true;
                        else but2.IsEnabled = false;
                        break;
                    case 3:
                        if (memorySkills[1] == 1 && memorySkills[3] == 0) but11.IsEnabled = true;
                        else but11.IsEnabled = false;
                        break;
                    case 4:
                        if (memorySkills[1] == 1 && memorySkills[2] == 1 && memorySkills[4] == 0) butT.IsEnabled = true;
                        else butT.IsEnabled = false;
                        break;
                    case 5:
                        if (memorySkills[2] == 1 && memorySkills[5] == 0) but23.IsEnabled = true;
                        else but23.IsEnabled = false;
                        break;
                    case 6:
                        if (memorySkills[3] == 1 && memorySkills[6] == 0) but111.IsEnabled = true;
                        else but111.IsEnabled = false;
                        break;
                    case 7:
                        if (memorySkills[3] == 1 && memorySkills[4] == 1 && memorySkills[7] == 0) but11TT.IsEnabled = true;
                        else but11TT.IsEnabled = false;
                        break;
                    case 8:
                        if (memorySkills[4] == 1 && memorySkills[5] == 1 && memorySkills[8] == 0) butT3T.IsEnabled = true;
                        else butT3T.IsEnabled = false;
                        break;
                    case 9:
                        if (memorySkills[5] == 1 && memorySkills[9] == 0) but234.IsEnabled = true;
                        else but234.IsEnabled = false;
                        break;
                    case 10:
                        if (memorySkills[6] == 1 && memorySkills[10] == 0) but1111.IsEnabled = true;
                        else but1111.IsEnabled = false;
                        break;
                    case 11:
                        if (memorySkills[7] == 1 && memorySkills[11] == 0) but11TT2.IsEnabled = true;
                        else but11TT2.IsEnabled = false;
                        break;
                    case 12:
                        if (memorySkills[8] == 1 && memorySkills[12] == 0) butT3T3.IsEnabled = true;
                        else butT3T3.IsEnabled = false;
                        break;
                    case 13:
                        if (memorySkills[9] == 1 && memorySkills[13] == 0) but2344.IsEnabled = true;
                        else but2344.IsEnabled = false;
                        break;
                }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            memorySkills[3] = 1;
            (sender as Button).IsEnabled = false;
            LazyCheck();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            memorySkills[0] = 1;
            (sender as Button).IsEnabled = false;
            LazyCheck();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            memorySkills[1] = 1;
            (sender as Button).IsEnabled = false;
            LazyCheck();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            memorySkills[2] = 1;
            (sender as Button).IsEnabled = false;
            LazyCheck();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            memorySkills[4] = 1;
            (sender as Button).IsEnabled = false;
            LazyCheck();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            memorySkills[5] = 1;
            (sender as Button).IsEnabled = false;
            LazyCheck();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            memorySkills[6] = 1;
            (sender as Button).IsEnabled = false;
            LazyCheck();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            memorySkills[7] = 1;
            (sender as Button).IsEnabled = false;
            LazyCheck();
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            memorySkills[8] = 1;
            (sender as Button).IsEnabled = false;
            LazyCheck();
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            memorySkills[9] = 1;
            (sender as Button).IsEnabled = false;
            LazyCheck();
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            memorySkills[10] = 1;
            (sender as Button).IsEnabled = false;
            LazyCheck();
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            memorySkills[11] = 1;
            (sender as Button).IsEnabled = false;
            LazyCheck();
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            memorySkills[12] = 1;
            (sender as Button).IsEnabled = false;
            LazyCheck();
        }

        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            memorySkills[13] = 1;
            (sender as Button).IsEnabled = false;
            LazyCheck();
        }
    }
}
