using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GrandTichuPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameLabel;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    [SerializeField] private UnityBridge bridge;

    private int _currentPlayerIndex;

    void Start()
    {
        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
    }

    public void ShowForPlayer(string playerName, int playerIndex)
    {
        Debug.Log("Panel is active for player: " + playerName);
        gameObject.SetActive(true);
        _currentPlayerIndex  = playerIndex;
        playerNameLabel.text = $"{playerName}: Call Grand Tichu?";
    }

    private void OnYesClicked()
    {
        gameObject.SetActive(false);
        bridge.OnGrandTichuClicked(_currentPlayerIndex, true);
    }

    private void OnNoClicked()
    {
        gameObject.SetActive(false);
        bridge.OnGrandTichuClicked(_currentPlayerIndex, false);
    }
}