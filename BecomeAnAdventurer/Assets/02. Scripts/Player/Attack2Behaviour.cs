using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2Behaviour : StateMachineBehaviour
{
    private PlayerManager playerManager;
    private bool isNextAttack = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerManager = animator.GetComponentInParent<PlayerManager>();
        playerManager.isAttack = true;
        isNextAttack = false;
        animator.SetBool("HAS_NEXT_ATTACK", false);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isNextAttack) // 다음 공격이 있다면 검사 안함
        {
            return;
        }

        if (Input.GetKeyDown(playerManager.playerInfo.SHORTKEY_ATTACK))
        {
            isNextAttack = true;
            animator.SetBool("HAS_NEXT_ATTACK", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isNextAttack == false)
        {
            playerManager.isAttack = false;
            animator.SetBool("HAS_NEXT_ATTACK", false);
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
