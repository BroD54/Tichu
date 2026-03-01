using System.Collections.Generic;

namespace Core.Combination
{
    using Card;

    public class FourKind : Combination
    {
        public override int Strength { get; }
        public override CombinationType Type => CombinationType.FourKind;
        public override bool IsBomb => true;

        public FourKind (List<Card> cards, int strength) : base(cards)
        {
            Strength = strength;
        }

        protected override bool BeatsSameType(Combination other)
        {
            var fourKind = (FourKind)other;
            
            return Strength > fourKind.Strength;
        }

        protected override bool BeatsBomb(Combination other)
        {
            return other is FourKind && BeatsSameType(other);
        }
    }
}