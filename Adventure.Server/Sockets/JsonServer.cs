
using System;
using System.Net.Sockets;
using Newtonsoft.Json;
using Adventure.Core.Commands;

namespace Adventure.Server.Sockets
{
    public class JsonServer : AsyncSocketServer, ICommandSender
    {

        private readonly JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };

        public void SendCommand(Socket connection, ICommand command)
        {
            var msg = JsonConvert.SerializeObject(command, settings);
            SendMessage(connection, msg);
        }

        public void SendCommand(SocketConnection connection, ICommand command)
        {
            SendCommand(connection.GetClient(), command);
        }

        public void BroadcastCommand(ICommand command)
        {
            var i = 1;
            foreach (var con in connections)
            {
                Console.WriteLine($"Sending Reply to connection: [{con.GetID()}]..");
                SendCommand(con, command);
                i++;
            }
        }

        public void Send(ICommand command, Socket receiver)
        {
            SendCommand(receiver, command);
        }

        public override void OnMessageRecieved(SocketConnection connection, string msg)
        {
            var cmd = JsonConvert.DeserializeObject(msg, settings);
            if (cmd != null)
            {

                ((ICommand)cmd).ExecuteServer(this, connection.GetClient());
            }
            else
            {
                Console.WriteLine("Cmd was null!");
            }
        }

        public override void OnConnect(SocketConnection connection)
        {
            Console.WriteLine("New Connection found: " + connection.GetID());
        }

        public override void OnDisconnect(SocketConnection connection)
        {
            Console.WriteLine($"Client [{connection.GetID()}] disconnected: " + connection.GetClient().RemoteEndPoint.ToString());
            connections.Remove(connection);
        }

        public override void OnError(string msg)
        {
            Console.WriteLine("OnError: " + msg);
        }
    }
}