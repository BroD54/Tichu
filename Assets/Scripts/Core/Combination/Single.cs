using System.Collections.Generic;

namespace Core.Combination
{
    using Card;

    public class Single : Combination
    {
        public override int Strength { get; }
        public override CombinationType Type => CombinationType.Single;


        public Single (List<Card> cards, int strength) : base(cards)
        {
            Strength = strength;
        }

        protected override bool BeatsSameType(Combination other)
        {
            var single = (Single)other;
            
            return Strength > single.Strength;
        }
    }
}