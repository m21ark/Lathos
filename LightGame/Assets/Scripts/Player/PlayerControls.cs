using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 8f; 
    public float jumpForce = 10f;
    public GameObject projectilePrefab;

    private Rigidbody rb;
    private bool isGrounded = false;

    private bool isDashing = false;
    private GameLogic gameLogic;

    private float projectileLoadingTime = 0.5f;
    private float lastShotTime = 0;

    private Transform cameraPivot;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameLogic = GameObject.FindWithTag("GameController").GetComponent<GameLogic>();
        lastShotTime = Time.time;

        // Camera Rotation Pivot
        cameraPivot = transform.Find("CameraPivot");
        if (cameraPivot == null)
            Debug.LogError("CameraPivot not found. Make sure to create an empty GameObject as a child of the player and name it 'CameraPivot'.");
    }

    void Update()
    {
        Move();

        if(!gameLogic.isPaused)
            rotateCamera();
    }

    void Move(){
        // Move the player horizontally
        float moveHorizontal = 0f;
        float moveVertical = 0f;
        
        if (Input.GetKey(KeyCode.D)) moveHorizontal = 1f;
        else if (Input.GetKey(KeyCode.A))  moveHorizontal = -1f;
        if (Input.GetKey(KeyCode.W)) moveVertical = 1f;
        else if (Input.GetKey(KeyCode.S)) moveVertical = -1f;

        Vector3 movement = (cameraPivot.forward * moveVertical + cameraPivot.right * moveHorizontal).normalized * moveSpeed;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
            StartCoroutine(Dash());
        
        // Shooting if ammo reloaded
        bool ammoReloaded = Time.time - lastShotTime >= projectileLoadingTime;
        if (ammoReloaded && (Input.GetKeyDown(KeyCode.K) || Input.GetMouseButtonDown(0))){
            ShootProjectile();
            lastShotTime = Time.time;
        }
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



        if (Input.GetKey(KeyCode.W)) {
            moveDirection += cameraPivot.forward;
        }
        if (Input.GetKey(KeyCode.S)) {
            moveDirection -= cameraPivot.forward;
        }
        if (Input.GetKey(KeyCode.D)) {
            moveDirection += cameraPivot.right;
        }
        if (Input.GetKey(KeyCode.A)) {
            moveDirection -= cameraPivot.right;
        }
        if (moveDirection == Vector3.zero) {
            moveDirection = cameraPivot.forward;
        }

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            // Ensure the movement remains horizontal
            moveDirection.y = 0;
            moveDirection.Normalize();

            // Apply movement
            transform.position += moveDirection * dashSpeed * Time.deltaTime;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isDashing = false;
    }

    void ShootProjectile()
    {
        ProtoProjectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<ProtoProjectile>();
        projectile.Fire(10, cameraPivot.forward, 20f, 0.0f);
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
