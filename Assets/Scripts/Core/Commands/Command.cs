namespace Core.Commands
{
    public abstract class Command : ICommand
    {
        public abstract void Execute();
    }
}