using System.Collections.Generic;
using NUnit.Framework;

namespace Tests.Combination
{
    using Core.Card;
    using Core.Combination;


    public class CombinationFactoryTest
    {
        [Test]
        public void TestCreateNoCards()
        {
            List<Card> cards = new List<Card>();
            
            Combination combination = CombinationFactory.Create(cards);

            Assert.AreEqual(combination, null);
        }
        
        [Test]
        public void TestCreateValidSingle()
        {
            List<Card> cards = new List<Card>
            {
                new (Rank.Ace, Suit.Jade, CardType.Standard),
            };
            
            Combination combination = CombinationFactory.Create(cards);
            
            Assert.AreEqual(combination.Type, CombinationType.Single);
            Assert.AreEqual(combination.Strength, 14);
        }
        
        [Test]
        public void TestCreateValidSingleDog()
        {
            List<Card> cards = new List<Card>
            {
                new (Suit.None, CardType.Dog),
            };
            
            Combination combination = CombinationFactory.Create(cards);
            
            Assert.AreEqual(combination.Type, CombinationType.Single);
            Assert.AreEqual(combination.Strength, 0);
        }
        
        [Test]
        public void TestCreatePairStandard()
        {
            List<Card> cards = new List<Card>
            {
                new (Rank.Ace, Suit.Jade, CardType.Standard),
                new (Rank.Ace, Suit.Pagoda, CardType.Standard),
            };
            
            Combination combination = CombinationFactory.Create(cards);
            
            Assert.AreEqual(combination.Type, CombinationType.Pair);
            Assert.AreEqual(combination.Strength, 14);
        }
        
        [Test]
        public void TestCreatePairPhoenix()
        {
            List<Card> cards = new List<Card>
            {
                new (Rank.King, Suit.Jade, CardType.Standard),
                new (Suit.None,  CardType.Phoenix),
            };
            
            Combination combination = CombinationFactory.Create(cards);
            
            Assert.AreEqual(combination.Type, CombinationType.Pair);
            Assert.AreEqual(combination.Strength, 13);
        }

        [Test]
        public void TestBeats()
        {
            // Not Implemented
            Assert.False(true);
        }
    }
}