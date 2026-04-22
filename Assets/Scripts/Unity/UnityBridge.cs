

using System.Collections.Generic;
using Core.Game;
using UnityEngine;

public class UnityBridge : MonoBehaviour
{
    // private Game _game;
    //
    // void Start()
    // {
    //     var players = BuildPlayers();
    //     var teams   = BuildTeams(players);
    //     _game = new Game(players, teams);
    //
    //     _game.Events.OnGrandTichuDecisionNeeded += HandleGrandTichuDecisionNeeded;
    //     _game.Events.OnTurnChanged              += HandleTurnChanged;
    //     _game.Events.OnTrickWon                 += HandleTrickWon;
    //     _game.Events.OnGameWon                  += HandleGameWon;
    // }
    //
    // public void OnPlayCardsClicked(int playerIndex, List<string> cardIds)
    //     => _game.SubmitMove(playerIndex, cardIds);
    //
    // public void OnGrandTichuClicked(int playerIndex, bool called)
    //     => _game.SubmitGrandTichuDecision(playerIndex, called);
}