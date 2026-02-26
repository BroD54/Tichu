using System.Collections.Generic;
using NUnit.Framework;

namespace Tests.Combination
{
    using Core.Card;
    using Core.Combination;


    public class CombinationTest
    {
        [Test]
        public void TestConstructor()
        {
            List<Card> cards = new List<Card>
            {
                new (Rank.Ace, Suit.Jade, CardType.Standard),
                new (Rank.Ace, Suit.Pagoda, CardType.Standard),
            };
            
            Combination combination = new Combination(CombinationType.Pair, cards, 14);
            
            Assert.AreEqual(combination.Cards, cards);
            Assert.AreEqual(combination.Type, CombinationType.Pair);
            Assert.AreEqual(combination.Strength, 14);
        }

        [Test]
        public void TestBeats()
        {
            Assert.False(true);
        }
    }
}