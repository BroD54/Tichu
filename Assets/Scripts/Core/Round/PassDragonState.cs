using System.Collections.Generic;
using System.Linq;

namespace Core.Round
{
    using Player;
    using Game;
    using Card;

    public class PassDragonState : IRoundState
    {
        private Player _winner;
        private List<Card> _trickCards;
        private RoundStateFactory _roundStateFactory = new();

        public PassDragonState(Player winner, List<Card> trickCards)
        {
            _winner = winner;
            _trickCards = trickCards;
        }

        public void OnEnter(Round round)
        {
            round.Events.RaiseDragonGiftNeeded(round.Players.IndexOf(_winner));
        }

        public void OnExit(Round round)
        {
        }

        public IRoundState NextState()
        {
            return _roundStateFactory.Create(RoundPhase.Playing);
        }

        public bool GiftToOpponent(Round round, int opponentIndex)
        {
            Player opponent = round.Players[opponentIndex];

            bool isOpponent = !round.Teams.Any(t => t.Contains(_winner) && t.Contains(opponent));
            if (!isOpponent) return false;

            opponent.TricksWon.Add(_trickCards);
            round.Events.RaiseTrickWon(opponentIndex, _trickCards.Select(c => c.ToString()).ToList());

            round.CurrentTrick = new Trick(_winner, new List<Move>());
            round.CurrentPlayerIndex = round.Players.IndexOf(_winner);

            round.TransitionToNext();
            return true;
        }
    }
}