using System.Collections.Generic;
using Core.Card;
using Core.Combination;
using Core.Events;
using NUnit.Framework;
using Core.Game;
using Core.Player;
using Core.Round;

namespace Tests.Game
{
    public class GameStateTest
    {
        [Test]
        public void TestGameStateConstructor()
        {
            // GameState gameState = new GameState();
        }
        
        [Test]
        public void TestCombinationContainsMahjong()
        {
            var cards = new List<Card>
            {
                new Card(Rank.Mahjong, Suit.Jade, CardType.Standard),
                new Card(Rank.Five, Suit.Sword, CardType.Standard)
            };

            Combination combo = new Pair(cards, 5);

            Assert.True(combo.ContainsMahjong());
        }
       
    }
}