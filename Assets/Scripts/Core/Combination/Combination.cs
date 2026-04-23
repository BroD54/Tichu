using System.Collections.Generic;
using System.Linq;
using Codice.Client.BaseCommands.Merge;

namespace Core.Combination
{
    using Card;
    public abstract class Combination
    { 
        public IReadOnlyList<Card> Cards { get; }
        public abstract CombinationType Type { get; }
        public abstract int Strength { get; }
        public virtual bool IsBomb => false;
        
        protected Combination(List<Card> cards)
        {
            Cards = cards.ToList().AsReadOnly();
        }

        public bool Beats(Combination other)
        {
            if (other == null) return true;
            if (IsBomb && !other.IsBomb) return true;
            if (!IsBomb && other.IsBomb) return false;
            if (IsBomb && other.IsBomb) return BeatsBomb(other);
            
            if(Type != other.Type) return false;

            return BeatsSameType(other);
        }

        protected virtual bool BeatsSameType(Combination other)
        {
            return Strength > other.Strength;
        }

        protected virtual bool BeatsBomb(Combination other)
        {
            return false;
        }

        public bool ContainsMahjong()
        {
            return Cards.Any(c => c.Rank == Rank.Mahjong);
        }
        
        public bool ContainsRank(Rank rank)
        {
            return Cards.Any(c => c.Rank == rank);
        }
        
        
    }
}