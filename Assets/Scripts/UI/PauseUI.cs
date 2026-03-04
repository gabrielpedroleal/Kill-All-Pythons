using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitToMenuButton;

    private void Awake()
    {
        continueButton.onClick.AddListener(ClosePauseMenu);
        optionsButton.onClick.AddListener(OpenOptionsMenu);
        quitToMenuButton.onClick.AddListener(GoToMainMenu);

    }

    private void ClosePauseMenu() 
    {
        GameManager.Instance.AudioManager.PlaySFX(SFX.ButtonClick);
        GameManager.Instance.UIManager.OpenClosePauseMenu();
    }

    private void OpenOptionsMenu()
    {
        GameManager.Instance.AudioManager.PlaySFX(SFX.ButtonClick);
        GameManager.Instance.UIManager.OpenOptionPanel();
    }

    private void GoToMainMenu()
    {
        GameManager.Instance.AudioManager.PlaySFX(SFX.ButtonClick);
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuAnimated");
    }

}
