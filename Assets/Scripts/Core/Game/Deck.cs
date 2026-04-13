using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Game
{
    using Card;
    using Player;
    
    public class Deck
    {
        private static readonly Random Rng = new ();
        public const int  FirstDealCount = 8;
        public const int SecondDealCount = 6;
        public List<Card> Cards { get; private set; }

        public Deck()
        {
            ResetDeck();
            Shuffle();
        }

        public void ResetDeck()
        {
            Cards = new List<Card>();

            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                if (suit == Suit.None) continue;

                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    if (rank is Rank.Dragon or Rank.Mahjong) continue;
                    
                    Card card = new Card(rank, suit, CardType.Standard);
                    Cards.Add(card);
                }
            }

            Card mahjong = new Card(Rank.Mahjong, Suit.None, CardType.Mahjong);
            Card dragon = new Card(Rank.Dragon, Suit.None, CardType.Dragon);
            Card phoenix = new Card(Suit.None, CardType.Phoenix);
            Card dog = new Card(Suit.None, CardType.Dog);
            
            Cards.Add(mahjong);
            Cards.Add(dragon);
            Cards.Add(phoenix);
            Cards.Add(dog);
        }

        public void Deal(List<Player> players, int count)
        {
            foreach (Player player in players)
            {
                List<Card> dealt = new List<Card>();
                for (var i = 0; i < count; i++)
                {
                    Card card = Cards.First();
                    Cards.RemoveAt(0);
                    dealt.Add(card);
                }
                player.ReceiveCards(dealt);
            }
        }

        private void Shuffle()
        {
            Cards = Cards.OrderBy(_ => Rng.Next()).ToList();
        }
    }
}