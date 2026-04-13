using System.Collections.Generic;
using System.Linq;

namespace Core.Player
{
    using Card;
    using Game;
    using Round;
    
    public abstract class Player
    {
        public string Name { get; }
        public List<Card> Hand { get; private set; }
        public List<List<Card>> TricksWon { get; private set; }
        public bool DeclaredTichu { get; private set; }
        public bool DeclaredGrandTichu { get; private set; }
        
        public bool HasMahjong => Hand.Any(card =>  card.IsMahjong);

        protected Player(string name)
        {
            Name = name;
            Hand = new List<Card>();
            TricksWon = new List<List<Card>>();
            DeclaredTichu = false;
            DeclaredGrandTichu = false;
        }

        public void ReceiveCards(List<Card> cards)
        {
            foreach (var card in cards)
            {
                Hand.Add(card);
            }
        }

        public void RemoveCards(List<Card> cards)
        {
            foreach (var card in cards)
            {
                Hand.Remove(card);
            }
        }

        public void DeclareGrandTichu()
        {
            DeclaredGrandTichu = true;
        }

        public void DeclareTichu()
        {
            DeclaredTichu = true;
        }

        public abstract Move MakeMove(Round currentRound);
    }
}