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

        Debug.Log($"CardUI Awake — button={_button}, background={_background}, label={_label}");

        _button.onClick.AddListener(OnClicked);
    }

    public void Init(string cardId)
    {
        CardId           = cardId;
        _label.text      = cardId;
        IsSelected       = false;
        _background.color = Color.white;
    }

    public void SetInteractable(bool interactable)
        => _button.interactable = interactable;

    private void OnClicked()
    {
        IsSelected = !IsSelected;
        _background.color = IsSelected ? Color.yellow : Color.white;
        Debug.Log($"Card clicked: {CardId}, IsSelected: {IsSelected}, this.GetInstanceID(): {GetInstanceID()}");
    }
}