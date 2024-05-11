using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionBehavior1 : ProtoMob
{
    GameObject player;

    void Update()
    {
        Move();
        if (GameLogic.instance.player)
            player = GameLogic.instance.player.gameObject;
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
        if (collision.gameObject.CompareTag("Player"))
        {
            GameLogic.instance.player.TakeDamage(damage);
            JumpAwayFromPlayer();
        }

    }

    void JumpAwayFromPlayer()
    {
        // Calculate direction away from the player
        Vector3 direction = transform.position - player.transform.position;
        direction.y = 0f;
        direction.Normalize();

        float jumpForce = 20.0f;
        GetComponent<Rigidbody>().AddForce(direction * jumpForce, ForceMode.Impulse);
    }
}
