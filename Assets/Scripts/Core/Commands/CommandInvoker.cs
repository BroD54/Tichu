namespace Core.Commands
{
    public class CommandInvoker
    {
        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
        }
    }
}