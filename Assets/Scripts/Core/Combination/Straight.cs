using System.Collections.Generic;

namespace Core.Combination
{
    using Card;

    public class Straight : Combination
    {
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
    }
}