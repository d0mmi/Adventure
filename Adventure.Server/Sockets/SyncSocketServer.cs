
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Adventure.Server.Sockets
{
    class SyncSocketServer : SocketServer
    {
        public override void Start()
        {
            try
            {
                var listener = Connect();
                // Start listening for connections.  
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.  
                    var connection = listener.Accept();
                    OnConnect(connection);
                    // An incoming connection needs to be processed.  
                    while (true)
                    {
                        OnMessageRecieved(connection, ReadMessage(connection));
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }

        protected override void OnMessageRecieved(Socket connection, string msg)
        {
            Console.WriteLine("OnMessageRecieved: " + msg);
            SendMessage(connection, "Echo: " + msg);
        }

        protected override void OnConnect(Socket connection){
            Console.WriteLine("New Connection found!");
        }

        protected override void OnDisconnect(Socket connection){
            
        }

        protected override void OnError(string msg){
            
        }
    }
}