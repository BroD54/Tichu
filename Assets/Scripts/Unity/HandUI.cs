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
        Debug.Log("ShowHand called with " + cardIds.Count + " cards");

        foreach (Transform child in cardContainer)
            Destroy(child.gameObject);
        _cards.Clear();

        foreach (var id in cardIds)
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

    public List<string> GetSelectedCardIds() =>
        _cards.Where(c => c.IsSelected).Select(c => c.CardId).ToList();
}