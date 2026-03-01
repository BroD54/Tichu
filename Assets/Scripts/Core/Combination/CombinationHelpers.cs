using System.Collections.Generic;
using System.Linq;

namespace Core.Combination
{
    using Card; 
    
    public static class CombinationHelpers
    {
        public static bool ContainsIllegalSpecial(List<Card> cards)
        {
            return cards.Any(card => card.IsDog || card.IsDragon || card.IsMahjong);
        }

        public static bool ContainsPhoenix(List<Card> cards)
        {
            return cards.Any(card => card.IsPhoenix);
        }

        public static bool HasCount(List<Card> cards, int expectedCount)
        {
            return cards.Count == expectedCount;
        }

        public static List<Card> GetStandardCards(List<Card> cards)
        {
            return cards.Where(card => card.Type == CardType.Standard).ToList();
        }

        public static List<(Rank Rank, int Count)> GroupByRank(List<Card> cards)
        {
            return cards
                .GroupBy(card => card.Rank!.Value)
                .Select(group => (group.Key, group.Count()))
                .OrderByDescending(group => group.Item2)
                .ThenByDescending(group => group.Key)
                .ToList();
        }

        public static List<int> OrderByRank(List<Card> cards, bool descending = false)
        {
            var ranks = cards
                .Select(card => (int)card.Rank!)
                .Distinct();

            return descending
                ? ranks.OrderByDescending(r => r).ToList()
                : ranks.OrderBy(r => r).ToList();

        }

        public static bool RanksEqual(List<Card> cards)
        {
            if (cards.Count == 0) return true;
            if (ContainsPhoenix(cards) || ContainsIllegalSpecial(cards)) return false;

            var rankNull = cards[0].Rank;
            if (rankNull == null) return false;
            
            Rank rank = (Rank)rankNull;

            return cards.All(card => card.Rank == rank);
        }
        
        public static int TotalGaps(List<int> sortedRanks)
        {
            return sortedRanks
                .Skip(1)
                .Select((rank, i) => rank - sortedRanks[i] - 1)
                .Sum();
        }

        public static bool IsStraight(List<Card> standardCards, bool hasPhoenix)
        {
            List<int> orderedRanks = OrderByRank(standardCards);

            if (orderedRanks.Count != standardCards.Count) return false;
            
            int totalGaps = TotalGaps(orderedRanks);

            int phoenixCount = hasPhoenix ? 1 : 0;
            return totalGaps <= phoenixCount;
        }
    }
}