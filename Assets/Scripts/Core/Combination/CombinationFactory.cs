using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Core.Combination
{
    using Card;
    
    public static class CombinationFactory
    {
        [CanBeNull]
        public static Combination Create(List<Card> cards)
        {
            if (cards == null || cards.Count == 0) return null;
            
            cards = cards.OrderBy(c => c.Rank).ToList();

            if (IsSingle(cards)) return CreateSingle(cards);
            if (IsPair(cards)) return CreatePair(cards);
            if (IsTriple(cards)) return CreateTriple(cards);

            return null;
        }

        private static bool IsSingle(List<Card> cards)
        {
            return cards.Count == 1;
        }

        [CanBeNull]
        private static Combination CreateSingle(List<Card>  cards)
        {
            if (cards.Count != 1) return null;
            
            var card = cards[0];

            switch (card.Type)
            {
                case CardType.Standard:
                case CardType.Mahjong:
                case CardType.Dragon:
                    if (card.Rank == null) return null;

                    var strength = (int)card.Rank.Value;
                    return new Combination(CombinationType.Single, cards, strength);
                case CardType.Phoenix:
                    // strength contextual, handled by GameEngine
                    return new Combination(CombinationType.Single, cards, -1);
                case CardType.Dog:
                    // strength irrelevant
                    return new Combination(CombinationType.Single, cards, 0);
                default:
                    return null;
            }
        }
        
        private static bool IsPair(List<Card> cards)
        {
            if (cards.Count != 2) return false;

            var first = cards[0];
            var second = cards[1];
            
            if (cards.Any(card => card.IsDragon || card.IsDog || card.IsMahjong)) return false;
            if (cards.Any(card => card.IsPhoenix)) return true;

            return first.Rank == second.Rank;
        }

        [CanBeNull]
        private static Combination CreatePair(List<Card>  cards)
        {
            if (cards.Count != 2) return null;
            
            var first = cards[0];
            var second = cards[1];

            int strength;

            if (first.Rank != null) strength = (int)first.Rank.Value;
            else strength = (int)second.Rank.Value;
            
            return new Combination(CombinationType.Pair, cards, strength);
        }
        
        private static bool IsTriple(List<Card> cards)
        {
            if (cards.Count != 3) return false;

            var first = cards[0];
            var second = cards[1];
            var third = cards[2];
            
            if (cards.Any(card => card.IsDragon || card.IsDog || card.IsMahjong)) return false;
            if (cards.Any(card => card.IsPhoenix))
            {
                return first.Rank == second.Rank || first.Rank == third.Rank || second.Rank == third.Rank;
            }

            return first.Rank == second.Rank && first.Rank == third.Rank;
        }
        
        [CanBeNull]
        private static Combination CreateTriple(List<Card>  cards)
        {
            if (cards.Count != 3) return null;
            
            var first = cards[0];
            var second = cards[1];
            var third = cards[2];

            int strength;

            if (first.Rank != null) strength = (int)first.Rank.Value;
            else strength = (int)second.Rank.Value;
            
            return new Combination(CombinationType.Triple, cards, strength);
        }
        
    }
}