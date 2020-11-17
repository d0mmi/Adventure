
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;

namespace Adventure.Server.Sockets
{

    public abstract class SocketServer
    {

        protected List<SocketConnection> connections;



        public SocketServer()
        {
            connections = new List<SocketConnection>();
        }

        public abstract void Start();

        protected Socket Connect()
        {
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
            listener.Bind(localEndPoint);
            listener.Listen(10);

            return listener;
        }

        public void SendMessage(SocketConnection connection, string msg)
        {
            SendMessage(connection.GetClient(), msg);
        }

        public void SendMessage(Socket connection, string msg)
        {
            byte[] msgBytes = Encoding.ASCII.GetBytes(msg + "<EOF>");

            connection.Send(msgBytes, 0, msgBytes.Length, SocketFlags.None);
        }

        public abstract void OnMessageRecieved(SocketConnection connection, string msg);

        public abstract void OnConnect(SocketConnection connection);

        public abstract void OnDisconnect(SocketConnection connection);

        public abstract void OnError(string msg);

        protected string ReadMessage(SocketConnection connection)
        {
            // Data buffer for incoming data.  
            byte[] bytes = new Byte[1024];
            var data = "";
            while (true)
            {
                int bytesRec = connection.GetClient().Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                if (data.IndexOf("<EOF>") > -1)
                {
                    return data;
                }
            }
        }

        public void Shutdown()
        {
            /* TODO
            connection.Shutdown(SocketShutdown.Both);
            connection.Close();
            connection = null;
            */
        }
    }
}