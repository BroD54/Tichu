using System.Collections.Generic;

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
        public TichuEventBus Events { get; }
        public bool IsOver { get; private set; }

        public Game(List<Player> players, List<Team> teams)
        {
            Players = players;
            Teams = teams;
            Events = new TichuEventBus();
            IsOver = false;

            StartNewRound();
        }

        private void StartNewRound()
        {
            CurrentRound = new Round(Players, Teams, new Deck());
            CurrentRound.OnRoundComplete = OnRoundFinished;
            
            CurrentRound.Events.OnCardsPlayed      += (i, c)    => Events.RaiseCardsPlayed(i, c);
            CurrentRound.Events.OnTurnChanged      += i         => Events.RaiseTurnChanged(i);
            CurrentRound.Events.OnTrickWon         += (i, c)    => Events.RaiseTrickWon(i, c);
            CurrentRound.Events.OnPlayerFinished   += i         => Events.RaisePlayerFinished(i);
            CurrentRound.Events.OnTichuDeclared    += i         => Events.RaiseTichuDeclared(i);
            CurrentRound.Events.OnGrandTichuDeclared += i       => Events.RaiseGrandTichuDeclared(i);
            CurrentRound.Events.OnGrandTichuDecisionNeeded += (n, i) => Events.RaiseGrandTichuDecisionNeeded(n, i);
            CurrentRound.Events.OnExchangePhaseStarted     += () => Events.RaiseExchangePhaseStarted();
            CurrentRound.Events.OnCardsExchanged   += (i, c)    => Events.RaiseCardsExchanged(i, c);
            CurrentRound.Events.OnGameWon          += i         => HandleGameWon(i);
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
    }
}