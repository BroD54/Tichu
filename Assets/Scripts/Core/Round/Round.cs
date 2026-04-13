using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Core.Round
{
    using Player;
    using Combination;
    using Game;
    using Events;
    
    public class Round
    {
        public List<Player> Players { get; }
        public List<Team>  Teams { get; }
        public Deck Deck { get; }
        public int CurrentPlayerIndex { get; internal set; }
        [CanBeNull] public Trick CurrentTrick { get; internal set; }
        public List<Player> FinishOrder { get; }
        public Dictionary<Player, TichuCall> TichuCalls { get; }
        
        private IRoundState _currentState;
        
        public event Action<GrandTichuDecisionNeededEvent> OnGrandTichuDecisionNeeded;

        public Round(List<Player> players, List<Team> teams, Deck deck)
        {
            Players = players;
            Teams = teams;
            Deck = deck;
            
            FinishOrder = new List<Player>();
            TichuCalls = new Dictionary<Player, TichuCall>();

            CurrentTrick = null;
            
            _currentState = RoundStateFactory.Create(RoundPhase.DealingFirstCards);
            _currentState.OnEnter(this);
        }

        public void TransitionToNext()
        {
            _currentState.OnExit(this);
            _currentState = _currentState.NextState();
            _currentState.OnEnter(this);
        }

        public bool IsInState<T>() where T : IRoundState
        {
            return _currentState is T;
        }

        public T GetState<T>() where T : class, IRoundState
        {
            return _currentState as T;
        }

        private bool IsValidIndex(int index)
        {
            return index >= 0 && index < Players.Count;
        }

        public bool SubmitGrandTichuDecision(int playerIndex, bool calledGrandTichu)
        {
            if (!IsValidIndex(playerIndex)) return false;

            return GetState<GrandTichuCallsState>()?.SubmitDecision(this, Players[playerIndex], calledGrandTichu) ??
                   false;
        }

        internal void FireGrandTichuDecisionNeeded(Player player)
        {
            OnGrandTichuDecisionNeeded?.Invoke(new GrandTichuDecisionNeededEvent(player.Name, Players.IndexOf(player)));
        }

    }
}