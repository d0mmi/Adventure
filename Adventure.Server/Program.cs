using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Adventure.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            SynchronousSocketListener listener = new SynchronousSocketListener();
            listener.StartListening();
            //listener.StopListening();
        }
    }

    class SynchronousSocketListener
    {
        private Socket connection;
        public void StartListening()
        {
            if (connection != null)
            {
                StopListening();
            }

            // Establish the local endpoint for the socket.  
            // Dns.GetHostName returns the name of the
            // host running the application.  
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and
            // listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                // Start listening for connections.  
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.  
                    connection = listener.Accept();

                    // An incoming connection needs to be processed.  
                    while (true)
                    {
                        // Show the data on the console.  
                        var data = Listen();
                        Console.WriteLine("Text received : {0}", data);


                        // Echo the data back to the client.  
                        byte[] msg = Encoding.ASCII.GetBytes(data);

                        connection.Send(msg);
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

        private string Listen()
        {
            // Data buffer for incoming data.  
            byte[] bytes = new Byte[1024];
            var data = "";
            while (true)
            {
                int bytesRec = connection.Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                if (data.IndexOf("<EOF>") > -1)
                {
                    return data;
                }
            }
        }

        public void StopListening()
        {

            connection.Shutdown(SocketShutdown.Both);
            connection.Close();
            connection = null;
        }
    }
}
