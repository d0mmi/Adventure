
using System;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using Adventure.Core.Commands;

namespace Adventure.Client.Sockets
{
    class JsonClient : AsyncSocketClient, ICommandSender
    {

        private readonly JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };

        public void SendCommand(ICommand command)
        {
            var msg = JsonConvert.SerializeObject(command, settings);
            SendMessage(msg);
        }

        public void Send(ICommand command)
        {

        }

        public override void SendInitialMessage()
        {
            SendCommand(new ClientConnectedCommand());
        }

        protected override void OnMessageRecieved(string msg)
        {
            var cmd = JsonConvert.DeserializeObject<ICommand>(msg);
            cmd.ExecuteServer(this);
        }

        protected override void OnConnect(Socket connection)
        {
            Console.WriteLine("Connected!");
        }

        protected override void OnDisconnect(Socket connection)
        {

        }

        protected override void OnError(string msg)
        {

        }
    }
}