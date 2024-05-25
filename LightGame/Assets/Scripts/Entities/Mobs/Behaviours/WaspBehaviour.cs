using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaspBehaviour : ProtoMob
{
    public Transform projectileSpawnPoint; 

    public override void AttackPlayer()
    {
        agent.SetDestination(transform.position); 

        // Look at the player
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Attack code here
            ShootProjectile();

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    protected void ShootProjectile()
    {
        if (projectile != null && player != null && projectileSpawnPoint != null)
        {
            GameObject spawnedProjectile = Instantiate(projectile, projectileSpawnPoint.position, Quaternion.identity);
            Rigidbody rb = spawnedProjectile.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 direction = (player.position - projectileSpawnPoint.position).normalized;
                rb.AddForce(direction * 20f, ForceMode.Impulse); // Adjust the force as needed
            }
        }
    }
}
