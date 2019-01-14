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
    /// Логика взаимодействия для Backpack.xaml
    /// </summary>
    public partial class Backpack : Page
    {
        private byte checkDrop = 0;

        public Backpack()
        {
            InitializeComponent();
        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragDrop.DoDragDrop(sender as Button, (sender as Button).Content, DragDropEffects.Copy);
            if (checkDrop == 1)
            {
                (sender as Button).Content = "";
                checkDrop = 0;
            }
        }

        private void Button_Drop(object sender, DragEventArgs e)
        {
            checkDrop++;
            (sender as Button).Content = e.Data.GetData(DataFormats.Text);
        }
    }
}
