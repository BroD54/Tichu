using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Core.Combination
{
    using Card;

    public class Pair : Combination
    {
        private const int PairCount = 2;
        public override int Strength { get; }
        public override CombinationType Type => CombinationType.Pair;


        public Pair (List<Card> cards, int strength) : base(cards)
        {
            Strength = strength;
        }
        
        [CanBeNull]
        public static Combination TryCreate(List<Card>  cards)
        {
            if (!CombinationHelpers.HasCount(cards, PairCount)) return null;
            
            var first = cards[0];
            var second = cards[1];

            if (CombinationHelpers.ContainsIllegalSpecial(cards)) return null;
            if (!CombinationHelpers.ContainsPhoenix(cards) && !CombinationHelpers.RanksEqual(cards)) return null;
            
            int strength;

            if (first.Rank != null) strength = (int)first.Rank.Value;
            else strength = (int)second.Rank!.Value;
            
            return new Pair(cards, strength);
        }
    }
}