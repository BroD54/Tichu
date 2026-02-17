namespace Tichu.Core.Card {
    public class Card
    {
        public Rank rank { get; }
        public Suit suit { get; }

        public Card(Rank rank, Suit suit)
        {
            this.rank = rank;
            this.suit = suit;
        }
    }

}