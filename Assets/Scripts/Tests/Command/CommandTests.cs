using System.Collections.Generic;
using NUnit.Framework;

namespace Tests.Command
{
    using Core.Commands;
    using Core.Player;
    using Core.Card;
    using Core.Round;
    using Core.Game;
    using Core.Combination;

    public class CommandTests
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

        [Test]
        public void TestAdvanceTurnCommand()
        {
            var initialIndex = _round.CurrentPlayerIndex;

            var command = _commandFactory.CreateAdvanceTurnCommand(_round);
            command.Execute();

            Assert.AreEqual((initialIndex + 1) % _round.Players.Count, _round.CurrentPlayerIndex);
        }

        [Test]
        public void TestExchangeCardsCommand()
        {
            var card = new Card(Rank.Ace, Suit.Jade, CardType.Standard);
            _player1.ReceiveCards(new List<Card> { card });

            var command = _commandFactory.CreateExchangeCardsCommand(_player1, _player2, card);
            command.Execute();

            Assert.IsFalse(_player1.Hand.Contains(card));
            Assert.IsTrue(_player2.Hand.Contains(card));
        }

        [Test]
        public void TestDeclareTichuCommand()
        {
            var command = _commandFactory.CreateDeclareTichuCommand(_round, _player1, TichuCall.Tichu);
            command.Execute();

            Assert.IsTrue(_player1.DeclaredTichu);
            Assert.AreEqual(TichuCall.Tichu, _round.TichuCalls[_player1]);
        }

        [Test]
        public void TestPlayCardsCommand_DoesNothing_WhenNoTrick()
        {
            var card = new Card(Rank.King, Suit.Jade, CardType.Standard);
            _player1.ReceiveCards(new List<Card> { card });

            var combination = _combinationFactory.Create(new List<Card> { card });
            var move = new Move(_player1, combination);

            var command = _commandFactory.CreatePlayCardsCommand(_round, move);
            command.Execute();

            Assert.IsTrue(_player1.Hand.Contains(card));
        }

        [Test]
        public void TestAwardTrickCommand()
        {
            var card = new Card(Rank.Ten, Suit.Jade, CardType.Standard);

            var combination = _combinationFactory.Create(new List<Card> { card });
            var move = new Move(_player1, combination);

            var trick = new Trick(_player1, new List<Move>());
            trick.TryAddMove(move);

            var command = _commandFactory.CreateAwardTrickCommand(_round, _player1, trick);
            command.Execute();
            
            Assert.AreEqual(1, _player1.TricksWon.Count);
            Assert.AreEqual(1, _player1.TricksWon[0].Count);
            Assert.AreEqual(card, _player1.TricksWon[0][0]);
        }
    }
}