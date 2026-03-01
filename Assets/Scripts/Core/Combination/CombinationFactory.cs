using System.Collections.Generic;
using JetBrains.Annotations;

namespace Core.Combination
{
    using Card;
    
    public static class CombinationFactory
    {
        [CanBeNull]
        public static Combination Create(List<Card> cards)
        {
            if (cards == null || cards.Count == 0) return null;
            
            return StraightFlush.TryCreate(cards)
                ?? FourKind.TryCreate(cards)
                ?? FullHouse.TryCreate(cards)
                ?? Straight.TryCreate(cards)
                ?? StraightPairs.TryCreate(cards)
                ?? Triple.TryCreate(cards)
                ?? Pair.TryCreate(cards)
                ?? Single.TryCreate(cards);
        }
    }
}