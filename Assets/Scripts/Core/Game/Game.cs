using System.Collections.Generic;
using UnityEngine;

namespace Core.Game
{
    using Player;
    using Round;
    using Events;

    public class Game
    {
        public List<Player> Players { get; }
        public List<Team> Teams { get; }
        public Round CurrentRound { get; private set; }
        
        private readonly TichuEventBus _events = new TichuEventBus();
        public TichuEventBus Events => _events;        
        public bool IsOver { get; private set; }

        public Game(List<Player> players, List<Team> teams)
        {
            Players = players;
            Teams = teams;
            IsOver = false;
            
            Debug.Log($"Game Events instance: {Events.GetHashCode()}");

        }

        private void StartNewRound()
        {
            CurrentRound = new Round(Players, Teams, new Deck(), Events);
            CurrentRound.OnRoundComplete = OnRoundFinished;
            
            CurrentRound.Events.OnGameWon += i => HandleGameWon(i);
        }

        private void HandleGameWon(int teamIndex)
        {
            IsOver = true;
            Events.RaiseGameWon(teamIndex);
        }

        public bool SubmitGrandTichuDecision(int playerIndex, bool called)
            => CurrentRound.SubmitGrandTichuDecision(playerIndex, called);

        public bool SubmitCardExchange(int playerIndex, List<string> cardIds)
            => CurrentRound.SubmitCardExchange(playerIndex, cardIds);

        public bool SubmitMove(int playerIndex, List<string> cardIds)
            => CurrentRound.SubmitMove(playerIndex, cardIds);

        public bool SubmitTichuDeclaration(int playerIndex)
            => CurrentRound.SubmitTichuDeclaration(playerIndex);

        public void OnRoundFinished()
        {
            if (IsOver) return;
            StartNewRound();
        }

        public void Start()
        {
            Debug.Log("Game constructor - starting round");
            StartNewRound();
        }
    }
}