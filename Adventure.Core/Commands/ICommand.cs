namespace Adventure.Core.Commands
{
    public interface ICommand
    {
        void ExecuteClient(ICommandSender sender);
        
         void ExecuteServer(ICommandSender sender);
    }
}