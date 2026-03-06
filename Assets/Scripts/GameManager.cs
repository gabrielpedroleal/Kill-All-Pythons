using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public UIManager UIManager;

    public AudioManager AudioManager;
    public InputManager InputManager { get; private set; }

    [SerializeField] private GameObject bossDoor;
    [SerializeField] private PlayerBehavior player;
    [SerializeField] private BossBehavior boss;
    [SerializeField] private BossFightTrigger bossFightTrigger;

    private int totalKeys;
    private int keysLeftToCollect;

    private void Awake()
    {
        if (Instance != null) Destroy(this.gameObject);
        Instance = this;

        InputManager = new InputManager();

        totalKeys = FindObjectsOfType<CollectableKey>().Length;
        keysLeftToCollect = totalKeys;
        UIManager.UpdateKeysLefText(totalKeys, keysLeftToCollect);
        bossFightTrigger.OnPlayerEnteredBossFight += ActivateBossBehavior;

        player.GetComponent<Health>().OnDead += HandleGameOver;
        boss.GetComponent<Health>().OnDead += HandleVictory;
    }


    public void UpdateKeysLeft()
    {
        keysLeftToCollect--;
        UIManager.UpdateKeysLefText(totalKeys, keysLeftToCollect);
        CheckAllKeysCollect();
    }

    private void CheckAllKeysCollect()
    {
        if(keysLeftToCollect <= 0)
        {
            Destroy(bossDoor);
        }
    }

    private void HandleGameOver()
    {
        UIManager.OpenGameOverPanel();
    }

    private void HandleVictory()
    {
       UIManager.OpenVictoryText();
        StartCoroutine(GoToCreditsScene());
    }

    private IEnumerator GoToCreditsScene()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Credits");
    }

    private void ActivateBossBehavior()
    {
        boss.StartChasing();
    }

    public void UpdateHealth(int amount)
    {
        UIManager.UptadeHealthLeftText(amount);
    }

    public PlayerBehavior GetPlayer() => player;
}
