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
    }
}