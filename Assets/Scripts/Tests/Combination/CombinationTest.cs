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
        public void TestAceBeatsKing()
        {
            Card ace = new Card(Rank.Ace, Suit.Jade,  CardType.Standard);
            Card king = new Card(Rank.King, Suit.Jade,  CardType.Standard);

            Combination aceSingle = new Combination(CombinationType.Single, new List<Card> { ace }, 14);
            Combination kingSingle = new Combination(CombinationType.Single, new List<Card> { king }, 13);
            
            Assert.True(aceSingle.Beats(kingSingle));
        }
        
        [Test]
        public void TestDragonBeatsAce()
        {
            Card ace = new Card(Rank.Ace, Suit.Jade,  CardType.Standard);
            Card dragon = new Card(Rank.Dragon, Suit.None,  CardType.Dragon);

            Combination aceSingle = new Combination(CombinationType.Single, new List<Card> { ace }, 14);
            Combination dragonSingle = new Combination(CombinationType.Single, new List<Card> { dragon }, 15);
            
            Assert.True(dragonSingle.Beats(aceSingle));
        }
        
        [Test]
        public void TestPairOnSingle()
        {
            Combination pair = new Combination(CombinationType.Pair, new List<Card> (), 14);
            Combination single = new Combination(CombinationType.Single, new List<Card> (), 13);
            
            Assert.False(pair.Beats(single));
        }
        
        [Test]
        public void TestBombBeatsBomb()
        {
            Combination highBomb = new Combination(CombinationType.StraightFlush, new List<Card> (), 14);
            Combination lowBomb = new Combination(CombinationType.StraightFlush, new List<Card> (), 13);
            
            Assert.True(highBomb.Beats(lowBomb));
        }
        
        [Test]
        public void TestFiveStraightBeatsSixStraight()
        {
            List<Card> fiveStraightCards = new List<Card>
            {
                new(Rank.Ace, Suit.Jade, CardType.Standard),
                new(Rank.King, Suit.Jade, CardType.Standard),
                new(Rank.Queen, Suit.Jade, CardType.Standard),
                new(Rank.Jack, Suit.Star, CardType.Standard),
                new(Rank.Ten, Suit.Jade, CardType.Standard),
                new(Rank.Nine, Suit.Jade, CardType.Standard),
            };
            
            List<Card> sixStraightCards = new List<Card>
            {
                new(Rank.King, Suit.Jade, CardType.Standard),
                new(Rank.Queen, Suit.Jade, CardType.Standard),
                new(Rank.Jack, Suit.Star, CardType.Standard),
                new(Rank.Ten, Suit.Jade, CardType.Standard),
                new(Rank.Nine, Suit.Jade, CardType.Standard),
            };
            
            Combination fiveStraight = new Combination(CombinationType.Straight, fiveStraightCards, 14);
            Combination sixStraight = new Combination(CombinationType.Straight, sixStraightCards, 13);
            
            Assert.False(fiveStraight.Beats(sixStraight));
        }
    }
}