using UnityEngine;

public class BossWalk : StateMachineBehaviour
{
    private BossBehavior bossBehavior;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        bossBehavior = animator.GetComponent<BossBehavior>();
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossBehavior.FollowPlayer();
        if (bossBehavior.GetCanAttack() == true)
        {
            animator.SetTrigger("attack");
        }   
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("attack");

    }

}
