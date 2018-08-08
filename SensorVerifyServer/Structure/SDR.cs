using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SensorVerifyServer
{
    public partial class MainWindow
    {
        public class Client
        {
            //用于保存与每个客户相关信息：套接字与接收缓存
            public Socket socket;
            public byte[] buffer = new byte[1024];
            public string ip;

            public Client(Socket socket)
            {
                this.socket = socket;
                ip = ((System.Net.IPEndPoint)(socket.RemoteEndPoint)).Address.ToString();
            }
            public Client(Client c)
            {
                this.socket = c.socket;
                for (int i = 0; i < 1024; ++i)
                    this.buffer[i] = c.buffer[i];
                this.ip = c.ip;
            }
            public void ClearBuffer()
            {
                buffer = new byte[1024];
            }
        }
        private List<string> listIP;

        TcpListener listener;
        List<string> listIP2 = new List<string>();


        // List<string> model_modify = new List<string>();//2017.7.10

        Client sdr;
        
        public List<string> getIP()
        {
            listIP = new List<string>();
            try
            {
                string HostName = Dns.GetHostName(); //得到主机名                
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        listIP.Add(IpEntry.AddressList[i].ToString());
                    }
                }
                return listIP;
            }
            catch (Exception e)
            {
                listIP.Clear();
                return listIP;
            }
        }

        public bool startServer(int port)
        {
          

            getIP2();

            // listIP[0] = ip;
            IPEndPoint localep = new IPEndPoint(IPAddress.Parse(ip), port);
     
            System.Console.WriteLine("ServerIP:" + listIP2[0]);
            listener = new TcpListener(localep);
            //启动对具有最大挂起连接数的传入连接请求的侦听。
            listener.Start(10);

            AsyncCallback callback = new AsyncCallback(AcceptCallBack);
            //开始一个异步操作来接受一个传入的连接尝试。BeginAcceptTcpClient时候编译器就会在线程池中创建一个线程监听连接请求。
            listener.BeginAcceptSocket(callback, listener);

            return true;
        }

        //监听连接请求的回调函数
        public void AcceptCallBack(IAsyncResult ar)
        {
            try
            {

                Socket s = listener.EndAcceptSocket(ar);
                Client client = new Client(s);

                log(client.ip + " is connecting");

                if(sdr==null)
                {
                    sdr = client;
                    Light(true, 3);
                }


                client.socket.BeginReceive(client.buffer, 0, client.buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), client);


                AsyncCallback callback;
                //继续调用异步方法接收连接请求

                callback = new AsyncCallback(AcceptCallBack);
                listener.BeginAcceptSocket(callback, listener);



            }
            catch (System.Net.Sockets.SocketException ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }

        public void ReceiveCallback(IAsyncResult ar)
        {
            //获取BeginReceive读取时传递的对象..读的内容都保存在那个对象里了..
            Client client = (Client)ar.AsyncState;
            try
            {
                int i = client.socket.EndReceive(ar);

                if (i == 0)
                {
                    //移除这个
                    //clientlist.Remove(client);
                    //moblielist.Remove(client);

                    sdr = null;
                    Light(false, 3);

                    log(client.ip + " has been disconnected");
                    
                    return;
                }
                else
                {

                    //真正读到的数据
                    string data = Encoding.UTF8.GetString(client.buffer, 0, i);

                    client.ClearBuffer();
                    AsyncCallback callback = new AsyncCallback(ReceiveCallback);
                    client.socket.BeginReceive(client.buffer, 0, client.buffer.Length, SocketFlags.None, callback, client);

                    operateModeA(client, data);
                }
            }
            catch (Exception e)
            {
            }
        }

        public static void DoAcceptSocketCallback(IAsyncResult ar)
        {
            // Get the listener that handles the client request.
            TcpListener listener = (TcpListener)ar.AsyncState;

            // End the operation and display the received data on the
            //console.
            Socket clientSocket = listener.EndAcceptSocket(ar);

            // Process the connection here. (Add the client to a 
            // server table, read data, etc.)
            Console.WriteLine("Client connected completed");

            // Signal the calling thread to continue.
            
        }

        public void SendData(Client client, string data)
        {
            try
            {
                //变成数组
                byte[] msg = Encoding.UTF8.GetBytes(data);
                //两行代码(无需清空缓存)
                AsyncCallback callback = new AsyncCallback(SendCallback);
                client.socket.BeginSend(msg, 0, msg.Length, SocketFlags.None, callback, client);
            }
            catch (Exception e)
            {
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            //结束方法,两行关键代码 1获取对象 2调用对象的socket的end
            Client client = (Client)ar.AsyncState;
            try
            {
                client.socket.EndSend(ar);
            }
            catch (Exception e)
            {
            }

        }

    
        public List<string> getIP2()
        {
            listIP2 = new List<string>();
            try
            {
                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        listIP2.Add(IpEntry.AddressList[i].ToString());
                    }
                }
                return listIP2;
            }
            catch (Exception e)
            {
                listIP2.Clear();
                return listIP2;
            }
        }
        
        public void operateModeA(Client client, String data)
        {
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

            string sendtoSDS4 = "server@Model4";// SDR is not connected
            string sendtoSDS5 = "server@Model5";//Model is not exist
            string sendtoSDS6 = "server@Model6";//flagR is not allowed

            log("get \"" + data + "\"" + " from " + client.ip);

            string[] ss = data.Split('@');
            
            if(ss[0]=="android"&&ss[1]=="1")
            {
                string[] tt = ss[2].Split('#');
                d = int.Parse(tt[0]);
                Msg2 = new double[d];
                string[] rr = tt[1].Split(',');
                for (int i = 0; i < d; ++i)
                    Msg2[i] = double.Parse(rr[i]);

                train t = new train(train_id++, d, Msg1, Msg2, flag);
                Write(t);
            }
            if (ss[0] == "android" && ss[1] == "2")
            {
                string[] tt = ss[2].Split('#');
                d = int.Parse(tt[0]);
                Msg2 = new double[d];
                string[] rr = tt[1].Split(',');
                for (int i = 0; i < d; ++i)
                    Msg2[i] = double.Parse(rr[i]);

                data validation_data = new data(d, Msg1, Msg2);
                List<bool> flagList = ClassifierList(validation_data);
                string flaglist = "";
                int flen = flagList.Count;
                for (int i = 0; i < flen; ++i)
                    if (i < flen - 1)
                        flaglist += flagList[i] + "#";
                    else
                        flaglist += flagList[i];
                this.flaglist = flaglist;
                string text = "server@3@" + flaglist;
                iwsc.Send(text);
                log("send " + "\"" + text + "\"" + " to " + iwsc.ConnectionInfo.ClientIpAddress);

                IsflagRopen = true;
            }


            // MessageBox.Show(data);
            if (client.ip != "192.168.1.108") //第二台有效坐标
            {

            }
            else if (client.ip == "192.168.1.108") //第一台有效坐标
            {


            }
        }
        
    }
}
