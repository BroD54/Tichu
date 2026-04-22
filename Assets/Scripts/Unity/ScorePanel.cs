using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultLabel;
    [SerializeField] private Button restartButton;

    void Awake()
    {
        restartButton.onClick.AddListener(OnRestartClicked);
    }

    public void Show(string message)
    {
        gameObject.SetActive(true);
        resultLabel.text = message;
    }

    private void OnRestartClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}