using Core.Events;
using UnityEngine;

namespace Core.Round
{
    using Player;
    using Game;
    
    public class GrandTichuCallsState : IRoundState
    {
        private int _currentIndex = 0;
        private RoundStateFactory _roundStateFactory = new();

        public void OnEnter(Round round)
        {
            _currentIndex = 0;
            AskNext(round);
        }

        public void OnExit(Round round)
        {
        }

        public IRoundState NextState()
        {
            return _roundStateFactory.Create(RoundPhase.DealingRemainingCards);
        }
        
        public bool SubmitDecision(Round round, Player player, bool calledGrandTichu)
        {
            if (round.TichuCalls.ContainsKey(player)) return false;

            round.TichuCalls[player] = calledGrandTichu ? TichuCall.GrandTichu : TichuCall.None;
            if (calledGrandTichu)
            {
                player.DeclareGrandTichu();
                round.Events.RaiseGrandTichuDeclared(round.Players.IndexOf(player));

            }
            
            _currentIndex++;
            
            if (_currentIndex >= round.Players.Count)
                round.TransitionToNext();
            else
                AskNext(round);

            return true;

        }
        
        private void AskNext(Round round)
        {
            if (_currentIndex >= round.Players.Count)
            {
                round.TransitionToNext();
                return;
            }

            var player = round.Players[_currentIndex];

            round.Events.RaiseGrandTichuDecisionNeeded(
                player.Name,
                _currentIndex
            );
        }
    }
}