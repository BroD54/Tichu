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
        public void TestCreateTriple()
        {
            List<Card> cards = new List<Card>
            {
                new (Rank.Eight, Suit.Jade, CardType.Standard),
                new (Rank.Eight, Suit.Pagoda, CardType.Standard),
                new (Rank.Eight, Suit.Star, CardType.Standard),
            };
            
            Combination combination = CombinationFactory.Create(cards);
            
            Assert.AreEqual(combination.Type, CombinationType.Triple);
            Assert.AreEqual(combination.Strength, 8);
        }
        
        [Test]
        public void TestCreateTriplePhoenix()
        {
            List<Card> cards = new List<Card>
            {
                new (Rank.Two, Suit.Jade, CardType.Standard),
                new (Rank.Two, Suit.Star, CardType.Standard),
                new (Suit.None,  CardType.Phoenix),
            };
            
            Combination combination = CombinationFactory.Create(cards);
            
            Assert.AreEqual(combination.Type, CombinationType.Triple);
            Assert.AreEqual(combination.Strength, 2);
        }

        [Test]
        public void TestCreateFullHouse()
        {
            List<Card> cards = new List<Card>
            {
                new (Rank.Two, Suit.Jade, CardType.Standard),
                new (Rank.Two, Suit.Star, CardType.Standard),
                new (Rank.Two, Suit.Sword, CardType.Standard),
                new (Rank.King, Suit.Sword, CardType.Standard),
                new (Rank.King, Suit.Sword, CardType.Standard),
            };
            
            Combination combination = CombinationFactory.Create(cards);
            
            Assert.AreEqual(combination.Type, CombinationType.FullHouse);
            Assert.AreEqual(combination.Strength, 2);
        }
        
        [Test]
        public void TestCreateFullHousePhoenix()
        {
            List<Card> cards = new List<Card>
            {
                new (Rank.Two, Suit.Jade, CardType.Standard),
                new (Rank.Two, Suit.Star, CardType.Standard),
                new (Suit.None, CardType.Phoenix),
                new (Rank.King, Suit.Sword, CardType.Standard),
                new (Rank.King, Suit.Pagoda, CardType.Standard),
            };
            
            Combination combination = CombinationFactory.Create(cards);
            
            Assert.AreEqual(combination.Type, CombinationType.FullHouse);
            Assert.AreEqual(combination.Strength, 13);
        }
        
        [Test]
        public void TestCreateFiveStraight()
        {
            List<Card> cards = new List<Card>
            {
                new (Rank.Mahjong, Suit.Jade, CardType.Mahjong),
                new (Rank.Two, Suit.Star, CardType.Standard),
                new (Rank.Three, Suit.Star, CardType.Standard),
                new (Rank.Four, Suit.Sword, CardType.Standard),
                new (Rank.Five, Suit.Pagoda, CardType.Standard),
            };
            
            Combination combination = CombinationFactory.Create(cards);
            
            Assert.AreEqual(combination.Type, CombinationType.Straight);
            Assert.AreEqual(combination.Strength, 5);
        }
        
        [Test]
        public void TestCreateEightStraightPhoenix()
        {
            List<Card> cards = new List<Card>
            {
                new (Rank.Two, Suit.Star, CardType.Standard),
                new (Rank.Three, Suit.Star, CardType.Standard),
                new (Rank.Four, Suit.Sword, CardType.Standard),
                new (Suit.None, CardType.Phoenix),
                new (Rank.Six, Suit.Star, CardType.Standard),
                new (Rank.Seven, Suit.Star, CardType.Standard),
                new (Rank.Eight, Suit.Sword, CardType.Standard),
                new (Rank.Nine, Suit.Pagoda, CardType.Standard),
            };
            
            Combination combination = CombinationFactory.Create(cards);
            
            Assert.AreEqual(combination.Type, CombinationType.Straight);
            Assert.AreEqual(combination.Strength, 9);
        }
        
        [Test]
        public void TestCreateEightStraightPhoenixExtends()
        {
            List<Card> cards = new List<Card>
            {
                new (Rank.Two, Suit.Star, CardType.Standard),
                new (Rank.Three, Suit.Star, CardType.Standard),
                new (Rank.Four, Suit.Sword, CardType.Standard),
                new (Rank.Five, Suit.Sword, CardType.Standard),
                new (Rank.Six, Suit.Star, CardType.Standard),
                new (Rank.Seven, Suit.Star, CardType.Standard),
                new (Rank.Eight, Suit.Sword, CardType.Standard),
                new (Suit.None, CardType.Phoenix),
            };
            
            Combination combination = CombinationFactory.Create(cards);
            
            Assert.AreEqual(combination.Type, CombinationType.Straight);
            Assert.AreEqual(combination.Strength, 9);
        }
        
        [Test]
        public void TestCreateTwoStraightPairs()
        {
            List<Card> cards = new List<Card>
            {
                new (Rank.Two, Suit.Jade, CardType.Standard),
                new (Rank.Two, Suit.Star, CardType.Standard),
                new (Rank.Three, Suit.Star, CardType.Standard),
                new (Rank.Three, Suit.Sword, CardType.Standard),
            };
            
            Combination combination = CombinationFactory.Create(cards);
            
            Assert.AreEqual(combination.Type, CombinationType.StraightPairs);
            Assert.AreEqual(combination.Strength, 3);
        }
        
        [Test]
        public void TestCreateTwoStraightPairsPhoenix()
        {
            List<Card> cards = new List<Card>
            {
                new (Rank.Two, Suit.Jade, CardType.Standard),
                new (Rank.Two, Suit.Star, CardType.Standard),
                new (Rank.Three, Suit.Star, CardType.Standard),
                new (Suit.None, CardType.Phoenix),
            };
            
            Combination combination = CombinationFactory.Create(cards);
            
            Assert.AreEqual(combination.Type, CombinationType.StraightPairs);
            Assert.AreEqual(combination.Strength, (int)Rank.Three);
        }
        
        [Test]
        public void TestCreateFourKind()
        {
            List<Card> cards = new List<Card>
            {
                new (Rank.King, Suit.Jade, CardType.Standard),
                new (Rank.King, Suit.Star, CardType.Standard),
                new (Rank.King, Suit.Sword, CardType.Standard),
                new (Rank.King, Suit.Pagoda, CardType.Standard),
            };
            
            Combination combination = CombinationFactory.Create(cards);
            
            Assert.AreEqual(combination.Type, CombinationType.FourKind);
            Assert.AreEqual(combination.Strength, (int)Rank.King);
        }
        
                
        [Test]
        public void TestStraightFlush()
        {
            List<Card> cards = new List<Card>
            {
                new (Rank.Two, Suit.Star, CardType.Standard),
                new (Rank.Three, Suit.Star, CardType.Standard),
                new (Rank.Four, Suit.Star, CardType.Standard),
                new (Rank.Five, Suit.Star, CardType.Standard),
                new (Rank.Six, Suit.Star, CardType.Standard),
            };
            
            Combination combination = CombinationFactory.Create(cards);
            
            Assert.AreEqual(combination.Type, CombinationType.StraightFlush);
            Assert.AreEqual(combination.Strength, (int)Rank.Six);
        }
    }
}