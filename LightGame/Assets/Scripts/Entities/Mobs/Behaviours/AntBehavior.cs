using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntBehavior : ProtoMob
{
    GameObject player = null;

    void Update()
    {
        Move();
    }

    public override void Move()
    {
        if (player != null)
        {
            // Calculate direction vector towards the player
            Vector3 direction = (player.transform.position - transform.position).normalized;
            direction.y = 0f; // Ensure the minion doesn't move up or down
            direction.Normalize();

            // Move the minion towards the player
            transform.Translate(direction * moveSpeed * Time.deltaTime);

            // Get the child object that contains the mesh
            GameObject mesh = transform.GetChild(0).gameObject;

            // Calculate rotation based on movement direction
            Quaternion targetRotation = Quaternion.LookRotation(-direction);

            // Apply rotation smoothly
            mesh.transform.rotation = Quaternion.Slerp(mesh.transform.rotation, targetRotation, 0.1f);

            // Calculate torque axis based on movement direction
            Vector3 torqueAxis = Vector3.Cross(mesh.transform.forward, direction);

            // Apply torque to simulate car wheel axis
            Rigidbody rb = GetComponent<Rigidbody>();
            float torqueAmount = 3.0f;
            rb.AddTorque(torqueAxis * torqueAmount, ForceMode.Impulse);
        }
        else if (GameLogic.instance.player)
            player = GameLogic.instance.player.gameObject;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Deal Damage
        if (collision.gameObject.CompareTag("Player"))
            GameLogic.instance.player.TakeDamage(damage);
        
    }

    void OnCollisionStay(Collision other)
    {
        OnCollisionEnter(other);
    }
}
