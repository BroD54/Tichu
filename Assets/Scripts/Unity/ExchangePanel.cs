using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExchangePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI instructionLabel;
    [SerializeField] private HandUI handUI;
    [SerializeField] private Button confirmButton;
    [SerializeField] private UnityBridge bridge;

    private int _currentPlayerIndex;

    void Awake()
    {
        confirmButton.onClick.AddListener(OnConfirmClicked);
    }

    public void ShowForPlayer(int playerIndex, List<string> cardIds)
    {
        gameObject.SetActive(true);
        _currentPlayerIndex  = playerIndex;
        instructionLabel.text = $"Player {playerIndex + 1}: Select 3 cards to exchange";
        handUI.ShowHand(cardIds);
        handUI.SetInteractable(true);
    }

    public void OnConfirmClicked()
    {
        var selected = handUI.GetSelectedCardIds();
        Debug.Log($"Selected {selected.Count} cards: {string.Join(", ", selected)}");
    
        if (selected.Count != 3)
        {
            instructionLabel.text = $"Select exactly 3 cards (you selected {selected.Count})";
            return;
        }

        gameObject.SetActive(false);
        Debug.Log($"Submitting exchange for player {_currentPlayerIndex}");
        bridge.OnCardExchangeSubmitted(_currentPlayerIndex, selected);
    }
}