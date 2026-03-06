using System.Collections;
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
    [SerializeField] private float attackCooldown = 1f;
    private float attackCooldownTimer = Mathf.Infinity;

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

    private void Update()
    {
        attackCooldownTimer += Time.deltaTime;
    }

    private void HandleHurt()
    {
        animator.SetTrigger("hurt");
        GameManager.Instance.AudioManager.PlaySFX(SFX.EnemyHurt);

    }

    private void HandleDeath()
    {
        animator.SetTrigger("dead");
        GameManager.Instance.AudioManager.PlaySFX(SFX.EnemyDeath);
        GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(DestroyEnemy(2));
    }

    private IEnumerator DestroyEnemy(int time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
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
        if (attackCooldownTimer < attackCooldown) return;

        attackCooldownTimer = 0f;
        GameManager.Instance.AudioManager.PlaySFX(SFX.EnemyAttack);

        attackPosition = transform.position;
        attackPosition += transform.right * attackOffSet.x;
        attackPosition += transform.up * attackOffSet.y;

        Collider2D collisionInfo = Physics2D.OverlapCircle(attackPosition, attackSize, attackMask);
        if (collisionInfo != null)
        {
            collisionInfo.GetComponent<Health>().TakeDamage();
        }

    }

    public void StartChasing()
    {
        animator.SetBool("canChase", true);
    }

    public bool GetCanAttack() => canAttack && attackCooldownTimer >= attackCooldown;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition, attackSize);
    }
}
