using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnLabel;
    [SerializeField] private TextMeshProUGUI trickLabel;
    [SerializeField] private HandUI handUI;
    [SerializeField] private Button playButton;
    [SerializeField] private Button passButton;
    [SerializeField] private Button tichuButton;
    [SerializeField] private UnityBridge bridge;

    private int _currentPlayerIndex;

    void Awake()
    {
        playButton.onClick.AddListener(OnPlayClicked);
        passButton.onClick.AddListener(OnPassClicked);
        tichuButton.onClick.AddListener(OnTichuClicked);
    }

    public void ShowTurn(int playerIndex, List<string> cardIds)
    {
        gameObject.SetActive(true);
        _currentPlayerIndex = playerIndex;
        turnLabel.text      = $"Player {playerIndex + 1}'s turn";
        handUI.ShowHand(cardIds);
        handUI.SetInteractable(true);
    }

    public void UpdateTrickLabel(string text)
        => trickLabel.text = text;

    private void OnPlayClicked()
    {
        var selected = handUI.GetSelectedCardIds();
        if (selected.Count == 0) return;
        bridge.OnCardsPlayed(_currentPlayerIndex, selected);
    }

    private void OnPassClicked()
        => bridge.OnPassClicked(_currentPlayerIndex);

    private void OnTichuClicked()
        => bridge.OnTichuClicked(_currentPlayerIndex);
}