using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Core.Combination
{
    using Card;

    public class StraightFlush : Combination
    {
        private const int MinimumLength = 5;
        public override int Strength { get; }
        public override CombinationType Type => CombinationType.StraightFlush;
        public override bool IsBomb => true;

        public StraightFlush (List<Card> cards, int strength) : base(cards)
        {
            Strength = strength;
        }

        protected override bool BeatsSameType(Combination other)
        {
            var straightFlush = (StraightFlush)other;

            if (Strength == straightFlush.Strength)
            {
                return Cards.Count > straightFlush.Cards.Count;
            }
            
            return Strength > straightFlush.Strength;
        }

        protected override bool BeatsBomb(Combination other)
        {
            return other is FourKind || BeatsSameType(other);
        }
        
        [CanBeNull]
        public static Combination TryCreate(List<Card> cards)
        {
            if (cards.Count < MinimumLength) return null;
            if (CombinationHelpers.ContainsIllegalSpecial(cards)) return null;

            var hasPhoenix = CombinationHelpers.ContainsPhoenix(cards);
            if (hasPhoenix) return null;
            if (!CombinationHelpers.IsStraight(cards, hasPhoenix)) return null;
            
            var suit = cards[0].Suit;
            if (cards.Any(card => card.Suit != suit)) return null;
            
            List<int> orderedRanks = CombinationHelpers.OrderByRank(cards, descending:true);

            int strength = orderedRanks[0];
            
            
            return new StraightFlush(cards, strength);
        }
    }
}