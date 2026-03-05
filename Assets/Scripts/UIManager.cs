using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keysText;
    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] private GameObject OptionsPanel;
    [SerializeField] private GameObject PausePanel;

    private void Awake()
    {
        OptionsPanel.SetActive(false);
        PausePanel.SetActive(false);

    }

    private void Start()
    {
        GameManager.Instance.InputManager.OnMenuOpenClose += OpenClosePauseMenu;
    }

    public void OpenClosePauseMenu()
    {
        bool isPausing = !PausePanel.activeSelf;
        PausePanel.SetActive(isPausing);
        if (isPausing)
        {
            Time.timeScale = 0f;
            GameManager.Instance.InputManager.DisablePlayerGameplayInput();
        }
        else 
        {
            Time.timeScale = 1f;
            GameManager.Instance.InputManager.EnablePlayerGameplayInput();
            OptionsPanel.SetActive(false);
        }
    }

    public void OpenOptionPanel()
    {
       OptionsPanel.SetActive(true);
       PausePanel.SetActive(false);
    }

    public void CloseOptionPanel() 
    {
        OptionsPanel.SetActive(false);
        PausePanel.SetActive(true);
    }

    public void UpdateKeysLefText(int totalValue, int leftValue)
    {
        keysText.text = $"{leftValue}/{totalValue}";
    }

    public void UptadeHealthLeftText(int amount) 
    {
        healthText.text = $"{amount}";
    }
}
