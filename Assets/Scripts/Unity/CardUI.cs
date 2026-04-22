using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    public string CardId { get; private set; }
    public bool IsSelected { get; private set; }

    [SerializeField] private Button button;
    [SerializeField] private Image background;

    [SerializeField] private TMP_Text label;

    void Awake()
    {
        button.onClick.AddListener(OnClicked);
    }

    public void Init(string cardId)
    {
        Debug.Log($"label={label}, background={background}, cardId={cardId}");
        
        CardId = cardId;

        label.text = cardId;

        IsSelected = false;
        background.color = Color.white;
    }
    
    public void SetInteractable(bool interactable)
        =>  button.interactable = interactable;

    private void OnClicked()
    {
        Debug.Log($"cardId={CardId}");
        IsSelected = !IsSelected;
        background.color = IsSelected ? Color.yellow : Color.white;
    }
}