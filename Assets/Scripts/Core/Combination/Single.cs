using System.Collections.Generic;
using JetBrains.Annotations;

namespace Core.Combination
{
    using Card;

    public class Single : Combination
    {
        private const int SingleCount = 1;
        public override int Strength { get; }
        public override CombinationType Type => CombinationType.Single;


        public Single (List<Card> cards, int strength) : base(cards)
        {
            Strength = strength;
        }

        [CanBeNull]
        public static Combination TryCreate(List<Card> cards)
        {
            if (!CombinationHelpers.HasCount(cards, SingleCount)) return null;
            
            var card = cards[0];

            switch (card.Type)
            {
                case CardType.Standard:
                case CardType.Mahjong:
                case CardType.Dragon:
                    if (card.Rank == null) return null;

                    var strength = (int)card.Rank.Value;
                    return new Single(cards, strength);
                case CardType.Phoenix:
                    // strength contextual, handled by GameEngine
                    return new Single(cards, -1);
                case CardType.Dog:
                    // strength irrelevant
                    return new Single(cards, 0);
                default:
                    return null;
            }
        }
    }
}