
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

        public void Send(ICommand command){

        }

        protected override void OnMessageRecieved(Socket connection, string msg)
        {
            var cmd = JsonConvert.DeserializeObject<ICommand>(msg);
            cmd.ExecuteServer(this);
        }

        protected override void OnConnect(Socket connection)
        {
            Console.WriteLine("New Connection found!");
        }

        protected override void OnDisconnect(Socket connection)
        {

        }

        protected override void OnError(string msg)
        {

        }
    }
}