
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Adventure.Server.Sockets
{

    public class SocketConnection
    {
        private Guid _id;
        private Socket _client;
        private SocketServer _server;
        private Thread _thread;

        public SocketConnection(Guid id, Socket client, SocketServer server)
        {
            this._id = id;
            _client = client;
            _server = server;
            _thread = new Thread(HandleConnection);
            _thread.Start();
        }

        private void HandleConnection()
        {
            try
            {
                while (true)
                {
                    var messageLengthBytes = new byte[4];
                    byte[] messageBytes = null;

                    var bytesRec = _client.Receive(messageLengthBytes);
                    var messageLength = BitConverter.ToInt32(messageLengthBytes);

                    if (bytesRec != 4)
                    {
                        Console.WriteLine($"Error receiving packet length. Expected 4, but got {bytesRec}.");
                    }
                    else
                    {

                        messageBytes = new byte[messageLength];
                        bytesRec = _client.Receive(messageBytes);
                        var data = Encoding.ASCII.GetString(messageBytes, 0, bytesRec);

                        _server.OnMessageRecieved(this, data);
                    }
                }
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode == SocketError.ConnectionReset)
                {
                    _server.OnDisconnect(this);
                    _thread.Interrupt();
                }
            }
        }

        public Guid GetID()
        {
            return _id;
        }

        public Socket GetClient()
        {
            return _client;
        }
    }

    public abstract class AsyncSocketServer : SocketServer
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
                    connection.ReceiveTimeout = -1;
                    var socketConnection = new SocketConnection(Guid.NewGuid(), connection, this);
                    connections.Add(socketConnection);
                    OnConnect(socketConnection);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }


    }
}