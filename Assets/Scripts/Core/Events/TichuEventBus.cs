using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Events
{
    public class TichuEventBus
    {
        public event Action<IReadOnlyList<string>> OnFirstCardsDealt;
        public event Action<IReadOnlyList<string>> OnAllCardsDealt;

        public event Action<string, int> OnGrandTichuDecisionNeeded;
        public event Action<int> OnGrandTichuDeclared;
        public event Action<int> OnTichuDeclared;

        public event Action OnExchangePhaseStarted;
        public event Action<int, List<string>> OnCardsExchanged;

        public event Action<int> OnTurnChanged;
        public event Action<int, List<string>> OnCardsPlayed;
        public event Action<int> OnPlayerPassed;
        public event Action<int, List<string>> OnTrickWon;
        public event Action<int, List<string>> OnBombPlayed;
        public event Action<int, int> OnWishMade;
        public event Action<int> OnDragonGiftNeeded;
        public event Action<int> OnPlayerFinished;
        public event Action<int> OnGameWon;

        public TichuEventBus()
        {
        }

        public void RaiseFirstCardsDealt(IReadOnlyList<string> playerNames)
            => OnFirstCardsDealt?.Invoke(playerNames);

        public void RaiseAllCardsDealt(IReadOnlyList<string> playerNames)
            => OnAllCardsDealt?.Invoke(playerNames);

        public void RaiseGrandTichuDecisionNeeded(string playerName, int playerIndex)
        {
            Debug.Log($"Raise EventBus instance: {this.GetHashCode()}");
            Debug.Log("Need decision from" +  playerName);
            OnGrandTichuDecisionNeeded?.Invoke(playerName, playerIndex);
        }

        public void RaiseGrandTichuDeclared(int playerIndex)
            => OnGrandTichuDeclared?.Invoke(playerIndex);

        public void RaiseTichuDeclared(int playerIndex)
            => OnTichuDeclared?.Invoke(playerIndex);

        public void RaiseExchangePhaseStarted()
            => OnExchangePhaseStarted?.Invoke();

        public void RaiseCardsExchanged(int playerIndex, List<string> cardIds)
            => OnCardsExchanged?.Invoke(playerIndex, cardIds);

        public void RaiseTurnChanged(int playerIndex)
            => OnTurnChanged?.Invoke(playerIndex);

        public void RaiseCardsPlayed(int playerIndex, List<string> cardIds)
            => OnCardsPlayed?.Invoke(playerIndex, cardIds);

        public void RaisePlayerPassed(int playerIndex)
            => OnPlayerPassed?.Invoke(playerIndex);

        public void RaiseTrickWon(int playerIndex, List<string> cardIds)
            => OnTrickWon?.Invoke(playerIndex, cardIds);

        public void RaiseBombPlayed(int playerIndex, List<string> cardIds)
            => OnBombPlayed?.Invoke(playerIndex, cardIds);

        public void RaiseWishMade(int playerIndex, int rank)
            => OnWishMade?.Invoke(playerIndex, rank);

        public void RaiseDragonGiftNeeded(int winnerIndex)
            => OnDragonGiftNeeded?.Invoke(winnerIndex);

        public void RaisePlayerFinished(int playerIndex)
            => OnPlayerFinished?.Invoke(playerIndex);

        public void RaiseGameWon(int teamIndex)
            => OnGameWon?.Invoke(teamIndex);
    }
}