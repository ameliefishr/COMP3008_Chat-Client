using InterfaceLib;
using System;
using System.ServiceModel;

namespace ChatServer
{
    internal class Program
    {
        // program for our chat server
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Chat Server");


            //initialising server
            ServiceHost host;
            NetTcpBinding tcp = new NetTcpBinding();

            host = new ServiceHost(typeof(ChatServerImplement));
            host.AddServiceEndpoint(typeof(ChatServerInterface), tcp, "net.tcp://0.0.0.0:8100/ChatService");

            host.Open();
            Console.WriteLine("System Online");
            Console.ReadLine();
            host.Close();
        }
    }
}
