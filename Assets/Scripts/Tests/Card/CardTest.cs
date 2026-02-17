using NUnit.Framework;
using Core.Card;

public class CardTest
{
    [Test]
    public void TestConstructor()
    {
        var card = new Card(Rank.Ace, Suit.Jade);

        Assert.AreEqual(Rank.Ace, card.Rank);
        Assert.AreEqual(Suit.Jade, card.Suit);
    }
}