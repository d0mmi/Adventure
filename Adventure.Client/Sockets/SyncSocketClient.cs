using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Adventure.Client.Sockets
{
    class SyncSocketClient : SocketClient
    {

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
            sender.Connect(remoteEP);
            connection = sender;
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
                    while (true)
                    {
                        OnMessageRecieved(ReadMessage(connection));
                    }

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

        protected override void OnMessageRecieved(string msg)
        {
            Console.WriteLine("OnMessageRecieved: " + msg);
        }

        protected override void OnConnect(Socket connection){

        }

        protected override void OnDisconnect(Socket connection){
            
        }

        protected override void OnError(string msg){
            
        }
    }
}