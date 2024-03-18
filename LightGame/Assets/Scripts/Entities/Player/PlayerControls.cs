using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private GameLogic gameLogic;
    private Player player;

    private bool isGrounded = false;
    private bool isDashing = false;

    private Transform cameraPivot;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Get GameLogic and Player 
        gameLogic = GameObject.FindWithTag("GameController").GetComponent<GameLogic>();

        // Camera Rotation Pivot
        cameraPivot = transform.Find("CameraPivot");
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
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
            StartCoroutine(Dash());
        
        // Shoot
        if (Input.GetKeyDown(KeyCode.K) || Input.GetMouseButtonDown(0))
            ShootProjectile();
        
    }

    void rotateCamera(){
        // Rotate the camera based on mouse movement
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float rotationX = cameraPivot.localEulerAngles.y + mouseX;
        float rotationY = cameraPivot.localEulerAngles.x - mouseY;

        cameraPivot.localEulerAngles = new Vector3(rotationY, rotationX, 0);
    }

    IEnumerator Dash(){
        float duration = 0.5f; 
        float dashSpeed = 25f;

        float elapsedTime = 0f;

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

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            moveDirection.y = 0;
            moveDirection.Normalize();
            transform.position += moveDirection * dashSpeed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isDashing = false;
    }

    void ShootProjectile()
    {
       /*  ProtoProjectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<ProtoProjectile>();
        projectile.Fire(10, cameraPivot.forward, 20f, 0.0f); */
        Debug.Log("Shoot projectile is not implemented");
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
