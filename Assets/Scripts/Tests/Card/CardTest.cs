using NUnit.Framework;
using Tichu.Core.Card;

public class CardTest
{
    [Test]
    public void testConstructor()
    {
        var card = new Card(Rank.Ace, Suit.Jade);

        Assert.AreEqual(Rank.Ace, card.rank);
        Assert.AreEqual(Suit.Jade, card.suit);
    }
}
