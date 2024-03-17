using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    public int health = 100;
    public float moveSpeed = 5f;

    private GameLogic gameLogic;

    // Start is called before the first frame update
    void Start()
    {
        gameLogic = GameObject.FindWithTag("GameController").GetComponent<GameLogic>();
    }

    // Update is called once per frame
    void Update()
    {
       FollowPlayer();
    }

    void FollowPlayer()
    {
        GameObject player = gameLogic.player;

        if (player != null)
        {
            // Calculate direction vector towards the player
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0f; // Ensure the minion doesn't move up or down

            // Normalize the direction vector
            direction.Normalize();

            // Move the minion towards the player
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
    }

    void JumpAwayFromPlayer()
    {
        GameObject player = gameLogic.player;

        // Calculate direction away from the player
        Vector3 direction = transform.position - player.transform.position;
        direction.y = 0f; 
        direction.Normalize();

        float jumpForce = 10.0f;
        GetComponent<Rigidbody>().AddForce(direction * jumpForce, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Take damage
        if (collision.gameObject.CompareTag("Projectile")){
            health -= 10; // hardcoded for now

            if(health <= 0){
                Destroy(gameObject);
            }

            Destroy(collision.gameObject);
        }

        // Deal Damage
        if (collision.gameObject.CompareTag("Player")){
            gameLogic.damagePlayer(10);
            JumpAwayFromPlayer();
        }
      
    }
}
