using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorVerifyServer
{
    class DKEY
    {
        public  string time;
        public  string userid;

        public DKEY(string time,string userid)
        {
            this.time = time;
            this.userid = userid;
        }

        public void generation()
        {
            string input = time + userid;
            string P0 = SM3(input);
            int[] I = new int[8];
            int index = 0;
            for(int i = 0; i < 8; ++i)
            {
              //  I[i] = P0[index++] << 24 || P0[index++] << 16 || P0[index++] << 8 || P0[index++];
            }

        }

        public string SM3(string S)
        {

            return S;
        }
    }
}
