using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntBehavior : ProtoMob
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
            Vector3 direction = (player.transform.position - transform.position).normalized;
            direction.y = 0f; // Ensure the minion doesn't move up or down

            // Normalize the direction vector
            direction.Normalize();

            // Move the minion towards the player
            transform.Translate(direction * moveSpeed * Time.deltaTime);

            // Get the child object that contains the mesh
            GameObject mesh = transform.GetChild(0).gameObject;
            mesh.transform.rotation = Quaternion.Slerp(mesh.transform.rotation, Quaternion.LookRotation(-direction), 0.1f);

            // TODO: make head turn correctly and make the ant like a car axis with a head that turns
            
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
