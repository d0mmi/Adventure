using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Adventure.Client
{
    public abstract class AlternateSocketClient
    {
        private Socket _sender;

        public void StartClient(string serverHostname, int serverPort)
        {
            try
            {
                var host = Dns.GetHostEntry(serverHostname);
                var ipAddress = host.AddressList[0];
                var remoteEP = new IPEndPoint(ipAddress, serverPort);
                _sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                _sender.Connect(remoteEP);
                _sender.ReceiveTimeout = -1;
                Console.WriteLine("Socket connected to {0}", _sender.RemoteEndPoint.ToString());

                SendInitialMessage();

                while (true)
                {
                    var messageLengthBytes = new byte[4];
                    byte[] messageBytes = null;

                    var bytesRec = _sender.Receive(messageLengthBytes);
                    var messageLength = BitConverter.ToInt32(messageLengthBytes);

                    if (bytesRec != 4)
                    {
                        Console.WriteLine($"Error receiving packet length. Expected 4, but got {bytesRec}.");
                    }

                    messageBytes = new byte[messageLength];
                    bytesRec = _sender.Receive(messageBytes);
                    var data = Encoding.ASCII.GetString(messageBytes, 0, bytesRec);

                    OnMessageReceived(_sender, data);
                }
            }
            catch (SocketException)
            {
                Console.WriteLine("Could not reach the game server. Did you try turning it on?");
            }
        }

        protected abstract void SendInitialMessage();

        protected abstract void OnMessageReceived(Socket socket, string message);

        protected void SendMessage(string message)
        {
            var responseBuffer = new byte[1024];
            var msg = (byte[])Encoding.ASCII.GetBytes(message);
            var messageLengthBytes = BitConverter.GetBytes(message.Length);

            _sender.Send(messageLengthBytes);
            _sender.Send(msg);
        }
    }
}