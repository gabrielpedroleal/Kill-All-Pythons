using UnityEngine;
using UnityEngine.EventSystems;

public class BossBehavior : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private Transform playerPosition;
    private Health bossHealth;
    private Animator animator;

    [SerializeField] private float moveSpeed = 3f;

    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackSize = 1f;
    [SerializeField] private Vector3 attackOffSet;
    [SerializeField] private LayerMask attackMask;

    private Vector3 attackPosition;

    private bool canAttack = false;
    private bool isFlipped = false;

    private void Awake()
    {
        bossHealth = GetComponent<Health>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        bossHealth.OnHurt += HandleHurt;
        bossHealth.OnDead += HandleDeath;

    }

    private void Start()
    {
        playerPosition = GameManager.Instance.GetPlayer().transform;
    }

    private void HandleHurt()
    {
        animator.SetTrigger("hurt");
        GameManager.Instance.AudioManager.PlaySFX(SFX.EnemyHurt);

    }

    private void HandleDeath()
    {
        animator.SetTrigger("death");
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void FollowPlayer()
    {
        Vector2 target = new Vector2(playerPosition.position.x, transform.position.y);
        Vector2 newPos = Vector2.MoveTowards(rigidbody.position, target, moveSpeed * Time.fixedDeltaTime);
        rigidbody.MovePosition(newPos);
        LookAtPlayer();
        CheckPositionFromPlayer();
    }

    private void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > playerPosition.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < playerPosition.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    public void CheckPositionFromPlayer()
    {
        float distanceFromPlayer = Vector2.Distance(transform.position, playerPosition.position);
        if (distanceFromPlayer < attackRange) 
        {
            canAttack = true;
        } else 
        { 
            canAttack = false;
        }
    }

    private void Attack()
    {
        attackPosition = transform.position;
        attackPosition += transform.right * attackOffSet.x;
        attackPosition += transform.up * attackOffSet.y;

        Collider2D collisionInfo = Physics2D.OverlapCircle(attackPosition, attackSize, attackMask);
        if (collisionInfo != null)
        {
            collisionInfo.GetComponent<Health>().TakeDamage();
        }

    }

    public bool GetCanAttack() => canAttack;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition, attackSize);
    }
}
