using System.Collections.Generic;
using System.Linq;
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
        private (Round round, List<Player> players, TichuEventBus events) BuildRound(List<List<Card>> hands)
        {
            var players = new List<Player>
            {
                new HumanPlayer("P0"),
                new HumanPlayer("P1"),
                new HumanPlayer("P2"),
                new HumanPlayer("P3"),
            };

            var teams = new List<Team>
            {
                new Team(0, players[0], players[2]),
                new Team(1, players[1], players[3]),
            };

            var events = new TichuEventBus();
            var round = new Round(players, teams, new Deck(), events);

            foreach (var player in players)
                player.Hand.Clear();

            bool anyHasMahjong = hands.Exists(h => h.Exists(c => c.IsMahjong));
            if (!anyHasMahjong)
                hands[0].Add(new Card(Rank.Mahjong, Suit.Jade, CardType.Mahjong));

            for (int i = 0; i < players.Count; i++)
                players[i].ReceiveCards(hands[i]);

            round.TransitionTo(new PlayingState());
            return (round, players, events);
        }

        private Card Dog() => new Card(Suit.Jade, CardType.Dog);
        private Card Mahjong() => new Card(Rank.Mahjong, Suit.Jade, CardType.Mahjong);
        private Card Std(Rank r, Suit s = Suit.Jade) => new Card(r, s, CardType.Standard);

        [Test]
        public void TestDogPassesToTeammateWhenTeammateIsActive()
        {
            var dog = Dog();
            var hands = new List<List<Card>>
            {
                new List<Card> { Mahjong(), dog },
                new List<Card> { Std(Rank.Three, Suit.Sword) },
                new List<Card> { Std(Rank.Five, Suit.Jade) },
                new List<Card> { Std(Rank.Seven, Suit.Star) },
            };

            var (round, players, events) = BuildRound(hands);

            int turnChangedTo = -1;
            events.OnTurnChanged += idx => turnChangedTo = idx;

            bool accepted = round.SubmitMove(0, new List<string> { dog.ToString() });

            Assert.IsTrue(accepted);
            Assert.AreEqual(2, turnChangedTo);
            Assert.AreEqual(2, round.CurrentPlayerIndex);
        }

        [Test]
        public void TestDogGrantsLeadToSelfWhenOnlyPlayerLeft() 
        {
            var dog = Dog();
            var hands = new List<List<Card>>
            {
                new List<Card> { Mahjong(), dog },
                new List<Card>(),
                new List<Card>(),
                new List<Card>(),
            };

            var (round, players, events) = BuildRound(hands);

            round.SubmitMove(0, new List<string> { dog.ToString() });

            Assert.IsTrue(round.IsInState<FinishedState>());
        }

        // [Test]
        // public void TestWishEnforcedWhenPlayerHasWish()
        // {
        //     var mahjong = Mahjong();
        //     var five = Std(Rank.Five, Suit.Sword);
        //     var three = Std(Rank.Three, Suit.Sword);
        //
        //     var hands = new List<List<Card>>
        //     {
        //         new List<Card> { mahjong },
        //         new List<Card> { three, five },
        //         new List<Card> { Std(Rank.Six, Suit.Jade) },
        //         new List<Card> { Std(Rank.Eight, Suit.Star) },
        //     };
        //
        //     var (round, players, events) = BuildRound(hands);
        //
        //     round.SubmitMove(0, new List<string> { mahjong.ToString() });
        //     round.GetState<DeclareWishState>()?.DeclareWish(round, Rank.Five);
        //
        //     bool rejectedWrongCard = round.SubmitMove(1, new List<string> { three.ToString() });
        //     bool acceptedCorrectCard = round.SubmitMove(1, new List<string> { five.ToString() });
        //
        //     Assert.IsFalse(rejectedWrongCard);
        //     Assert.IsTrue(acceptedCorrectCard);
        // }

        // [Test]
        // public void TestWishIsClearedAfterWishPlayed()
        // {
        //     var mahjong = Mahjong();
        //     var five = Std(Rank.Five, Suit.Sword);
        //
        //     var hands = new List<List<Card>>
        //     {
        //         new List<Card> { mahjong },
        //         new List<Card> { five },
        //         new List<Card> { Std(Rank.Six, Suit.Jade) },
        //         new List<Card> { Std(Rank.Eight, Suit.Star) },
        //     };
        //
        //     var (round, players, events) = BuildRound(hands);
        //
        //     round.SubmitMove(0, new List<string> { mahjong.ToString() });
        //     round.GetState<DeclareWishState>()?.DeclareWish(round, Rank.Five);
        //     round.SubmitMove(1, new List<string> { five.ToString() });
        //
        //     Assert.IsNull(round.ActiveWish);
        // }

        [Test]
        public void TestCombinationContainsMahjong()
        {
            var cards = new List<Card>
            {
                new Card(Rank.Mahjong, Suit.Jade, CardType.Mahjong),
                new Card(Rank.Two, Suit.Sword, CardType.Standard)
            };

            Combination combo = new Pair(cards, 1);
            Assert.True(combo.ContainsMahjong());
        }
    }
}