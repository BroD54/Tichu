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
        foreach (var id in cardIds)
        {
            var obj  = Instantiate(cardPrefab, cardContainer, false);
            var card = obj.GetComponent<CardUI>();
            card.Init(id);
            _cards.Add(card);
        }

        Debug.Log($"ShowHand built {_cards.Count} cards");
    }

    public void SetInteractable(bool interactable)
    {
        foreach (var card in _cards)
            card.SetInteractable(interactable);
    }

    public List<string> GetSelectedCardIds()
    {
        foreach (var card in _cards)
            Debug.Log($"Card in list: {card.CardId}, IsSelected: {card.IsSelected}, ID: {card.GetInstanceID()}");
    
        return _cards.Where(c => c.IsSelected).Select(c => c.CardId).ToList();
    }
}