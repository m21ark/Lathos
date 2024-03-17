using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed at which the player moves
    public float jumpForce = 10f; // Force applied when the player jumps
    private Rigidbody rb;
    private bool isGrounded = false;

    private GameLogic gameLogic;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameLogic = GameObject.FindWithTag("GameController").GetComponent<GameLogic>();
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
        {
            StartCoroutine(Roll());
        }
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

    void OnCollisionEnter(Collision collision)
    {
        // Check if the player is touching the ground
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }


        // if collision with boss, damage both player and boss for now
        if (collision.gameObject.CompareTag("Boss"))
        {
            gameLogic.damagePlayer(1);   
            gameLogic.damageBoss(5);   
        }
    }
}
