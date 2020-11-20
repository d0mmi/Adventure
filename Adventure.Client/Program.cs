using System;
using System.Net;
using Adventure.Client.Sockets;

namespace Adventure.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            JsonClient client = new JsonClient();
            client.StartClient(Dns.GetHostName(), 11000);
        }
    }


}
