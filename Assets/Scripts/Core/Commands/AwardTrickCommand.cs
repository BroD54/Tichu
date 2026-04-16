using System.Collections.Generic;

namespace Core.Commands
{
    using Player;
    using Card;
    using Game;
    using Round;
    
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