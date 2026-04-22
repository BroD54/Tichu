using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoundSummaryPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roundScoreLabel;
    [SerializeField] private TextMeshProUGUI totalScoreLabel;
    [SerializeField] private Button nextRoundButton;
    [SerializeField] private UnityBridge bridge;

    void Awake()
    {
        nextRoundButton.onClick.AddListener(OnNextRoundClicked);
    }

    public void Show(int team1Round, int team2Round, int team1Total, int team2Total)
    {
        gameObject.SetActive(true);
        roundScoreLabel.text = $"Round Score — Team 1: {team1Round}  |  Team 2: {team2Round}";
        totalScoreLabel.text = $"Total Score — Team 1: {team1Total}  |  Team 2: {team2Total}";
    }

    private void OnNextRoundClicked()
    {
        gameObject.SetActive(false);
        bridge.OnNextRoundClicked();
    }
}