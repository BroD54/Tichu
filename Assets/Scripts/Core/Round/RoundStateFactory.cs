using System;
using System.Collections.Generic;

namespace Core.Round
{
    public class RoundStateFactory
    {

        public IRoundState Create(RoundPhase phase) => phase switch
        {
            RoundPhase.DealingFirstCards => new DealingFirstCardsState(),
            RoundPhase.GrandTichuCalls => new GrandTichuCallsState(),
            RoundPhase.DealingRemainingCards => new DealRemainingCardsState(),
            RoundPhase.CardExchange => new CardsExchangeState(),
            RoundPhase.Playing => new PlayingState(),
            RoundPhase.Scoring => new ScoringState(),
            RoundPhase.Finished => new FinishedState(),
            RoundPhase.Wish => throw new InvalidOperationException("Use CreateWish"),
            RoundPhase.DragonGift => throw new InvalidOperationException("Use CreateDragonGift"),
            _ => throw new ArgumentException()
        };
        public IRoundState CreateWish(int playerIndex) { return new DeclareWishState(playerIndex); }
        
        public IRoundState CreateDragonGift(Player.Player winner, List<Card.Card> trickCards)
            => new PassDragonState(winner, trickCards);
}
}