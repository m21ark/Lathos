using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaspBehaviour : ProtoMob
{
    public Transform projectileSpawnPoint;
    public Transform leftWing;
    public Transform rightWing;

    public float flapFrequency = 2f;
    public float flapAmplitude = 30f;

    private float flapTime;


    new void Update()
    {
        base.Update();
        FlapWings();
    }

    private void FlapWings()
    {
        flapTime += Time.deltaTime * flapFrequency;
        float flapAngle = Mathf.Sin(flapTime) * flapAmplitude;

        leftWing.localRotation = Quaternion.Euler(-1.801f, flapAngle - 118.732f, -128.849f);
        rightWing.localRotation = Quaternion.Euler(3.247f, -flapAngle - 61.065f, 51.17f);
    }

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
                rb.AddForce(direction * 40f, ForceMode.Impulse); // Adjust the force as needed
            }
        }
    }
}
