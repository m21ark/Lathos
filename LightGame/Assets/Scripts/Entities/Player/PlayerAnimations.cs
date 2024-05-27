using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    bool isJumping = false;
    bool isRunning = false;
    bool isDashing = false;

    private Animator animator;
    private ProtoClass playerClass;
    private PlayerController playerController;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerClass = GetComponent<ProtoClass>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        isJumping = playerController.isJumping;
        isRunning = playerController.isMoving;
        isDashing = playerClass.lastDashTime > 0.5f;

        if (isDashing || isJumping) 
        {
            isRunning = false;
        }
        if (isDashing)
        {
            isJumping = false;
            playerController.isJumping = false;
        }
        
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isDashing", isDashing);
        
        animator.SetBool("isAttacking0", playerClass.isAttacking);
        animator.SetBool("isAttacking1", playerClass.isAttack1ing);
        animator.SetBool("isAttacking2", playerClass.isAttack2ing);

        Debug.Log(playerClass.isAttacking + " " + playerClass.isAttack1ing + " " + playerClass.isAttack2ing);
    }
}