using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Core.Combination
{
    using Card;

    public class Straight : Combination
    {
        private const int MinimumLength = 5;
        public override int Strength { get; }
        public override CombinationType Type => CombinationType.Straight;


        public Straight (List<Card> cards, int strength) : base(cards)
        {
            Strength = strength;
        }

        protected override bool BeatsSameType(Combination other)
        {
            var straight = (Straight)other;

            if (Cards.Count != straight.Cards.Count) return false;
            
            return Strength > straight.Strength;
        }

        [CanBeNull]
        public static Combination TryCreate(List<Card> cards)
        {
            if (cards.Count < MinimumLength) return null;
            if (cards.Any(card => card.Type is CardType.Dog or CardType.Dragon)) return null;
            
            var standardCards = cards.Where(card => card.Type is not CardType.Phoenix).ToList();
            var hasPhoenix = CombinationHelpers.ContainsPhoenix(cards);
            if (!CombinationHelpers.IsStraight(standardCards, hasPhoenix)) return null;
            
            List<int> orderedRanks = CombinationHelpers.OrderByRank(standardCards);

            int strength = orderedRanks[^1];
            if (hasPhoenix)
            {
                int totalGaps = CombinationHelpers.TotalGaps(orderedRanks);
                if (totalGaps == 0) strength = Math.Min(strength + 1, (int)Rank.Ace);
            }

            return new Straight(cards, strength);
        }
    }
}