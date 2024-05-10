using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private GameLogic gameLogic;
    private ProtoClass player;
    private Transform cameraPivot;

    private bool isGrounded = false;
    private float rotationSpeed = 10f;
    private Vector3 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Get GameLogic and Player 
        gameLogic = GameObject.FindWithTag("GameController").GetComponent<GameLogic>();

        // Camera Rotation Pivot
        cameraPivot = transform.parent.transform.Find("CameraPivot");
    }

    void Update()
    {
        if (!gameLogic.isPaused)
        {
            this.direction = GetFacedDirection();
            this.direction = SlopeDirection(this.direction);
            Move();
            RotateCamera();
        }
    }

    private Vector3 SlopeDirection(Vector3 movement)
    {
        if (movement == Vector3.zero) return movement;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
            movement = Vector3.ProjectOnPlane(movement, hit.normal);
        return movement.normalized;
    }

    private Vector3 GetFacedDirection()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
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

    private void CalculateVel()
    {
        player = gameLogic.player;

        // limit the Y velocity to avoid the player flying
        float y = rb.velocity.y;
        y = Mathf.Clamp(y, -100, player.jumpForce);

        if (this.direction != Vector3.zero)
        {
            rb.velocity = this.direction * player.moveSpeed;
            rb.velocity = new Vector3(rb.velocity.x, y, rb.velocity.z);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);

            // check if the player is grounded after the dash (special case for slopes)
            RaycastHit hit;
            isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, 1.65f);

            // to avoid the player jumping on a slope because Y velocity is not 0
            if (isGrounded) rb.velocity = new Vector3(0, 0, 0);
        }
    }

    void Move()
    {
        player = gameLogic.player;
        CalculateVel();

        // Rotate the player based on camera rotation on the y-axis only if moving
        if (this.direction != Vector3.zero)
            CharacterFaceDirection();

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // reset the Y velocity
            rb.AddForce(Vector3.up * player.jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && player.lastDashTime <= 0)
            StartCoroutine(Dash());
        else player.lastDashTime -= Time.deltaTime;

        // Basic Attack
        Attack0();

        // Base Class Attack
        Attack1();

        // Special Class Attack
        Attack2();

        // if player is attacking, rotate in that direction in the end
        if (player.isAttacking || player.isAttack1ing || player.isAttack2ing){
            this.direction = cameraPivot.forward;
            CharacterFaceDirection();
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

    IEnumerator Dash()
    {
        float duration = 0.5f;
        float elapsedTime = 0f;
        player.lastDashTime = player.dashCooldown;

        Vector3 dashDirection = new Vector3(this.direction.x, 0, this.direction.z).normalized;

        // If the player is not moving in any direction, dash in character's forward
        if (dashDirection == Vector3.zero)
            dashDirection = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;

        // Debug.DrawLine(transform.position, transform.position + (dashDirection) * 10f, Color.blue, 1f);

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            rb.velocity += dashDirection * player.dashSpeed;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    void Attack0()
    {
        if (Input.GetKeyDown(KeyCode.K) || Input.GetMouseButtonDown(0))
            player.isAttacking = true;

        if (Input.GetKeyUp(KeyCode.K) || Input.GetMouseButtonUp(0))
            player.isAttacking = false;

        player.lastAttackTime -= Time.deltaTime;
        if (player.isAttacking)
        {
            if (player.lastAttackTime <= 0)
            {
                player.Attack();
                player.lastAttackTime = player.A0ReloadTime * player.attackSpeed;
            }
        }
    }


    void Attack1()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(1)) player.isAttack1ing = true;
        if (Input.GetKeyUp(KeyCode.E) || Input.GetMouseButtonUp(1)) player.isAttack1ing = false;

        player.lastAttack1Time -= Time.deltaTime;
        if (player.isAttack1ing)
        {
            if (player.lastAttack1Time <= 0)
            {
                player.BaseAbility();
                player.lastAttack1Time = player.A1ReloadTime;
            }
        }
    }

    void Attack2()
    {
        if (Input.GetKeyDown(KeyCode.Q)) player.isAttack2ing = true;
        if (Input.GetKeyUp(KeyCode.Q)) player.isAttack2ing = false;

        player.lastAttack2Time -= Time.deltaTime;
        if (player.isAttack2ing)
        {
            if (player.lastAttack2Time <= 0)
            {
                player.SpecialAbility();
                player.lastAttack2Time = player.A2ReloadTime;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the player is touching the ground
        if (collision.gameObject.CompareTag("Floor"))
            isGrounded = true;
    }
}
