using System.Collections.Generic;
using Core.Card;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WishPanel : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown rankDropdown;
    [SerializeField] private Button confirmButton;
    [SerializeField] private UnityBridge bridge;

    private static readonly List<(string label, Rank rank)> Ranks = new()
    {
        ("None", Rank.Mahjong), ("2", Rank.Two), ("3", Rank.Three), ("4", Rank.Four),
        ("5", Rank.Five), ("6", Rank.Six), ("7", Rank.Seven),
        ("8", Rank.Eight), ("9", Rank.Nine), ("10", Rank.Ten),
        ("J", Rank.Jack), ("Q", Rank.Queen), ("K", Rank.King), ("A", Rank.Ace)
    };


    void Awake()
    {
        rankDropdown.ClearOptions();
        rankDropdown.AddOptions(Ranks.ConvertAll(r => r.label));
        confirmButton.onClick.AddListener(OnConfirm);
    }

    public void Show(int playerIndex)
    {
        gameObject.SetActive(true);
        rankDropdown.value = 0;
    }

    private void OnConfirm()
    {
        var rank = Ranks[rankDropdown.value].rank;
        gameObject.SetActive(false);
        bridge.OnWishSubmitted((int)rank);
    }
    
}