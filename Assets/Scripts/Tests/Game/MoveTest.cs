using System.Collections.Generic;
using Core.Card;
using Core.Player;
using Core.Combination;
using Core.Game;
using NUnit.Framework;

namespace Tests.Game
{
    public class MoveTest
    {

        [Test]
        public void TestMoveConstructor()
        {
            Player player = new HumanPlayer("Player 1");
            List<Card> cards = new List<Card> { (new Card(Suit.None, CardType.Dog)) };
            Combination combination = new Combination(CombinationType.Single, cards, 0);
            
            Move move = new Move(player, combination);
            
            Assert.AreEqual(player, move.Player);
            Assert.AreEqual(combination, move.Combination);
        }
    }
}