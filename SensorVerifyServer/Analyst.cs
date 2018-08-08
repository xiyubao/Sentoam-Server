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
        /*public void AnalystValidationData()
        {
            int length = validation_set.Count;
     
            int model_num = validation_set[0].flag.Split('#').Count();
            int[] right = new int[model_num];
            for (int i = 0; i <model_num; ++i)
                right[i] = 0;
            for (int i = 0; i < length; ++i)
            {
                bool flagR = validation_set[i].flagR;
                string[] ss = validation_set[i].flag.Split('#');
                for (int j = 0; j < model_num; ++j)
                {
                    bool flag = bool.Parse(ss[j]);
                    if (flagR == flag)
                        ++right[i];
                }
            }
            for (int j = 0; j < model_num; ++j)
                iwsc.Send("Accuracy rate : " + right[j] / (double)length * 100 + " %");
        }*/
    }
}
