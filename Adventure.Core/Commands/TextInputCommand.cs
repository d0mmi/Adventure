using System;
using System.Net.Sockets;

namespace Adventure.Core.Commands
{
    public class TextInputCommand : ICommand
    {

        public string Response;
        public TextInputCommand()
        {
        }

        void ICommand.ExecuteClient(ICommandSender sender, Socket responseReceiver)
        {
            Response = Console.ReadLine();
            sender.Send(this, null);
        }

        void ICommand.ExecuteServer(ICommandSender sender, Socket responseReceiver)
        {
            throw new NotImplementedException();
        }

    }
}