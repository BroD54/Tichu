using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Core.Round
{
    using Player;
    using Combination;
    using Game;
    
    public class Round
    {
        public List<Player> Players { get; }
        public List<Team>  Teams { get; }
        public Deck Deck { get; }
        public int CurrentPlayerIndex { get; private set; }
        [CanBeNull] public Trick CurrentTrick { get; private set; }
        public List<Player> FinishOrder { get; }
        public Dictionary<Player, TichuCall> TichuCalls { get; }
        
        private IRoundState _currentState;

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
        
        public bool ApplyMove(Move move)
        {
            if (move.Player != Players[CurrentPlayerIndex]) return false;
        
            var isLegalMove = CurrentTrick!.TryAddMove(move);
            if  (!isLegalMove) return false;
        
            AdvanceTurn();
            
            return true;
        }
        
        private void AdvanceTurn()
        {
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
        }
        
        private void StartNewTrick(Player leader)
        {
            CurrentTrick = new Trick(leader, new List<Move>());
            CurrentPlayerIndex = Players.IndexOf(leader);
        }

    }
}