using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace SocketServer
{
    class Program
    {
        //本地ip地址
        private static string localHost = "127.0.0.1";
        //服务器对应的端口
        private static int myPort = 8888;
        //服务器socket实例
        private static Socket server;
        //接受client发送过来的数据
        private static byte[] result = new byte[1024];
        static void Main(string[] args)
        {
            /***启动服务端的监听***/
            IPAddress ip = IPAddress.Parse(localHost);
            IPEndPoint ipe = new IPEndPoint(ip, myPort);
            //创建socket实例
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //socket实例绑定对应ip以及端口
            server.Bind(ipe);
            //socket连接的最大容量
            server.Listen(10);
            Console.WriteLine("启动监听{0}成功",server.LocalEndPoint.ToString());
            /***开启一个线程与客户端建立连接***/
            Thread listenThread = new Thread(ClientConnect);
            listenThread.Start();//开启线程
        }
        /// <summary>
        /// 监听客户端连接请求
        /// </summary>
        private static void ClientConnect(){
            while (true)
            {
                //服务端~客户端尝试通讯
                Socket clientSocket = server.Accept();
                clientSocket.Send(Encoding.UTF8.GetBytes("Server say hello to Client..."));
                Console.WriteLine("Server send hello to Client...");
                //开启一个线程准备接受客户端消息
                Thread receiveThread = new Thread(ReceiveMessage);
                receiveThread.Start();
            }
        }
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="clientSocket"></param>
        private static void ReceiveMessage(object clientSocket)
        {
            Socket myClientSocket = (Socket)clientSocket;
            while (true)
            {
                try
                {
                    //通过clientSocket接收消息
                    int receiveNum = myClientSocket.Receive(result);
                    Console.WriteLine("服务器接收客户端{0}消息{1}",myClientSocket.RemoteEndPoint.ToString(),
                        Encoding.UTF8.GetString(result,0,receiveNum));

                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                    myClientSocket.Shutdown(SocketShutdown.Both);
                    myClientSocket.Close();
                    break;
                }
            }
        }
    }
}
