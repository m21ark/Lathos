using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntBehavior : ProtoMob
{
    private GameObject player = null;
    private float lastHitTime = 0f;

    public float hitRate = 1.0f;

    void Update()
    {
            Move();
    }

    public override void Move()
    {
        if (player != null)
        {
            if(moveSpeed <= 0)
                return;

                        // Get the child object that contains the mesh
                        GameObject mesh = transform.GetChild(0).gameObject;

                        // Calculate direction vector towards the player
                        Vector3 direction = player.transform.position - transform.position;
                        direction.y = 0f; // Ensure the minion doesn't move up or down
                        direction.Normalize();

                        // Cast a ray downwards to detect the ground and have the minion follow the terrain slope
                        RaycastHit hit;
                        // Debug.DrawRay(mesh.transform.position + mesh.transform.forward * -2.5f, Vector3.down * 100, Color.red, 0.1f);
                        if (Physics.Raycast(mesh.transform.position + mesh.transform.forward * -2.5f, Vector3.down, out hit, 10f))
                        {

                            // Move the minion towards the player
                            transform.Translate(direction * moveSpeed * Time.deltaTime);

                            // Rotate the minion to face the player
                            Quaternion targetRotation = Quaternion.LookRotation(-direction);

                            // Apply rotation smoothly
                            mesh.transform.rotation = Quaternion.Slerp(mesh.transform.rotation, targetRotation, 0.1f);

                            // Adjust the Y rotation to conform to the terrain slope using the hit normal if its different from the current rotation
                            targetRotation = Quaternion.FromToRotation(mesh.transform.up, hit.normal) * mesh.transform.rotation;
                            if (Quaternion.Angle(mesh.transform.rotation, targetRotation) > 0.1f)
                                mesh.transform.rotation = Quaternion.Lerp(mesh.transform.rotation, targetRotation, 0.5f);
                            
                            // Calculate torque axis based on movement direction
                            Vector3 torqueAxis = Vector3.Cross(mesh.transform.forward, direction);

                            // Apply torque to simulate car wheel axis
                            Rigidbody rb = GetComponent<Rigidbody>();
                            float torqueAmount = 3.0f;
                            rb.AddTorque(torqueAxis * torqueAmount, ForceMode.Impulse);
                        }
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
        if (other.gameObject.CompareTag("Player"))
            if (Time.time - lastHitTime > hitRate)
            {
                lastHitTime = Time.time;
                OnCollisionEnter(other);
            }
    }
}
