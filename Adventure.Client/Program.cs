using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Adventure.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            SynchronousSocketClient client = new SynchronousSocketClient();
            client.StartClient();
            while (true)
            {
                var msg = Console.ReadLine();
                client.SendMessage(msg, true);
                Console.WriteLine("Echoed test = {0}", client.ReceiveMessage());
            }
            client.StopClient();
        }
    }

    class SynchronousSocketClient
    {
        private Socket connection;

        public void StartClient()
        {
            if (connection != null)
            {
                StopClient();
            }
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];

            // Connect to a remote device.  
            try
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
                try
                {
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());
                    connection = sender;

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

        public void StopClient()
        {
            // Release the socket.  
            connection.Shutdown(SocketShutdown.Both);
            connection.Close();
            connection = null;
        }

        public void SendMessage(string message, bool eof)
        {
            // Encode the data string into a byte array.  
            byte[] msg = Encoding.ASCII.GetBytes(message + (eof ? "<EOF>" : ""));

            // Send the data through the socket.  
            int bytesSent = connection.Send(msg);
        }

        public string ReceiveMessage()
        {
            // Receive the response from the remote device.  
            byte[] bytes = new byte[1024];
            int bytesRec = connection.Receive(bytes);
            return Encoding.ASCII.GetString(bytes, 0, bytesRec);
        }
    }
}
