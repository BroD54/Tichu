using System.Collections.Generic;
using JetBrains.Annotations;

namespace Core.Combination
{
    using Card;

    public class FourKind : Combination
    {
        private const int FourKindCount = 4;
        public override int Strength { get; }
        public override CombinationType Type => CombinationType.FourKind;
        public override bool IsBomb => true;

        public FourKind (List<Card> cards, int strength) : base(cards)
        {
            Strength = strength;
        }

        protected override bool BeatsBomb(Combination other)
        {
            return other is FourKind && BeatsSameType(other);
        }

        [CanBeNull]
        public static Combination TryCreate(List<Card> cards)
        {
            if (!CombinationHelpers.HasCount(cards, FourKindCount)) return null;
            if (CombinationHelpers.ContainsIllegalSpecial(cards)) return null;
            if (CombinationHelpers.ContainsPhoenix(cards)) return null;
            if (!CombinationHelpers.RanksEqual(cards)) return null;
            
            var strength = (int)cards[0].Rank!;
            return new FourKind(cards, strength);
        }
    }
}