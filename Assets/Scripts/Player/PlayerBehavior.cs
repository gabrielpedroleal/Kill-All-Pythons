using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    [Header("Propriedades de movimentação")]
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float jumpForce = 10;

    [Header("Propriedades de ataque")]
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private Transform attackPosition;
    [SerializeField] private LayerMask attackLayer;
    [SerializeField] private ParticleSystem hitParticle;

    private Rigidbody2D rigidbody;
    private IsGroundedChecker isGroundedChecker;
    private float moveDirection;
    private Health health;
    private Vector3 initialScale;

    private void Awake()
    {
        initialScale = transform.localScale;
        rigidbody = GetComponent<Rigidbody2D>();
        isGroundedChecker = GetComponent<IsGroundedChecker>();
        health = GetComponent<Health>();

        health.OnDead += HandlePlayerDeath;
        health.OnHurt += HandleHurt;
    }

    private void Start()
    {
        GameManager.Instance.InputManager.OnJump += HandleJump;
    }

    private void Update()
    {
        MovePlayer();
        FlipSpriteAccordingToMoveDirection();
    
    }

    private void MovePlayer()
    {
        moveDirection = GameManager.Instance.InputManager.Movement;
    }

    private void FlipSpriteAccordingToMoveDirection()
    {
        if (moveDirection < 0)
        {
            transform.localScale = new Vector3(-initialScale.x,initialScale.y,initialScale.z);
        }
        else if (moveDirection > 0)
        {
            transform.localScale = initialScale;
        }
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        rigidbody.linearVelocity = new Vector2(moveDirection * moveSpeed, rigidbody.linearVelocity.y);
    }

    private void HandleJump()
    {
        if (isGroundedChecker.IsGrounded() == false) return;
        GameManager.Instance.AudioManager.PlaySFX(SFX.PlayerJump);
        rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x, 0);
        rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void HandleHurt()
    {
        GameManager.Instance.AudioManager.PlaySFX(SFX.PlayerHurt);
        PlayHitParticle();
        UpdateHealth(health.GetHealth());
        
    }

    private void HandlePlayerDeath()
    {
        GameManager.Instance.AudioManager.PlaySFX(SFX.PlayerDeath);
        PlayHitParticle();
        GetComponent<Collider2D>().enabled = false;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        GameManager.Instance.InputManager.DisablePlayerGameplayInput();
    }

    private void UpdateHealth(int amount)
    {
        GameManager.Instance.UpdateHealth(amount);
    }

    private void PlayWalkSound()
    {
        GameManager.Instance.AudioManager.PlaySFX(SFX.PlayerWalk);
    }

    private void Attack()
    {
        GameManager.Instance.AudioManager.PlaySFX(SFX.PlayerAttack);
        Collider2D[] hittedEnemies = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, attackLayer);
        print("Making enemy take damage");
        print(hittedEnemies.Length);

        foreach (Collider2D hittedEnemy in hittedEnemies)
        {
            print("Cheking enemy");
            if(hittedEnemy.TryGetComponent(out Health enemyHealth))
            {
                if (enemyHealth.IsDead) continue;
                print("Getting damage");
                enemyHealth.TakeDamage();
            }
        }
    }

    private void PlayHitParticle()
    {
        ParticleSystem instantiatedParticle = Instantiate(hitParticle, transform.position, transform.rotation);
        instantiatedParticle.Play();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }
}
