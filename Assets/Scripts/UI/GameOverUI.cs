using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button backToMenuButton;

    private void Awake()
    {
        restartButton.onClick.AddListener(RestartGame);
        backToMenuButton.onClick.AddListener(BackToMenu);
    }

    private void BackToMenu()
    {
        SceneManager.LoadScene("MenuAnimated");
    }

    private void RestartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
