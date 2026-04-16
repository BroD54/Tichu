namespace Core.Commands
{
    public class CommandInvoker
    {
        public void ExecuteCommand(Command command)
        {
            command.Execute();
        }
    }
}