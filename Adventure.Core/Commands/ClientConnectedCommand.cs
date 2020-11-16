using System;

namespace Adventure.Core.Commands
{
    public class ClientConnectedCommand : ICommand
    {
        void ICommand.ExecuteClient(ICommandSender sender)
        {
            Console.WriteLine("Client: Server approved  new Connection!");
            
        }

        void ICommand.ExecuteServer(ICommandSender sender)
        {
            Console.WriteLine("Server: New Client Connected!");
            sender.Send(new ClientConnectedCommand());
        }
    }
}