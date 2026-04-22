using UnityEngine;
using Core.Events;
using Core.Player;
using System.Collections.Generic;
using Core.Card;
using Core.Combination;

public class HandUIController : MonoBehaviour
{
    [SerializeField] private int playerIndex;

    private void OnEnable()
    {
        // TichuEventBus.OnFirstCardsDealt += HandleCardsDealt;
        // TichuEventBus.OnCardsPlayed    += HandleCardsPlayed;
    }

    private void OnDisable()
    {
        // TichuEventBus.OnFirstCardsDealt -= HandleCardsDealt;
        // TichuEventBus.OnCardsPlayed    -= HandleCardsPlayed;
    }

    private void HandleCardsDealt(IReadOnlyList<Player> players)
    {
        RenderHand(players[playerIndex].Hand);
    }

    private void HandleCardsPlayed(int who, IReadOnlyList<Card> cards, Combination _)
    {
        if (who == playerIndex) RemoveCardsFromDisplay(cards);
    }

    private void RenderHand(IReadOnlyList<Card> cards) { /* spawn card GameObjects */ }
    private void RemoveCardsFromDisplay(IReadOnlyList<Card> cards) { /* destroy them */ }
}