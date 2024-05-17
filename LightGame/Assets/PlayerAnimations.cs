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
    private Rigidbody rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerClass = GetComponent<ProtoClass>();
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

        CheckBooleanAnimation();

        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isDashing", isDashing);
    }

    void CheckBooleanAnimation(){
        isJumping = !playerController.isGrounded;
        isRunning = rb.velocity.x > 0.1f || rb.velocity.z > 0.1f;
        isDashing = playerClass.lastDashTime > 0;
    }
}