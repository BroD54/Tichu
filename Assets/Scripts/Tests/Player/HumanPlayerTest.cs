using System.Collections.Generic;
using NUnit.Framework;

namespace Tests.Player
{
    using Core.Player;
    using Core.Card;

    public class HumanPlayerTest
    {
        private Player _player;
        
        [SetUp]
        public void Setup()
        {
            _player = new HumanPlayer("Player 1");
        }

        [Test]
        public void TestConstructor()
        {
            Assert.AreEqual("Player 1", _player.Name);
            Assert.AreEqual(false, _player.DeclaredGrandTichu);
            Assert.AreEqual(false, _player.DeclaredTichu);
            Assert.IsEmpty(_player.Hand);
            Assert.IsEmpty(_player.TricksWon);
        }
        
        [Test]
        public void TestReceiveCards()
        {
            var cards = new List<Card>
            {
                new (Rank.Ace, Suit.Jade, CardType.Standard), 
                new (Suit.None,  CardType.Dog), 
                new (Rank.Eight, Suit.Star, CardType.Standard)
            };
            
            _player.ReceiveCards(cards);
            
            Assert.AreEqual(cards, _player.Hand);    
        }

        [Test]
        public void TestRemoveCards()
        {
            var cards = new List<Card>
            {
                new (Rank.Ace, Suit.Jade, CardType.Standard), 
                new (Suit.None,  CardType.Dog), 
                new (Rank.Eight, Suit.Star, CardType.Standard)
            };
            
            _player.ReceiveCards(cards);
            _player.RemoveCards(cards);
            
            Assert.IsEmpty(_player.Hand);
        }
    }
}