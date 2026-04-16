using System.Collections.Generic;

namespace Core.Commands
{
    using Player;
    using Card;
    using Game;
    using Round;

    /// <summary>
    /// Gives the completed trick's cards to the recipient and clears CurrentTrick.
    /// For normal tricks, recipient == CurrentWinner.
    /// For Dragon tricks, recipient is the opponent chosen by the winning player.
    /// Requires Trick.Moves to be exposed: public IReadOnlyList<Move> Moves => _moves.AsReadOnly();
    /// </summary>
    public class AwardTrickCommand : Command
    {
        private readonly Round _round;
        private readonly Player _recipient;
        private readonly Trick _trick;

        public AwardTrickCommand(Round round, Player recipient, Trick trick)
        {
            _round = round;
            _recipient = recipient;
            _trick = trick;
        }

        public override void Execute()
        {
            var trickCards = new List<Card>();
            foreach (var move in _trick.Moves)
            {
                if (!move.IsPass)
                    trickCards.AddRange(move.Combination.Cards);
            }

            _recipient.TricksWon.Add(trickCards);
            _round.CurrentTrick = null;
        }
    }
}