using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 10; // Damage dealt by the projectile

    private void Start()
    {
        // Destroy the projectile after 5 seconds
        Destroy(gameObject, 5f);
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
                playerHealth.TakeDamage(damage);
            }

            // Destroy the projectile after hitting the player
            Destroy(gameObject);
        }
    }
}
