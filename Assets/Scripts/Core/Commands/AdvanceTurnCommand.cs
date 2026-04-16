namespace Core.Commands
{
    using Round;

    public class AdvanceTurnCommand : Command
    {
        private readonly Round _round;

        public AdvanceTurnCommand(Round round)
        {
            _round = round;
        }

        public override void Execute()
        {
            _round.CurrentPlayerIndex = (_round.CurrentPlayerIndex + 1) % _round.Players.Count;
        }
    }
}