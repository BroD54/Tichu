namespace Core.Commands
{
    using Player;
    using Game;
    using Round;
    using System.Collections.Generic;
    using Card;

    public class PlayCardsCommand : Command
    {
        private readonly Round _round;
        private readonly Move _move;

        public PlayCardsCommand(Round round, Move move)
        {
            _round = round;
            _move = move;
        }

        public override void Execute()
        {
            if (_round.CurrentTrick == null) return;

            bool accepted = _round.CurrentTrick.TryAddMove(_move);

            if (accepted && !_move.IsPass)
                _move.Player.RemoveCards(new List<Card>(_move.Combination.Cards));
        }
    }
}