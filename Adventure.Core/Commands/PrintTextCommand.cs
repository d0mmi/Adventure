using System;
using System.Net.Sockets;

namespace Adventure.Core.Commands
{
    public class PrintTextCommand : ICommand
    {
        public string Text;
        public PrintTextCommand(string text)
        {
            this.Text = text;
        }

        void ICommand.ExecuteClient(ICommandSender sender, Socket responseReceiver)
        {
            Console.WriteLine(Text);
        }

        void ICommand.ExecuteServer(ICommandSender sender, Socket responseReceiver)
        {
            Console.WriteLine(Text);
        }
    }
}