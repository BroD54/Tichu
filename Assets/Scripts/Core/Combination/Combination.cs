using System.Collections.Generic;
using System.Linq;

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
        
        protected abstract bool BeatsSameType(Combination other);

        protected virtual bool BeatsBomb(Combination other)
        {
            return false;
        }
    }
}