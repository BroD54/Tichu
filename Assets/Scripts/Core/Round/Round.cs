using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Core.Round
{
    using Player;
    using Game;
    using Events;
    
    public class Round
    {
        public List<Player> Players { get; }
        public List<Team>  Teams { get; }
        public Deck Deck { get; }
        public int CurrentPlayerIndex { get; internal set; }
        public Trick CurrentTrick { get; internal set; }
        public List<Player> FinishOrder { get; }
        public Dictionary<Player, TichuCall> TichuCalls { get; }
        
        public Action OnRoundComplete { get; set; }

        
        public TichuEventBus Events { get; }
        
        private IRoundState _currentState;
        
        public Round(List<Player> players, List<Team> teams, Deck deck)
        {
            Players = players;
            Teams = teams;
            Deck = deck;
            
            FinishOrder = new List<Player>();
            TichuCalls = new Dictionary<Player, TichuCall>();
            CurrentTrick = null;
            Events = new TichuEventBus();
            
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
        
        public bool SubmitTichuDeclaration(int playerIndex)
        {
            if (!IsValidIndex(playerIndex)) return false;

            var player = Players[playerIndex];
            if (player.Hand.Count != 14 || player.DeclaredTichu) return false;
            
            player.DeclareTichu();
            TichuCalls[player] = TichuCall.Tichu;
            
            Events.RaiseTichuDeclared(playerIndex);
            
            return true;
        }

        public bool SubmitCardExchange(int playerIndex, List<string> cardIds)
        {
            if (!IsValidIndex(playerIndex)) return false;
            
            return GetState<CardsExchangeState>()
                ?.SubmitExchange(this, Players[playerIndex], cardIds) ?? false;
        }

        public bool SubmitMove(int playerIndex, List<string> cardIds)
        {
            if (!IsValidIndex(playerIndex)) return false;
            
            return GetState<PlayingState>()
                ?.SubmitMove(this, Players[playerIndex], cardIds) ?? false;
        }

    }
}