using System;
using Adventure.Server.Sockets;

namespace Adventure.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            SocketServer server = new JsonServer();
            server.Start();
        }
    }


}
