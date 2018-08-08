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
        private string GetTimeNow()
        {
            return DateTime.Now.ToLongTimeString().ToString()+":"+ string.Format("{0:D3}", DateTime.Now.Millisecond) + " ";
        }
    }
}
