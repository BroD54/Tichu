using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HandUI : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardContainer;

    private List<CardUI> _cards = new();

    public void ShowHand(List<string> cardIds)
    {
        // Clear list first
        _cards.Clear();
    
        // Destroy all children immediately
        List<Transform> children = new List<Transform>();
        foreach (Transform child in cardContainer)
            children.Add(child);
        foreach (Transform child in children)
            DestroyImmediate(child.gameObject);

        // Build fresh
        var sorted = SortCardIds(cardIds);

        
        foreach (var id in sorted)
        {
            var obj  = Instantiate(cardPrefab, cardContainer, false);
            var card = obj.GetComponent<CardUI>();
            card.Init(id);
            _cards.Add(card);
        }

    }

    public void SetInteractable(bool interactable)
    {
        foreach (var card in _cards)
            card.SetInteractable(interactable);
    }

    public List<string> GetSelectedCardIds()
    {
        return _cards.Where(c => c.IsSelected).Select(c => c.CardId).ToList();
    }
    
    private List<string> SortCardIds(List<string> cardIds)
    {
        var specials = new[] { "Special_Mahjong", "Special_Dog", "Special_Phoenix", "Special_Dragon" };
    
        var normal  = cardIds.Where(id => !specials.Contains(id)).ToList();
        var special = cardIds.Where(id => specials.Contains(id)).ToList();

        normal.Sort((a, b) =>
        {
            int rankA = GetRankValue(a);
            int rankB = GetRankValue(b);
            return rankA.CompareTo(rankB);
        });

        return normal.Concat(special).ToList();
    }

    private int GetRankValue(string cardId)
    {
        var rank = cardId.Split('_')[0];
        return rank switch
        {
            "Two"   => 2,
            "Three" => 3,
            "Four"  => 4,
            "Five"  => 5,
            "Six"   => 6,
            "Seven" => 7,
            "Eight" => 8,
            "Nine"  => 9,
            "Ten"   => 10,
            "Jack"  => 11,
            "Queen" => 12,
            "King"  => 13,
            "Ace"   => 14,
            _       => 0
        };
    }
}