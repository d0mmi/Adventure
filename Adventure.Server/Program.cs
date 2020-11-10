using System;
using Adventure.Server.Sockets;

namespace Adventure.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            SocketServer server = new JsonServer();
            server.Start();
            //listener.StopListening();
        }
    }

    
}
