using System.Collections.Generic;
using Core.Card;
using Core.Game;
using NUnit.Framework;

namespace Tests.Game
{


    public class CombinationTest
    {
        [Test]
        public void TestConstructor()
        {
            List<Card> cards = new List<Card>
            {
                new (Rank.Ace, Suit.Jade),
                new (Rank.Ace, Suit.Pagoda),
            };
            Combination combination = new Combination();
        }
    }
}