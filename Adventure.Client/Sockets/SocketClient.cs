using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Adventure.Client.Sockets
{
    abstract class SocketClient
    {
        protected Socket connection;

        public abstract void Start();

        protected string ReadMessage(Socket connection)
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

        public void Shutdown()
        {
            // Release the socket.  
            connection.Shutdown(SocketShutdown.Both);
            connection.Close();
            connection = null;
        }

        public void SendMessage(string msg)
        {
            // Encode the data string into a byte array.  
            byte[] msgBytes = Encoding.ASCII.GetBytes(msg + "<EOF>");

            // Send the data through the socket.
            connection.Send(msgBytes, 0, msgBytes.Length, SocketFlags.None);
        }


        public virtual void SendInitialMessage()
        {
            SendMessage("SendInitialMessage");
        }

        protected abstract void OnMessageRecieved(string msg);

        protected abstract void OnConnect(Socket connection);

        protected abstract void OnDisconnect(Socket connection);

        protected abstract void OnError(string msg);

        public virtual void Receive()
        {
            // Receive the response from the remote device.  
            byte[] bytes = new byte[1024];
            int bytesRec = connection.Receive(bytes);
            OnMessageRecieved(Encoding.ASCII.GetString(bytes, 0, bytesRec));
        }
    }
}