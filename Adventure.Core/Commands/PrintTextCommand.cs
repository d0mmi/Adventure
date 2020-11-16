using System;

namespace Adventure.Core.Commands
{
    public class PrintTextCommand : ICommand
    {
        public string text;
        public PrintTextCommand(string text)
        {
            this.text = text;
        }

        void ICommand.ExecuteClient(ICommandSender sender)
        {
            Console.WriteLine("Client: " + text);
        }

        void ICommand.ExecuteServer(ICommandSender sender)
        {
            Console.WriteLine("Server: " + text);
        }
    }
}