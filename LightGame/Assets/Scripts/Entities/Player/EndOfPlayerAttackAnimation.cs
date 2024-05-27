using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfPlayerAttackAnimation : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        PlayerController player = animator.GetComponent<PlayerController>();
        player.UnlockMovementAfterAttack();
    }
}
