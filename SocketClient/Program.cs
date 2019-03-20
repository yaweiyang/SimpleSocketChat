using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;
using System.Net;
using System.Threading;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

using System.Runtime.Serialization.Json;

namespace SocketClient
{
    class Program
    {
        //本地ip地址
        private static string localHost = "192.168.1.123";//"127.0.0.1";
        //客户端需要访问对应的端口
        private static int myPort = 8888;
        //客户端接受服务器发送的消息
        private static byte[] result = new byte[4096];
        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse(localHost);
            IPEndPoint ipe = new IPEndPoint(ip, myPort);
            //创建socket实例
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientSocket.Connect(ipe);
                Console.WriteLine("连接服务器成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine("连接服务器失败 /n"+ex.Message);
                Console.ReadLine();
                return;
            }
            //尝试和服务器通讯
            try
            {
                //接收来自服务器的消息
                int receiveNum = clientSocket.Receive(result);
                Console.WriteLine("客户端接收服务器消息：{0}", Encoding.UTF8.GetString(result, 0, receiveNum));
                //客户端发送消息给服务器
                string sendMessage = "Clinet get message from server:" + DateTime.Now;

                //**方法一***//
                Dictionary<string,string> dic=new Dictionary<string,string>();
                dic.Add("name","yangyawei");
                dic.Add("age","18");
                string json = JsonConvert.SerializeObject(dic);
                //*****//
                //clientSocket.Send(Encoding.UTF8.GetBytes(sendMessage));
                clientSocket.Send(Encoding.UTF8.GetBytes(json));
                Console.WriteLine("Client send hello to Server...");
            }
            catch (Exception)
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            Console.WriteLine("发送完毕");
            Console.ReadLine();
        }
    }
}
