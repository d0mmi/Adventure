using System;

namespace Adventure.Core.Commands
{
    public class ClientConnectedCommand : ICommand
    {
        void ICommand.ExecuteClient(ICommandSender sender)
        {
            
        }

        void ICommand.ExecuteServer(ICommandSender sender)
        {
            Console.WriteLine("Server: New Client Connected!");
        }
    }
}