using System.Net.Sockets;

namespace Adventure.Core.Commands
{
    public interface ICommand
    {
        void ExecuteClient(ICommandSender sender, Socket responseReceiver);

        void ExecuteServer(ICommandSender sender, Socket responseReceiver);
    }
}