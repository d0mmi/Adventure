using System;
using Adventure.Client.Sockets;
using Adventure.Core.Commands;

namespace Adventure.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            JsonClient client = new JsonClient();
            client.Start();
            client.SendCommand(new PrintTextCommand("PrintTextCommand test!!!"));
            while (true)
            {
                /*
                Console.WriteLine("Message:");
                var msg = Console.ReadLine();
                client.SendMessage(msg);
                client.Receive();
                */
            }
        }
    }


}
