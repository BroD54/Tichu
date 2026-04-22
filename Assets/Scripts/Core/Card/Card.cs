namespace Core.Card {
    public class Card
    {
        public Rank? Rank { get; }
        public Suit Suit { get; }
        public CardType Type { get; }
        public bool IsPhoenix => Type == CardType.Phoenix;
        public bool IsDragon => Type ==  CardType.Dragon;
        public bool IsMahjong => Type ==  CardType.Mahjong;
        public bool IsDog => Type ==  CardType.Dog;

        public Card(Rank rank, Suit suit, CardType type)
        {
            this.Rank = rank;
            this.Suit = suit;
            this.Type = type;
        }

        public Card(Suit suit, CardType type)
        {
            this.Suit = suit;
            this.Type = type;
        }
        
        public override string ToString() => Type switch
        {
            CardType.Standard => $"{Rank}_{Suit}",
            CardType.Mahjong  => "Special_Mahjong",
            CardType.Dragon   => "Special_Dragon",
            CardType.Phoenix  => "Special_Phoenix",
            CardType.Dog      => "Special_Dog",
            _ => $"{Rank}_{Suit}"
        };
    }

}