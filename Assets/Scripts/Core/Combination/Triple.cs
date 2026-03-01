using System.Collections.Generic;

namespace Core.Combination
{
    using Card;

    public class Triple : Combination
    {
        public override int Strength { get; }
        public override CombinationType Type => CombinationType.Triple;


        public Triple (List<Card> cards, int strength) : base(cards)
        {
            Strength = strength;
        }

        protected override bool BeatsSameType(Combination other)
        {
            var triple = (Triple)other;
            
            return Strength > triple.Strength;
        }
    }
}