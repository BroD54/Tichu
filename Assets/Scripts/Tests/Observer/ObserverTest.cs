using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Tests.Observer
{
    using Core.Player;
    using Core.Card;
    using Core.Round;
    using Core.Game;

    public class ObserverTests
    {
        private Round _round;
        private List<Player> _players;

        [SetUp]
        public void Setup()
        {
            _players = new List<Player>
            {
                new HumanPlayer("P1"),
                new HumanPlayer("P2"),
                new HumanPlayer("P3"),
                new HumanPlayer("P4")
            };

            var teams = new List<Team>
            {
                new Team(0, _players[0], _players[2]),
                new Team(1, _players[1], _players[3])
            };

            _round = new Round(_players, teams, new Deck());
        }
        
        [Test]
        public void GrandTichuDecisionNeeded_FiredForAllPlayers_OnRoundStart()
        {
            var received = new List<string>();
            var players = new List<Player>
            {
                new HumanPlayer("P1"),
                new HumanPlayer("P2"),
                new HumanPlayer("P3"),
                new HumanPlayer("P4")
            };
            var teams = new List<Team>
            {
                new Team(0, _players[0], _players[2]),
                new Team(1, _players[1], _players[3])
            };
            
            var round = new Round(players, teams, new Deck());
            Assert.That(round.IsInState<GrandTichuCallsState>(), Is.True);
            Assert.That(round.TichuCalls.Count, Is.EqualTo(0));
        }

        [Test]
        public void GrandTichuDeclared_IsFired_WhenPlayerCalls()
        {
            int firedIndex = -1;
            _round.Events.OnGrandTichuDeclared += idx => firedIndex = idx;

            _round.SubmitGrandTichuDecision(0, true);

            Assert.That(firedIndex, Is.EqualTo(0));
        }

        [Test]
        public void GrandTichuDeclared_NotFired_WhenPlayerDeclines()
        {
            bool fired = false;
            _round.Events.OnGrandTichuDeclared += _ => fired = true;

            _round.SubmitGrandTichuDecision(0, false);

            Assert.That(fired, Is.False);
        }

        [Test]
        public void GrandTichuDecision_ReturnsFalse_WhenSubmittedTwice()
        {
            _round.SubmitGrandTichuDecision(0, true);
            var second = _round.SubmitGrandTichuDecision(0, false);

            Assert.That(second, Is.False);
        }

        [Test]
        public void AllGrandTichuDecisions_TransitionsTo_CardExchange()
        {
            for (int i = 0; i < 4; i++)
                _round.SubmitGrandTichuDecision(i, false);

            Assert.That(_round.IsInState<CardsExchangeState>(), Is.True);
        }
        
        [Test]
        public void TichuDeclared_IsFired_WhenPlayerDeclarestichu()
        {
            var round = BuildRoundInPlayingState();
            int firedIndex = -1;
            round.Events.OnTichuDeclared += idx => firedIndex = idx;

            round.SubmitTichuDeclaration(0);

            Assert.That(firedIndex, Is.EqualTo(0));
        }

        [Test]
        public void TichuDeclared_CannotBeCalled_Twice()
        {
            var round = BuildRoundInPlayingState();

            var first  = round.SubmitTichuDeclaration(0);
            var second = round.SubmitTichuDeclaration(0);

            Assert.That(first,  Is.True);
            Assert.That(second, Is.False);
        }

        [Test]
        public void TichuDeclared_CannotBeCalled_AfterPlayingCards()
        {
            var round = BuildRoundInPlayingState();
            var currentIndex = round.CurrentPlayerIndex;

            var cardId = round.Players[currentIndex].Hand
                .First(c => !c.IsDog).ToString();
            round.SubmitMove(currentIndex, new List<string> { cardId });

            var result = round.SubmitTichuDeclaration(currentIndex);
            Assert.That(result, Is.False);
        }
        
        [Test]
        public void CardExchange_ReturnsFalse_WhenNotInExchangeState()
        {
            var result = _round.SubmitCardExchange(0, new List<string> { "Five_Jade" });
            Assert.That(result, Is.False);
        }

        [Test]
        public void CardExchange_TransitionsToPlaying_WhenAllPlayersSubmit()
        {
            var round = BuildRoundInExchangeState();

            for (int i = 0; i < 4; i++)
            {
                var cardIds = round.Players[i].Hand.Take(3)
                    .Select(c => c.ToString()).ToList();
                round.SubmitCardExchange(i, cardIds);
            }

            Assert.That(round.IsInState<PlayingState>(), Is.True);
        }
        
        [Test]
        public void TurnChanged_IsFired_WhenRoundEntersPlayingState()
        {
            int firedIndex = -1;
            var round = BuildRoundInExchangeState();
            round.Events.OnTurnChanged += idx => firedIndex = idx;

            for (int i = 0; i < 4; i++)
            {
                var cardIds = round.Players[i].Hand.Take(3)
                    .Select(c => c.ToString()).ToList();
                round.SubmitCardExchange(i, cardIds);
            }

            Assert.That(firedIndex, Is.GreaterThanOrEqualTo(0));
        }

        [Test]
        public void SubmitMove_ReturnsFalse_WhenNotInPlayingState()
        {
            var result = _round.SubmitMove(0, new List<string> { "Five_Jade" });
            Assert.That(result, Is.False);
        }

        [Test]
        public void SubmitMove_ReturnsFalse_WhenWrongPlayerMoves()
        {
            var round = BuildRoundInPlayingState();
            var wrongIndex = (round.CurrentPlayerIndex + 1) % 4;
            var cardIds = round.Players[wrongIndex].Hand.Take(1)
                .Select(c => c.ToString()).ToList();

            var result = round.SubmitMove(wrongIndex, cardIds);
            Assert.That(result, Is.False);
        }

        [Test]
        public void CardsPlayed_IsFired_WhenValidMoveSubmitted()
        {
            var round = BuildRoundInPlayingState();
            int firedIndex = -1;
            round.Events.OnCardsPlayed += (idx, _) => firedIndex = idx;

            var currentIndex = round.CurrentPlayerIndex;
            var cardId = round.Players[currentIndex].Hand
                .First(c => !c.IsDog)
                .ToString();

            round.SubmitMove(currentIndex, new List<string> { cardId });

            Assert.That(firedIndex, Is.EqualTo(currentIndex));
        }

        [Test]
        public void PlayerPassed_IsFired_WhenPassSubmitted()
        {
            var round = BuildRoundInPlayingState();

            var leadIndex = round.CurrentPlayerIndex;
            var cardId = round.Players[leadIndex].Hand
                .First(c => !c.IsDog).ToString();
            round.SubmitMove(leadIndex, new List<string> { cardId });

            int passedIndex = -1;
            round.Events.OnPlayerPassed += idx => passedIndex = idx;

            round.SubmitMove(round.CurrentPlayerIndex, null);

            Assert.That(passedIndex, Is.EqualTo((leadIndex + 1) % 4));
        }

        [Test]
        public void TrickWon_IsFired_AfterThreeConsecutivePasses()
        {
            var round = BuildRoundInPlayingState();

            var leadIndex = round.CurrentPlayerIndex;
            var cardId = round.Players[leadIndex].Hand
                .First(c => !c.IsDog).ToString();
            round.SubmitMove(leadIndex, new List<string> { cardId });

            int trickWinnerIndex = -1;
            round.Events.OnTrickWon += (idx, _) => trickWinnerIndex = idx;

            for (int i = 0; i < 3; i++)
                round.SubmitMove(round.CurrentPlayerIndex, null);

            Assert.That(trickWinnerIndex, Is.EqualTo(leadIndex));
        }

        [Test]
        public void PlayerFinished_IsFired_WhenHandIsEmpty()
        {
            var round = BuildRoundInPlayingState();
            var currentIndex = round.CurrentPlayerIndex;
            var player = round.Players[currentIndex];

            var singleCard = player.Hand.First(c => !c.IsDog && !c.IsDragon);
            player.Hand.Clear();
            player.ReceiveCards(new List<Card> { singleCard });

            int finishedIndex = -1;
            round.Events.OnPlayerFinished += idx => finishedIndex = idx;

            round.SubmitMove(currentIndex, new List<string> { singleCard.ToString() });

            Assert.That(finishedIndex, Is.EqualTo(currentIndex));
        }
        
        private Round BuildRoundInExchangeState()
        {
            var players = BuildFourPlayers();
            var round = new Round(players.players, players.teams, new Deck());

            for (int i = 0; i < 4; i++)
                round.SubmitGrandTichuDecision(i, false);

            Assert.That(round.IsInState<CardsExchangeState>(), Is.True);
            return round;
        }

        private Round BuildRoundInPlayingState()
        {
            var round = BuildRoundInExchangeState();

            for (int i = 0; i < 4; i++)
            {
                var cardIds = round.Players[i].Hand.Take(3)
                    .Select(c => c.ToString()).ToList();
                round.SubmitCardExchange(i, cardIds);
            }

            Assert.That(round.IsInState<PlayingState>(), Is.True);
            return round;
        }

        private (List<Player> players, List<Team> teams) BuildFourPlayers()
        {
            var players = new List<Player>
            {
                new HumanPlayer("P1"),
                new HumanPlayer("P2"),
                new HumanPlayer("P3"),
                new HumanPlayer("P4")
            };
            var teams = new List<Team>
            {
                new Team(0, _players[0], _players[2]),
                new Team(1, _players[1], _players[3])
            };
            return (players, teams);
        }
    }
}