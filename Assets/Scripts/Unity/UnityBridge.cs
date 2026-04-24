using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Core.Card;
using Core.Game;
using Core.Player;
using Core.Round;
using UnityEngine;

public class UnityBridge : MonoBehaviour
{
    [SerializeField] private GrandTichuPanel    grandTichuPanel;
    [SerializeField] private ExchangePanel      exchangePanel;
    [SerializeField] private PlayPanel          playPanel;
    [SerializeField] private ScorePanel         scorePanel;
    [SerializeField] private RoundSummaryPanel  roundSummaryPanel;
    [SerializeField] private WishPanel wishPanel;

    private Game _game;
    private Queue<int> _exchangeQueue;

    void Start()
    {
        var players = new List<Player>
        {
            new HumanPlayer("Player 1"),
            new HumanPlayer("Player 2"),
            new HumanPlayer("Player 3"),
            new HumanPlayer("Player 4")
        };
        var teams = new List<Team>
        {
            new Team(0, players[0], players[2]),
            new Team(1, players[1], players[3])
        };

        _game = Game.GetInstance(players, teams);

        _game.Events.OnGrandTichuDecisionNeeded += HandleGrandTichuDecisionNeeded;
        _game.Events.OnExchangePhaseStarted     += HandleExchangePhaseStarted;
        _game.Events.OnCardsExchanged           += HandleCardsExchanged;
        _game.Events.OnTurnChanged              += HandleTurnChanged;
        _game.Events.OnCardsPlayed              += HandleCardsPlayed;
        _game.Events.OnPlayerPassed             += HandlePlayerPassed;
        _game.Events.OnTrickWon                 += HandleTrickWon;
        _game.Events.OnPlayerFinished           += HandlePlayerFinished;
        _game.Events.OnRoundScored              += HandleRoundScored;
        _game.Events.OnGameWon                  += HandleGameWon;
        _game.Events.OnWishNeeded               += HandleWishNeeded; 
        _game.StartNextRound();
    }

    void OnDestroy()
    {
        if (_game == null || _game.Events == null) return;

        _game.Events.OnGrandTichuDecisionNeeded -= HandleGrandTichuDecisionNeeded;
        _game.Events.OnExchangePhaseStarted     -= HandleExchangePhaseStarted;
        _game.Events.OnCardsExchanged           -= HandleCardsExchanged;
        _game.Events.OnTurnChanged              -= HandleTurnChanged;
        _game.Events.OnCardsPlayed              -= HandleCardsPlayed;
        _game.Events.OnPlayerPassed             -= HandlePlayerPassed;
        _game.Events.OnTrickWon                 -= HandleTrickWon;
        _game.Events.OnPlayerFinished           -= HandlePlayerFinished;
        _game.Events.OnRoundScored              -= HandleRoundScored;
        _game.Events.OnGameWon                  -= HandleGameWon;
        _game.Events.OnWishNeeded               -= HandleWishNeeded;
    }
    
    
    
    public void OnGrandTichuClicked(int playerIndex, bool called)
        => _game.SubmitGrandTichuDecision(playerIndex, called);

    public void OnCardExchangeSubmitted(int playerIndex, List<string> cardIds)
        => _game.SubmitCardExchange(playerIndex, cardIds);

    public void OnCardsPlayed(int playerIndex, List<string> cardIds)
        => _game.SubmitMove(playerIndex, cardIds);

    public void OnPassClicked(int playerIndex)
        => _game.SubmitMove(playerIndex, new List<string>());

    public void OnTichuClicked(int playerIndex)
        => _game.SubmitTichuDeclaration(playerIndex);

    public void OnNextRoundClicked()
    {
        roundSummaryPanel.gameObject.SetActive(false);
        StartCoroutine(StartNextRoundNextFrame());
    }
    public void OnWishSubmitted(int rank)
        => _game.SubmitWish((Rank)rank);

    private IEnumerator StartNextRoundNextFrame()
    {
        yield return null;
        _game.StartNextRound();
    }
    
    private void HandleWishNeeded(int playerIndex)
    {
        playPanel.gameObject.SetActive(false);
        wishPanel.Show(playerIndex);
    }
    
    private void HandleGrandTichuDecisionNeeded(string playerName, int playerIndex)
        => grandTichuPanel.ShowForPlayer(playerName, playerIndex);

    private void HandleExchangePhaseStarted()
    {
        playPanel.gameObject.SetActive(false);
        _exchangeQueue = new Queue<int>(new[] { 0, 1, 2, 3 });
        ShowNextExchange();
    }

    private void HandleCardsExchanged(int playerIndex, List<string> cardIds)
        => StartCoroutine(ShowNextExchangeNextFrame());

    private IEnumerator ShowNextExchangeNextFrame()
    {
        yield return null;
        ShowNextExchange();
    }

    private void HandleTurnChanged(int playerIndex)
    {
        var player = _game.CurrentRound.Players[playerIndex];
        if (player.Hand.Count == 0) return;

        var cardIds = player.Hand
            .Select(c => c.ToString())
            .ToList();

        playPanel.ShowTurn(playerIndex, cardIds);
    }

    private void HandleCardsPlayed(int playerIndex, List<string> cardIds)
        => playPanel.UpdateTrickLabel(
            $"Player {playerIndex + 1} played: {FormatCardList(cardIds)}");

    private void HandlePlayerPassed(int playerIndex)
        => playPanel.UpdateTrickLabel($"Player {playerIndex + 1} passed");

    private void HandleTrickWon(int playerIndex, List<string> cardIds)
        => playPanel.UpdateTrickLabel(
            $"Player {playerIndex + 1} won: {FormatCardList(cardIds)}");

    private void HandlePlayerFinished(int playerIndex)
        => playPanel.UpdateTrickLabel($"Player {playerIndex + 1} finished!");

    private void HandleRoundScored(int team1Round, int team2Round, int team1Total, int team2Total)
    {
        playPanel.gameObject.SetActive(false);
        roundSummaryPanel.Show(team1Round, team2Round, team1Total, team2Total);
    }

    private void HandleGameWon(int teamIndex)
    {
        playPanel.gameObject.SetActive(false);
        roundSummaryPanel.gameObject.SetActive(false);
        scorePanel.Show($"Team {teamIndex + 1} wins!");
    }


    private void ShowNextExchange()
    {
        if (_exchangeQueue == null || _exchangeQueue.Count == 0) return;

        int playerIndex = _exchangeQueue.Dequeue();
        var cardIds = _game.CurrentRound.Players[playerIndex].Hand
            .Select(c => c.ToString()).ToList();

        exchangePanel.ShowForPlayer(playerIndex, cardIds);
    }

    private string FormatCardList(List<string> cardIds)
        => string.Join(", ", cardIds.Select(FormatCard));

    private string FormatCard(string cardId)
    {
        if (cardId.StartsWith("Special"))
        {
            return cardId.Split('_')[1] switch
            {
                "Mahjong" => "1",
                "Dragon"  => "Dr",
                "Phoenix" => "Ph",
                "Dog"     => "Dog",
                _         => cardId
            };
        }

        var parts = cardId.Split('_');
        if (parts.Length < 2) return cardId;

        var rank = parts[0] switch
        {
            "Two"   => "2",  "Three" => "3",
            "Four"  => "4",  "Five"  => "5",
            "Six"   => "6",  "Seven" => "7",
            "Eight" => "8",  "Nine"  => "9",
            "Ten"   => "10", "Jack"  => "J",
            "Queen" => "Q",  "King"  => "K",
            "Ace"   => "A",  _       => parts[0]
        };

        var suit = parts[1] switch
        {
            "Jade"   => "♦",
            "Sword"  => "♠",
            "Pagoda" => "♣",
            "Star"   => "*",
            _        => ""
        };

        return $"{rank} {suit}";
    }
}