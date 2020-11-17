using System;
using System.Net.Sockets;

namespace Adventure.Core.Commands
{
    public class ClientConnectedCommand : ICommand
    {
        void ICommand.ExecuteClient(ICommandSender sender, Socket responseReceiver)
        {
            throw new NotImplementedException();
        }

        void ICommand.ExecuteServer(ICommandSender sender, Socket responseReceiver)
        {
            sender.Send(new PrintTextCommand("Hello traveler, what is your name?"), responseReceiver);
            sender.Send(new TextInputCommand(), responseReceiver);

        }
    }
}