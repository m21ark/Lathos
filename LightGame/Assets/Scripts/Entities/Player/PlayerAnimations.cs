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

    private string[] classIDs = { "Base", "Mage", "Sorcerer", "Wizard", "Ranger", "Sharpshooter", "Rogue", "Berserker", "Knight", "Fighter" }; 
    private int classID = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerClass = GetComponent<ProtoClass>();
        playerController = GetComponent<PlayerController>();

        for(int i = 0; i < classIDs.Length; i++)
            if(playerClass.getClassName() == classIDs[i]) {
                classID = i;
                break;
            }
    }

    void Update()
    {
        isJumping = playerController.isJumping;
        isRunning = playerController.isMoving;
        isDashing = playerClass.lastDashTime > 0.5f;

        if (isDashing || isJumping) isRunning = false;

        if (isDashing)
        {
            isJumping = false;
            playerController.isJumping = false;
        }
        
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isDashing", isDashing);

        //trigger attack animations
        if(playerClass.hasPendingAnimation(0)) animator.SetTrigger("A0");
        if(playerClass.hasPendingAnimation(1)) animator.SetTrigger("A1");
        if(playerClass.hasPendingAnimation(2)) animator.SetTrigger("A2");

        animator.SetInteger("classID", classID);
    }
}