using System;

namespace Core.Round
{
    public static class RoundStateFactory
    {

        public static IRoundState Create(RoundPhase phase) => phase switch
        {
            RoundPhase.DealingFirstCards => new DealingFirstCardsState(),
            RoundPhase.GrandTichuCalls => new GrandTichuCallsState(),
            RoundPhase.DealingRemainingCards => new DealRemainingCardsState(),
            RoundPhase.CardExchange => new CardsExchangeState(),
            RoundPhase.Playing => new PlayingState(),
            RoundPhase.Scoring => new ScoringState(),
            RoundPhase.Finished => new FinishedState(),
            _ => throw new ArgumentException()
        };
}
}