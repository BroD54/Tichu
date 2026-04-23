using System.Collections.Generic;
using Core.Events;
using Core.Game;
using Core.Player;
using Core.Round;

public class Game
{
    private static Game _game; 
    public List<Player> Players { get; }
    public List<Team> Teams { get; }
    public Round CurrentRound { get; private set; }
    public TichuEventBus Events { get; }
    public bool IsOver { get; private set; }

    private Game(List<Player> players, List<Team> teams)
    {
        Players = players;
        Teams   = teams;
        Events  = new TichuEventBus();
        IsOver  = false;
    }
    public static Game GetInstance(List<Player> players, List<Team> teams)
    {
        if (_game == null)
        {
            _game = new Game(players, teams);
        }
        return _game;
    }

    public void StartNextRound()
    {
        if (IsOver) return;
        StartNewRound();
    }

    private void StartNewRound()
    {
        CurrentRound = new Round(Players, Teams, new Deck(), Events);
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
    
}