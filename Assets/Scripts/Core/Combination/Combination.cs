using System.Collections.Generic;

namespace Core.Game
{
    using Card;
    public class Combination
    {
        public CombinationType Type { get; }
        public List<Card> Cards { get; }
        public int Strength { get; }
        
        public Combination(CombinationType type, List<Card> cards, int strength)
        {
            Type = type;
            Cards = cards;
            Strength = strength;
        }

        public bool Beats(Combination other)
        {

            return false;
        }
    }
}