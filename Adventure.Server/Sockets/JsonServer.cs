
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Newtonsoft.Json;
using Adventure.Core.Commands;

namespace Adventure.Server.Sockets
{
    public class JsonServer : AsyncSocketServer, ICommandSender
    {

        List<Socket> connections;

        public JsonServer()
        {
            connections = new List<Socket>();
        }

        private readonly JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };

        public void SendCommand(Socket connection, ICommand command)
        {
            var msg = JsonConvert.SerializeObject(command, settings);
            SendMessage(connection, msg);
        }

        public void BroadcastCommand(ICommand command)
        {
            var i = 1;
            foreach (var con in connections)
            {
                Console.WriteLine($"Sending Reply {i}..");
                SendCommand(con, command);
                i++;
            }
        }

        public void Send(ICommand command)
        {
            //TODO fix
            Console.WriteLine("Sending Replies..");
            BroadcastCommand(command);

        }

        protected override void OnMessageRecieved(Socket connection, string msg)
        {
            var cmd = JsonConvert.DeserializeObject(msg, settings);
            if (cmd != null)
            {

                ((ICommand)cmd).ExecuteServer(this);
            }
            else
            {
                Console.WriteLine("Cmd was null!");
            }
        }

        protected override void OnConnect(Socket connection)
        {
            Console.WriteLine("New Connection found!");
            connections.Add(connection);
            SendCommand(connection, new TextInputCommand("Moin was geht?"));
        }

        protected override void OnDisconnect(Socket connection)
        {
            Console.WriteLine("Client disconnected: " + connection.RemoteEndPoint.ToString());
        }

        protected override void OnError(string msg)
        {
            Console.WriteLine("OnError: " + msg);
        }
    }
}