using System.Collections.Generic;
using NUnit.Framework;

namespace Tests.Observer
{
    using Core.Events;
    using Core.Player;
    using Core.Card;
    using Core.Round;
    using Core.Game;
    using Core.Combination;
    using Core.Commands;

    public class ObserverTests
    {
        private Round _round;
        private Player _player1;
        private Player _player2;

        private CommandFactory _commandFactory;
        private CombinationFactory _combinationFactory;

        [SetUp]
        public void Setup()
        {
            _player1 = new HumanPlayer("P1");
            _player2 = new HumanPlayer("P2");

            var players = new List<Player> { _player1, _player2 };
            var teams = new List<Team>();
            var deck = new Deck();

            _round = new Round(players, teams, deck);

            _commandFactory = new CommandFactory();
            _combinationFactory = new CombinationFactory();
        }

        [TearDown]
        public void TearDown()
        {
            TichuEventBus.OnGrandTichuDecisionNeeded -= null;
            TichuEventBus.OnGrandTichuDeclared       -= null;
            TichuEventBus.OnTichuDeclared            -= null;
            TichuEventBus.OnCardsPlayed              -= null;
            TichuEventBus.OnPlayerPassed             -= null;
            TichuEventBus.OnTrickWon                 -= null;
            TichuEventBus.OnBombPlayed               -= null;
            TichuEventBus.OnPlayerFinished           -= null;
            TichuEventBus.OnTurnChanged              -= null;
        }
        
        [Test]
        public void GrandTichuDecisionNeededContainsCorrectPlayerNames()
        {
            var received = new List<GrandTichuDecisionNeededEvent>();
            TichuEventBus.OnGrandTichuDecisionNeeded += e => received.Add(e);

            var round = new Round(
                new List<Player> { _player1, _player2 },
                new List<Team>(),
                new Deck());

            Assert.IsTrue(received.Exists(e => e.PlayerName == "P1"));
            Assert.IsTrue(received.Exists(e => e.PlayerName == "P2"));
        }

        [Test]
        public void GrandTichuDeclaredFiredWhenPlayerCallsGrandTichu()
        {
            int firedForIndex = -1;
            TichuEventBus.OnGrandTichuDeclared += idx => firedForIndex = idx;

            _round.SubmitGrandTichuDecision(0, true);

            Assert.AreEqual(0, firedForIndex);
        }

        [Test]
        public void GrandTichuDeclaredNotFiredWhenPlayerDeclines()
        {
            bool fired = false;
            TichuEventBus.OnGrandTichuDeclared += _ => fired = true;

            _round.SubmitGrandTichuDecision(0, false);

            Assert.IsFalse(fired);
        }

        [Test]
        public void CardsPlayedNotFiredWhenTrickIsNull()
        {
            bool fired = false;
            TichuEventBus.OnCardsPlayed += (_, __, ___) => fired = true;

            var card = new Card(Rank.King, Suit.Jade, CardType.Standard);
            _player1.ReceiveCards(new List<Card> { card });
            var combo = _combinationFactory.Create(new List<Card> { card });
            var move = new Move(_player1, combo);
            
            var command = _commandFactory.CreatePlayCardsCommand(_round, move);
            command.Execute();

            Assert.IsFalse(fired);
        }

        [Test]
        public void PlayerPassedFiredWhenMoveIsPass()
        {
            var round = BuildRoundInPlayingState();

            var card = new Card(Rank.King, Suit.Jade, CardType.Standard);
            _player1.ReceiveCards(new List<Card> { card });
            var combo = _combinationFactory.Create(new List<Card> { card });
            var firstMove = new Move(_player1, combo);
            _commandFactory.CreatePlayCardsCommand(round, firstMove).Execute();

            int passedIndex = -1;
            TichuEventBus.OnPlayerPassed += idx => passedIndex = idx;

            var passMove = new Move(_player2, null);
            _commandFactory.CreatePlayCardsCommand(round, passMove).Execute();

            Assert.AreEqual(1, passedIndex);
        }

        

        [Test]
        public void BombPlayedFiredWhenCombinationIsBomb()
        {
            var round = BuildRoundInPlayingState();

            var cards = new List<Card>
            {
                new Card(Rank.Ace, Suit.Jade,   CardType.Standard),
                new Card(Rank.Ace, Suit.Sword,  CardType.Standard),
                new Card(Rank.Ace, Suit.Pagoda, CardType.Standard),
                new Card(Rank.Ace, Suit.Star,   CardType.Standard),
            };
            _player1.ReceiveCards(cards);
            var combo = _combinationFactory.Create(cards);

            bool fired = false;
            TichuEventBus.OnBombPlayed += (_, __) => fired = true;

            _commandFactory.CreatePlayCardsCommand(round, new Move(_player1, combo)).Execute();

            Assert.IsTrue(fired);
        }
        
        [Test]
        public void PlayerFinishedFiredWhenHandIsEmptiedByPlay()
        {
            var round = BuildRoundInPlayingState();

            var card = new Card(Rank.King, Suit.Jade, CardType.Standard);
            _player1.ReceiveCards(new List<Card> { card }); // exactly one card
            var combo = _combinationFactory.Create(new List<Card> { card });

            int finishedIndex = -1;
            TichuEventBus.OnPlayerFinished += idx => finishedIndex = idx;

            _commandFactory.CreatePlayCardsCommand(round, new Move(_player1, combo)).Execute();

            Assert.AreNotEqual(-1, finishedIndex);
        }

        [Test]
        public void PlayerFinishedNotFiredWhenHandStillHasCards()
        {
            var round = BuildRoundInPlayingState();

            var cards = new List<Card>
            {
                new Card(Rank.King,  Suit.Jade, CardType.Standard),
                new Card(Rank.Queen, Suit.Jade, CardType.Standard),
            };
            _player1.ReceiveCards(cards);
            var combo = _combinationFactory.Create(new List<Card> { cards[0] });

            bool fired = false;
            TichuEventBus.OnPlayerFinished += _ => fired = true;

            _commandFactory.CreatePlayCardsCommand(round, new Move(_player1, combo)).Execute();

            Assert.IsFalse(fired);
        }


        [Test]
        public void TichuDeclaredFiredWhenDeclareTichuCommandExecutes()
        {
            int firedForIndex = -1;
            TichuEventBus.OnTichuDeclared += idx => firedForIndex = idx;

            var command = _commandFactory.CreateDeclareTichuCommand(_round, _player1, TichuCall.Tichu);
            command.Execute();

            Assert.AreEqual(0, firedForIndex);
        }

        [Test]
        public void TichuDeclaredNotFiredForGrandTichuCall()
        {
            bool fired = false;
            TichuEventBus.OnTichuDeclared += _ => fired = true;

            var command = _commandFactory.CreateDeclareTichuCommand(_round, _player1, TichuCall.GrandTichu);
            command.Execute();

            Assert.IsFalse(fired);
        }

        private Round BuildRoundInPlayingState()
        {
            var p1 = new HumanPlayer("P1");
            var p2 = new HumanPlayer("P2");
            var round = new Round(new List<Player> { p1, p2 }, new List<Team>(), new Deck());

            round.SubmitGrandTichuDecision(0, false);
            round.SubmitGrandTichuDecision(1, false);
            return round;
        }
    }
}