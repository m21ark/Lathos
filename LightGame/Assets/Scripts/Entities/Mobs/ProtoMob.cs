using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoMob : MonoBehaviour
{
    public int health = 20;
    public int enemyLevel = 5; // For now ... see level progression later  
    public float moveSpeed = 5f;
    public int damage = 10;

    public GameObject xpOrbPreFab;

    public virtual void Attack()
    {
        // Attack behavior
    }

    public virtual void Move()
    {
        // Move behavior
    }

    public void Die()
    {
        // GameObject xpOrbPreFab = Resources.Load<GameObject>("OrbXP");

        // Randomly determine the number of orbs to spawn based on enemy level
        int numOrbs = Random.Range(enemyLevel, enemyLevel * 2);

        // Spawn orbs
        for (int i = 0; i < numOrbs; i++) {
            // Calculate random offset from the center
            float xOffset = Random.Range(-2f, 2f);
            float zOffset = Random.Range(-2f, 2f);
            Vector3 spawnPosition = gameObject.transform.position + new Vector3(xOffset, 0, zOffset);

            // Instantiate orb with slight offset
            GameObject orb = Instantiate(xpOrbPreFab, spawnPosition, Quaternion.identity);

            Destroy(orb, 10f); // Despawn orbs if not collected in 10s
        }

        // Destroy the enemy GameObject
        Destroy(gameObject);
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
}
