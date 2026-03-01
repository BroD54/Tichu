using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Core.Combination
{
    using Card;

    public class FullHouse : Combination
    {
        private const int SingleCount = 1;
        private const int PairCount = 2;
        private const int TripleCount = 3;
        private const int FullHouseCount = 5;
        public override int Strength { get; }
        public override CombinationType Type => CombinationType.FullHouse;


        public FullHouse (List<Card> cards, int strength) : base(cards)
        {
            Strength = strength;
        }

        [CanBeNull]
        public static Combination TryCreate(List<Card> cards)
        {
            if (!CombinationHelpers.HasCount(cards, FullHouseCount)) return null;
            if (CombinationHelpers.ContainsIllegalSpecial(cards)) return null;
            
            var hasPhoenix = CombinationHelpers.ContainsPhoenix(cards);
            var standardCards = CombinationHelpers.GetStandardCards(cards);
            var groups = CombinationHelpers.GroupByRank(standardCards);

            var tripleRank = ResolveTripleRank(groups, hasPhoenix);
            if (tripleRank == null) return null;
            
            return new FullHouse(cards, (int)tripleRank);
        }

        private static Rank? ResolveTripleRank(List<(Rank Rank, int Count)> groups, bool hasPhoenix)
        {
            var expectedGroupCount = 2;
            if (groups.Count != expectedGroupCount) return null;

            if (!hasPhoenix)
            {
                var isThreePlusTwo = groups[0].Count == TripleCount &&
                                     groups[1].Count == PairCount;
                if (isThreePlusTwo)
                {
                    return groups[0].Rank;
                }

                return null;
            }

            var isThreePlusOne = groups[0].Count == TripleCount && groups[1].Count == SingleCount;
            var isTwoPlusTwo = groups[0].Count == PairCount && groups[1].Count == PairCount;

            if (isThreePlusOne || isTwoPlusTwo) 
                return groups[0].Rank;

            return null;
        }
    }
}