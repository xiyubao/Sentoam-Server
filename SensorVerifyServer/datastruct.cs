using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorVerifyServer
{
    public class train
    {
        public int id;
        public int d;
        public double[]msg1 = new double[MainWindow.M];
        public double[] msg2= new double[MainWindow.M];
        public bool flag;
        public train() { }
        public train(int id,int d,double []msg1,double []msg2,bool flag)
        {
            this.id = id;
            this.d = d;
            for(int i=0;i< d; ++i)
            {
                this.msg1[i] = msg1[i];
                this.msg2[i] = msg2[i];
            }
            this.flag = flag;
        }
    }

    public class validation
    {
        public int id;
        public int d;
        public double[] msg1 = new double[MainWindow.M];
        public double[] msg2 = new double[MainWindow.M];
        public String flag;
        public bool flagR;
        public validation() { }
        public validation(int id, int d, double[] msg1, double[] msg2, String flag,bool flagR)
        {
            this.id = id;
            this.d = d;
            for (int i = 0; i < d; ++i)
            {
                this.msg1[i] = msg1[i];
                this.msg2[i] = msg2[i];
            }
            this.flag = flag;
            this.flagR = flagR;
        }
    }

    public class data
    {
        public int d;
        public double[] msg = new double[MainWindow.M];
        public data() { }
        public data(int d, double[] msg1, double[] msg2)
        {
            this.d = d;
            for (int i = 0; i < d; ++i)
            {
                this.msg[i] = Math.Abs(msg1[i] - msg2[i]);
            }
        }
    }

}
