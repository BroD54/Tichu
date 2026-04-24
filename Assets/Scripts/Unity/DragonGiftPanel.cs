using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DragonGiftPanel : MonoBehaviour
{
    [SerializeField] private Button opponent1Button;
    [SerializeField] private Button opponent2Button;
    [SerializeField] private TMP_Text titleLabel;
    [SerializeField] private UnityBridge bridge;
    
    private static readonly int[][] Opponents = {
        new[]{1,3}, new[]{0,2}, new[]{1,3}, new[]{0,2}
    };

    void Awake()
    {
        gameObject.SetActive(true);
    }

    public void Show(int winnerIndex)
    {
        gameObject.SetActive(true);
        titleLabel.text = $"Player {winnerIndex + 1}: give Dragon trick to opponent";

        var opps = Opponents[winnerIndex];
        opponent1Button.GetComponentInChildren<TMP_Text>().text = $"Player {opps[0] + 1}";
        opponent2Button.GetComponentInChildren<TMP_Text>().text = $"Player {opps[1] + 1}";

        opponent1Button.onClick.RemoveAllListeners();
        opponent2Button.onClick.RemoveAllListeners();
        opponent1Button.onClick.AddListener(() => Submit(opps[0]));
        opponent2Button.onClick.AddListener(() => Submit(opps[1]));
    }

    private void Submit(int opponentIndex)
    {
        gameObject.SetActive(false);
        bridge.OnDragonGiftSubmitted(opponentIndex);
    }
}