using System;
using System.Collections.Generic;

namespace Core.Events
{
    using Player;
    using Card;
    using Combination;

    public static class TichuEventBus
    {
        public static event Action<IReadOnlyList<Player>> OnFirstCardsDealt;
        public static event Action<IReadOnlyList<Player>> OnAllCardsDealt;

        public static event Action<GrandTichuDecisionNeededEvent> OnGrandTichuDecisionNeeded;
        public static event Action<int> OnGrandTichuDeclared;

        public static event Action<int> OnTichuDeclared;

        public static event Action OnExchangePhaseStarted;
        public static event Action<int, IReadOnlyList<Card>> OnCardsExchanged;

        public static event Action<int> OnTurnChanged;
        public static event Action<int, IReadOnlyList<Card>, Combination> OnCardsPlayed;
        public static event Action<int> OnPlayerPassed;
        public static event Action<int, IReadOnlyList<Card>> OnTrickWon;
        public static event Action<int, IReadOnlyList<Card>> OnBombPlayed;
        public static event Action<int, int> OnWishMade;
        public static event Action<int> OnDragonGiftNeeded;

        public static event Action<int> OnPlayerFinished;
        public static event Action<int> OnGameWon;

        internal static void RaiseFirstCardsDealt(IReadOnlyList<Player> p)
            => OnFirstCardsDealt?.Invoke(p);

        internal static void RaiseAllCardsDealt(IReadOnlyList<Player> p)
            => OnAllCardsDealt?.Invoke(p);

        internal static void RaiseGrandTichuDecisionNeeded(GrandTichuDecisionNeededEvent e)
            => OnGrandTichuDecisionNeeded?.Invoke(e);

        internal static void RaiseGrandTichuDeclared(int playerIndex)
            => OnGrandTichuDeclared?.Invoke(playerIndex);

        internal static void RaiseTichuDeclared(int playerIndex)
            => OnTichuDeclared?.Invoke(playerIndex);

        internal static void RaiseExchangePhaseStarted()
            => OnExchangePhaseStarted?.Invoke();

        internal static void RaiseCardsExchanged(int playerIndex, IReadOnlyList<Card> cards)
            => OnCardsExchanged?.Invoke(playerIndex, cards);

        internal static void RaiseTurnChanged(int playerIndex)
            => OnTurnChanged?.Invoke(playerIndex);

        internal static void RaiseCardsPlayed(int playerIndex, IReadOnlyList<Card> cards, Combination combo)
            => OnCardsPlayed?.Invoke(playerIndex, cards, combo);

        internal static void RaisePlayerPassed(int playerIndex)
            => OnPlayerPassed?.Invoke(playerIndex);

        internal static void RaiseTrickWon(int playerIndex, IReadOnlyList<Card> cards)
            => OnTrickWon?.Invoke(playerIndex, cards);

        internal static void RaiseBombPlayed(int playerIndex, IReadOnlyList<Card> cards)
            => OnBombPlayed?.Invoke(playerIndex, cards);

        internal static void RaiseWishMade(int playerIndex, int rank)
            => OnWishMade?.Invoke(playerIndex, rank);

        internal static void RaiseDragonGiftNeeded(int winnerIndex)
            => OnDragonGiftNeeded?.Invoke(winnerIndex);

        internal static void RaisePlayerFinished(int playerIndex)
            => OnPlayerFinished?.Invoke(playerIndex);
        

        internal static void RaiseGameWon(int teamIndex)
            => OnGameWon?.Invoke(teamIndex);
    }
}