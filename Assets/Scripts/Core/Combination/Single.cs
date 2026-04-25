using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Core.Combination
{
    using Card;

    public class Single : Combination
    {
        public override int Strength { get; }
        public override CombinationType Type => CombinationType.Single;
        
        private float? _phoenixStrength;

        public Single (List<Card> cards, int strength) : base(cards)
        {
            Strength = strength;
        }

        [CanBeNull]
        public static Combination TryCreate(List<Card> cards)
        {
            if (!CombinationHelpers.HasCount(cards, 1)) return null;
            
            var card = cards[0];

            switch (card.Type)
            {
                case CardType.Standard:
                case CardType.Mahjong:
                case CardType.Dragon:
                    if (card.Rank == null) return null;
                    return new Single(cards, (int)card.Rank.Value);
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
        
        public void CoverWith(int coveredStrength)
        {
            _phoenixStrength = coveredStrength + 0.5f;
        }
        public override bool Beats(Combination other)
        {
            if (other == null) return true;
            if (IsBomb && !other.IsBomb) return true;
            if (!IsBomb && other.IsBomb) return false;
            if (other.Type != CombinationType.Single) return false;

            bool iAmPhoenix     = Cards[0].IsPhoenix;
            bool otherIsDragon  = other.Cards[0].IsDragon;
            bool otherIsPhoenix = other.Cards[0].IsPhoenix;

            if (iAmPhoenix)    return !otherIsDragon;
            if (otherIsDragon) return false;

            if (otherIsPhoenix)
            {
                var otherSingle = (Single)other;
                if (otherSingle._phoenixStrength == null) return false;
                return Strength > otherSingle._phoenixStrength.Value;
            }

            return Strength > other.Strength;
        }
    }
}