using UnityEngine;

public class PlayerAnim : MonoBehaviour
{

    private Animator animator;
    private IsGroundedChecker isGrounded;
    private Health playerHealth;
    private bool isDead;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        isGrounded = GetComponent<IsGroundedChecker>();
        playerHealth = GetComponent<Health>();

        playerHealth.OnHurt += PlayHurtAnim;
        playerHealth.OnDead += PlayDeadAnim;
    }

    private void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.InputManager != null)
        {
            GameManager.Instance.InputManager.OnAttack += PlayAttackAnim;
        }
        else
        {
            Debug.LogError("GameManager or InputManager not find!");
        }
    }

    private void Update()
    {
        if (isDead) return;
        bool isMoving = GameManager.Instance.InputManager.Movement != 0;
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isJumping", !isGrounded.IsGrounded());
    }

    private void PlayHurtAnim()
    {
        if (isDead) return;
        animator.SetTrigger("hurt");
    }

    private void PlayDeadAnim()
    {
        isDead = true;
        animator.SetTrigger("dead");
    }

    private void PlayAttackAnim()
    {
        animator.SetTrigger("attack");
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null && GameManager.Instance.InputManager != null)
        {
            GameManager.Instance.InputManager.OnAttack -= PlayAttackAnim;
        }
    }
}
