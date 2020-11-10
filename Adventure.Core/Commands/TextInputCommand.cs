using System;

namespace Adventure.Core.Commands
{
    public class TextInputCommand : ICommand
    {
        void ICommand.ExecuteClient(ICommandSender sender)
        {
            SendInput(sender);
        }

        void ICommand.ExecuteServer(ICommandSender sender)
        {
            SendInput(sender);
        }

        protected void SendInput(ICommandSender sender)
        {
            var msg = Console.ReadLine();
            sender.Send(new PrintTextCommand(msg));
        }

    }
}