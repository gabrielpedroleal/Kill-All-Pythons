using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public abstract class BaseEnemy : MonoBehaviour
{
    protected Animator animator;
    protected AudioSource audioSource;
    public Health enemyHealth;
    private bool isDead;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<Health>();
        enemyHealth.OnHurt += EnemyHurtAnim;
        enemyHealth.OnDead += HandleEnemyDeath;
  
    }

    protected abstract void Update();

    private void EnemyHurtAnim()
    {
        if (isDead) return;
        animator.SetTrigger("hurt");
    }

    private void HandleEnemyDeath()
    {
        isDead = true;
        animator.SetTrigger("dead");
        StartCoroutine(DestroyEnemy(1));
    }

    private IEnumerator DestroyEnemy(int time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }

}
