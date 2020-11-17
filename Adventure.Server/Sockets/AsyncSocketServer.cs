
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

        public SocketConnection(Guid id, Socket client, SocketServer server)
        {
            this._id = id;
            _client = client;
            _server = server;
            var thread = new Thread(HandleConnection);
            thread.Start();
        }

        private void HandleConnection()
        {
            // Create the state object.  
            StateObject state = new StateObject();
            state.workSocket = _client;
            _client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        public Guid GetID()
        {
            return _id;
        }

        public Socket GetClient()
        {
            return _client;
        }
        public void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            try
            {
                // Read data from the client socket.
                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There  might be more data, so store the data received so far.  
                    state.sb.Append(Encoding.ASCII.GetString(
                        state.buffer, 0, bytesRead));

                    // Check for end-of-file tag. If it is not there, read
                    // more data.  
                    content = state.sb.ToString();
                    if (content.IndexOf("<EOF>") > -1)
                    {
                        // All the data has been read from the
                        // client. Display it on the console.  
                        _server.OnMessageRecieved(this, content.Substring(0, content.Length - 5));
                        StateObject newState = new StateObject();
                        newState.workSocket = handler;
                        handler.BeginReceive(newState.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), newState);
                    }
                    else
                    {
                        // Not all data received. Get more.  
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReadCallback), state);
                    }
                }
            }
            catch (SocketException e)
            {
                _server.OnError(e.Message);
                _server.OnDisconnect(this);
            }
        }
    }

    public class StateObject
    {
        // Size of receive buffer.  
        public const int BufferSize = 1024;

        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];

        // Received data string.
        public StringBuilder sb = new StringBuilder();

        // Client socket.
        public Socket workSocket = null;
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