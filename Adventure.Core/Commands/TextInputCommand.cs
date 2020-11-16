using System;

namespace Adventure.Core.Commands
{
    public class TextInputCommand : ICommand
    {
        
        public string text;
        public TextInputCommand(string text)
        {
            this.text = text;
        }
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
            Console.WriteLine(text + ":");
            var msg = Console.ReadLine();
            sender.Send(new PrintTextCommand(msg));
        }

    }
}