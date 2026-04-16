using System.Collections.Generic;

namespace Core.Commands
{
    using Player;
    using Card;

    public class ExchangeCardsCommand : Command
    {
        private readonly Player _from;
        private readonly Player _to;
        private readonly Card _card;

        public ExchangeCardsCommand(Player from, Player to, Card card)
        {
            _from = from;
            _to = to;
            _card = card;
        }

        public override void Execute()
        {
            _from.RemoveCards(new List<Card> { _card });
            _to.ReceiveCards(new List<Card> { _card });
        }
    }
}