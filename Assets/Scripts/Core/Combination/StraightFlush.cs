using System.Collections.Generic;

namespace Core.Combination
{
    using Card;

    public class StraightFlush : Combination
    {
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
            
            return Strength > straightFlush.Strength;
        }

        protected override bool BeatsBomb(Combination other)
        {
            return other is FourKind || BeatsSameType(other);
        }
    }
}