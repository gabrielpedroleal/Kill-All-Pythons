using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int lives;
    private bool isDead = false;

    public event Action OnDead;
    public event Action OnHurt;

    public void GetHealth()
    {

    }

    public void TakeDamage()
    {
        lives--;
        HandleDamageTaken();
    }

    private void HandleDamageTaken()
    {
        if (isDead) return;
        if (lives <= 0)
        {
            OnDead?.Invoke();
        }
        else
        {
            OnHurt?.Invoke();
        }
    }
}
