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
    private GameLogic gameLogic;

    private float projectileLoadingTime = 0.5f;
    private float lastShotTime = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameLogic = GameObject.FindWithTag("GameController").GetComponent<GameLogic>();
        lastShotTime = Time.time;
    }

    void Update()
    {
        // Move the player horizontally
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical) * moveSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        // Roll TODO: The dashing part is working but rotation is not
        if (Input.GetKeyDown(KeyCode.LeftShift))
            StartCoroutine(Roll());
        

        // Shooting if ammo reloaded
        bool ammoReloaded = Time.time - lastShotTime >= projectileLoadingTime;
        if (ammoReloaded && Input.GetKeyDown(KeyCode.K)){
            ShootProjectile();
            lastShotTime = Time.time;
        }

        rotateCamera();
    }

    void rotateCamera(){
        // TODO
    }

    IEnumerator Roll(){
        float duration = 0.5f; 
        float rollSpeed = 50f;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = transform.rotation * Quaternion.Euler(360f, 360f, 360f);

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.position += transform.forward * rollSpeed * Time.deltaTime;

            // Rotate the player smoothly
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the player ends up with the correct rotation
        transform.rotation = endRotation;
    }

    void ShootProjectile()
    {
        float projectileSpeed = 50f;

        GameObject projectile = Instantiate(projectilePrefab, transform.position + transform.forward, Quaternion.identity);
        
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        if (projectileRb != null)
            projectileRb.velocity = transform.forward * projectileSpeed;
        
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
