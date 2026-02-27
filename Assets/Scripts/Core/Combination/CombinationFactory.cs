using System;
using System.Collections.Generic;
using System.Linq;
using Codice.CM.Common;
using JetBrains.Annotations;

namespace Core.Combination
{
    using Card;
    
    public static class CombinationFactory
    {
        private const int SINGLE_COUNT = 1;
        private const int PAIR_COUNT = 2;
        private const int TRIPLE_COUNT = 3;
        private const int FULL_HOUSE_COUNT = 5;
        private const int STRAIGHT_MINIMIMUM_COUNT = 5;
        private const int STRAIGHT_PAIRS_MINIMUM_COUNT = 2;
        private const int FOUR_KIND_COUNT = 4;
        
        [CanBeNull]
        public static Combination Create(List<Card> cards)
        {
            if (cards == null || cards.Count == 0) return null;
            
            cards = cards.OrderBy(c => c.Rank).ToList();

            if (IsSingle(cards)) return CreateSingle(cards);
            if (IsPair(cards)) return CreatePair(cards);
            if (IsTriple(cards)) return CreateTriple(cards);
            if (IsFullHouse(cards)) return CreateFullHouse(cards);
            if (IsStraightFlush(cards)) return CreateStraightFlush(cards);
            if (IsStraight(cards)) return CreateStraight(cards);
            if (IsStraightPairs(cards)) return CreateStraightPairs(cards);
            if (IsFourKind(cards)) return CreateFourKind(cards);
                
            return null;
        }

        private static bool IsSingle(List<Card> cards)
        {
            return cards.Count == SINGLE_COUNT;
        }

        [CanBeNull]
        private static Combination CreateSingle(List<Card>  cards)
        {
            if (cards.Count != SINGLE_COUNT) return null;
            
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
            if (cards.Count != PAIR_COUNT) return false;

            var first = cards[0];
            var second = cards[1];
            
            if (cards.Any(card => card.IsDragon || card.IsDog || card.IsMahjong)) return false;
            if (cards.Any(card => card.IsPhoenix)) return true;

            return first.Rank == second.Rank;
        }

        [CanBeNull]
        private static Combination CreatePair(List<Card>  cards)
        {
            if (cards.Count != PAIR_COUNT) return null;
            
            var first = cards[0];
            var second = cards[1];

            int strength;

            if (first.Rank != null) strength = (int)first.Rank.Value;
            else strength = (int)second.Rank.Value;
            
            return new Combination(CombinationType.Pair, cards, strength);
        }
        
        private static bool IsTriple(List<Card> cards)
        {
            if (cards.Count != TRIPLE_COUNT) return false;

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
            if (cards.Count != TRIPLE_COUNT) return null;
            
            var first = cards[0];
            var second = cards[1];

            int strength;

            if (first.Rank != null) strength = (int)first.Rank.Value;
            else strength = (int)second.Rank.Value;
            
            return new Combination(CombinationType.Triple, cards, strength);
        }
        
        private static bool IsFullHouse(List<Card> cards)
        {
            if (cards.Count != FULL_HOUSE_COUNT) return false;
            
            if (cards.Any(card => card.IsDragon || card.IsDog || card.IsMahjong)) return false;

            var standardCards = cards.Where(card => card.Type == CardType.Standard).ToList();
            var groups = standardCards
                .GroupBy(card => card.Rank)
                .Select(group => group.Count())
                .OrderByDescending(count => count)
                .ToList();

            if (!cards.Any(card => card.IsPhoenix))
            {
                return groups.SequenceEqual(new List<int> {TRIPLE_COUNT, PAIR_COUNT});
            }

            var fullHouseGroupCount = 2;
            
            if (groups.Count == fullHouseGroupCount)
            {
                if (groups[0] == TRIPLE_COUNT && groups[1] == SINGLE_COUNT) return true;
                if (groups[0] == PAIR_COUNT && groups[1] == PAIR_COUNT) return true;
            }

            return false;
        }

        [CanBeNull]
        private static Combination CreateFullHouse(List<Card> cards)
        {
            var standardCards = cards.Where(card => card.Type == CardType.Standard);
            var containsPhoenix = cards.Any(card => card.IsPhoenix);

            var groups = standardCards
                .GroupBy(card => card.Rank)
                .OrderByDescending(group => group.Key!.Value)
                .ToList();

            Rank tripleRank;
            if (!containsPhoenix)
            {
                tripleRank = groups.First(group => group.Count() == TRIPLE_COUNT).Key!.Value;
            }
            else
            {
                tripleRank = groups[0].Key!.Value;
            }

            int strength = (int)tripleRank;
            
            return new Combination(CombinationType.FullHouse, cards, strength);
        }
        
        private static bool IsStraight(List<Card> cards)
        {
            if (cards.Count < STRAIGHT_MINIMIMUM_COUNT) return false;
            
            if (cards.Any(card => card.IsDragon || card.IsDog)) return false;

            var straightCards = cards.Where(card => card.Type is CardType.Standard or CardType.Mahjong).ToList();
            var orderedRanks = straightCards
                .GroupBy(card => card.Rank)
                .Select(group => group.Key!.Value)
                .OrderBy(rank => rank)
                .ToList();

            if (orderedRanks.Count != straightCards.Count) return false;
            
            int totalGaps = 0;
            for(int i = 1; i < orderedRanks.Count; i++)
            {
                totalGaps += orderedRanks[i] - orderedRanks[i - 1] - 1;
            }

            var phoenixCount = cards.Any(card => card.IsPhoenix) ? 1 : 0;
            return totalGaps <= phoenixCount;
        }

        [CanBeNull]
        private static Combination CreateStraight(List<Card> cards)
        {
            var standardCards = cards.Where(card => card.Type == CardType.Standard).ToList();
            var containsPhoenix = cards.Any(card => card.IsPhoenix);

            var orderedRanks = standardCards
                .GroupBy(card => card.Rank)
                .Select(group => group.Key!.Value)
                .OrderByDescending(rank => rank)
                .ToList();

            int strength;
            
            if (!containsPhoenix)
            {
                strength = (int)orderedRanks[0];
            }
            else
            {
                int totalGaps = 0;
                for(int i = 1; i < orderedRanks.Count; i++)
                {
                    totalGaps += orderedRanks[i - 1] - orderedRanks[i] - 1;
                }

                bool canPhoenixExtendStraight = totalGaps == 0;
                if (canPhoenixExtendStraight)
                {
                    strength = Math.Min((int)orderedRanks[0] + 1, (int)Rank.Ace);
                }
                else
                {
                    strength = (int)orderedRanks[0];
                }
            }
            
            return new Combination(CombinationType.Straight, cards, strength);
        }

        private static bool IsStraightPairs(List<Card> cards)
        {
            if (cards.Count % 2 != 0) return false;
            
            if (cards.Any(card => card.IsDragon || card.IsDog || card.IsMahjong)) return false;

            var standardCards = cards.Where(card => card.Type == CardType.Standard).ToList();
            var orderedRanks = standardCards
                .GroupBy(card => card.Rank)
                .Select(group => group.Key!.Value)
                .OrderBy(rank => rank)
                .ToList();

            var expectedPairs = (int)Math.Ceiling(standardCards.Count / 2.0);
            if (orderedRanks.Count != expectedPairs) return false;
            if (expectedPairs < STRAIGHT_PAIRS_MINIMUM_COUNT) return false;
            
            for(int i = 1; i < orderedRanks.Count; i++)
            {
                var gap = orderedRanks[i] - orderedRanks[i - 1] - 1;
                if (gap > 0) return false;
            }
 
            var incompletePairs = (standardCards.Count) - (expectedPairs * 2);
            var phoenixCount = cards.Any(card => card.IsPhoenix) ? 1 : 0;
            
            return incompletePairs <= phoenixCount;
        }

        [CanBeNull]
        private static Combination CreateStraightPairs(List<Card> cards)
        {
            var standardCards = cards.Where(card => card.Type == CardType.Standard).ToList();
            var orderedRanks = standardCards
                .GroupBy(card => card.Rank)
                .Select(group => group.Key!.Value)
                .OrderByDescending(rank => rank)
                .ToList();
            
            int strength =  (int)orderedRanks[0];
            return new Combination(CombinationType.StraightPairs, cards, strength);
        }

        private static bool IsFourKind(List<Card> cards)
        {
            if (cards.Count != FOUR_KIND_COUNT) return false;
            if (cards.Any(card => card.IsDragon || card.IsMahjong || card.IsDog || card.IsPhoenix)) return false;

            var rank = cards[0].Rank;
            return cards.All(card => card.Rank == rank);
        }

        [CanBeNull]
        private static Combination CreateFourKind(List<Card> cards)
        {
            int strength = (int)cards[0].Rank!.Value;
            return new Combination(CombinationType.FourKind, cards, strength);
        }
        
        private static bool IsStraightFlush(List<Card> cards)
        {
            if (!IsStraight(cards)) return false;
            if (cards.Any(card => card.IsPhoenix)) return false;
            
            var suit = cards[0].Suit;
            return cards.All(card => card.Suit == suit);
        }

        [CanBeNull]
        private static Combination CreateStraightFlush(List<Card> cards)
        {
            var standardCards = cards.Where(card => card.Type == CardType.Standard).ToList();

            var orderedRanks = standardCards
                .GroupBy(card => card.Rank)
                .Select(group => group.Key!.Value)
                .OrderByDescending(rank => rank)
                .ToList();
            var strength = (int)orderedRanks[0];
            
            
            return new Combination(CombinationType.StraightFlush, cards, strength);
        }
    }
}