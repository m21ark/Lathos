using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private ProtoClass player;
    public Transform cameraPivot;

    [HideInInspector] public bool isGrounded = false;
    [HideInInspector] public bool isJumping = false;
    [HideInInspector] public bool isMoving = false;

    private float rotationSpeed = 10f;
    private Vector3 direction;

    private bool attackingStandingStill = false;
    private bool attackAnimationEnded = true;

    private KeyCode attackKey = KeyCode.K;
    private KeyCode attack1Key = KeyCode.E;
    private KeyCode attack2Key = KeyCode.Q;
    private KeyCode dashKey = KeyCode.LeftShift;
    private KeyCode jumpKey = KeyCode.Space;

    // FMOD Events
    private int footstepsInstanceID;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Camera Rotation Pivot
        cameraPivot = transform.parent.transform.Find("CameraPivot");

        // FMOD Events
        footstepsInstanceID = AudioManager.instance.CreateInstanceIDOnPlayer(FMODEvents.instance.playerFootsteps);
    }

    void Update()
    {
        if (!GameLogic.instance.isPaused)
        {
            this.direction = GetFacedDirection();
            this.direction = SlopeDirection(this.direction);
            Move();
            RotateCamera();

            if(isGrounded){
                isJumping = false;
            }
        }
    }

    private Vector3 SlopeDirection(Vector3 movement)
    {
        if (movement == Vector3.zero) return movement;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f, ~LayerMask.GetMask("LampLight")))
            movement = Vector3.ProjectOnPlane(movement, hit.normal);
        return movement.normalized;
    }

    private Vector3 GetFacedDirection()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        isMoving = moveHorizontal != 0 || moveVertical != 0;

        Vector3 direction = (cameraPivot.forward * moveVertical + cameraPivot.right * moveHorizontal);
        return direction.normalized;
    }

    private void CharacterFaceDirection()
    {
        Vector3 targetDirection = new Vector3(this.direction.x, 0, this.direction.z);
        Quaternion targetRotation;
        if (targetDirection != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public void ForceCharacterFaceCrosshair(){
        this.direction = cameraPivot.forward;
        Vector3 targetDirection = new Vector3(this.direction.x, 0, this.direction.z);
        if (targetDirection != Vector3.zero) transform.rotation = Quaternion.LookRotation(targetDirection);
    }


    private void CalculateVel()
    {
        // if the player is performing an attack that requires standing still, don't move
        if(attackingStandingStill) return; 

        player = GameLogic.instance.player;

        // limit the Y velocity to avoid the player flying
        float y = rb.velocity.y;
        y = Mathf.Clamp(y, -100, player.jumpForce);

        if (this.direction != Vector3.zero)
        {
            rb.velocity = this.direction * player.moveSpeed;
            rb.velocity = new Vector3(rb.velocity.x, y, rb.velocity.z);

            // if the player is moving, play the footsteps sound
            if (isGrounded && !player.isAttacking && !player.isAttack1ing && !player.isAttack2ing)
                AudioManager.instance.PlayInstanceIfNotPlayingOnPlayer(footstepsInstanceID);

            
            // check if the player is grounded after the dash (special case for slopes)
            RaycastHit hit;
            isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, 1.65f, ~LayerMask.GetMask("LampLight"));
        }
        else
        {
            AudioManager.instance.StopInstancePlayingOnPlayer(footstepsInstanceID); // stop the footsteps sound

            rb.velocity = new Vector3(0, rb.velocity.y, 0);

            // check if the player is grounded after the dash (special case for slopes)
            RaycastHit hit;
            isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, 1.65f, ~LayerMask.GetMask("LampLight"));

            // to avoid the player jumping on a slope because Y velocity is not 0
            if (isGrounded) rb.velocity = new Vector3(0, 0, 0);
        }
    }

    public void AttackAnimationEnded()
    {
        attackingStandingStill = false;
        attackAnimationEnded = true;
    }

    void Move()
    {
        player = GameLogic.instance.player;
        CalculateVel();

        // Rotate the player based on camera rotation on the y-axis only if moving
        if (this.direction != Vector3.zero && !attackingStandingStill && attackAnimationEnded)
            CharacterFaceDirection();

        // Jumping
        if (Input.GetKeyDown(jumpKey) && isGrounded )
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.playerJump, transform.position);
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // reset the Y velocity
            rb.AddForce(Vector3.up * player.jumpForce, ForceMode.Impulse);
            isJumping = true;
            isGrounded = false;
        }

        // Dash
        if (Input.GetKeyDown(dashKey) && player.lastDashTime <= 0 && !attackingStandingStill) 
            StartCoroutine(Dash());
        else player.lastDashTime -= Time.deltaTime;

        // Basic Attack
        Attack0();

        // Base Class Attack
        Attack1();

        // Special Class Attack
        Attack2();

        // if player is attacking, rotate in that direction in the end
        if (player.isAttacking || player.isAttack1ing || player.isAttack2ing)
        {
            this.direction = cameraPivot.forward;
            // CharacterFaceDirection();
            ForceCharacterFaceCrosshair();
        }
    }

    void RotateCamera()
    {
        // Rotate the camera based on mouse movement
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float rotationX = cameraPivot.localEulerAngles.y + mouseX;
        float rotationY = cameraPivot.localEulerAngles.x - mouseY;

        if (rotationY > 180) rotationY -= 360;
        rotationY = Mathf.Clamp(rotationY, -85, 85);

        cameraPivot.localEulerAngles = new Vector3(rotationY, rotationX, 0);

        // Set pivot position to player position
        cameraPivot.position = transform.position;
    }

    public IEnumerator Dash()
    {
        float duration = 0.5f;
        float elapsedTime = 0f;
        player.lastDashTime = player.dashCooldown;

        Vector3 dashDirection = new Vector3(this.direction.x, 0, this.direction.z).normalized;

        // If the player is not moving in any direction, dash in character's forward
        if (dashDirection == Vector3.zero)
            dashDirection = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
            
        while (elapsedTime < duration)
        {
            if(attackingStandingStill) break; // if the player is attacking and standing still, stop the dash (special case for attacks that require standing still)
            float t = elapsedTime / duration;
            rb.velocity += dashDirection * player.dashSpeed;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    void HandleAttack(KeyCode attackKey, ref bool isAttacking, ref float lastAttackTime, ref bool pendingAnimation, float reloadTime, bool attackStandingStill, Action attackAction, bool useMouseButton = false, int mouseButton = 0)
    {        
        if (Input.GetKeyDown(attackKey) || (useMouseButton && Input.GetMouseButtonDown(mouseButton)))
            isAttacking = true;
          
        if (Input.GetKeyUp(attackKey) || (useMouseButton && Input.GetMouseButtonUp(mouseButton)))
            isAttacking = false;

        // Check if the player can attack again
        lastAttackTime -= Time.deltaTime;
        if(lastAttackTime > 0) return;

        if (isAttacking)
        {
            attackAction();
            pendingAnimation = true;
            attackAnimationEnded = false;
            lastAttackTime = reloadTime;
        }

        // if the player is attacking and standing still, stop the player's movement in x and z
        if (attackStandingStill && isAttacking){
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            attackingStandingStill = true;
        }
    }

    void Attack0()
    {
        HandleAttack(attackKey, ref player.isAttacking, ref player.lastAttackTime, ref player.pendingA0Animate, player.A0ReloadTime * player.attackSpeed, player.stopsMovementA0, player.Attack, true, 0);
    }

    void Attack1()
    {
        HandleAttack(attack1Key, ref player.isAttack1ing, ref player.lastAttack1Time, ref player.pendingA1Animate, player.A1ReloadTime, player.stopsMovementA1, player.BaseAbility, true, 1);
    }

    void Attack2()
    {
        HandleAttack(attack2Key, ref player.isAttack2ing, ref player.lastAttack2Time, ref player.pendingA2Animate, player.A2ReloadTime, player.stopsMovementA2, player.SpecialAbility);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the player is touching the ground
        if (collision.gameObject.CompareTag("Floor"))
            isGrounded = true;
    }
}
