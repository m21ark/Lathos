using UnityEngine;

public class Projectile : ProtoAttack
{

    public override void Fire(int damage, Vector3 direction, params (string key, object value)[] kwargs)
    {
        attackRb = gameObject.GetComponent<Rigidbody>();
        if (attackRb != null) attackRb.velocity = direction * speed;

        projDamage = damage;

        // Despawn attack when despawn time is reached
        Destroy(gameObject, despawnTime);
    }

    void FixedUpdate()
    {
        attackRb = GetComponent<Rigidbody>();

        // Calculate the custom gravity vector
        Vector3 customGravity = -transform.up * gravity * 9.8F;

        // Apply the custom gravity force to the Rigidbody
        attackRb.AddForce(customGravity, ForceMode.Acceleration);
    }

    public void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.CompareTag("Player")) {
            ProtoClass playerHealth = collider.GetComponent<ProtoClass>();
            if (playerHealth != null) {
                playerHealth.TakeDamage(projDamage);
            }
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the projectile hit the player
        if (collision.collider.CompareTag("Player"))
        {
            // Try to get the player's health component and apply damage
            ProtoClass playerHealth = collision.collider.GetComponent<ProtoClass>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(projDamage);
            }

            // Destroy the projectile after hitting the player
            Destroy(gameObject);
        }
    }
}
