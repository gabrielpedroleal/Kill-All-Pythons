using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public UIManager UIManager;

    public AudioManager AudioManager;
    public InputManager InputManager { get; private set; }

    [SerializeField] private GameObject bossDoor;

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
}
