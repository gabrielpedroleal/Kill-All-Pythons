using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    [SerializeField] private Transform detectPosition;
    [SerializeField] private Vector2 detectBoxSize;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attackCooldown;
    [SerializeField] private AudioClip[] audioClips;
  
  

    private float cooldownTimer;

    protected override void Awake()
    {
        base.Awake();
        base.enemyHealth.OnHurt += PlayHurtAudio;
        base.enemyHealth.OnDead += PlayDeadAudio;
      
    }

    private void PlayHurtAudio()
    {
        GameManager.Instance.AudioManager.PlaySFX(SFX.EnemyHurt);
    }

    private void PlayDeadAudio()
    {
        GameManager.Instance.AudioManager.PlaySFX(SFX.EnemyDeath);
    }

    protected override void Update()
    {
        cooldownTimer += Time.deltaTime;
        VerifyCanAttack();
    }

   

    private void VerifyCanAttack()
    {
        if (cooldownTimer < attackCooldown) return;
        if (PlayerInSight())
        {
            animator.SetTrigger("attack");
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        GameManager.Instance.AudioManager.PlaySFX(SFX.EnemyAttack);

        cooldownTimer = 0;
        if(CheckPlayerInDetectArea().TryGetComponent(out Health playerHealth))
        {
            print("Making player take damage");
            playerHealth.TakeDamage();
        }
    }

    private Collider2D CheckPlayerInDetectArea()
    {
        return Physics2D.OverlapBox(detectPosition.position, detectBoxSize, 0f, playerLayer);
    }

    private bool PlayerInSight()
    {
        Collider2D playerCollider = CheckPlayerInDetectArea();
        return playerCollider != null;
    }

    private void OnDrawGizmos()
    {
        if (detectPosition == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(detectPosition.position, detectBoxSize);
    }

}
