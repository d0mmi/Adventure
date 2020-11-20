
using System;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using Adventure.Core.Commands;

namespace Adventure.Client.Sockets
{
    class JsonClient : AlternateSocketClient, ICommandSender
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

        public void Send(ICommand command, Socket receiver)
        {
            SendCommand(command);
        }

        protected override void SendInitialMessage()
        {
            Console.WriteLine("SendInitialMessage");
            SendCommand(new ClientConnectedCommand());
        }

        protected override void OnMessageReceived(Socket socket, string message)
        {
            try
            {
                var cmd = JsonConvert.DeserializeObject(message, settings);
                if (cmd != null)
                {
                    ((ICommand)cmd).ExecuteClient(this, socket);
                }
                else
                {
                    Console.WriteLine("Cmd was null!");
                }
            }
            catch (JsonReaderException e)
            {
                Console.WriteLine("Error while deserializing Message: " + message);
            }
        }
    }
}