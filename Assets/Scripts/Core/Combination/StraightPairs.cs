using System.Collections.Generic;

namespace Core.Combination
{
    using Card;

    public class StraightPairs : Combination
    {
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
    }
}