using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SensorVerifyServer
{
    public partial class MainWindow : Window
    {

        public static string ip = "192.168.1.104";
        public static int port1 = 8888;
        public static int port2 = 8887;

        private bool Server_on = false;
        private bool Server_first = false;
        private void Open_Server(object sender, RoutedEventArgs e)
        {
            if (!Server_on)
            {
                Light(true, 1);
                if (!Server_first)
                {
                    startServer(port1);
                    Server_first = true;
                }
                startServer2(port2);
                log("server is runing");
                button1.Content = "Close Server";
                Server_on = true;
                // Media2.Source = new Uri(Environment.CurrentDirectory.ToString() + "/22.gif");
                Picturebox2.ImageLocation = "22.gif";
            }
            else
            {
                Light(false, 1);
                log("server is closed");
                button1.Content = "Run Server";
                Server_on = false;

                Server_Clear();
                 Picturebox2.ImageLocation = "21.gif";
               // Media2.Source = new Uri(Environment.CurrentDirectory.ToString() + "/21.gif");
            }
        }

        private void Server_Clear()
        {
            endServer2();
        }
    }
}
