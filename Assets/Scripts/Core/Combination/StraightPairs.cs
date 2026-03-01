using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Core.Combination
{
    using Card;

    public class StraightPairs : Combination
    {
        private const int StraightPairsMinimum = 2;
        public override int Strength { get; }
        public override CombinationType Type => CombinationType.StraightPairs;


        public StraightPairs (List<Card> cards, int strength) : base(cards)
        {
            Strength = strength;
        }

        protected override bool BeatsSameType(Combination other)
        {
            var straightPairs = (StraightPairs)other;

            if (Cards.Count != straightPairs.Cards.Count) return false;
            
            return Strength > straightPairs.Strength;
        }
        
        [CanBeNull]
        public static StraightPairs TryCreate(List<Card> cards)
        {
            if (cards.Count % 2 != 0) return null;
            if (CombinationHelpers.ContainsIllegalSpecial(cards)) return null;
            
            var standardCards = CombinationHelpers.GetStandardCards(cards);
            var orderedRanks = CombinationHelpers.OrderByRank(standardCards);
            var expectedPairs = (int)Math.Ceiling(standardCards.Count / 2.0);

            if (orderedRanks.Count != expectedPairs) return null;
            if (expectedPairs < StraightPairsMinimum) return null;
            
            var totalGaps = CombinationHelpers.TotalGaps(orderedRanks);
            
            if (totalGaps > 0) return null;
            
            var incompletePairs = (standardCards.Count) - (expectedPairs * 2);
            var phoenixCount = CombinationHelpers.ContainsPhoenix(cards) ? 1 : 0;

            if (incompletePairs > phoenixCount) return null;

            var strength = orderedRanks[^1];
            
            return new StraightPairs(cards, strength);
        }
    }
}