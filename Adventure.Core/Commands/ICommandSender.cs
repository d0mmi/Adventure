using System.Net.Sockets;

namespace Adventure.Core.Commands
{
    public interface ICommandSender
    {
        void Send(ICommand command, Socket receiver);
    }
}