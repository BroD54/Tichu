using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    public string CardId { get; private set; }
    public bool IsSelected { get; private set; }

    private Button _button;
    private Image _background;
    private TMP_Text _label;

    void Awake()
    {
        _button     = GetComponent<Button>();
        _background = GetComponent<Image>();
        _label      = GetComponentInChildren<TMP_Text>();
        
        _button.onClick.AddListener(OnClicked);
    }

    public void Init(string cardId)
    {
        CardId            = cardId;
        IsSelected        = false;
        _background.color = Color.white;
        _label.text       = FormatCardLabel(cardId);
        _label.color      = GetSuitColor(cardId);
    }

    private string FormatCardLabel(string cardId)
    {
        if (cardId.StartsWith("Special"))
        {
            return cardId.Split('_')[1] switch
            {
                "Mahjong" => "1",
                "Dragon"  => "Dr",
                "Phoenix" => "Ph",
                "Dog"     => "Dog",
                _         => cardId
            };
        }

        var parts = cardId.Split('_');
        if (parts.Length < 2) return cardId;

        var rankSymbol = parts[0] switch
        {
            "Two"   => "2",  "Three" => "3",
            "Four"  => "4",  "Five"  => "5",
            "Six"   => "6",  "Seven" => "7",
            "Eight" => "8",  "Nine"  => "9",
            "Ten"   => "10", "Jack"  => "J",
            "Queen" => "Q",  "King"  => "K",
            "Ace"   => "A",  _       => parts[0]
        };

        var suitSymbol = parts[1] switch
        {
            "Jade"   => "♦",
            "Sword"  => "♠",
            "Pagoda" => "♣",
            "Star"   => "*",   // plain asterisk instead of ★
            _        => ""
        };

        return $"{rankSymbol} {suitSymbol}";
    }

    private Color GetSuitColor(string cardId)
    {
        if (cardId.StartsWith("Special")) return Color.black;

        var suit = cardId.Split('_')[1];
        return suit switch
        {
            "Jade"   => new Color(0.0f, 0.6f, 0.0f),  // green
            "Sword"  => new Color(0.1f, 0.1f, 0.1f),  // dark grey
            "Pagoda" => new Color(0.0f, 0.4f, 0.8f),  // blue
            "Star"   => new Color(0.8f, 0.1f, 0.1f),  // red
            _        => Color.black
        };
    }

    public void SetInteractable(bool interactable)
        => _button.interactable = interactable;

    private void OnClicked()
    {
        IsSelected = !IsSelected;
        _background.color = IsSelected ? Color.cornsilk : Color.white;
    }
}