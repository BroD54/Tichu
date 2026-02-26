using System.Collections.Generic;

namespace Core.Combination
{
    using Card;
    public class Combination
    {
        public CombinationType Type { get; }
        public List<Card> Cards { get; }
        public int Strength { get; }
        public bool IsBomb => Type is CombinationType.FourKind or CombinationType.StraightFlush;
        
        public Combination(CombinationType type, List<Card> cards, int strength)
        {
            Type = type;
            Cards = cards;
            Strength = strength;
        }

        public bool Beats(Combination other) // Does not handle phoenix for Strength
        {
            if (IsBomb && !other.IsBomb) return true;
            if (!IsBomb && other.IsBomb) return false;
            if (IsBomb && other.IsBomb) return CompareBombs(other);
            
            if (Type != other.Type) return false;
            
            return Strength > other.Strength;
        }

        private bool CompareBombs(Combination other)
        {
            if (Type == CombinationType.FourKind && other.Type == CombinationType.StraightFlush) return false;
            if (Type == CombinationType.StraightFlush && other.Type == CombinationType.FourKind) return true;

            return Strength > other.Strength;
        }
    }
}