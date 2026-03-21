using System;

namespace Core.Round
{
    public static class RoundStateFactory
    {

        public static IRoundState Create(RoundPhase phase) => phase switch
        {
            RoundPhase.DealingFirstCards => new DealingFirstCardsState(),
            RoundPhase.GrandTichuCalls => new DealingFirstCardsState(),
            _ => throw new ArgumentException()
        };
}
}