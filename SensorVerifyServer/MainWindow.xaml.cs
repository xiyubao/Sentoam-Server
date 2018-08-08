using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

using MySql.Data.MySqlClient;

namespace SensorVerifyServer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeWindows();
        }

        List<train> train_set;
        List<validation> validation_set;

        List<train> validition_tmp;


        public int train_id = 0;
        public int validation_id = 0;
        public const int M = 9;
        public int d; public double[] Msg1; public double[] Msg2;
        public bool flag; public bool flagR; public bool IsflagRopen = false;
        public String flaglist = "";

        private void InitializeWindows()
        {
            train_set = new List<train>();
            validation_set = new List<validation>();
           // Media1.Source = new Uri(Environment.CurrentDirectory.ToString() + "/1.gif");
           // Media2.Source = new Uri(Environment.CurrentDirectory.ToString() + "/21.gif");
          //  Media3.Source = new Uri(Environment.CurrentDirectory.ToString() + "/3.gif");
        }


        const int debug = 3;
       

        private void Clear()
        {
            Database_Clear();
        }

        private void test(object sender, RoutedEventArgs e)
        {
            if (debug == 0)
            {
                string text = "test";
                SendData(sdr, text);
                log("send " + "\"" + text + "\"" + " to " + sdr.ip);
            }
            else if(debug==1)
            {
                Random random = new Random();
                double[] msg1 = new double[M];
                double[] msg2 = new double[M];
                for (int i = 0; i < M; ++i)
                {
                    msg1[i] = random.NextDouble();
                    msg2[i] = random.NextDouble();
                }
                train t = new train(train_id++ , M, msg1, msg2, true);
                Write(t);
                log("insert a test data("+t.id+ ") to database");
            }
            else if(debug==2)
            {
                string text = "server@1";
                SendData(sdr, text);
                log("send " + "\"" + text + "\"" + " to " + sdr.ip);
            }
            else if(debug==3)
            {
                if (sqlconnection == null)
                    log("database is close");
                else if (ReadTrain())
                {
                    List<train> train_tmp;
                    List<train> validacation_tmp;
                    CrossValidation(out train_tmp, out validacation_tmp, train_set);
                    TrianModelList(train_tmp);
                    validition_tmp = validacation_tmp;
                }

                


               // if (ReadValidation())  //改一下
                //    AnalystValidationData();
            }
        }

        private void test2(object sender, RoutedEventArgs e)
        {
            if (debug == 0)
            {
                string text = "test";
                iwsc.Send(text);
                log("send " + "\"" + text + "\"" + " to " + iwsc.ConnectionInfo.ClientIpAddress);
            }
            else if(debug==1)
            {
                Random random = new Random();
                double[] msg1 = new double[M];
                double[] msg2 = new double[M];
                for (int i = 0; i < M; ++i)
                {
                    msg1[i] = random.NextDouble();
                    msg2[i] = random.NextDouble();
                }
                validation t = new validation(validation_id++, M, msg1, msg2, "1",false);
                Write(t);
                log("insert a validation(" + t.id + ") to database");
            }
            else if(debug==2)
            {
                TrainAndTestByIrisData();
            }
            else if(debug==3)
            {
                AnalystValidationData(validition_tmp);
            }
        }

        private void log(string msg)
        {
            textBlock1.Dispatcher.Invoke(new Action(() => { textBlock1. Text+= GetTimeNow() + msg + "\n"; ; }));
            ScrollViewer1.Dispatcher.Invoke(new Action(() => { ScrollViewer1.ScrollToEnd(); }));
        }

        private void Light(bool on, int index)
        {
            if (on)
                switch (index)
                {
                    case 1: Ellipse1.Dispatcher.Invoke(new Action(() => { Ellipse1.Fill = Brushes.GreenYellow; })); break;
                    case 2: Ellipse2.Dispatcher.Invoke(new Action(() => { Ellipse2.Fill = Brushes.GreenYellow; })); break;
                    case 3: Ellipse3.Dispatcher.Invoke(new Action(() => { Ellipse3.Fill = Brushes.GreenYellow; })); break;
                    case 4: Ellipse4.Dispatcher.Invoke(new Action(() => { Ellipse4.Fill = Brushes.GreenYellow; })); break;
                }
            else
                switch (index)
                {
                    case 1: Ellipse1.Dispatcher.Invoke(new Action(() => { Ellipse1.Fill = Brushes.Red; })); break;
                    case 2: Ellipse2.Dispatcher.Invoke(new Action(() => { Ellipse2.Fill = Brushes.Red; })); break;
                    case 3: Ellipse3.Dispatcher.Invoke(new Action(() => { Ellipse3.Fill = Brushes.Red; })); break;
                    case 4: Ellipse4.Dispatcher.Invoke(new Action(() => { Ellipse4.Fill = Brushes.Red; })); break;
                }
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            ((MediaElement)sender).Position = ((MediaElement)sender).Position.Add(TimeSpan.FromMilliseconds(1));
        }
    }
}
