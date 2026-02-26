using NUnit.Framework;

namespace Tests.Card
{
    using Core.Card;

    public class CardTest
    {
        [Test]
        public void TestConstructor()
        {
            var card = new Card(Rank.Ace, Suit.Jade, CardType.Standard);

            Assert.AreEqual(Rank.Ace, card.Rank);
            Assert.AreEqual(Suit.Jade, card.Suit);
        }
    }
}