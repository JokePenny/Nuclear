using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace Nuclear.src
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        // адрес и порт сервера, к которому будем подключаться
        static int port = 8005; // порт сервера
        static string address = "127.0.0.1"; // адрес сервера
        Socket socket = null;

        public Registration()
        {
            InitializeComponent();
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipPoint);
                ChatTextBlock.Text = ChatTextBlock.Text + "\r\n" + "Сервер активен!";
            }
            catch (Exception ex)
            {
                ChatTextBlock.Text = ChatTextBlock.Text + "\r\n" + "Сервер неактивен!";
                RegistrUser.Opacity = 0.3;
                RegistrUser.IsEnabled = false;
                Login.Opacity = 0.3;
                Login.IsEnabled = false;
                Password.Opacity = 0.3;
                Password.IsEnabled = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string message = Login.Text;
            message += "\r\n" + Password.Text;
            byte[] data = Encoding.Unicode.GetBytes(message);
            socket.Send(data);
            data = new byte[256];
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = socket.Receive(data, data.Length, 0);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (socket.Available > 0);
            Console.WriteLine("ответ сервера: " + builder.ToString());
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new StartMenu());
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
