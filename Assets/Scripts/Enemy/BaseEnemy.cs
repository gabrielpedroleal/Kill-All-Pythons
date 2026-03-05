using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public abstract class BaseEnemy : MonoBehaviour
{
    protected Animator animator;
    protected AudioSource audioSource;
    public Health enemyHealth;
    protected private bool isDead;

    [SerializeField] private ParticleSystem hitParticle;
    

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<Health>();
        enemyHealth.OnHurt += EnemyHurt;
        enemyHealth.OnDead += EnemyDeath;
  
    }

    protected abstract void Update();

    private void EnemyHurt()
    {
        if (isDead) return;
        animator.SetTrigger("hurt");
        PlayHitParticle();
    }

    private void EnemyDeath()
    { 
        isDead = true;
        animator.SetTrigger("dead");
        PlayHitParticle();
        StartCoroutine(DestroyEnemy(2));
    }

    private IEnumerator DestroyEnemy(int time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }

    private void PlayHitParticle()
    {
        ParticleSystem instantiatedParticle = Instantiate(hitParticle, transform.position, transform.rotation);
        instantiatedParticle.Play();
    }

}
