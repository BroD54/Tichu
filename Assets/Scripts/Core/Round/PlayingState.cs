using System.Collections.Generic;
using System.Linq;
using Core.Events;
using Core.Game;
using Core.Player;

namespace Core.Round
{
    public class PlayingState : IRoundState
    {
        public void OnEnter(Round round)
        {
            round.CurrentTrick = new Trick(round.Players[round.CurrentPlayerIndex], new List<Move>());
            TichuEventBus.RaiseTurnChanged(round.CurrentPlayerIndex);
        }

        public void OnExit(Round round)
        {
            throw new System.NotImplementedException();
        }

        public IRoundState NextState()
        {
            return RoundStateFactory.Create(RoundPhase.Scoring);
        }
        public void NotifyCardsPlayed(Round round, Player.Player player, Move move)
        {
            //use TichuEventBus to notify the observer of played cards
            //check if winner and raise trick won with a collection of trick cards won 
        }

        public void NotifyPlayerPassed(Round round, Player.Player player)
        {
            TichuEventBus.RaisePlayerPassed(round.Players.IndexOf(player));
        }
    }
}