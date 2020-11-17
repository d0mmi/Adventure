using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Adventure.Client.Sockets
{
    // State object for receiving data from remote device.  
    public class StateObject
    {
        // Client socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 256;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.  
        public StringBuilder sb = new StringBuilder();
    }

    abstract class AsyncSocketClient : SocketClient
    {
        // ManualResetEvent instances signal completion.  
        private ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private ManualResetEvent receiveDone =
            new ManualResetEvent(false);




        protected void Connect()
        {
            // Establish the remote endpoint for the socket.  
            // This example uses port 11000 on the local computer.  
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP  socket.  
            Socket sender = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Connect the socket to the remote endpoint. Catch any errors.  
            sender.BeginConnect(remoteEP,
                new AsyncCallback(ConnectCallback), sender);
            connectDone.WaitOne();
            connection = sender;
        }

        protected void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.  
                client.EndConnect(ar);

                OnConnect(client);

                // Signal that the connection has been made.  
                connectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public override void Start()
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];

            // Connect to a remote device.  
            try
            {
                Connect();
                try
                {

                    Console.WriteLine("Socket connected to {0}", connection.RemoteEndPoint.ToString());
                    SendInitialMessage();
                    Receive();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public override void Receive()
        {
            Console.WriteLine("Start Receiving..");
            try
            {
                // Create the state object.  
                StateObject state = new StateObject();
                state.workSocket = connection;

                // Begin receiving the data from the remote device.  
                connection.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        protected void ReceiveCallback(IAsyncResult ar)
        {
            String content = String.Empty;
            try
            {
                // Retrieve the state object and the client socket
                // from the asynchronous state object.  
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.  
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.  
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));


                    // Check for end-of-file tag. If it is not there, read
                    // more data.  
                    content = state.sb.ToString();
                    if (content.IndexOf("<EOF>") > -1)
                    {
                        // All the data has been read from the
                        // client. Display it on the console.  
                        content = content.Substring(0, content.Length - 5);
                        if (content.IndexOf("<EOF>") > -1)
                        {
                            Console.WriteLine("Recieved multiple Messages:");
                            foreach (var msg in content.Split("<EOF>"))
                            {
                                OnMessageRecieved(msg);
                            }
                        }
                        else
                        {
                            OnMessageRecieved(content);
                        }
                        //Receive();
                    }
                    else
                    {
                        // Not all data received. Get more.  
                        client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


    }
}