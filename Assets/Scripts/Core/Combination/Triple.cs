using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Core.Combination
{
    using Card;

    public class Triple : Combination
    {
        private const int TripleCount = 3;
        public override int Strength { get; }
        public override CombinationType Type => CombinationType.Triple;


        public Triple (List<Card> cards, int strength) : base(cards)
        {
            Strength = strength;
        }
        
        [CanBeNull]
        public static Combination TryCreate(List<Card>  cards)
        {
            if (!CombinationHelpers.HasCount(cards, TripleCount)) return null;
            
            var first = cards[0];
            var second = cards[1];
            var third = cards[2];
            
            if (CombinationHelpers.ContainsIllegalSpecial(cards)) return null;
            if (CombinationHelpers.ContainsPhoenix(cards))
            {
                var isStandardCardsSameRank = first.Rank == second.Rank || first.Rank == third.Rank || second.Rank == third.Rank;
                if (!isStandardCardsSameRank)
                    return null;
            }
            else if (!CombinationHelpers.RanksEqual(cards)) return null;

            int strength;

            if (first.Rank != null) strength = (int)first.Rank.Value;
            else strength = (int)second.Rank!.Value;
            
            return new Triple(cards, strength);
        }
    }
}