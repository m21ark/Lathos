using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion1 : ProtoMob
{

    private GameLogic gameLogic;
    GameObject player;

    void Start()
    {
        gameLogic = GameObject.FindWithTag("GameController").GetComponent<GameLogic>();
    }
    void Update()
    {
       Move();
       if(gameLogic.player)
            player = gameLogic.player.getGameObject();
    }

    public override void Move()
    {
        
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

    void OnCollisionEnter(Collision collision)
    {

        // Deal Damage
        if (collision.gameObject.CompareTag("Player")){
            gameLogic.player.TakeDamage(10);
            JumpAwayFromPlayer();
        }
      
    }

    void JumpAwayFromPlayer()
    {
        // Calculate direction away from the player
        Vector3 direction = transform.position - player.transform.position;
        direction.y = 0f; 
        direction.Normalize();

        float jumpForce = 10.0f;
        GetComponent<Rigidbody>().AddForce(direction * jumpForce, ForceMode.Impulse);
    }
}
