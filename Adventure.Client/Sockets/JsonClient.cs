
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
            SendCommand(command);
        }

        public override void SendInitialMessage()
        {
            SendCommand(new ClientConnectedCommand());
        }

        protected override void OnMessageRecieved(string msg)
        {
            Console.WriteLine("OnMessageRecieved");
            try
            {
                var cmd = JsonConvert.DeserializeObject(msg, settings);
                if (cmd != null)
                {
                    ((ICommand)cmd).ExecuteClient(this);
                }
                else
                {
                    Console.WriteLine("Cmd was null!");
                }
            }
            catch (JsonReaderException e)
            {
                OnError("Error while deserializing Message: " + msg);
            }
            Receive();
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
            Console.WriteLine("OnError: " + msg);
        }
    }
}