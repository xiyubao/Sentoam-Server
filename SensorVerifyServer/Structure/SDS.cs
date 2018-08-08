using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fleck;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows;

namespace SensorVerifyServer
{
    public partial class MainWindow : Window
    {

        public IWebSocketConnection iwsc;

        WebSocketServer server = null;

        public void startServer2(int port)
        {
            server = new WebSocketServer("ws://"+ip+":"+port+"");
            server.Start(socket =>
                {
                    iwsc = socket;
                    socket.OnOpen = () =>
                    {
                        log(socket.ConnectionInfo.ClientIpAddress + " is connecting");
                        Light(true, 4);
                    };

                    socket.OnClose = () =>
                    {
                        Light(false, 4);
                        log(socket.ConnectionInfo.ClientIpAddress + " has been disconnected");
                    };
                    socket.OnMessage = message =>
                    {
                        log("get \"" + message + "\"" + " from " + socket.ConnectionInfo.ClientIpAddress);

                        string getfromSDS1 = "web@Mode1@d#Msg1#flag";
                        string sendtoSDR1 = "server@Mode1";
                        string getfromSDR1 = "android@Model@d#Msg2";
                        string sendtoDB1 = "send train data to database";

                        string getfromSDS2 = "web@Mode2@d#Msg1";
                        string sendtoSDR2 = "server@Mode2";
                        string getfromSDR2 = "android@Mode2@d#Msg2";
                        string sendtoNB2 = "send validation data to naive bayes and get flag";

                        string sendtoSDS3 = "server@Model3@flag";
                        string getfromSDS3 = "web@Model3@flagR";
                        string sendtoDB3 = "send validation data to database";
                        
                        string getfromSDS4 = "web@Model4"; //begin to train
                        string getfromSDS5 = "web@Model5"; //begin to analyst validation data

                        string sendtoSDS4 = "server@Model4";// SDR is not connected
                        string sendtoSDS5 = "server@Model5";//Model is not exist
                        string sendtoSDS6 = "server@Model6";//flagR is not allowed

                        string[] ss = message.Split('@');
                        if (ss[0] == "web" && ss[1] == "1")
                        {
                            if (IsSDRconnect())
                            {
                                string[] tt = ss[2].Split('#');
                                d = int.Parse(tt[0]);
                                Msg1 = new double[d];
                                string[] rr = tt[1].Split(',');
                                for (int i = 0; i < d; ++i)
                                    Msg1[i] = double.Parse(rr[i]);
                                flag = int.Parse(tt[2]) == 1;

                                // socket.Send("server@1");
                                string text = "server@1";
                                log("send " + "\"" + text + "\"" + " to " + sdr.ip);
                                SendData(sdr, text);
                            }
                        }
                        if (ss[0] == "web" && ss[1] == "2")
                        {
                            if (IsSDRconnect())
                            {
                                if (IsTrainModelExists())
                                {
                                    string[] tt = ss[2].Split('#');
                                    d = int.Parse(tt[0]);
                                    Msg1 = new double[d];
                                    string[] rr = tt[1].Split(',');
                                    for (int i = 0; i < d; ++i)
                                        Msg1[i] = double.Parse(rr[i]);

                                    string text = "server@2";
                                    log("send " + "\"" + text + "\"" + " to " + sdr.ip);
                                    SendData(sdr, text);
                                }
                            }
                        }
                        if(ss[0]== "web" && ss[1]=="3")
                        {
                            if (IsFlagRAllowed())
                            {
                                IsflagRopen = false;
                                flagR = int.Parse(ss[2]) == 1;
                                validation v = new validation(validation_id++, d, Msg1, Msg2, flaglist, flagR);
                                Write(v);
                            }
                        }
                        else if(ss[0]=="web"&&ss[1]=="4")
                        {
                            if (ReadTrain())
                            {
                                TrianModelList(train_set);
                            }
                            else
                                log("train dataset is not enough");
                        }
                        else if(ss[0]=="web" && ss[1]=="5")
                        {
                            if (ReadValidation())  //改一下
                                ;// AnalystValidationData();
                            else
                                log("validation dataset is not enough");
                        }
                    };

                });
            Console.ReadLine();

        }

        public bool IsSDRconnect()
        {
            bool result = sdr != null;
            if (!result)
            {
                iwsc.Send("server@4");
            }
            return result;
        }

        public bool IsTrainModelExists()
        {
            bool result = tree != null;
            if(!result)
            {
                iwsc.Send("server@5");
            }
            return result;
        }

        public bool IsFlagRAllowed()
        {
            bool result = IsflagRopen;
            if(!result)
            {
                iwsc.Send("server@6");
            }
            return result;
        }

        public void endServer2()

        {
            server.Dispose();
        }
    }
}
