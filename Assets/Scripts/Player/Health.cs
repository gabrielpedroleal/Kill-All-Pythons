using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int lives;
    private bool isDead = false;

    public event Action OnDead;
    public event Action OnHurt;

    public bool IsDead => isDead;

    public void TakeDamage()
    {
        if (IsDead) return;
        lives--;
        HandleDamageTaken();
    }

    private void HandleDamageTaken()
    {
        if (isDead) return;
        if (lives <= 0)
        {
            isDead = true;
            OnDead?.Invoke();
        }
        else
        {
            OnHurt?.Invoke();
        }
    }

    public int GetHealth() => lives;

}
