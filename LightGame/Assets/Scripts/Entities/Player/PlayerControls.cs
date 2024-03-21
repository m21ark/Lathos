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
        if(!gameLogic.isPaused){
            Move();
            rotateCamera();
        }     
    }

    void Move(){
        // Move the player horizontally
        float moveHorizontal = 0f;
        float moveVertical = 0f;

        player = gameLogic.player;
        
        if (Input.GetKey(KeyCode.D)) moveHorizontal = 1f;
        else if (Input.GetKey(KeyCode.A))  moveHorizontal = -1f;
        if (Input.GetKey(KeyCode.W)) moveVertical = 1f;
        else if (Input.GetKey(KeyCode.S)) moveVertical = -1f;

        Vector3 movement = (cameraPivot.forward * moveVertical + cameraPivot.right * moveHorizontal).normalized * player.moveSpeed;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * player.jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && player.lastDashTime <= 0)
            StartCoroutine(Dash());
        else 
            player.lastDashTime -= Time.deltaTime;
        
        // Basic Attack
        Attack0();

        // Base Class Attack
        Attack1();

        // Special Class Attack
        Attack2();

    }

    void Attack0(){
        if (Input.GetKeyDown(KeyCode.K) || Input.GetMouseButtonDown(0))
            player.isAttacking = true;
        
        if (Input.GetKeyUp(KeyCode.K) || Input.GetMouseButtonUp(0))
            player.isAttacking = false;

        player.lastAttackTime -= Time.deltaTime;
        if (player.isAttacking){
            if (player.lastAttackTime <= 0){
                player.Attack();
                player.lastAttackTime = player.A0ReloadTime;
            }
        }
    }


    void Attack1(){
        if (Input.GetKeyDown(KeyCode.E)) player.isAttack1ing = true;
        if (Input.GetKeyUp(KeyCode.E)) player.isAttack1ing = false;

        player.lastAttack1Time -= Time.deltaTime;
        if(player.isAttack1ing){
            if(player.lastAttack1Time <= 0){
                player.BaseAbility();
                player.lastAttack1Time = player.A1ReloadTime;
            }
        }
    }

    void Attack2(){
        if (Input.GetKeyDown(KeyCode.Q)) player.isAttack2ing = true;
        if (Input.GetKeyUp(KeyCode.Q)) player.isAttack2ing = false;
        
        player.lastAttack2Time -= Time.deltaTime;
        if(player.isAttack2ing){
            if(player.lastAttack2Time <= 0){
                player.SpecialAbility();
                player.lastAttack2Time = player.A2ReloadTime;
            }
        }
    }


    void rotateCamera(){
        // Rotate the camera based on mouse movement
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float rotationX = cameraPivot.localEulerAngles.y + mouseX;
        float rotationY = cameraPivot.localEulerAngles.x - mouseY;


        cameraPivot.localEulerAngles = new Vector3(rotationY, rotationX, 0);

        // rotate the player based on camera rotation on the y-axis
        transform.rotation = Quaternion.Euler(0, rotationX, 0);

        // set pivot position to player position
        cameraPivot.position = transform.position;
    }

    IEnumerator Dash(){
        float duration = 0.5f; 
        float elapsedTime = 0f;
        player.lastDashTime = player.dashCooldown;

        // Calculate movement direction based on player input
        Vector3 moveDirection = Vector3.zero;

        // Apply dash direction according to the key presses
        if (Input.GetKey(KeyCode.W)) 
            moveDirection += cameraPivot.forward;
        if (Input.GetKey(KeyCode.S)) 
            moveDirection -= cameraPivot.forward;
        if (Input.GetKey(KeyCode.D)) 
            moveDirection += cameraPivot.right;
        if (Input.GetKey(KeyCode.A))
            moveDirection -= cameraPivot.right;
        if (moveDirection == Vector3.zero)
            moveDirection = cameraPivot.forward;

        Rigidbody rb = GetComponent<Rigidbody>(); // Assuming this script is attached to the same GameObject as the Rigidbody

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            moveDirection.y = 0;
            moveDirection.Normalize();

            Vector3 tempDirection = new Vector3(moveDirection.x, 0, moveDirection.z);
            
            // Instead of modifying transform.position, modify the Rigidbody's velocity
            rb.velocity += tempDirection * player.dashSpeed;
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the player is touching the ground
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }
}
